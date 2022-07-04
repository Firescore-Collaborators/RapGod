using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TouchInputType
{
    swipeRight,
    swipeLeft,
    swipeUp,
    swipeDown,
}
public class TouchInputs : MonoBehaviour
{
    public event System.Action swipeRight;
    public event System.Action swipeLeft;
    public event System.Action swipeUp;
    public event System.Action swipeDown;
    [SerializeField] Vector3 startMousePos;
    [SerializeField] Vector3 currentMousePos;
    bool onHeld;
    public float swipeThreshold = 100f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onHeld = true;
            startMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            onHeld = false;
            startMousePos = Vector3.zero;
        }
        currentMousePos = Input.mousePosition;

        CheckSwipeRight();
        CheckSwipeLeft();
        CheckSwipeUp();
        CheckSwipeDown();
    }

    void CheckSwipeRight()
    {
        if (!onHeld) return;
        if (currentMousePos.x - startMousePos.x > swipeThreshold)
        {
            onHeld = false;
            swipeRight?.Invoke();
        }
    }

    void CheckSwipeLeft()
    {
        if (!onHeld) return;
        if (currentMousePos.x - startMousePos.x < -swipeThreshold)
        {
            onHeld = false;
            swipeLeft?.Invoke();
        }
    }

    void CheckSwipeUp()
    {
        if (!onHeld) return;
        if (currentMousePos.y - startMousePos.y > swipeThreshold)
        {
            onHeld = false;
            swipeUp?.Invoke();
        }
    }

    void CheckSwipeDown()
    {
        if (!onHeld) return;
        if (currentMousePos.y - startMousePos.y < -swipeThreshold)
        {
            onHeld = false;
            swipeDown?.Invoke();
        }
    }
}
