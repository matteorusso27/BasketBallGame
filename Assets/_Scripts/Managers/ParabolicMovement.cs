using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameSettings;
using static Selectors;
// This manager handles the movement (parabolic + physics based) of the balls
public class ParabolicMovement : MonoBehaviour
{
    public static IEnumerator ParabolicMotion(IMoveable obj, Vector3 finalPosition)
    {
        var startingPosition = obj.CurrentPosition();
        var curve = new QuadraticCurve(startingPosition, finalPosition);

        var distance = Vector3.Distance(startingPosition, finalPosition);
        var time = 0f;
        var totalTime = 1.5f;
        // Loop until the desired point and the ball gets close enough
        while (distance >= 0.5f)
        {
            float timeScaleFactor = 1f - Mathf.Clamp01(time / totalTime);

            time += Time.deltaTime * timeScaleFactor;
            obj.SetPosition(curve.Evaluate(time));
            distance = Vector3.Distance(obj.CurrentPosition(), curve.FinalPoint);
            yield return null;
        }

        // Then simulate physics mode to add realism
        (obj as Ball).SimulatePhysicsMode();
    }
}
