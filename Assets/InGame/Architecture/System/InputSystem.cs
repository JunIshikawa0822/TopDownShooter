using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : ASystem, IOnPreUpdate
{
    private InputSystem_Actions _gameInputs;
    private Vector2 _screenPosition;
    private float _maxVerticalAngle;
    private float _assistRadius;

    private Transform _baseTrans;
    private LayerMask _targetLayerMask;
    private LayerMask _obstacleLayerMask;
    private LayerMask _groundLayerMask;
    private LayerMask _combinedLayerMask;
    public override void OnSetUp()
    {
        _gameInputs = new InputSystem_Actions();

        _gameInputs.Player.Move.started += OnMoveInput;
        _gameInputs.Player.Move.performed += OnMoveInput;
        _gameInputs.Player.Move.canceled += OnMoveInput;

        _gameInputs.Enable();

        _maxVerticalAngle = gameStat.maxVerticalAngle;

        _targetLayerMask = gameStat.targetLayerMask;
        _obstacleLayerMask = gameStat.obstacleLayerMask;
        _groundLayerMask = gameStat.groundLayerMask;
        _combinedLayerMask = _targetLayerMask | _obstacleLayerMask | _groundLayerMask;
    }

    public void OnPreUpdate()
    {
        _baseTrans = gameStat.baseTrans;
        gameStat.screenPosition = _screenPosition = _gameInputs.UI.Point.ReadValue<Vector2>();
        gameStat.worldPosition = GetCursorPos();
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

        if (!initialHitFound)
        {
            // 最終フォールバック（Rayが何も当たらなかった場合）
            return basePos + _baseTrans.forward * 50;
        }

        Vector3 targetPoint = hit.point;
        Vector3 directionToTarget = (targetPoint - basePos).normalized; 
        
        //【射角チェック】 銃口から見た照準点の角度
        Vector3 flatDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z).normalized;
        float verticalAngle = Vector3.Angle(directionToTarget, flatDirection);

        if (verticalAngle <= _maxVerticalAngle)
        {
            return targetPoint;
        }
        else
        {
            // 射角NGの場合: ターゲットを無視し、水平維持ロジックに進む
            // 銃口の高さの仮想平面で交差点を計算
            Plane aimPlane = new Plane(Vector3.up, new Vector3(0, basePos.y, 0));
            float enter;

            if (aimPlane.Raycast(mouseRay, out enter))
            {
                // 水平維持: カーソルRayが平面と交差した点を返す
                // カーソル Ray の壁チェックは既に行われているため、ここでは単純に平面上の点を返す
                return mouseRay.GetPoint(enter);
            }

            // 最終フォールバック
            return basePos + _baseTrans.forward * 50;
        }
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        gameStat.moveDirection = context.ReadValue<Vector2>();
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {

    }

    private void OnAttackStartInput(InputAction.CallbackContext context)
    {

    }

    private void OnAttackProcessInput(InputAction.CallbackContext context)
    {

    }

    private void OnAttackEndInput(InputAction.CallbackContext context)
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

