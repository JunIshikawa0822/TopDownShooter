using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : ASystem, IOnPreUpdate
{
    private InputSystem_Actions _gameInputs;
    public override void OnSetUp()
    {
        _gameInputs = new InputSystem_Actions();

        _gameInputs.Player.Move.started += OnMoveInput;
        _gameInputs.Player.Move.performed += OnMoveInput;
        _gameInputs.Player.Move.canceled += OnMoveInput;

        _gameInputs.Enable();
    }

    public void OnPreUpdate()
    {

    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        gameStat.moveDirection = context.ReadValue<Vector2>();
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {

    }

    private void OnAttackInput(InputAction.CallbackContext context)
    {

    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {

    }

    private void OnCrouchInput(InputAction.CallbackContext context)
    {

    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {

    }
    
    private void OnSprintInput(InputAction.CallbackContext context)
    {
        
    }
}

