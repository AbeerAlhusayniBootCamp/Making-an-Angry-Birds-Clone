
using System;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    #region Singletion
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
    private TouchControll touchControlls;
    private void Awake()
    {
        touchControlls = new TouchControll();
    }
    private void OnEnable()
    {
        touchControlls.Enable();
    }
    private void OnDisable()
    {
        touchControlls.Disable();
    }
    #endregion
    private void Start()
    {
        touchControlls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchControlls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void EndTouch(InputAction.CallbackContext ctx)
    {
        throw new NotImplementedException();
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        if (OnStartTouch != null) OnStartTouch(touchControlls.Touch.TouchPosition.ReadValue<Vector2>(),(float)ctx.startTime);
        if (OnEndTouch != null) OnEndTouch(touchControlls.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.time);
    }
}
