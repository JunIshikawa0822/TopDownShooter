using UnityEngine;
using UnityEngine.UIElements;

public class GameUIRoot : MonoBehaviour
{
    private UIEvents _uiEvents;
    private UIService _uiService;

    private UIDocument _uiDocument;

    void Awake()
    {
        _uiEvents = new UIEvents();
        _uiDocument = GetComponent<UIDocument>();
        _uiService = new UIService(_uiDocument.rootVisualElement);
        _uiService.Init();
    }
}

