using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfViewCalculate))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {   
        int angle = 360;
        float halfAngle = 0.5f;

        FieldOfViewCalculate fieldOfView = (FieldOfViewCalculate)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fieldOfView.transform.position, Vector3.up, Vector3.forward, angle, fieldOfView.Radius);

        Vector3 viewAngle1 = DirectionFromAngle(fieldOfView.transform.eulerAngles.y, -fieldOfView.Angle * halfAngle);
        Vector3 viewAngle2 = DirectionFromAngle(fieldOfView.transform.eulerAngles.y, fieldOfView.Angle * halfAngle);

        Handles.color = Color.yellow;
        Handles.DrawLine(fieldOfView.transform.position, fieldOfView.transform.position + viewAngle1 * fieldOfView.Radius);
        Handles.DrawLine(fieldOfView.transform.position, fieldOfView.transform.position + viewAngle2 * fieldOfView.Radius);

        if (fieldOfView.CanSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fieldOfView.transform.position, fieldOfView.Player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float anglesDegrees)
    {
        anglesDegrees += eulerY;

        return new Vector3(Mathf.Sin(anglesDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(anglesDegrees * Mathf.Deg2Rad));
    }
}
