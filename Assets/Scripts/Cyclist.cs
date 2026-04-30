using System;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cyclist : MonoBehaviour
{
    public GameObject startPosition; 
    public bool isAlive = false;
    public bool destinationReached = false;
    private const float RotationOffset = 90f;
    public GameObject[] waypoints;
    private Waypoint _nextWaypoint;
    private int _waypointsPassed;

    [Header("Movement")] public bool swerve = true;
    public bool speedVariation = true;
    public float moveSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    public float swerveSpeed = 1f;
    public float swerveAmt = 0.3f;
    public float speedVariance = 1f;
    public float speedChangeRate = 1f;
    
    private Vector3 _directionToWaypoint;
    private Vector3 _swerveDirection;
    private Vector3 _facingDirection;
    private float _swerveSeed;
    private float _speedSeed;
    
    public event Action<Cyclist, string> OnCollision;
    public event Action OnArrival;
    
    void Start()
    {
        Initialize();
    }
    
    void Update()
    {
        _nextWaypoint.DrawDebugCircle(_nextWaypoint.GetPosition(), _nextWaypoint.radius);
        if(_waypointsPassed == waypoints.Length ) return;
        if (!isAlive) return;
        
        ////// Rotation
        
        // direction to next waypoint and current diretion of cyclist
        _directionToWaypoint = (_nextWaypoint.GetPosition() - transform.position).normalized; // normaliseret vektor der peger i retningen mod wp
        _facingDirection = Vector3.RotateTowards(transform.up, _directionToWaypoint, Time.deltaTime * turnSpeed, 0.0f);
        
        // calculate swerve direction
        Vector3 perpendicular = Vector3.Cross(_facingDirection, Vector3.forward).normalized;
        float swerveNoise = (Mathf.PerlinNoise(Time.time * swerveSpeed, _swerveSeed) - 0.5f) * 2f;
        if (!swerve) swerveAmt = 0f;
        _swerveDirection = perpendicular * (swerveNoise * swerveAmt);

        // calculate final direction to move the cyclist in, and apply direction to the transform's rotation 
        Vector3 moveDirection = (_facingDirection + _swerveDirection).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - RotationOffset);
        
        // bevæg mod waypoint
        float speedNoise = (Mathf.PerlinNoise(Time.time * speedChangeRate, _speedSeed) - 0.5f) * 2f;
        if (!speedVariation) speedVariance = 0f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, (moveSpeed + speedNoise * speedVariance) * Time.deltaTime);
       
        
        // check distance til waypoint
        float distanceToWaypoint = Vector3.Distance(transform.position, _nextWaypoint.GetPosition());
        if (distanceToWaypoint <= _nextWaypoint.radius)
        {
            _waypointsPassed+=1;
            //Debug.Log("Reached waypoint " +  _waypointsPassed);
            if (_waypointsPassed == waypoints.Length)
            {
                destinationReached = true;
                isAlive = false;
                Debug.Log(gameObject.name + " has reached their destination");
                OnArrival?.Invoke();
                return;
            }
           
            _nextWaypoint = waypoints[_waypointsPassed].GetComponent<Waypoint>();
            //Debug.Log("Waypoint changed");
            
        }
        
        // debug stuff
        Debug.DrawRay(transform.position, perpendicular, Color.red);
        Debug.DrawRay(transform.position, moveDirection*6, Color.orange);
        Debug.DrawRay(transform.position, _facingDirection*6, Color.green);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "CamerasBoundary") return;
        if (isAlive) OnCollision?.Invoke(this, other.tag);
        isAlive = false;
        
    }

    public void Initialize()
    {
        if(startPosition != null) transform.position = startPosition.transform.position;
        isAlive = false;
        destinationReached = false;
        _waypointsPassed = 0;
        _nextWaypoint = waypoints[_waypointsPassed].GetComponent<Waypoint>();
        _directionToWaypoint = (_nextWaypoint.gameObject.transform.position - transform.position).normalized;
        _facingDirection = transform.up;
        _swerveSeed = Random.Range(0f, 999f);
        _speedSeed = Random.Range(0f, 999f);
    }
}
