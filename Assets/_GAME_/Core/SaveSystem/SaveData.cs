using System;
using UnityEngine;

// Keep this class flat & serializable.
// Using Unity's JsonUtility for now (fast, built-in). If you need
// dictionaries or polymorphism later, we can swap to Newtonsoft JSON.
[Serializable]
public class SaveData
{
    // --- Player location ---
    public float posX;
    public float posY;
    public string scene;     // Optional: store current scene 

    // --- Player stats ---
    public string playerName;
    public int playerLevel;
    public int playerDamage;
    public int playerMaxHP;
    public int playerCurrentHP;

    // --- Ready for future growth ---
    public int[] inventoryItemIds; // JsonUtility likes arrays more than List<T>
    public string version = "1.0.0";
    public string savedAtIsoUtc;

    // Create new saves
    public static SaveData FromPosition(Vector2 pos, string sceneName = "")
    {
        return new SaveData
        {
            posX = pos.x,
            posY = pos.y,
            scene = sceneName,
            savedAtIsoUtc = DateTime.UtcNow.ToString("o")
        };
    }

    public static SaveData FromStats(Stats playerStats)
    {
        return new SaveData
        {
            playerName = playerStats.name,
            playerLevel = playerStats.lvl,
            playerDamage = playerStats.damage,
            playerMaxHP = playerStats.maxHP,
            playerCurrentHP = playerStats.currentHP,
            savedAtIsoUtc = DateTime.UtcNow.ToString("o")
        };
    }

    // Update existing saves
    public void UpdatePosition(Vector2 pos, string sceneName = "")
    {
        posX = pos.x;
        posY = pos.y;
        scene = sceneName;
        savedAtIsoUtc = DateTime.UtcNow.ToString("o");
    }
    public void UpdateStats(Stats playerStats)
    {
        playerName = playerStats.name;
        playerLevel = playerStats.lvl;
        playerDamage = playerStats.damage;
        playerMaxHP = playerStats.maxHP;
        playerCurrentHP = playerStats.currentHP;
        savedAtIsoUtc = DateTime.UtcNow.ToString("o");
    }

    public Vector2 ToVector2() => new Vector2(posX, posY);
}