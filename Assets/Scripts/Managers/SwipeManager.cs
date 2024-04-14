using System.Collections;
using UnityEngine;
using System;
// Manages the swipe detection
public class SwipeManager : Singleton<SwipeManager>
{
    [SerializeField] private SwipeListener SwipeListener;
    [SerializeField] private float SwipingTime = 0.8f;

    private Coroutine allowedSwipeTimeCoroutine; //timeout

    private bool swipeMeasured;
    private void OnEnable()
    {
        SwipeListener.OnSwipeDetection += OnSwipeDetection;
        SwipeListener.OnSwipeMeasured += OnSwipeFinished;
    }

    private void OnDisable()
    {
        SwipeListener.OnSwipeDetection -= OnSwipeDetection;
        SwipeListener.OnSwipeMeasured -= OnSwipeFinished;
    }

    public void OnSwipeFinished()
    {
        allowedSwipeTimeCoroutine = null;
        swipeMeasured = true;
        SwipeListener.CanListenToInputs = false;
    }

    public void OnSwipeDetection()
    {
        if (allowedSwipeTimeCoroutine == null)
        {
            allowedSwipeTimeCoroutine = StartCoroutine(CoroutineSwipeDetection());
            swipeMeasured = false;
        }
    }

    private IEnumerator CoroutineSwipeDetection()
    {
        yield return new WaitForSeconds(SwipingTime);
        if (!swipeMeasured) OnSwipeFinished();
    }
}
