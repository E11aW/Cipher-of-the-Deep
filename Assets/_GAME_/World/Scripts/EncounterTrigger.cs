using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    public enum TriggerType { Enemy, Chest }
    public TriggerType triggerType = TriggerType.Enemy;
    public string enemyName;
    public GameObject chestContents;

    private bool triggered = false;

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
        Debug.Log("Enemy encounter");
        SceneController.Instance.LoadBattle();
    }
    void HandleChestEncounter()
    {
        Debug.Log("Chest encounter");
        SceneController.Instance.LoadChest();
    }
}
