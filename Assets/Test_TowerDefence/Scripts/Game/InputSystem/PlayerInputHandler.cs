using System;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance { get; private set; }

    private PlayerControls playerControls;

    [SerializeField] private PointerOverUI pointerOverUI;

    public event Action<Vector2> TouchPressed;
    public event Action<Vector2> CameraDragingStarted;
    public event Action<Vector2> CameraDraging;
    public event Action CameraDragingEnded;
    public Vector2 TouchPosition { get; private set; }
    private bool isDraging;

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 120;

        playerControls = new PlayerControls(); 
    }

    private void RegisterInputActions()
    {
        playerControls.Player.TouchPosition.performed += ctx => { TouchPositionPrimary(ctx); };


        playerControls.Player.TouchEnded.started += ctx => OnStartTouch(ctx);
        playerControls.Player.TouchEnded.performed += ctx => OnTouchPerformed(ctx);
        playerControls.Player.TouchEnded.canceled += ctx => OnTouchCanceled(ctx);



        playerControls.Player.CameraDrag.performed += ctx => { OnDrag(ctx.ReadValue<Vector2>()); };
        
    }

    private void OnTouchPerformed(InputAction.CallbackContext ctx)
    {
        isDraging = true;
    }

    private void OnTouchCanceled(InputAction.CallbackContext ctx)
    {
        isDraging = false;
        CameraDragingEnded?.Invoke();
    }

    private void OnStartTouch(InputAction.CallbackContext ctx)
    {
        CameraDragingStarted?.Invoke(TouchPosition);
    }

    private void OnEndDrag()
    {
        CameraDragingEnded?.Invoke();
    }

    private void OnStartDrag(Vector2 fingerPosition)
    {
        CameraDragingStarted?.Invoke(TouchPosition);    
    }

    private void OnDrag(Vector2 fingerPosition)
    {
        if (isDraging)
        {
            CameraDraging?.Invoke(fingerPosition);
        }
    }

    private void TouchPositionPrimary(InputAction.CallbackContext ctx)
    {
        TouchPosition = ctx.ReadValue<Vector2>();

        if (!PointerOverUI.Instance.IsPointerOverUIObject(ctx.ReadValue<Vector2>()) && !isDraging)
        {
            TouchPressed?.Invoke(TouchPosition);
        }    
    }

    private void OnEnable()
    {
        playerControls.Enable();

        RegisterInputActions();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
