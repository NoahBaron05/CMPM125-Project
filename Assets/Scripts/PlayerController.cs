using System.Reflection.Metadata.Ecma335;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Player movement
    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    Rigidbody2D player;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    //Gun controls and variables
    InputAction fireAction;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletCooldown = 0.5f;

    float bulletCooldownTimer;

    InputAction fireAction2;

    [SerializeField] float hitscanDistance = 100f;
    [SerializeField] float hitscanCooldown = 0.25f;

    float hitscanCooldownTimer;

    Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        fireAction = InputSystem.actions.FindAction("Fire");
        fireAction2 = InputSystem.actions.FindAction("Fire2");

        player = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        BulletLogic();
        RotateFirePoint();
        FireHitScan();
    }

    void PlayerMovement()
    {
        float moveValue = moveAction.ReadValue<float>();

        player.linearVelocity = new Vector2(moveValue * moveSpeed, player.linearVelocity.y);

        JumpLogic();
    }
    
    void JumpLogic()
    {
        if (jumpAction.WasPressedThisFrame() && isGrounded())
        {
            player.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void BulletLogic()
    {
        bulletCooldownTimer += Time.deltaTime;

        if (fireAction.WasPressedThisFrame() && bulletCooldownTimer > bulletCooldown)
        {
            bulletCooldownTimer = 0;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePosition.z = 0;

            Vector2 direction = (mousePosition - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        }
    }

    //Allows firing bullet/hitscan from any point around the player, bullets do not collide with player
    void RotateFirePoint()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        mousePosition.z = 0;

        Vector2 direction = mousePosition - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float radius = 1f;

        firePoint.position = transform.position + (Vector3)(direction * radius);
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireHitScan()
    {
        hitscanCooldownTimer += Time.deltaTime;

        if (!fireAction2.WasPressedThisFrame() || hitscanCooldownTimer < hitscanCooldown)
        {
            return;
        }

        hitscanCooldownTimer = 0;

        Vector2 direction = firePoint.right;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, hitscanDistance);

        Debug.DrawRay(firePoint.position, direction * hitscanDistance, Color.red, 0.5f);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
