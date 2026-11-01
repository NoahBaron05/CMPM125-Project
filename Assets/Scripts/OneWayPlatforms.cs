using UnityEngine;

public class OneWayPlatforms : MonoBehaviour
{
    GameObject player;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject platform = GetComponent<GameObject>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
