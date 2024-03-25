using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 0, -10); 
    public float smoothing = 5f; 
    public float cameraDistance = 10f; 
    public Rect bounds; 
    private Vector2 minPosition, maxPosition; 

    private void Start()
    {
        CalculateBounds();
    }

    private void Update()
    {
        // Set target position with offset
        Vector3 targetPosition = target.position + offset;

        // Limit position to bounds
        if (bounds.width > 0 && bounds.height > 0)
        {
            targetPosition = LimitPositionToBounds(targetPosition);
        }

        // Move camera to target position smoothly
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }

    private void CalculateBounds()
    {
        // Calculate screen boundaries
        Vector3 screenMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraDistance));
        Vector3 screenMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraDistance));

        float halfWidth = (screenMax.x - screenMin.x) / 2f;
        float halfHeight = (screenMax.y - screenMin.y) / 2f;

        // Calculate min and max camera positions based on bounds
        minPosition = new Vector2(bounds.xMin + halfWidth, bounds.yMin + halfHeight);
        maxPosition = new Vector2(bounds.xMax - halfWidth, bounds.yMax - halfHeight);
    }

    private Vector3 LimitPositionToBounds(Vector3 position)
    {
        // Limit position within bounds
        position.x = Mathf.Clamp(position.x, minPosition.x, maxPosition.x);
        position.y = Mathf.Clamp(position.y, minPosition.y, maxPosition.y);
        return position;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw camera bounds gizmos
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(bounds.xMin, bounds.yMin), new Vector2(bounds.xMax, bounds.yMin));
        Gizmos.DrawLine(new Vector2(bounds.xMax, bounds.yMin), new Vector2(bounds.xMax, bounds.yMax));
        Gizmos.DrawLine(new Vector2(bounds.xMax, bounds.yMax), new Vector2(bounds.xMin, bounds.yMax));
        Gizmos.DrawLine(new Vector2(bounds.xMin, bounds.yMax), new Vector2(bounds.xMin, bounds.yMin));
    }
}
