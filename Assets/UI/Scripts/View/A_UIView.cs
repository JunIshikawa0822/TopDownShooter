using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace Game.UI
{
    public abstract class AUIView : MonoBehaviour, IDisposable
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] protected string _rootElementName = "";
        protected VisualElement _rootElement;
        //このUIが初期化（Initialize）されたときに、すぐに非表示にするかどうかの設定です。
        [SerializeField] protected bool _hideOnAwake = false;

        //このUIが部分的に透けて背後のUIを見せる「オーバーレイ」として機能するかどうかを示すフラグです。
        [SerializeField] protected bool _isOverlay;

        //このクラスが管理するUIコンポーネントのルート要素（一番上の VisualElement）を保持します。UXMLファイルで定義されたUI階層全体への入り口となります。

        //Properties
        public VisualElement Root => _rootElement;
        public bool IsTransparent => _isOverlay;
        public bool IsHidden => _rootElement.style.display == DisplayStyle.None;

        public virtual void Initialize()
        {
            if (_uiDocument == null)
            {
                Debug.LogError("UIDocumentを設定してください");
                return;
            }

            _rootElement = _uiDocument.rootVisualElement;

            if (!string.IsNullOrEmpty(_rootElementName))
            {
                VisualElement target = _rootElement.Q(_rootElementName);
                if (target == null)
                {
                    Debug.LogError($"RootElementが見つかりませんでした : {_rootElementName}");
                }
                _rootElement = target ?? _rootElement;
            }

            if (_hideOnAwake)
            {
                Hide();
            }

            SetVisualElements();
            RegisterButtonCallbacks();
        }

        //UXMLで定義された特定のボタン、ラベル、テキストフィールドなどの名前付きの VisualElement を、C#のフィールドに取得（クエリ）するためのロジックを記述します。
        protected virtual void SetVisualElements()
        {

        }

        //SetVisualElements で取得したボタン類に対して、クリックされたときの処理（イベントハンドラー）を登録（例: button.clicked += MyMethod;）するためのロジックを記述します。
        protected virtual void RegisterButtonCallbacks()
        {

        }

        //UI表示
        public virtual void Show()
        {
            _rootElement.style.display = DisplayStyle.Flex;
        }

        //UI非表示
        public virtual void Hide()
        {
            _rootElement.style.display = DisplayStyle.None;
        }

        //登録したイベント解除
        public virtual void Dispose()
        {

        }
    }
}
