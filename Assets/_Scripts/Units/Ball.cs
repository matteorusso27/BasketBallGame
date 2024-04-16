using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IMoveable
{
    public void Move(Vector3 toPosition)
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

public interface IMoveable
{
    void Move(Vector3 toPosition);
}
