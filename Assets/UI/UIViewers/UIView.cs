using UnityEngine.UIElements;
using System;

namespace Game.UI
{
    public class UIView : IDisposable
    {
        //このUIが初期化（Initialize）されたときに、すぐに非表示にするかどうかの設定です。
        protected bool m_HideOnAwake = true;

        //このUIが部分的に透けて背後のUIを見せる「オーバーレイ」として機能するかどうかを示すフラグです。
        protected bool m_IsOverlay;

        //このクラスが管理するUIコンポーネントのルート要素（一番上の VisualElement）を保持します。UXMLファイルで定義されたUI階層全体への入り口となります。
        protected VisualElement m_TopElement;

        // Properties
        public VisualElement Root => m_TopElement;
        public bool IsTransparent => m_IsOverlay;
        public bool IsHidden => m_TopElement.style.display == DisplayStyle.None;

        //コンストラクタで、UIのルート要素である VisualElement を受け取ります
        public UIView(VisualElement topElement)
        {
            m_TopElement = topElement ?? throw new ArgumentNullException(nameof(topElement));
            Initialize();
        }

        public virtual void Initialize()
        {
            if (m_HideOnAwake)
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
            m_TopElement.style.display = DisplayStyle.Flex;
        }

        //UI非表示
        public virtual void Hide()
        {
            m_TopElement.style.display = DisplayStyle.None;
        }

        //登録したイベント解除
        public virtual void Dispose()
        {

        }
    }
}
