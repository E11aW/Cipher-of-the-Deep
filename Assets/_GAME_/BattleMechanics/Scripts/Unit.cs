using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Identity")]
    public string unitName;
    [HideInInspector] public string currentEnemyID;

    [Header("Base Stats")]
    public int maxHP;
    public int currentHP;
    public int damage;
    public int defense;

    [Header("Special Moves")]
    public int chargeDamage;
    public int healAmount;
    [HideInInspector] public bool isCharged;

    [Header("Combat Chances")]
    [Range(0f, 1f)] public float critChance;
    public int critMultiplier;
    [Range(0f, 1f)] public float missChance;

    [Header("Enemy AI Chances")]
    [Range(0f, 1f)] public float enemyAttackChance;
    [Range(0f, 1f)] public float enemyHealChance;
    [Range(0f, 1f)] public float enemyChargeChance;


    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg - defense;

        return currentHP <= 0;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

}
