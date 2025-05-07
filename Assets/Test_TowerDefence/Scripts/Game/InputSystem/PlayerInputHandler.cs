using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;



public class PlayerInputHandler : MonoBehaviour, IService
{
    public event Action<Vector2> TouchPressed;
    public event Action<Vector2> CameraDragingStarted;
    public event Action<Vector2> CameraDraging;
    public event Action CameraDragingEnded;
    public event Action<Vector2> TouchedGround;

    public Vector2 TouchPosition { get; private set; }

    private PlayerControls playerControls;
    private bool isDraging;

    private void Awake()
    {
        playerControls = new PlayerControls(); 
    }

    private void OnEnable()
    {
        playerControls.Enable();

        RegisterInputActions();
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
        if (!isDraging)
        {
            CameraDragingStarted?.Invoke(TouchPosition);
        }  
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

        if (!IsPointerOverUIObject(ctx.ReadValue<Vector2>()) && !isDraging)
        {
            TouchPressed?.Invoke(TouchPosition);
        }
        else if(IsPointerOverUIObject(ctx.ReadValue<Vector2>()) && !isDraging)
        {
            TouchedGround?.Invoke(TouchPosition);
        }    
    }

    public bool IsPointerOverUIObject(Vector2 touchPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touchPosition.x, touchPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == 5)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
