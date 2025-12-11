using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    public enum TriggerType { Enemy, Chest }
    public TriggerType triggerType = TriggerType.Enemy;
    public string ID = System.Guid.NewGuid().ToString();

    public GameObject chestContents;
    public GameObject enemyPrefab;

    private bool triggered = false;

    void Start()
    {
        // If this is an enemy and it's already been defeated, destroy it
        if (triggerType == TriggerType.Enemy)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                var data = SaveSystem.Load();
                if (data != null && data.IsEnemyDefeated(ID))
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
        BattleManager.Instance.enemyPrefabToBattle = enemyPrefab;
        // Pass the enemy ID to the battle system via PlayerPrefs (temporary)
        if (!string.IsNullOrEmpty(ID))
        {
            PlayerPrefs.SetString("CurrentEnemyID", ID);
            PlayerPrefs.Save();
        }

        SceneController.Instance.LoadBattle();
    }
    void HandleChestEncounter()
    {
        var data = SaveSystem.Load() ?? new SaveData();
        // Update visuals if chest is opened
        if (!data.IsChestOpened(ID) && !string.IsNullOrEmpty(ID))
        {
            PlayerPrefs.SetString("CurrentChestID", ID);
            PlayerPrefs.Save();
            SceneController.Instance.LoadChest();
        }
    }
}
