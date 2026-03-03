using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    public void TakeDamage(int amount)
    {
        Debug.Log($"Player took damage: {amount}");
        // later: decrease HP (Day 6)
        // later: trigger damage animation (Day 6)
    }
    
}