using UnityEngine;

public class ReflectPadScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Rigidbody2D bullet = other.GetComponent<Rigidbody2D>();

            if (bullet != null)
            {
                bullet.linearVelocity = new Vector2(-bullet.linearVelocity.x, bullet.linearVelocity.y);
            }
        }
    }
}
