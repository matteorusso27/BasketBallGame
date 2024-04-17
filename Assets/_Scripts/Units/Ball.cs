using System.Collections;
using UnityEngine;
using static GameSettings;
using static Selectors;
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour, IMoveable
{
    private Rigidbody rb;
    
    public  BallType BallType;
    public  Faction  Faction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        EnableKinematicMode();
    }

    // Ball movement is split in a deterministic parabolic movement
    // and a simulation of physics until it touches the ground
    public void SimulatePhysicsMode()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void EnableKinematicMode()
    {
        rb.isKinematic = true;
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
}

public interface IMoveable
{
    void SetPosition(Vector3 targetPosition);
    Vector3 CurrentPosition();
    void Move(Vector3 toPosition);
}
