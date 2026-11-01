using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField] int money = 0;
    [SerializeField] PlayerHUD playerHUD;
    public void AddMoney(int amount)
    {
        money += amount;
        playerHUD.UpdateCoins(money);
    }
}
