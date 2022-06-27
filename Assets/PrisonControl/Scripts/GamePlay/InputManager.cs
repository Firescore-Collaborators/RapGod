using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static Action<Vector2> MouseDragStarted = delegate { };
    public static Action<Vector2> MouseDragged = delegate { };
    public static Action<Vector2> MouseDragEnded = delegate { };

    public static InputManager inst;

    private Vector2 _lastMousePos;
    private Vector2 _startMousePos;

    public delegate void OnDrag(Vector2 currentPos);
    public delegate void OnClick(Vector2 startPos);
    public delegate void OnClickEnd(Vector2 endPos);

    public OnDrag OnDragCallback;
    public OnClick OnClickCallback;
    public OnClickEnd OnClickEndCallback;

    public bool IS_READY_TO_MOVE;

    private void Awake()
    {
        #region Singelton
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("More then one InputManager was created, destroying duplicates");
            Destroy(this);
        }
        #endregion

        Input.multiTouchEnabled = false;
    }


    public bool IsMouseOverUI()
    {
        return false;
        return EventSystem.current.IsPointerOverGameObject();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject(-1))
            return;


        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                return;
                // ui touched
            }
        }


        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {

            _lastMousePos = Input.mousePosition;
            _startMousePos = _lastMousePos;

            if (IS_READY_TO_MOVE && OnClickCallback != null)
            {
                OnClickCallback.Invoke(_startMousePos);
            }

            MouseDragStarted.Invoke(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && !IsMouseOverUI())
        {
            if (IS_READY_TO_MOVE && OnClickCallback != null)
            {
                OnDragCallback.Invoke(Input.mousePosition);
            }

            MouseDragged.Invoke((_lastMousePos - new Vector2(Input.mousePosition.x, Input.mousePosition.y)) / Screen.height);

            _lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && !IsMouseOverUI())
        {         

            if (IS_READY_TO_MOVE && OnClickEndCallback != null)
            {
                OnClickEndCallback.Invoke(Input.mousePosition);

            }
            _lastMousePos = Input.mousePosition;

            MouseDragEnded.Invoke(Input.mousePosition);
        }
    }
}
