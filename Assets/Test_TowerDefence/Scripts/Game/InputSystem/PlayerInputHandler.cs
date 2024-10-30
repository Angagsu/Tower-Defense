using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance { get; private set; }

    
    public int TapInput { get; private set; }
    public float SwipeInput { get; private set; }

    [SerializeField] private InputActionAsset playerControls;

    private const string actionMapName = "Player";
    private const string move = "Move";
    private const string tap = "Tap";
    private const string swipe = "Swipe";

    private InputAction tapAction;
    private InputAction swipeAction;


    private void Awake()
    {
        Instance = this;

        tapAction = playerControls.FindActionMap(actionMapName).FindAction(tap);
        swipeAction = playerControls.FindActionMap(actionMapName).FindAction(swipe);

        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        tapAction.performed += context => TapInput = context.ReadValue<int>();
        tapAction.canceled += context => TapInput = 0;

        swipeAction.performed += context => SwipeInput = context.ReadValue<float>();
        swipeAction.canceled += context => SwipeInput = 0f;
    }

    private void OnEnable()
    {
        tapAction.Enable();
        swipeAction.Enable();
    }

    private void OnDisable()
    {
        tapAction.Disable();
        swipeAction.Disable();
    }
}
