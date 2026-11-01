using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyBulletScript : MonoBehaviour
{
    InputAction fireAction;

    Rigidbody2D bullet;

    [SerializeField] float bulletSpeed;

    GameObject enemy;

    private void Awake()
    {
        bullet = GetComponent<Rigidbody2D>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bullet.linearVelocity = transform.up * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
