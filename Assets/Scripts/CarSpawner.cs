using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    private Transform _endPoint;
    public GameObject carPrefab;
    public float carSpeed = 1;
    public float spawnInterval = 2;
    private float _timeOfLastSpawn = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _endPoint = transform.GetChild(0);
        Invoke("SpawnCar", Random.Range(0, spawnInterval));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _timeOfLastSpawn >= spawnInterval)
        {
            SpawnCar();
        }
    }

    void SpawnCar()
    {
        Car newCar = Instantiate(carPrefab, transform.position, Quaternion.identity).gameObject.GetComponent<Car>();
        newCar.endPoint = _endPoint;
        newCar.speed = carSpeed;
        _timeOfLastSpawn = Time.time;
    }

}
