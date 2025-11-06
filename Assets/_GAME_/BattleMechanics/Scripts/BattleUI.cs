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
        UpdateUI();
    }

    public void UpdateUI()
    {
        playerText.text = "HP: " + playerStats.currentHP + "/" + playerStats.maxHP;
        enemyText.text = "HP: " + enemyStats.currentHP + "/" + enemyStats.maxHP;
    }
}
