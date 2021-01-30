using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonBehaviour<InputManager>
{
    private PlayerInput playerInput;
    //Current and Previous ActionMaps
    [SerializeField]
    private string _currentInputActionMap = "Player";
    [SerializeField]
    private string _previousInputActionMap;
    [SerializeField]

    //Current and Previous Cursor Lock States
    private CursorLockMode _currentCursorLockState;
    [SerializeField]
    private CursorLockMode _previousCursorLockState;

    //Player ActionMap
    [SerializeField]
    private Vector2 _moveInput;
    [SerializeField]
    private bool _interactInput;

    //Debug ActionMap
    [SerializeField]
    private bool _toggleConsoleInput;
    [SerializeField]
    private bool _returnInput;
    [SerializeField]
    private bool _previousCommandInput;
    [SerializeField]
    private bool _nextCommandInput;

    //Current and Previous ActionMaps
    public string CurrentInputActionMap { get => _currentInputActionMap; }
    public string PreviousInputActionMap { get => _previousInputActionMap; }

    //Current and Previous Cursor Lock States
    public CursorLockMode CurrentCursorLockState { get => _currentCursorLockState; }
    public CursorLockMode PreviousCursorLockState { get => _previousCursorLockState; }

    //Player ActionMap
    public Vector2 MoveInput { get => _moveInput; }
    public bool InteractInput { get => _interactInput; }

    //Debug ActionMap
    public bool ToggleConsoleInput { get => _toggleConsoleInput; }
    public bool ReturnInput { get => _returnInput; }
    public bool PreviousCommandInput { get => _previousCommandInput; }
    public bool NextCommandInput { get => _nextCommandInput; }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.onActionTriggered += OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                OnMove(context);
                break;
            case "Interact":
                OnInteract(context);
                break;
            case "ToggleConsole":
                OnToggleConsole(context);
                break;
            case "Return":
                OnReturn(context);
                break;
            case "NextCommand":
                OnNextCommand(context);
                break;
            case "PreviousCommand":
                OnPreviousCommand(context);
                break;
        }
    }

    public void SwitchInputMap(string actionMapName)
    {
        _previousInputActionMap = _currentInputActionMap;
        _currentInputActionMap = actionMapName;

        playerInput.SwitchCurrentActionMap(_currentInputActionMap);
    }
    public void SwitchCursorLockState(CursorLockMode cursorLockState)
    {
        _previousCursorLockState = _currentCursorLockState;
        _currentCursorLockState = cursorLockState;

        Cursor.lockState = _currentCursorLockState;
    }

    //Player ActionMap
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _interactInput = true;
        }
        else if (context.canceled)
        {
            _interactInput = false;
        }
    }

    //Debug ActionMap
    public void OnToggleConsole(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _toggleConsoleInput = true;
        }
        else if (context.canceled)
        {
            _toggleConsoleInput = false;
        }
    }
    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _returnInput = true;
        }
        else if (context.canceled)
        {
            _returnInput = false;
        }
    }
    public void OnPreviousCommand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _previousCommandInput = true;
        }
        else if (context.canceled)
        {
            _previousCommandInput = false;
        }
    }
    public void OnNextCommand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _nextCommandInput = true;
        }
        else if (context.canceled)
        {
            _nextCommandInput = false;
        }
    }
}