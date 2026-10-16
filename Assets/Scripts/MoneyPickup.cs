using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    int amount;

    public void SetAmount(int value)
    {
        amount = value;
    }

    public int GetAmount()
    {
        return amount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMoney playerMoney = other.GetComponent<PlayerMoney>();

            if (playerMoney != null)
            {
                playerMoney.AddMoney(amount);
                Destroy(gameObject);
            }
        }
    }
}
