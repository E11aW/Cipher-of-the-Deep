using System;
using UnityEngine;

// Keep this class flat & serializable.
// Using Unity's JsonUtility for now (fast, built-in). If you need
// dictionaries or polymorphism later, we can swap to Newtonsoft JSON.
[Serializable]
public class SaveData
{
    // --- Required now ---
    public float posX;
    public float posY;
    public string scene;     // Optional: store current scene if you have multiple

    // --- Ready for future growth ---
    public int playerHealth;
    public int[] inventoryItemIds; // JsonUtility likes arrays more than List<T>
    public string version = "1.0.0";
    public string savedAtIsoUtc;
    
    public static SaveData FromPosition(Vector2 pos, string sceneName = "")
    {
        return new SaveData
        {
            posX = pos.x,
            posY = pos.y,
            scene = sceneName,
            savedAtIsoUtc = DateTime.UtcNow.ToString("o"),
            // sensible defaults for future fields
            playerHealth = 100,
            inventoryItemIds = Array.Empty<int>()
        };
    }

    public Vector2 ToVector2() => new Vector2(posX, posY);
}