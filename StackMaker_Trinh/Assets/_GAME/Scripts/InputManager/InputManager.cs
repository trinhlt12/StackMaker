using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public bool CanAcceptInput { get; private set; } = true;

    public SwipeDirection CurrentSwipeDirection
    {
        get
        {
            if (!CanAcceptInput)
            {
                return SwipeDirection.None;
            }
            return _currentSwipeDirection;
        }
        private set => this._currentSwipeDirection = value;
    }

    private PlayerInputActions _inputActions;
    private SwipeDirection     _currentSwipeDirection;

    private       Vector2 _swipeEnd;
    private       Vector2 _swipeStart;
    private const float   swipeThreshold = 50f; //pixels

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        this._inputActions.Player.Touch.performed    += ctx => this._swipeEnd   = ctx.ReadValue<Vector2>();
        this._inputActions.Player.TouchPress.started += ctx => this._swipeStart = this._inputActions.Player.Touch.ReadValue<Vector2>();
        this._inputActions.Player.TouchPress.canceled += ctx =>
        {
            Vector2 delta = this._swipeEnd - this._swipeStart;
            DetectSwipeDirection(delta);
        };
    }

    private void DetectSwipeDirection(Vector2 delta)
    {
        if (delta.magnitude < swipeThreshold)
        {
            CurrentSwipeDirection = SwipeDirection.None;
            return;
        }

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            CurrentSwipeDirection = delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            CurrentSwipeDirection = delta.y > 0 ? SwipeDirection.Forward : SwipeDirection.Backward;
        }

        Debug.Log("Swipe Detected: " + CurrentSwipeDirection);
    }

    public void ResetSwipeDirection()
    {
        CurrentSwipeDirection = SwipeDirection.None;
    }

    public void SetCanAcceptInput(bool value)
    {
        CanAcceptInput = value;
        if (!value) ResetSwipeDirection();
    }
}

public enum SwipeDirection
{
    None     = 0,
    Left     = 1,
    Right    = 2,
    Forward  = 3,
    Backward = 4
}