using UnityEngine;

public class Cyclist : MonoBehaviour
{
    public GameObject[] waypoints;
    public float waypointRadius = 0.01f;
    public float rotationOffset = 90f; 
    
    [Header("Movement")]
    public float moveSpeed = 1.0f;
    public float swerveAmt = 0.3f;
    public float swerveSpeed = 1f;
    private float _swerveTime;
    
    private Transform _nextWaypoint;
    private int _waypointsPassed;
    private Vector3 _directionToWaypoint;
    private float _angle;
    private Vector3 _swerveOffset;
    private Vector3 _facingDirection;
    public float turnSpeed = 1.0f;
    
    
    public bool useTriForSwerve = false;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _waypointsPassed = 0;
        _nextWaypoint = waypoints[_waypointsPassed].transform;
        _directionToWaypoint = (_nextWaypoint.position - transform.position).normalized;
        _facingDirection = transform.up;

    }

    // Update is called once per frame
    void Update()
    
    {
        DrawDebugCircle(_nextWaypoint.transform.position, waypointRadius , Color.purple);
        if(_waypointsPassed == waypoints.Length ) return;
        
        float distanceToWaypoint = Vector3.Distance(transform.position, _nextWaypoint.position);
        
        // Rotation
        
        _directionToWaypoint = (_nextWaypoint.position - transform.position).normalized;
        _facingDirection = Vector3.RotateTowards(transform.up, _directionToWaypoint, Time.deltaTime * turnSpeed, 0.0f);
        
        
        
        
        Vector3 perpendicular = Vector3.Cross(_facingDirection, Vector3.forward).normalized;

        _swerveTime = Time.time * swerveSpeed;
        
        if (useTriForSwerve)
        {
            _swerveOffset = perpendicular * (Mathf.Asin(Mathf.Sin(_swerveTime)) / (Mathf.PI / 2f)) * swerveAmt;
        }
        else
        {
            _swerveOffset = perpendicular * Mathf.Sin(_swerveTime) * swerveAmt;
        }
        
        
        Vector3 moveDirection = (_facingDirection + _swerveOffset).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg; 
        transform.rotation = UpdateRotation(angle, rotationOffset);
        
        
        // bevæg mod waypoint
        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, moveSpeed * Time.deltaTime);

        if (distanceToWaypoint <= waypointRadius)
        {
            _waypointsPassed+=1;
            Debug.Log("Reached waypoint " +  _waypointsPassed);
            if(_waypointsPassed == waypoints.Length ) return;
            _nextWaypoint = waypoints[_waypointsPassed].transform;
            Debug.Log("Waypoint changed");
        }
        
        Debug.DrawRay(transform.position, perpendicular, Color.red);
        //Debug.DrawRay(transform.position, moveDirection, Color.green);
        Debug.DrawRay(transform.position, _facingDirection, Color.green);
        
        


    }

    private Quaternion UpdateRotation(float rotAngle, float rotOffset)
    {
        return transform.rotation = Quaternion.Euler(0f, 0f, rotAngle - rotOffset);
    }

    void DrawDebugCircle(Vector3 center, float radius, Color color, int segments = 36)
    {
        float angleStep = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;

            Vector3 point1 = center + new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1), 0f) * radius;
            Vector3 point2 = center + new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0f) * radius;

            Debug.DrawLine(point1, point2, color);
        }
    }

}
