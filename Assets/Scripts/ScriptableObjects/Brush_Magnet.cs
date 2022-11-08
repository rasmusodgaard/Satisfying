using UnityEngine;

[CreateAssetMenu(fileName = "Brush_Magnet", menuName = "ScriptableObjects/Brush_Magnet", order = 1)]
public class Brush_Magnet : Brush_Base
{
    public float magnetSpeed = 1;
    public float pushSpeedMultiplier = 2;
    public float minSqDist = 5;
    public float maxSqDist = 75;


    public override void Primary(Vector3 _mousePosition, float _radius, Color _paintColor)
    {

        OverlapCircleResults results = GetPixels(_mousePosition, _radius);

        for (int i = 0; i < results.count; i++)
        {
            float sqDist = Vector3.SqrMagnitude(_mousePosition - results.colliders[i].transform.position);
            if (sqDist > minSqDist)
            {
                Vector2 direction = ((_mousePosition - results.colliders[i].transform.position).normalized);
                //results.colliders[i].transform.position += (Vector3)direction * magnetSpeed * Time.deltaTime;
                var rigidbody = results.colliders[i].attachedRigidbody;
                rigidbody.MovePosition(rigidbody.position + direction * magnetSpeed * Time.deltaTime);
            }

        }
    }

    public override void Secondary(Vector3 _mousePosition, float _radius, Color _paintColor)
    {
        OverlapCircleResults results = GetPixels(_mousePosition, _radius);
        for (int i = 0; i < results.count; i++)
        {
            float sqDist = Vector3.SqrMagnitude(_mousePosition - results.colliders[i].transform.position);

            Vector2 direction = (_mousePosition - results.colliders[i].transform.position).normalized;
            results.colliders[i].transform.position += -(Vector3)direction * magnetSpeed * pushSpeedMultiplier * Time.deltaTime;

        }
    }
}
