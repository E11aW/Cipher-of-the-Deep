using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    public enum TriggerType { Enemy, Chest }
    public TriggerType triggerType = TriggerType.Enemy;
    public string enemyName;
    public string enemyID; // Unique identifier for this enemy instance
    public GameObject chestContents;

    private bool triggered = false;

    void Start()
    {
        // If this is an enemy and it's already been defeated, destroy it
        if (triggerType == TriggerType.Enemy)
        {
            if (!string.IsNullOrEmpty(enemyID))
            {
                var data = SaveSystem.Load();
                if (data != null && data.IsEnemyDefeated(enemyID))
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return; // Avoid multiple triggers
        if (other.CompareTag("Player"))
        {
            triggered = true;
            switch (triggerType)
            {
                case TriggerType.Enemy:
                    HandleEnemyEncounter();
                    break;
                case TriggerType.Chest:
                    HandleChestEncounter();
                    break;
            }
        }
    }

    void HandleEnemyEncounter()
    {        
        // Pass the enemy ID to the battle system via PlayerPrefs (temporary)
        if (!string.IsNullOrEmpty(enemyID))
        {
            PlayerPrefs.SetString("CurrentEnemyID", enemyID);
            PlayerPrefs.Save();
        }
        
        SceneController.Instance.LoadBattle();
    }
    void HandleChestEncounter()
    {
        Debug.Log("Chest encounter");
        SceneController.Instance.LoadChest();
    }
}
