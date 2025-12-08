using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public Stats player;
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
        player.damage += item.modifier;

        // Save updated stats
        var data = SaveSystem.Load() ?? new SaveData();
        data.UpdateStats(player);
        // Set chest as opened
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
