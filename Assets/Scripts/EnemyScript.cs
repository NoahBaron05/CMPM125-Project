using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    EnemyWaveManager waveManager;

    [SerializeField] int enemyHealth;
    [SerializeField] float flashDuration = 0.1f;

    int startingHealth;
    bool isDead;

    SpriteRenderer spriteRenderer;
    Color originalColor;
    Coroutine flashCoroutine;

    public GameObject PrefabSource { get; private set; }

    MoneyDrop moneyDrop;

    [SerializeField] private float viewDistance;
    private GameObject player;

    //Enemy types
    [SerializeField] private bool sniper;
    [SerializeField] private bool doubleShot;

    [SerializeField] GameObject enemyBulletPrefab;

    [SerializeField] float bulletCooldown = 0.5f;
    float bulletCooldownTimer;

    void Awake()
    {
        startingHealth = enemyHealth;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        moneyDrop = GetComponent<MoneyDrop>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        AIUpdate();
    }

    public void SetPoolInfo(EnemyWaveManager manager, GameObject prefabSource)
    {
        waveManager = manager;
        PrefabSource = prefabSource;
    }

    public void ResetForSpawn()
    {
        enemyHealth = startingHealth;
        isDead = false;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (moneyDrop != null)
        {
            moneyDrop.TryDropMoney();
        }

        if (waveManager != null)
        {
            waveManager.ReturnEnemy(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Damage(int damage)
    {
        if (isDead)
        {
            return;
        }

        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            Die();
            return;
        }

        if (spriteRenderer != null)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }

            flashCoroutine = StartCoroutine(FlashWhite());
        }
    }

    IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        flashCoroutine = null;
    }

    private (Vector2 direction, RaycastHit2D hit) PlayerDetection()
    {
        Vector2 direction = player.transform.position - transform.position;
        int layerMask = ~LayerMask.GetMask("Enemy");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance, layerMask);

        if (hit.collider != null && hit.collider.CompareTag("Player")) 
        {
            Debug.DrawRay(transform.position, direction, Color.red);
            return (direction, hit);
        }
        return (Vector2.zero, hit);
    }

    private void AIUpdate()
    {
        if (sniper) SniperAI();
        if (doubleShot) DoubleShotAI();
    }

    private void SniperAI()
    {
        var (direction, hit) = PlayerDetection();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        bulletCooldownTimer += Time.deltaTime;

        if (bulletCooldownTimer > bulletCooldown && hit.collider != null && hit.collider.CompareTag("Player"))
        {
            bulletCooldownTimer = 0;

            Instantiate(enemyBulletPrefab, this.transform.position, Quaternion.Euler(0, 0, angle));
        }
    }

    private void DoubleShotAI()
    {
        var (direction, hit) = PlayerDetection();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        bulletCooldownTimer += Time.deltaTime;

        if (bulletCooldownTimer > bulletCooldown && hit.collider != null && hit.collider.CompareTag("Player"))
        {
            bulletCooldownTimer = 0;

            StartCoroutine(DoubleShotAttack(enemyBulletPrefab, this.transform.position, Quaternion.Euler(0, 0, angle)));
        }
    }

    IEnumerator DoubleShotAttack(GameObject enemyBulletPrefab, Vector2 position, Quaternion angle)
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(enemyBulletPrefab, position, angle);
            yield return new WaitForSeconds(0.08f);
        }
    }
}
