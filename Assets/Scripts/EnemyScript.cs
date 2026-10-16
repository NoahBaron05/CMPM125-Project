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
}
