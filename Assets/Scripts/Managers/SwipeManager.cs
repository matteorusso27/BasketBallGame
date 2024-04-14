using System.Collections;
using UnityEngine;
using System;
// Manages the swipe detection
public class SwipeManager : Singleton<SwipeManager>
{
    [SerializeField] private SwipeListener SwipeListener;
    [SerializeField] private float SwipingTime = 0.2f;

    public Action OnSwipeAction; 
    private Coroutine allowedSwipeTimeCoroutine; //timeout

    public enum SwipeState
    {
        Ready,
        Detection,
        Measured
    }
    public SwipeState State;
    public void ChangeSwipeState(SwipeState newState)
    {
        State = newState;
    }
    public bool IsReady => State == SwipeState.Ready;
    public bool CanMeasureSwipe => State == SwipeState.Detection;
    public bool SwipeIsMeasured => State == SwipeState.Measured;

    private void OnEnable()
    {
        SwipeListener.OnSwipeMeasured += OnSwipeFinished;
        SwipeListener.OnSwipeCancelled += OnSwipeCancelled;

        ChangeSwipeState(SwipeState.Ready);
    }

    private void OnDisable()
    {
        // Unsubscribe from swipe events
        SwipeListener.OnSwipeMeasured -= OnSwipeFinished;
        SwipeListener.OnSwipeCancelled -= OnSwipeCancelled;
        ChangeSwipeState(SwipeState.Ready);
    }

    public void OnSwipeFinished(bool isSwipeUp)
    {
        //if (SwipeIsMeasured) return;
        if (!isSwipeUp)
        {
            OnSwipeCancelled();
        }
        else
        {
            if (allowedSwipeTimeCoroutine == null)
            {
                //allowedSwipeTimeCoroutine = StartCoroutine(CoroutineSwipeDetection());
                ChangeSwipeState(SwipeState.Detection);
            }
        }
        OnSwipeAction?.Invoke();
    }
   
    private void OnSwipeCancelled()
    {
        ResetState();
    }

    private void ResetState()
    {
        allowedSwipeTimeCoroutine = null;
        ChangeSwipeState(SwipeState.Measured);
    }

    private IEnumerator CoroutineSwipeDetection()
    {
        // Wait for swipe detection timeout
        yield return new WaitForSeconds(SwipingTime);

        // End swipe phase if swipe distance not measured
        if (!SwipeIsMeasured) ResetState();
    }
}
