using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystemNew : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;


    public BattleState state;
    void Awake()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        var data = SaveSystem.Load();
        if (data != null)
        {
            playerUnit.damage = data.playerDamage;
            playerUnit.maxHP = data.playerMaxHP;
            playerUnit.currentHP = data.playerCurrentHP;   
        }

        GameObject enemyGO = Instantiate(BattleManager.Instance.enemyPrefabToBattle, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        enemyUnit.currentEnemyID = PlayerPrefs.GetString("CurrentEnemyID", "");

        dialogueText.text = "A " + enemyUnit.unitName + " approaches!"; 

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        float roll = Random.Range(0f, 1f);
        int chargeChanceBonus = 1;
        if (playerUnit.isCharged)
        {
            chargeChanceBonus = 2;
        }

        if (roll > playerUnit.missChance * chargeChanceBonus)
        {
            int damage = playerUnit.damage;

            if (playerUnit.isCharged)
            {
                dialogueText.text = "You hit extra hard!";

                damage += playerUnit.chargeDamage;

                yield return new WaitForSeconds(1f);

                playerUnit.isCharged = false;
            }

            roll = Random.Range(0f, 1f);

            if (roll <= playerUnit.critChance * chargeChanceBonus)
            {
                damage *= playerUnit.critMultiplier;

                dialogueText.text = "Critical hit!";   
            }
            else
            {
                dialogueText.text = "The attack is successful!";   
            }

            bool isDead = enemyUnit.TakeDamage(damage);

            enemyHUD.SetHP(enemyUnit.currentHP);

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            } else
            {
                state = BattleState.ENEMYTURN;
                EnemyTurn();
            }
        } else
        {
            dialogueText.text = "The attack missed!";
            playerUnit.isCharged = false;

            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    public void EnemyTurn()
    {
        float roll = Random.Range(0f, 1f); // roll between 0 and 1

        if (roll <= enemyUnit.enemyAttackChance)
        {
            StartCoroutine(EnemyAttack());
        }
        else if (roll <= enemyUnit.enemyAttackChance + enemyUnit.enemyHealChance)
        {
            StartCoroutine(EnemyHeal());
        }
        else if (roll <= enemyUnit.enemyAttackChance + enemyUnit.enemyHealChance + enemyUnit.enemyChargeChance)
        {
            StartCoroutine(EnemyCharge());
        }
        else
        {
            StartCoroutine(EnemyPause());
        }
    }

    IEnumerator EnemyAttack()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        float roll = Random.Range(0f, 1f);
        
        int chargeChanceBonus = 1;
        if (enemyUnit.isCharged)
        {
            chargeChanceBonus = 2;
        }

        if (roll > enemyUnit.missChance * chargeChanceBonus)
        { 
            int damage = enemyUnit.damage;
            
            if (enemyUnit.isCharged)
            {
                dialogueText.text = enemyUnit.unitName + " hit extra hard!";

                damage += enemyUnit.chargeDamage;

                yield return new WaitForSeconds(1f);

                enemyUnit.isCharged = false;
            }

            roll = Random.Range(0f, 1f);

            if (roll <= enemyUnit.critChance * chargeChanceBonus)
            {
                damage *= enemyUnit.critMultiplier;

                dialogueText.text = "Critical hit!";
            }

            bool isDead = playerUnit.TakeDamage(damage);

            playerHUD.SetHP(playerUnit.currentHP);

            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            } else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
        else
        {
            dialogueText.text = enemyUnit.unitName + " missed!";

            yield return new WaitForSeconds(2f);

            enemyUnit.isCharged = false;

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }

        yield return new WaitForSeconds(2f);

        // Load existing save data (or create new if none exists)
        var data = SaveSystem.Load() ?? new SaveData();

        // If player won, mark this enemy as defeated
        if (state == BattleState.WON && !string.IsNullOrEmpty(enemyUnit.currentEnemyID))
        {
            data.AddDefeatedEnemy(enemyUnit.currentEnemyID);
            data.playerCurrentHP = playerUnit.currentHP;
        }

        SaveSystem.Save(data);
        SceneController.Instance.ReturnToWorld();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(playerUnit.healAmount);
        
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You heal for " + playerUnit.healAmount + "hp";

        yield return new WaitForSeconds(2f);

        playerUnit.isCharged = false;
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    IEnumerator EnemyHeal()
    {
        enemyUnit.Heal(enemyUnit.healAmount);
        
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = enemyUnit.unitName + " healed for " + enemyUnit.healAmount + "hp";

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerCharge()
    {
        playerUnit.isCharged = true;

        dialogueText.text = "You charge up!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    IEnumerator EnemyCharge()
    {
        enemyUnit.isCharged = true;

        dialogueText.text = enemyUnit.unitName + " charged up!";

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator EnemyPause()
    {
        dialogueText.text = enemyUnit.unitName + " is pausing...";

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void OnAttackButton()
    {   
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        state = BattleState.ENEMYTURN;
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {   
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        state = BattleState.ENEMYTURN;
        StartCoroutine(PlayerHeal());
    }

    public void OnChargeButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        state = BattleState.ENEMYTURN;
        StartCoroutine(PlayerCharge());
    }

    public void OnRunButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        dialogueText.text = "You ran away!";
        state = BattleState.ENEMYTURN;

        StartCoroutine(EndBattle());
    }
}
