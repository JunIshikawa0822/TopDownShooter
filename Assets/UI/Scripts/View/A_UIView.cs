using UnityEngine.UIElements;
using System;

namespace Game.UI
{
    public abstract class AUIView : IDisposable
    {
        //このUIが初期化（Initialize）されたときに、すぐに非表示にするかどうかの設定です。
        protected bool _hideOnAwake = false;

        //このUIが部分的に透けて背後のUIを見せる「オーバーレイ」として機能するかどうかを示すフラグです。
        protected bool _isOverlay;

        //このクラスが管理するUIコンポーネントのルート要素（一番上の VisualElement）を保持します。UXMLファイルで定義されたUI階層全体への入り口となります。
        protected VisualElement _topElement;

        // Properties
        public VisualElement Root => _topElement;
        public bool IsTransparent => _isOverlay;
        public bool IsHidden => _topElement.style.display == DisplayStyle.None;

        //コンストラクタで、UIのルート要素である VisualElement を受け取ります
        public AUIView(VisualElement topElement)
        {
            _topElement = topElement ?? throw new ArgumentNullException(nameof(topElement));
        }

        public virtual void Initialize()
        {
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
            _topElement.style.display = DisplayStyle.Flex;
        }

        //UI非表示
        public virtual void Hide()
        {
            _topElement.style.display = DisplayStyle.None;
        }

        //登録したイベント解除
        public virtual void Dispose()
        {

        }
    }
}
