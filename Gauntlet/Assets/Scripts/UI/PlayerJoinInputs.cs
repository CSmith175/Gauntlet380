using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerJoinInputs: MonoBehaviour
{
    //attatched to player UI on the join screen

    private PlayerActionMap _actionmap;

    public bool _currentFirstSlot = false;
    public int _selectControllerID = 0;

    public Action<int> OnSelectScroll;

    #region "Unity Functions"
    public void OnEnable()
    {
        if(_actionmap == null)
        {
            _actionmap = new PlayerActionMap();
        }

        _actionmap.Enable();
    }
    public void OnDisable()
    {
        if(_actionmap != null)
        {
            _actionmap.Disable();
        }
    }
    public void Start()
    {
        if (_actionmap != null)
        {
            _actionmap.PlayerMovement.ControllerButtonsClassScroll.performed += context => ClassScroll(context);
            _actionmap.PlayerMovement.ControllerButtonsClassSelect.performed += context => ClassSelect(context);
        }
    }
    #endregion

    public void RefreshControllerBinding()
    {
        _currentFirstSlot = false;
        _selectControllerID = 0;
    }

    private void ClassScroll(InputAction.CallbackContext context)
    {
        if (!ControllerManager.CheckControllerActive(context.control.device.deviceId))
        {
            if(_selectControllerID == 0 || _selectControllerID == context.control.device.deviceId)
            {
                if (context.ReadValue<float>() < 0)
                {
                    OnSelectScroll?.Invoke(-1);
                }
                else if (context.ReadValue<float>() > 0)
                {
                    OnSelectScroll?.Invoke(1);
                }
            }

        }
    }
    private void ClassSelect(InputAction.CallbackContext context)
    {
        if (!ControllerManager.CheckControllerActive(context.control.device.deviceId))
        {
            Debug.Log("Class Select");
        }
    }

}
