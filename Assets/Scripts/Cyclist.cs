using UnityEngine;

public class Cyclist : MonoBehaviour
{
    public GameObject[] waypoints;
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
        _facingDirection = Vector3.up;

    }

    // Update is called once per frame
    void Update()
    
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, _nextWaypoint.position);
        _directionToWaypoint = (_nextWaypoint.position - transform.position).normalized;
        
        
        
        
        
        // Rotation
        
        
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
        
        if (distanceToWaypoint >= 0.1f)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg; 
            transform.rotation = UpdateRotation(angle, rotationOffset);
        }
        
        // bevæg mod waypoint
        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _nextWaypoint.position) <= 0.01f && _waypointsPassed < waypoints.Length-1)
        {
            _waypointsPassed+=1;
            _nextWaypoint = waypoints[_waypointsPassed].transform;
            Debug.Log("Waypoint changed");
        }
        
        Debug.DrawRay(transform.position, perpendicular, Color.red);
        //Debug.DrawRay(transform.position, moveDirection, Color.green);
        Debug.DrawRay(transform.position, transform.up, Color.blue);
        _facingDirection = Vector3.up;


    }

    private Quaternion UpdateRotation(float rotAngle, float rotOffset)
    {
        return transform.rotation = Quaternion.Euler(0f, 0f, rotAngle - rotOffset);
    }


}
