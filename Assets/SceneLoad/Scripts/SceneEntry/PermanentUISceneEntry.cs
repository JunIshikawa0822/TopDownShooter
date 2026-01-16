using UnityEngine;
using Game.UI;

public class PermanentUISceneEntry : ASceneEntryPointBase
{
    [SerializeField] private InteractView _interactView;
    protected override void BuildDependencies()
    {
        RegisterDependency<InteractView>(_interactView);
    }

    //Awakeのタイミング
    protected override void OnSetUp()
    {
        base.OnSetUp();

        _interactView.Initialize();
    }
}