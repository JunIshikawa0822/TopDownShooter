using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class InteractView : AUIView
    {
        private VisualTreeAsset _buttonTemplate;
        private TemplateContainer _buttonInstance;
        private VisualElement _buttonParent;

        public override void Initialize()
        {
            base.Initialize();
            CreateButton();
        }
        public void CreateButton()
        {
            _buttonInstance = _buttonTemplate.Instantiate();
            _buttonInstance.style.position = Position.Absolute;
            _buttonInstance.style.display = DisplayStyle.None;
            _buttonParent.Add(_buttonInstance);
        }
        protected override void SetVisualElements()
        {
            _buttonTemplate = Resources.Load<VisualTreeAsset>("InteractIcon");

            _buttonParent = _rootElement.Q<VisualElement>("interacticon__parent");
        }

        protected override void RegisterButtonCallbacks()
        {
        }

        public void DisplayButton(bool isDisplay)
        {
            if(isDisplay) _buttonInstance.style.display = DisplayStyle.Flex;
            else _buttonInstance.style.display = DisplayStyle.None;
        }

        public void UpdateButtonPosition(IInteractable target, Camera camera)
        {
            Vector2 panelPos = RuntimePanelUtils.CameraTransformWorldToPanel(
                _buttonInstance.panel,
                target.WorldPosition, 
                camera
            );

            // _buttonInstance.style.left = panelPos.x - (_buttonInstance.layout.width / 2);
            // _buttonInstance.style.top = panelPos.y - (_buttonInstance.layout.height / 2);

            // left / top は 0 で固定（Absoluteの起点）
            _buttonInstance.style.left = 0;
            _buttonInstance.style.top = 0;

            // 最新の書き方：座標の指定と中心補正を translate で一括で行う
            // panelPos.x, panelPos.y で指定の位置へ動かし、
            // Length.Percent(-50) で自身のサイズの半分だけ戻す（中心合わせ）
            _buttonInstance.style.translate = new Translate(
                new Length(panelPos.x), 
                new Length(panelPos.y), 
                0
            );

            // カメラ背面チェック
            Vector3 viewPos = camera.WorldToViewportPoint(target.WorldPosition);
            if (viewPos.z < 0) _buttonInstance.style.display = DisplayStyle.None;
        }
    }
}