using UnityEngine;

public class QuadraticCurve
{
    private Vector3 _startingPoint;
    private Vector3 _finalPoint;
    private Vector3 _controlPoint;

    public float Throw_height = 10f;

    public Vector3 StartingPoint { get => _startingPoint; private set => _startingPoint = value; }
    public Vector3 FinalPoint { get => _finalPoint; private set => _finalPoint = value; }
    public Vector3 ControlPoint { get => _controlPoint; set => _controlPoint = value; }

    public QuadraticCurve(Vector3 startingPoint, Vector3 finalPoint)
    {
        StartingPoint = startingPoint;
        FinalPoint = finalPoint;
        var halfway = (StartingPoint + FinalPoint) / 2;

        ControlPoint = new Vector3(halfway.x, halfway.y + Throw_height, halfway.z);
    }

    public Vector3 Evaluate(float t)
    {
        Vector3 ac = Vector3.Lerp(StartingPoint, ControlPoint, t);
        Vector3 bc = Vector3.Lerp(ControlPoint, FinalPoint, t);
        return Vector3.Lerp(ac, bc, t);
    }

    //Debug purposes
    private void OnDrawGizmos()
    {
        if (StartingPoint == null || FinalPoint == null || ControlPoint == null) return;

        for (int i = 0; i < 20; i++)
            Gizmos.DrawWireSphere(Evaluate(i / 20f), 0.1f);
    }
}