using UnityEngine;

public class OpenChest : MonoBehaviour
{
    public Sprite openedChest;
    private SpriteRenderer spriteRenderer;
    private string currentChestID;


    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Get chest ID passed from EncounterTrigger
        currentChestID = PlayerPrefs.GetString("CurrentChestID", "");

        var data = SaveSystem.Load() ?? new SaveData();
        // Update visuals if chest is opened
        if (data.IsChestOpened(currentChestID))
        {
            ApplyOpenedSprite();
        }
    }

    // Open the correct chest
    private void ApplyOpenedSprite()
    {
        if (spriteRenderer != null && openedChest != null)
        {
            spriteRenderer.sprite = openedChest;
            Debug.Log("Chest " + currentChestID + " opened!");
        }
    }
}