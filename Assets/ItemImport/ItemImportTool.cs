using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;


#if UNITY_EDITOR
using UnityEditor;

public class ItemImportTool : EditorWindow
{
    // --- データ構造の定義 ---

    [Serializable]
    public class SheetSetting
    {
        public string key; // "Gun", "Ammo" などの識別名
        public string url; // スプレッドシートのURL
        public string targetTypeName;
    }

    [Serializable]
    public class SheetSettingList
    {
        // JsonUtilityでリストを扱うためのラップ用クラス
        public List<SheetSetting> settings = new List<SheetSetting>();
    }

    // --- 変数と保存キー ---

    private SheetSettingList _settingList = new SheetSettingList();
    private string[] _availableTypeNames;
    private int _selectedSheetIndex = 0;

    private const string SAVE_KEY_DATA = "ItemImportTool_SheetListData";
    private const string SAVE_KEY_INDEX = "ItemImportTool_SelectedIndex";

    //ウィンドウの表示処理
    [MenuItem("Tools/Item Import Tool")]
    public static void ShowWindow()
    {
        GetWindow<ItemImportTool>("Item Importer");
    }

    //ウィンドウが開いたとき（またはコンパイル後）に呼ばれる
    private void OnEnable()
    {
        //保存されているリストを読み込む
        string json = EditorPrefs.GetString(SAVE_KEY_DATA, "");
        if (!string.IsNullOrEmpty(json))
        {
            _settingList = JsonUtility.FromJson<SheetSettingList>(json);
        }

        //保存されている「選択中の番号」を読み込む
        _selectedSheetIndex = EditorPrefs.GetInt(SAVE_KEY_INDEX, 0);

        // 2. [追加] ItemData を継承した具象クラスを自動抽出する
        RefreshAvailableTypes();
    }

    // [追加] リフレクションで ItemData 継承クラスを探す
    private void RefreshAvailableTypes()
    {
        _availableTypeNames = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(ItemData)) && !type.IsAbstract)
            .Select(type => type.Name)
            .ToArray();
    }

    private void OnGUI()
    {
        DrawSettingsArea();
        EditorGUILayout.Space();
        // 区切り線
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        DrawExecuteArea();
    }

    private void DrawSettingsArea()
    {
        EditorGUILayout.LabelField("1. Spreadsheet Settings", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("スプレッドシートのURLを登録してください。Keyはシートの種類を識別する名前です。", MessageType.None);
        
        EditorGUILayout.Space();

        //ここから1. Spreadsheet Settingsの編集セクション
        EditorGUI.BeginChangeCheck();
        {
            for (int i = 0; i < _settingList.settings.Count; i++)
            {
                SheetSetting setting = _settingList.settings[i];
                EditorGUILayout.BeginHorizontal();
                
                //ラベルだけを描画（幅を30〜40くらいに固定すると密着します）
                EditorGUILayout.LabelField("Key", GUILayout.Width(35));
                // 2. テキストボックスを描画（第一引数に空文字を入れるのがコツ）
                _settingList.settings[i].key = EditorGUILayout.TextField("", setting.key, GUILayout.Width(150));

                EditorGUILayout.Space(10); //少しだけ隙間を空けてURLへ

                // URL側も同様に
                EditorGUILayout.LabelField("URL", GUILayout.Width(35));
                _settingList.settings[i].url = EditorGUILayout.TextField("", setting.url);

                // --- [変更] Typeプルダウン ---
                EditorGUILayout.LabelField("Type", GUILayout.Width(35));

                // 現在の保存されている名前がリストの何番目か探す
                int currentTypeIndex = Array.IndexOf(_availableTypeNames, setting.targetTypeName);
                if (currentTypeIndex < 0) currentTypeIndex = 0;

                // プルダウンを表示
                int newTypeIndex = EditorGUILayout.Popup(currentTypeIndex, _availableTypeNames, GUILayout.Width(100));
                setting.targetTypeName = _availableTypeNames[newTypeIndex];

                // 削除ボタン
                if (GUILayout.Button("delete", GUILayout.Width(45)))
                {
                    _settingList.settings.RemoveAt(i);
                    // リスト構造が変わるので、一旦描画を終了して次のフレームで再描画させる
                    GUIUtility.ExitGUI();
                }

                EditorGUILayout.EndHorizontal();
            }

            // if (GUILayout.Button("+ Add New Sheet Setting"))
            // {
            //     _settingList.settings.Add(new SheetSetting { key = "New Key", url = "" });
            // }

            if (GUILayout.Button("+ Add New Sheet Setting"))
            {
                // 新規追加時はリストの先頭の型をデフォルトに設定
                string defaultType = _availableTypeNames.Length > 0 ? _availableTypeNames[0] : "";
                _settingList.settings.Add(new SheetSetting { key = "New Key", url = "", targetTypeName = defaultType });
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            SaveSettings();
        }
    }

    private void DrawExecuteArea()
    {
        //ここから2. Import Executionの編集セクション
        EditorGUILayout.LabelField("2. Import Execution", EditorStyles.boldLabel);

        if (_settingList.settings.Count > 0)
        {
            // ドロップダウンに表示する名前の配列を作成（LINQを使用）
            string[] sheetNames = _settingList.settings.Select(s => s.key).ToArray();

            // インデックスが範囲外にならないよう調整（削除直後などの対策）
            if (_selectedSheetIndex >= sheetNames.Length) _selectedSheetIndex = 0;

            //キーでどのURLをロードするか選ぶためのドロップダウン
            EditorGUI.BeginChangeCheck();
            _selectedSheetIndex = EditorGUILayout.Popup("Select Target", _selectedSheetIndex, sheetNames);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt(SAVE_KEY_INDEX, _selectedSheetIndex);
            }

            EditorGUILayout.Space();

            // 実行ボタン
            GUI.color = Color.cyan; // ボタンを目立たせる
            if (GUILayout.Button($"{sheetNames[_selectedSheetIndex]} をインポート開始", GUILayout.Height(40)))
            {
                SheetSetting target = _settingList.settings[_selectedSheetIndex];
                OnPressImportButton(target).Forget();
            }
            GUI.color = Color.white;
        }
        else
        {
            EditorGUILayout.HelpBox("まずは上の「Add New Sheet Setting」から設定を追加してください。", MessageType.Warning);
        }
    }

    private async UniTaskVoid OnPressImportButton(SheetSetting setting)
    {
        if (string.IsNullOrEmpty(setting.url)) return;

        Debug.Log($"[{setting.key}] ({setting.targetTypeName}) インポート開始...");
        string csvUrl = SpreadsheetLoader.ConvertToCsvUrl(setting.url);

        try
        {
            string csvText = await SpreadsheetLoader.DownloadCsvAsync(csvUrl);
            List<string[]> rows = SpreadsheetLoader.ParseCsv(csvText);

            Debug.Log($"[{setting.key}] ダウンロード成功。行数: {rows.Count}");

            //TODO: ここで作成済みのScriptableObjectを探す、または新規作成して流し込む
            //次のステップでこの 'setting.targetTypeName' を使ったクラス分離処理を実装します
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{setting.key}] エラー: {ex.Message}");
        }
    }

    // --- 内部処理 ---
    private void SaveSettings()
    {
        string json = JsonUtility.ToJson(_settingList);
        EditorPrefs.SetString(SAVE_KEY_DATA, json);
        Debug.Log("設定を保存しました。");
    }
}
#endif