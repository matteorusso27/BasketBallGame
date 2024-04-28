using System.Collections;
using UnityEngine;
using System;
using static GameManager;
using static Selectors;
public class SwipeListenerManager : Singleton<SwipeListenerManager>
{
    [SerializeField] private SwipeListener SwipeListener;
    [SerializeField] private float SwipingTime = 0.5f;

    private Coroutine allowedSwipeTimeCoroutine; //timeout

    public Action<float> SwipeMeasured;
    private bool swipeMeasured;
    private void OnEnable()
    {
        SwipeListener.OnSwipeDetection += OnSwipeDetection;
        SwipeListener.OnSwipeMeasured += OnSwipeFinished;
    }

    private void Start()
    {
        GameM.OnBeforeStateChanged += HandleListener;
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
        var normDistance = SwipeListener.GetNormalizedDistance();
        SwipeMeasured?.Invoke(normDistance);
    }

    public void OnSwipeDetection()
    {
        if (allowedSwipeTimeCoroutine == null)
        {
            allowedSwipeTimeCoroutine = StartCoroutine(CoroutineSwipeDetection());
            swipeMeasured = false;
        }
    }

    public void HandleListener(GameState state)
    {
        SwipeListener.CanListenToInputs = state == GameState.ShootingPhase;
    }

    private IEnumerator CoroutineSwipeDetection()
    {
        yield return new WaitForSeconds(SwipingTime);
        if (!swipeMeasured) OnSwipeFinished();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SwipeListener.CanListenToInputs = true;
    }
}
