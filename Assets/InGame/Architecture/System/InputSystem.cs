using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : ASystem, IOnPreUpdate
{
    private InputSystem_Actions _gameInputs;
    private Vector2 _screenPosition;
    private float _assistRadius;
    private Transform _baseTrans;
    private LayerMask _targetLayerMask;
    private LayerMask _obstacleLayerMask;
    private LayerMask _combinedLayerMask;
    public override void OnSetUp()
    {
        _gameInputs = new InputSystem_Actions();

        _gameInputs.Player.Move.started += OnMoveInput;
        _gameInputs.Player.Move.performed += OnMoveInput;
        _gameInputs.Player.Move.canceled += OnMoveInput;

        _gameInputs.Player.Attack.started += OnAttackStartInput;
        _gameInputs.Player.Attack.performed += OnAttackProcessInput;
        _gameInputs.Player.Attack.canceled += OnAttackEndInput;

        _gameInputs.Enable();

        _targetLayerMask = gameStat.targetLayerMask;
        _obstacleLayerMask = gameStat.obstacleLayerMask;
        _combinedLayerMask = _targetLayerMask | _obstacleLayerMask;
    }

    public void OnPreUpdate()
    {
        _baseTrans = gameStat.player.AttackBaseTrans;
        gameStat.screenPosition = _screenPosition = _gameInputs.UI.Point.ReadValue<Vector2>();
        gameStat.worldPosition = GetCursorPos();
        gameStat.cursorTrans.transform.position = gameStat.worldPosition;
    }

    private Vector3 GetCursorPos()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(_screenPosition);
        RaycastHit hit;
        Vector3 basePos = _baseTrans.position;

        bool initialHitFound;
        if (gameStat.isCursorAssist)
        {
            // アシストあり: 太い判定で最初に当たったものを取得
            initialHitFound = Physics.SphereCast(mouseRay, _assistRadius, out hit, float.MaxValue, _combinedLayerMask);
        }
        else
        {
            // アシストなし: 正確なRayで最初に当たったものを取得
            initialHitFound = Physics.Raycast(mouseRay, out hit, float.MaxValue, _combinedLayerMask);
        }

        //壁や敵、対象がある
        if (initialHitFound)
        {
            return hit.point;
        }
        else
        {
            // 銃口の高さの仮想平面で交差点を計算
            Plane aimPlane = new Plane(Vector3.up, new Vector3(0, basePos.y, 0));
            float enter;

            if (aimPlane.Raycast(mouseRay, out enter))
            {
                // 水平維持: カーソルRayが平面と交差した点を返す
                // カーソル Ray の壁チェックは既に行われているため、ここでは単純に平面上の点を返す
                return mouseRay.GetPoint(enter);
            }
        }

            // 最終フォールバック
            return basePos + _baseTrans.forward * 50;
        //}
    }

    /// <summary>
/// WASDのVector2入力を、カメラの向きに基づいたワールド空間のVector3移動ベクトルに変換する。
/// </summary>
/// <param name="inputDirection">WASDから得られた入力Vector2 (x: 左右, y: 前後)。</param>
/// <returns>XZ平面上のワールド空間の移動ベクトルVector3。</returns>
    public Vector3 GetCameraSpaceMovementVector(Vector2 inputDirection, Camera camera)
    {
        Transform cameraTransform = camera.transform;

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Y成分をゼロにして、水平方向のベクトルにする
        cameraForward = cameraForward.normalized;

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0; // Y成分をゼロにして、水平方向のベクトルにする
        cameraRight = cameraRight.normalized;

        // 3. 入力とカメラの方向を合成して、ワールド空間の移動ベクトルを決定
        // inputDirection.y (W/S) * cameraForward
        // inputDirection.x (A/D) * cameraRight
        Vector3 worldMovementVector = (cameraForward * inputDirection.y) + (cameraRight * inputDirection.x);

        // 4. ベクトルを正規化し、斜め移動時の速度超過を防ぐ
        if (worldMovementVector.sqrMagnitude > 1f)
        {
            worldMovementVector = worldMovementVector.normalized;
        }

        return worldMovementVector;
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        gameStat.moveDirection = GetCameraSpaceMovementVector(direction, gameStat.mainCamera);
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {

    }

    private void OnAttackStartInput(InputAction.CallbackContext context)
    {
        gameEvent.attackStartEvent?.Invoke();
        gameStat.isPressProcessing = true;
    }

    private void OnAttackProcessInput(InputAction.CallbackContext context)
    {
        gameEvent.attackProcessEvent?.Invoke();
    }

    private void OnAttackEndInput(InputAction.CallbackContext context)
    {
        gameEvent.attackEndEvent?.Invoke();
        gameStat.isPressProcessing = false;
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

