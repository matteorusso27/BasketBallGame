using System.Collections;
using UnityEngine;
public class SwipeListenerManager : Singleton<SwipeListenerManager>
{
    [SerializeField] private SwipeListener SwipeListener;
    [SerializeField] private float SwipingTime = 0.5f;

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

    public void EnableListener()
    {
        SwipeListener.CanListenToInputs = true;
    }

    private IEnumerator CoroutineSwipeDetection()
    {
        yield return new WaitForSeconds(SwipingTime);
        if (!swipeMeasured) OnSwipeFinished();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EnableListener();
    }
}
