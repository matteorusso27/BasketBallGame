using UnityEngine;
using System;
using static GameSettings;
using static Helpers;
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour, IMoveable
{
    private Rigidbody rb;
    
    public  BallType BallType;
    public  Faction  Faction;

    public Action<Ball>   OnBallGrounded;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void SimulatePhysicsMode()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void Move(Vector3 toPosition)
    {
        StartCoroutine(ParabolicMovement.ParabolicMotion(this, new Vector3(0, 0, 10)));
    }

    public Vector3 CurrentPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GameTagToString(GameTag.Terrain)))
        {
            OnBallGrounded?.Invoke(this);
            rb.isKinematic = true;
        }
    }
}

public interface IMoveable
{
    void SetPosition(Vector3 targetPosition);
    Vector3 CurrentPosition();
    void Move(Vector3 toPosition);
}
