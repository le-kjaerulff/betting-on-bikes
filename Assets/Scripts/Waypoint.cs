using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool hidden = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!hidden) return;
        GetComponent<SpriteRenderer>().enabled = false;
    }
    
}
