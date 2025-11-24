using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public BattleUI ui;
    public Stats player;
    public Stats enemy;
    
    private string currentEnemyID;

    void Start()
    {
        // Get the enemy ID passed from the encounter trigger
        currentEnemyID = PlayerPrefs.GetString("CurrentEnemyID", "");
    }

    //------------------All player Options---------------------

    // Handles a basic attack from player
    public void PlayerAttack()
    {
        enemy.currentHP -= player.damage;

        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Play("Hit");
        }

        if (enemy.currentHP <= 0)
        {
            EndBattle(true);
            return;
        }

        StartCoroutine(EnemyTurn());
        ui.UpdateUI();
    }

    // Handles a heavy attack from player where they do twice as much damage but must recover for a turn
    public void PlayerHeavyAttack()
    {
        enemy.currentHP -= player.damage * 2;
        if (enemy.currentHP <= 0)
        {
            EndBattle(true);
            return;
        }
        StartCoroutine(EnemyTurn());
        ui.UpdateUI();
        StartCoroutine(EnemyTurn());
        ui.UpdateUI();
        Debug.Log("Enemy hit twice!");
    }

    // Handles player running from the battle
    public void PlayerRun()
    {
        EndBattle(false);
    }

    // Handles player using an item
    public void PlayerItem()
    {
        // does nothing for now <- will implement later
        Debug.Log("Item used");
        StartCoroutine(EnemyTurn());
        ui.UpdateUI();
    }
    //--------------------------------------------------------

    // Attacks player during the enemy turn
    public System.Collections.IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        player.currentHP -= enemy.damage;

        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Play("Hit");
        }

        if (player.currentHP <= 0)
        {
            EndBattle(false);
            player.currentHP = 0;
        }
        ui.UpdateUI();
    }

    // Ends the battle based on who won
    public void EndBattle(bool playerWon)
    {
        // Load existing save data (or create new if none exists)
        var data = SaveSystem.Load() ?? new SaveData();
        
        // Update player stats
        data.UpdateStats(player);

        // If player won, mark this enemy as defeated
        if (playerWon && !string.IsNullOrEmpty(currentEnemyID))
        {
            data.AddDefeatedEnemy(currentEnemyID);
        }

        SaveSystem.Save(data);
        SceneController.Instance.ReturnToWorld();
    }
}
