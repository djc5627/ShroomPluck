using System;
using MyAssets.ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    public bool usingMouse { get; private set; }

    #region Delegates

    public delegate void _OnControlsChanged(PlayerInput inputs);
    public delegate void _OnModifier_Pressed();
    public delegate void _OnModifier_Released();
    public delegate void _OnSlingshot();
    public delegate void _OnDash();
    public delegate void _OnDownwardAttack();
    public delegate void _OnLockOn();
    public delegate void _OnCycleTargetRight();
    public delegate void _OnCycleTargetLeft();
    public delegate void _OnCycleTarget_Pressed(Vector2 input);
    public delegate void _OnSave();
    public delegate void _OnLoad();
    public delegate void _OnJump_Pressed();
    public delegate void _OnJump_Released();
    public delegate void _OnInteract();
    public delegate void _OnPauseGame();
    public delegate void _OnBackspace();
    public delegate void _OnFrameForward();
    public delegate void _OnManualUpdate();
    public delegate void _OnReduceTimeScale();
    public delegate void _OnResetTimeScale();
    public delegate void _OnIncreaseTimeScale();
    public delegate void _OnEnableDebug();
    public delegate void _OnFunction();
    public delegate void _OnRestartScene();
    public delegate void _OnStart();
    public delegate void _OnGlide();
    public delegate void _OnCancel();
    public delegate void _OnNumpad1();
    public delegate void _OnNumpad2();
    public delegate void _OnNumpad3();
    public delegate void _OnNumpad4();
    public delegate void _OnNumpad5();
    public delegate void _OnNumpad6();
    public delegate void _OnNumpad7();
    public delegate void _OnNumpad8();
    public delegate void _OnNumpad9();

    #endregion

    #region Events

    public event _OnControlsChanged onControlsChanged;
    public event _OnModifier_Pressed onModifier_Pressed;
    public event _OnModifier_Released onModifier_Released;
    public event _OnSlingshot onSlingshot_Pressed;
    public event _OnSlingshot onSlingshot_Released;
    public event _OnDash onDash_Pressed;
    public event _OnDash onDash_Released;
    public event _OnDownwardAttack onDownwardAttack;
    public event _OnLockOn onLockOn;
    public event _OnCycleTargetRight onCycleTargetRight;
    public event _OnCycleTargetLeft onCycleTargetLeft;
    public event _OnCycleTarget_Pressed onCycleTarget_Pressed;
    public event _OnSave onSave;
    public event _OnLoad onLoad;
    public event _OnJump_Pressed onJump_Pressed;
    public event _OnJump_Released onJump_Released;
    public event _OnInteract onInteract;
    public event _OnPauseGame onPauseGame;
    public event _OnBackspace onBackspace;
    public event _OnFrameForward onFrameForward;
    public event _OnManualUpdate onManualUpdate;
    public event _OnReduceTimeScale onReduceTimeScale;
    public event _OnResetTimeScale onResetTimeScale;
    public event _OnIncreaseTimeScale onIncreaseTimeScale;
    public event _OnEnableDebug onEnableDebug;
    public event _OnFunction onFunction1;
    public event _OnFunction onFunction2;
    public event _OnFunction onFunction3;
    public event _OnFunction onFunction4;
    public event _OnFunction onFunction5;
    public event _OnFunction onFunction6;
    public event _OnFunction onFunction7;
    public event _OnFunction onFunction8;
    public event _OnFunction onFunction9;
    public event _OnFunction onFunction10;
    public event _OnFunction onFunction11;
    public event _OnFunction onFunction12;
    public event _OnRestartScene onRestartScene;
    public event _OnStart onStart;
    public event _OnGlide onGlide;
    public event _OnCancel onCancel;
    public event _OnNumpad1 onNumpad1;
    public event _OnNumpad2 onNumpad2;
    public event _OnNumpad3 onNumpad3;
    public event _OnNumpad4 onNumpad4;
    public event _OnNumpad5 onNumpad5;
    public event _OnNumpad6 onNumpad6;
    public event _OnNumpad7 onNumpad7;
    public event _OnNumpad8 onNumpad8;
    public event _OnNumpad9 onNumpad9;

    #endregion

    #region Output Events

    [SerializeField] private GameEvent OnSelectPressed;
    [SerializeField] private GameEvent OnSelectReleased;

    #endregion

    private PlayerInput _inputs;
    private InputActionMap playerActions, uiActions;

    private InputAction playerMove,
        playerLook,
        playerJump,
        mousePosition,
        playerSelect;

    public static bool PlatformInvertsScroll()
    {
        Debug.Log($"Platform: {Application.platform}, OS: {SystemInfo.operatingSystem}");
        // bool isWebGl = Application.platform == RuntimePlatform.WebGLPlayer;
        // bool isWebGlOnUnix = isWebGl && (new[] {"mac", "linux"}).Any(str => SystemInfo.operatingSystem.ToLower().Contains(str));
        bool isUnix = Application.platform == RuntimePlatform.LinuxPlayer ||
                      Application.platform == RuntimePlatform.LinuxEditor ||
                      Application.platform == RuntimePlatform.OSXPlayer ||
                      Application.platform == RuntimePlatform.OSXEditor;
        return isUnix;
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        _inputs = GetComponent<PlayerInput>();
        playerActions = _inputs.actions.FindActionMap("Player", true);
        uiActions = _inputs.actions.FindActionMap("UI", true);
        playerMove = playerActions.FindAction("Move");
        playerLook = playerActions.FindAction("Look");
        playerJump = playerActions.FindAction("Jump");
        playerSelect = playerActions.FindAction("Select");
        mousePosition = uiActions.FindAction("MousePosition");
        playerActions.Disable();
        
        playerJump.performed += OnJump_Pressed;
        playerJump.canceled += OnJump_Released;

        playerSelect.performed += OnSelect_Pressed;
        playerSelect.canceled += OnSelect_Released;
        
    }

    void Start()
    {
        onControlsChanged?.Invoke(_inputs);
        usingMouse = _inputs.currentControlScheme == "Keyboard&Mouse";
    }

    private void Update()
    {
        #warning Inefficient checking for input; update this later!
        OnControlsChanged(_inputs);
    }

    void OnEnable()
    {
        uiActions.Enable();
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
        uiActions.Disable();
    }

    #region Action Getters

    public InputAction GetPlayerMove_Action()
    {
        return playerMove;
    }

    public InputAction GetPlayerLook_Action()
    {
        return playerLook;
    }
    
    public InputAction GetMousePosition_Action()
    {
        return mousePosition;
    }

    #endregion

    #region Input callbacks

    public void OnControlsChanged(PlayerInput inputs)
    {
        if (onControlsChanged != null) onControlsChanged(inputs);
        usingMouse = inputs.currentControlScheme == "Keyboard&Mouse";
    }
    
    public void OnSlingshotPressed(InputAction.CallbackContext ctx)
    {
        if (onSlingshot_Pressed != null) onSlingshot_Pressed();
    }
    
    public void OnSlingshotReleased(InputAction.CallbackContext ctx)
    {
        if (onSlingshot_Released != null) onSlingshot_Released();
    }
    
    public void OnDash_Pressed(InputAction.CallbackContext ctx)
    {
        if (onDash_Pressed != null) onDash_Pressed();
    }
    
    public void OnDash_Released(InputAction.CallbackContext ctx)
    {
        if (onDash_Released != null) onDash_Released();
    }
    
    public void OnSelect_Pressed(InputAction.CallbackContext ctx)
    {
        OnSelectPressed?.Raise();
    }
    
    public void OnSelect_Released(InputAction.CallbackContext ctx)
    {
        OnSelectReleased?.Raise();
    }

    public void OnLockOn()
    {
        if (onLockOn != null) onLockOn();
    }

    public void OnSave()
    {
        if (onSave != null) onSave();
    }
    
    public void OnLoad()
    {
        if (onLoad != null) onLoad();
    }
    
    public void OnJump_Pressed(InputAction.CallbackContext ctx)
    {
        if (onJump_Pressed != null) onJump_Pressed();
    }
    
    public void OnJump_Released(InputAction.CallbackContext ctx)
    {
        if (onJump_Released != null) onJump_Released();
    }
    
    public void OnInteract()
    {
        if (onInteract != null) onInteract();
    }

    public void OnPauseGame()
    {
        if (onPauseGame != null) onPauseGame();
    }

    public void OnBackspace()
    {
        if (onBackspace != null) onBackspace();
    }
    
    public void OnFrameForward()
    {
        if (onFrameForward != null) onFrameForward();
    }
    
    public void OnManualUpdate()
    {
        if (onManualUpdate != null) onManualUpdate();
    }
    
    public void OnReduceTimeScale()
    {
        if (onReduceTimeScale != null) onReduceTimeScale();
    }
    
    public void OnResetTimeScale()
    {
        if (onResetTimeScale != null) onResetTimeScale();
    }
    
    public void OnIncreaseTimeScale()
    {
        if (onIncreaseTimeScale != null) onIncreaseTimeScale();
    }
    
    public void OnEnableDebug()
    {
        if (onEnableDebug != null) onEnableDebug();
    }
    
    public void OnFunction1()
    {
        if (onFunction1 != null) onFunction1();
    }
    
    public void OnFunction2()
    {
        if (onFunction2 != null) onFunction2();
    }
    
    public void OnFunction3()
    {
        if (onFunction3 != null) onFunction3();
    }
    
    public void OnFunction4()
    {
        if (onFunction4 != null) onFunction4();
    }
    
    public void OnFunction5()
    {
        if (onFunction5 != null) onFunction5();
    }
    
    public void OnFunction6()
    {
        if (onFunction6 != null) onFunction6();
    }
    
    public void OnFunction7()
    {
        if (onFunction7 != null) onFunction7();
    }
    
    public void OnFunction8()
    {
        if (onFunction8 != null) onFunction8();
    }
    
    public void OnFunction9()
    {
        if (onFunction9 != null) onFunction9();
    }
    
    public void OnFunction10()
    {
        if (onFunction10 != null) onFunction10();
    }
    
    public void OnFunction11()
    {
        if (onFunction11 != null) onFunction11();
    }
    
    public void OnFunction12()
    {
        if (onFunction12 != null) onFunction12();
    }
    
    public void OnRestartScene()
    {
        if (onRestartScene != null) onRestartScene();
    }
    
    public void OnStart()
    {
        if (onStart != null) onStart();
    }

    public void OnNumpad1()
    {
        if (onNumpad1 != null) onNumpad1();
    }
    
    public void OnNumpad2()
    {
        if (onNumpad2 != null) onNumpad2();
    }
    
    public void OnNumpad3()
    {
        if (onNumpad3 != null) onNumpad3();
    }
    
    public void OnNumpad4()
    {
        if (onNumpad4 != null) onNumpad4();
    }
    
    public void OnNumpad5()
    {
        if (onNumpad5 != null) onNumpad5();
    }
    
    public void OnNumpad6()
    {
        if (onNumpad6 != null) onNumpad6();
    }
    
    public void OnNumpad7()
    {
        if (onNumpad7 != null) onNumpad7();
    }
    
    public void OnNumpad8()
    {
        if (onNumpad8 != null) onNumpad8();
    }
    public void OnNumpad9()
    {
        if (onNumpad9 != null) onNumpad9();
    }

    #endregion
    

    public void EnablePlayerInputActions(bool isEnabled)
    {
        if (isEnabled) playerActions.Enable();
        else playerActions.Disable();
    }

}
