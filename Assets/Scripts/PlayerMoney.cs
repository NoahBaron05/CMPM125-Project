using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField] int money = 0;
    public void AddMoney(int amount)
    {
        money += amount;
    }
}
