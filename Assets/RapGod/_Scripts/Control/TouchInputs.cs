using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TouchInputType
{
    swipeRight,
    swipeLeft,
    swipeUp,
    swipeDown,
    multiTap
}
public class TouchInputs : MonoBehaviour
{
    public event System.Action swipeRight;
    public event System.Action swipeLeft;
    public event System.Action swipeUp;
    public event System.Action swipeDown;
    public event System.Action multiTapOver;
    public event System.Action<float, bool> multiTaping;
    Vector3 startMousePos;
    Vector3 currentMousePos;
    bool onHeld;
    bool toCheckMultiTap = false;
    public float swipeThreshold = 100f;
    public float multiTapLimit = 10f;
    [SerializeField] float currentTapScore = 0;
    float regression = 1f;

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
        CheckMultiTap();
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

    void CheckMultiTap()
    {
        if (!toCheckMultiTap) return;

        if (Input.GetMouseButtonDown(0))
        {
            currentTapScore += 1f;
            multiTaping?.Invoke(currentTapScore, true);
        }
        else
        {
            currentTapScore -= Time.deltaTime * regression;
        }
        currentTapScore = Mathf.Clamp(currentTapScore, 0f, multiTapLimit);
        if (currentTapScore >= multiTapLimit)
        {
            onHeld = false;
            toCheckMultiTap = false;
            multiTapOver?.Invoke();
            return;
        }
        multiTaping?.Invoke(currentTapScore, false);

    }

    public void StartMultiTap()
    {
        toCheckMultiTap = true;
        currentTapScore = 0;
    }
}
