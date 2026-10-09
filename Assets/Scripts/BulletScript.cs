using UnityEngine;
using UnityEngine.InputSystem;

public class BulletScript : MonoBehaviour
{
    InputAction fireAction;

    Rigidbody2D bullet;

    [SerializeField] float bulletSpeed;

    private void Awake()
    {
        bullet = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bullet.linearVelocity = transform.up * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
}
