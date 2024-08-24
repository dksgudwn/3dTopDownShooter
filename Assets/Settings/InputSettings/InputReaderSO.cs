using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReaderSO : ScriptableObject, Controls.IPlayerActions, IPlayerComponent
{
    public Vector2 Movement { get; private set; }
    public Vector2 MousePosition { get; private set; }

    [SerializeField] private LayerMask _whatIsGround, _whatIsEnemy;
    private Vector3 _beforeMouseWorldPosition;

    public event Action<bool> RunEvent;
    public event Action<bool> FireEvent;
    public event Action<int> ChangeWeaponSlotEvent;

    private Controls _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Enable();
            _controls.Player.SetCallbacks(this);
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }

    public Vector3 GetWorldMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _whatIsGround))
        {
            _beforeMouseWorldPosition = hitInfo.point;
        }
        return _beforeMouseWorldPosition;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _whatIsEnemy))
        {
            return hitInfo;
        }
        return default;
    }

    public void Initialize(Player player)
    {
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
            RunEvent?.Invoke(true);
        else if (context.canceled)
            RunEvent?.Invoke(false);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            FireEvent?.Invoke(true);
        else if (context.canceled)
            FireEvent?.Invoke(false);
    }

    public void OnEquitSlot1(InputAction.CallbackContext context)
    {
        if (context.performed)
            ChangeWeaponSlotEvent?.Invoke(0);
    }

    public void OnEquitSlot2(InputAction.CallbackContext context)
    {
        if (context.performed)
            ChangeWeaponSlotEvent?.Invoke(1);
    }

    public void OnEquitSlot3(InputAction.CallbackContext context)
    {
        if (context.performed)
            ChangeWeaponSlotEvent?.Invoke(2);
    }
}
