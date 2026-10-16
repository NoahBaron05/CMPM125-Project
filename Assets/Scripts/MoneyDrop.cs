using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    [SerializeField] GameObject moneyPrefab;
    
    public void TryDropMoney()
    {
        int die1 = Random.Range(1, 21);
        int die2 = Random.Range(1, 21);
        int moneyAmount = die1 + die2;

        GameObject money = Instantiate(moneyPrefab, transform.position, Quaternion.identity);

        MoneyPickup pickup = money.GetComponent<MoneyPickup>();

        if (pickup != null)
        {
            pickup.SetAmount(moneyAmount);
        }
    }
}
