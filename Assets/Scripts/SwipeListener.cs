using System;
using UnityEngine;

public class SwipeListener : MonoBehaviour
{
    [SerializeField] 
    private float MIN_SWIPE = Screen.height / 20;
        
    [SerializeField]
    private float   sensitivity = 1f;
    private float   offsetY;
        
    public Action         OnSwipeMeasured;
    public Action         OnSwipeDetection;

    public  Vector3 startingPoint;

    private bool canListenToInputs = true;
    public bool CanListenToInputs { get => canListenToInputs; set => canListenToInputs = value; }

    private void Update()
    {
        if (!canListenToInputs) return;

        if (Input.GetMouseButtonDown(0))
        {
            InitSwipe();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSwipe();
        }

        CheckSwipeCancellation();
        if (Input.GetKey(KeyCode.Mouse0))
            OnSwipeDetection?.Invoke();
    }

    // This will be called just one time at the start of the swipe
    private void InitSwipe()
    {
        startingPoint = Input.mousePosition;
    }

    private void CheckSwipeCancellation()
    {
        if (Input.GetMouseButtonUp(0))
        {
            offsetY = Input.mousePosition.y - startingPoint.y;
            if (offsetY >= MIN_SWIPE)
            {
                Debug.Log("annulla");
                OnSwipeMeasured?.Invoke();
            }
        }
    }
       
    private void UpdateSwipe()
    {
        offsetY = Input.mousePosition.y - startingPoint.y;
    }
    public float GetSwipeStartingPoint() => startingPoint.y;

    public float GetNormalizedDistance()
    {
        return Mathf.Clamp(GetCurrentSwipeDistance() / GetMaxSwipeDistance(), 0, 1);
    }

    public float GetMaxSwipeDistance() => Screen.height / 2f;

    public float GetCurrentSwipeDistance()
    {
        return Mathf.Abs(Input.mousePosition.y - startingPoint.y) * sensitivity;
    }
}