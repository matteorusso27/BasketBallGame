using System;
using UnityEngine;

public class SwipeListener : MonoBehaviour
{
    [SerializeField] 
    private float MIN_SWIPE = Screen.height / 20;
        
    [SerializeField]
    private float   sensitivity = 5f;
    private float   _minMoveDistance;
    private float   offsetY;
        
    public Action         OnSwipeCancelled;
    public Action<bool>   OnSwipeMeasured;
    public Action         OnSwipeDetection;

    public  Vector3 _swipePoint;

    private void Start()
    {
        UpdateSensitivity();
    }

    private void UpdateSensitivity()
    {
        _minMoveDistance = Screen.height / sensitivity;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitSwipe();
        }

        if (Input.GetMouseButton(0))
        {
            CheckSwipe();
        }

        CheckSwipeCancellation();
        if (Input.GetKey(KeyCode.Mouse0))
            OnSwipeDetection?.Invoke();
    }

    private void CheckSwipeCancellation()
    {
        if (Input.GetMouseButtonUp(0))
        {
            offsetY = Input.mousePosition.y - _swipePoint.y;
            if (offsetY < MIN_SWIPE) return;
            OnSwipeCancelled?.Invoke();
        }
    }
       
    // This will be called just one time at the start of the swipe
    private void InitSwipe()
    {
        _swipePoint = Input.mousePosition;
    }

    private void CheckSwipe()
    {
        var offset = Input.mousePosition - _swipePoint;
        if (offset.magnitude >= _minMoveDistance)
        {
            offsetY = offset.y;
            var isSwipeUp = offsetY < 0? false : true;
            OnSwipeMeasured?.Invoke(isSwipeUp);
        }
    }
    public float GetSwipeStartingPoint() => _swipePoint.y;

    public float GetNormalizedDistance()
    {
        return Mathf.Clamp(GetCurrentSwipeDistance() / GetMaxSwipeDistance(), 0, 1);
    }

    public float GetMaxSwipeDistance() => Screen.height / 2f;

    public float GetCurrentSwipeDistance()
    {
        return Mathf.Abs(Input.mousePosition.y - _swipePoint.y);
    }
}