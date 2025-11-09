using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{
    // stats used in updating UI components
    public TextMeshProUGUI playerText;
    public Stats playerStats;
    public TextMeshProUGUI enemyText;
    public Stats enemyStats;

    void Start()
    {
        // Load saved stats
        var data = SaveSystem.Load();
        if (data != null)
        {
            playerStats.lvl = data.playerLevel;
            playerStats.damage = data.playerDamage;
            playerStats.maxHP = data.playerMaxHP;
            playerStats.currentHP = data.playerCurrentHP;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        playerText.text = "HP: " + playerStats.currentHP + "/" + playerStats.maxHP;
        enemyText.text = "HP: " + enemyStats.currentHP + "/" + enemyStats.maxHP;
    }
}
