using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public Item item;
    private string currentChestID;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        // Get the chest ID passed from the encounter trigger
        currentChestID = PlayerPrefs.GetString("CurrentChestID", "");
        audioManager.PlaySFX(audioManager.chest);
    }

    public void yesClick()
    {
        // Load stats
        var data = SaveSystem.Load();

        if (data == null)
        {
            Debug.LogError("No save data found!");
            return;
        }
        data.playerDamage += item.modifier;
        // Set stats and chest
        data.AddOpenedChest(currentChestID);
        SaveSystem.Save(data);

        Debug.Log(currentChestID + " opened");
        SceneController.Instance.ReturnToWorld();
    }

    public void noClick()
    {
        Debug.Log("Missed out on item: " + item.ID);
        SceneController.Instance.ReturnToWorld();
    }
}
