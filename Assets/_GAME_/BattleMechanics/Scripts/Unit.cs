using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int damage;
    public int maxHP;
    public int currentHP;
    public string currentEnemyID;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        return currentHP <= 0;
    }
}
