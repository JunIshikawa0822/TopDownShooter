#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HTN
{
    [CustomPropertyDrawer(typeof(StateVariableAttribute))]
    public class StateVariableDrawer : PropertyDrawer
    {
        private class VariableOption
        {
            public string Path;      // "WorldState/Ammo"
            public string Name;      // "Ammo"
            public Type Type;        // typeof(int)
        }

        // キャッシュ（毎回リフレクションすると重いため）
        private static List<VariableOption> _cachedOptions;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // まずは通常のラベルを描画
            EditorGUI.BeginProperty(position, label, property);

            // 属性からフィルタリングしたい型を取得 (例: int, float)
            var attr = (StateVariableAttribute)attribute;
            Type targetType = attr.TargetType;

            // 選択肢を取得（初回のみ生成）
            if (_cachedOptions == null)
            {
                _cachedOptions = CollectVariables();
            }

            // ターゲット型に合うものだけを抽出
            var validOptions = _cachedOptions
                .Where(opt => targetType == null || opt.Type == targetType)
                .ToList();

            // ドロップダウン用の表示名リストを作成
            string[] displayOptions = new string[validOptions.Count + 1];
            displayOptions[0] = "None"; // 未選択状態

            int currentIndex = 0;
            string currentName = property.stringValue;

            for (int i = 0; i < validOptions.Count; i++)
            {
                displayOptions[i + 1] = validOptions[i].Path + " (" + validOptions[i].Type.Name + ")";
                
                // 名前が一致したらそれを選択状態にする
                if (validOptions[i].Name == currentName)
                {
                    currentIndex = i + 1;
                }
            }

            // ドロップダウンを表示
            int newIndex = EditorGUI.Popup(position, label.text, currentIndex, displayOptions);

            // 選択が変更されたら値を更新
            if (newIndex != currentIndex)
            {
                if (newIndex == 0)
                {
                    property.stringValue = ""; // None
                }
                else
                {
                    // 変数名だけを保存する
                    property.stringValue = validOptions[newIndex - 1].Name;
                }
            }

            EditorGUI.EndProperty();
        }

        private List<VariableOption> CollectVariables()
        {
            var list = new List<VariableOption>();

            // WorldState の変数を収集
            CollectFromType(typeof(WorldState), "WorldState", list);
            
            // SelfState の変数を収集
            CollectFromType(typeof(SelfState), "SelfState", list);

            return list;
        }

        private void CollectFromType(Type type, string prefix, List<VariableOption> list)
        {
            // Public Field を取得
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                list.Add(new VariableOption
                {
                    Path = prefix + "/" + field.Name,
                    Name = field.Name,
                    Type = field.FieldType
                });
            }

            // Public Property を取得 (読み取り可能なもの)
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.CanRead)
                {
                    list.Add(new VariableOption
                    {
                        Path = prefix + "/" + prop.Name,
                        Name = prop.Name,
                        Type = prop.PropertyType
                    });
                }
            }
        }
    }
}
#endif
