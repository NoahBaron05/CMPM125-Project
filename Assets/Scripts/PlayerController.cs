using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player movement
    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    Rigidbody2D player;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    // Gun controls
    InputAction fireAction;
    InputAction fireAction2;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletCooldown = 0.5f;
    float bulletCooldownTimer;

    [SerializeField] float hitscanDistance = 100f;
    [SerializeField] float hitscanCooldown = 0.25f;
    float hitscanCooldownTimer;

    [SerializeField] LineRenderer hitscanLine;

    // Aim line
    [SerializeField] LineRenderer aimLine;
    [SerializeField] float aimLineDistance = 5f;

    Camera mainCamera;

    [SerializeField] Image brightnessOverlay;

    [SerializeField] int playerHealth;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        fireAction = InputSystem.actions.FindAction("Fire");
        fireAction2 = InputSystem.actions.FindAction("Fire2");

        player = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        if (aimLine != null)
        {
            aimLine.positionCount = 2;
            aimLine.useWorldSpace = true;
        }

        if (SettingsManager.Instance != null && brightnessOverlay != null)
        {
            SettingsManager.Instance.SetBrightnessOverlay(brightnessOverlay);
        }
    }

    void Update()
    {
        PlayerMovement();
        BulletLogic();
        RotateFirePoint();
        FireHitScan();
        UpdateAimLine();
    }

    void PlayerMovement()
    {
        float moveValue = moveAction.ReadValue<float>();

        player.linearVelocity = new Vector2(
            moveValue * moveSpeed,
            player.linearVelocity.y
        );

        JumpLogic();
    }

    void JumpLogic()
    {
        if (jumpAction.WasPressedThisFrame() && isGrounded())
        {
            player.AddForce(
                transform.up * jumpPower,
                ForceMode2D.Impulse
            );
        }
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void BulletLogic()
    {
        bulletCooldownTimer += Time.deltaTime;

        if (fireAction.WasPressedThisFrame() &&
            bulletCooldownTimer > bulletCooldown)
        {
            bulletCooldownTimer = 0;

            Vector3 mousePosition =
                mainCamera.ScreenToWorldPoint(
                    Mouse.current.position.ReadValue()
                );

            mousePosition.z = 0;

            Vector2 direction =
                (mousePosition - firePoint.position).normalized;

            float angle =
                Mathf.Atan2(direction.y, direction.x) *
                Mathf.Rad2Deg - 90f;

            Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.Euler(0, 0, angle)
            );
        }
    }

    void RotateFirePoint()
    {
        if (Mouse.current == null)
            return;

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );

        mousePosition.z = 0;

        Vector2 direction = mousePosition - transform.position;
        direction.Normalize();

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float radius = 1f;

        firePoint.position =
            transform.position + (Vector3)(direction * radius);

        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireHitScan()
    {
        hitscanCooldownTimer += Time.deltaTime;

        if (!fireAction2.WasPressedThisFrame() ||
            hitscanCooldownTimer < hitscanCooldown)
        {
            return;
        }

        hitscanCooldownTimer = 0;

        Vector2 origin = firePoint.position;
        Vector2 direction = firePoint.right;

        RaycastHit2D hit = Physics2D.Raycast(
            origin, direction, hitscanDistance
        );

        if (hitscanLine == null)
            return;

        hitscanLine.useWorldSpace = true;
        hitscanLine.enabled = true;
        hitscanLine.SetPosition(0, origin);

        if (hit.collider != null)
        {
            hitscanLine.SetPosition(1, hit.point);
        }
        else
        {
            hitscanLine.SetPosition(
                1, origin + direction * hitscanDistance
            );
        }

        CancelInvoke(nameof(HideHitscanLine));
        Invoke(nameof(HideHitscanLine), 0.1f);

        Debug.DrawRay(
            origin, direction * hitscanDistance, Color.red, 0.5f
        );

        if (hit.collider != null &&
            hit.collider.CompareTag("Enemy"))
        {
            EnemyScript enemy =
                hit.collider.GetComponent<EnemyScript>();

            if (enemy != null)
            {
                enemy.Damage(2);
            }
            else
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void HideHitscanLine()
    {
        if (hitscanLine != null)
            hitscanLine.enabled = false;
    }

    void UpdateAimLine()
    {
        if (aimLine == null)
            return;

        bool showAimLine =
            SettingsManager.Instance != null &&
            SettingsManager.Instance.ShowAimLine;

        aimLine.enabled = showAimLine;

        if (!showAimLine || firePoint == null)
            return;

        Vector2 origin = firePoint.position;
        Vector2 direction = firePoint.right;

        aimLine.SetPosition(0, origin);
        aimLine.SetPosition(
            1, origin + direction * aimLineDistance
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();

            PlayerDamage();
        }
    }

    void PlayerDamage()
    {
        playerHealth -= 1;

        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}