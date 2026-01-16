using Game.UI;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractController : AUIController
{
    //依存
    private InteractView _interactView;

    public void InitializeModel(Inventory model)
    {
        
    }

    public void InitializeView(InteractView view)
    {
        _interactView = view;
    }
}
