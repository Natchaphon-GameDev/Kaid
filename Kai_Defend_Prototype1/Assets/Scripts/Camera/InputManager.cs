using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; } 

    //event
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;

    private Camera mainCamara;
    
    private MobileInput playerControls;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        mainCamara = Camera.main;
        playerControls = new MobileInput();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        playerControls.Touch.PrimaryContect.started += context => StartTouchPrimary(context);
        playerControls.Touch.PrimaryContect.canceled += context => EndTouchPrimary(context);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
        {
            OnStartTouch(Utils.ScreenToWorld(mainCamara,playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()),(float)context.startTime);
        }
    }
    
    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
        {
            OnEndTouch(Utils.ScreenToWorld(mainCamara,playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()),(float)context.time);
        }
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamara, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}
