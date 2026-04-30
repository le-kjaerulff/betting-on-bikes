using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool hidden = true;
    public float radius = 1;
    private Color _pointColor;
    private SpriteRenderer _sr;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _pointColor = _sr.color;
        if(!hidden) return;
        _sr.enabled = false;
        
    }
    
    public void DrawDebugCircle(Vector3 center, float r, int segments = 36)
    {
        float angleStep = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;

            Vector3 point1 = center + new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1), 0f) * r;
            Vector3 point2 = center + new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0f) * r;

            Debug.DrawLine(point1, point2, _pointColor);
        }
    }
    
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

 
}
