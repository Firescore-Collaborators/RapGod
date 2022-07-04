using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(TouchInputs))]
public class MultiTouchManager : MonoBehaviour
{
    public event System.Action onInputRaised;
    public event System.Action onSequenceComplete;
    public event System.Action<TouchInputType> onInputAssignedWithType;
    public event System.Action<TouchInputType> onInputRemovedWithType;
    public event System.Action<int> onInputRaisedWithIndex;

    [Expandable]
    public InputSequenceSO inputSequence;
    TouchInputs touchInputs
    {
        get
        {
            return GetComponent<TouchInputs>();
        }
    }
    int currentSequenceIndex = 0;


    public void Init()
    {
        currentSequenceIndex = 0;
        onInputRaised += OnCorrectInput;
        onInputRaised += IncreaseIndex;
        onInputRaised += AssingCallbacks;
        AssingCallbacks();

    }
    void OnDisable()
    {
        onInputRaised -= IncreaseIndex;
    }

    void IncreaseIndex()
    {
        currentSequenceIndex++;
    }

    void OnCorrectInput()
    {
        onInputRaisedWithIndex?.Invoke(currentSequenceIndex);
    }

    void AssingCallbacks()
    {
        UnsubscribeCallbacks();
        if (currentSequenceIndex >= inputSequence.inputSequence.Count)
        {
            onSequenceComplete?.Invoke();
            return;
        }
        switch (inputSequence.inputSequence[currentSequenceIndex])
        {
            case TouchInputType.swipeRight:
                touchInputs.swipeRight += onInputRaised;
                break;
            case TouchInputType.swipeLeft:
                touchInputs.swipeLeft += onInputRaised;
                break;
            case TouchInputType.swipeUp:
                touchInputs.swipeUp += onInputRaised;
                break;
            case TouchInputType.swipeDown:
                touchInputs.swipeDown += onInputRaised;
                break;
        }
        onInputAssignedWithType?.Invoke(inputSequence.inputSequence[currentSequenceIndex]);
    }


    void UnsubscribeCallbacks()
    {
        if (currentSequenceIndex == 0) { return; }
        switch (inputSequence.inputSequence[currentSequenceIndex - 1])
        {
            case TouchInputType.swipeRight:
                touchInputs.swipeRight -= onInputRaised;
                break;
            case TouchInputType.swipeLeft:
                touchInputs.swipeLeft -= onInputRaised;
                break;
            case TouchInputType.swipeUp:
                touchInputs.swipeUp -= onInputRaised;
                break;
            case TouchInputType.swipeDown:
                touchInputs.swipeDown -= onInputRaised;
                break;
        }
        onInputRemovedWithType?.Invoke(inputSequence.inputSequence[currentSequenceIndex - 1]);
    }



}
