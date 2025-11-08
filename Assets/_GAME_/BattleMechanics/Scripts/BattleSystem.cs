using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public BattleUI ui;
    public Stats player;
    public Stats enemy;

    //------------------All player Options---------------------

    // Handles a basic attack from player
    public void PlayerAttack()
    {
        if (enemy == null)
        {
            Debug.LogError("Enemy stats null");
            return;
        }
        if (player == null)
        {
            Debug.LogError("Player stats null");
            return;
        }
        enemy.currentHP -= player.damage;
        Debug.Log(player.damage);
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
        Debug.Log(playerWon ? "You win!" : "You lost");
        SceneController.Instance.ReturnToWorld();
    }
}
