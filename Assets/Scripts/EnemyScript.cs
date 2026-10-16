using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    EnemyWaveManager waveManager;

    [SerializeField] int enemyHealth;
    [SerializeField] float flashDuration = 0.1f;

    SpriteRenderer spriteRenderer;
    Color originalColor;

    void Start()
    {
        waveManager = FindAnyObjectByType<EnemyWaveManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void Die()
    {
        if (waveManager != null)
        {
            waveManager.EnemyDefeated();
        }

        Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        enemyHealth -= damage;

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashWhite());
        }

        if (enemyHealth <= 0) Die();
    }

    IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
