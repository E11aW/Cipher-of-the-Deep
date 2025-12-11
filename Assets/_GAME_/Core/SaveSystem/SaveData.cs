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
    public int playerDamage;
    public int playerMaxHP;
    public int playerCurrentHP;

    // --- Ready for future growth ---
    public int[] inventoryItemIds; // JsonUtility likes arrays more than List<T>
    public string[] defeatedEnemies; // Track defeated enemies by ID
    public string[] openedChests; // Track all opened chests by ID
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

    public static SaveData FromStats(Unit playerUnit)
    {
        return new SaveData
        {
            playerName = playerUnit.name,
            playerDamage = playerUnit.damage,
            playerMaxHP = playerUnit.maxHP,
            playerCurrentHP = playerUnit.currentHP,
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
    public void UpdateStats(Unit playerUnit)
    {
        playerName = playerUnit.name;
        playerDamage = playerUnit.damage;
        playerMaxHP = playerUnit.maxHP;
        playerCurrentHP = playerUnit.currentHP;
        savedAtIsoUtc = DateTime.UtcNow.ToString("o");
    }

    // Check if an enemy has been defeated
    public bool IsEnemyDefeated(string enemyID)
    {
        if (defeatedEnemies == null || string.IsNullOrEmpty(enemyID)) return false;
        return System.Array.Exists(defeatedEnemies, id => id == enemyID);
    }

    // Add a defeated enemy to the list
    public void AddDefeatedEnemy(string enemyID)
    {
        // Initialize if needed
        if (defeatedEnemies == null)
        {
            defeatedEnemies = new string[] { enemyID };
            savedAtIsoUtc = DateTime.UtcNow.ToString("o");
            return;
        }

        // Check if already defeated
        if (IsEnemyDefeated(enemyID)) return;

        // Add to array
        var newArray = new string[defeatedEnemies.Length + 1];
        defeatedEnemies.CopyTo(newArray, 0);
        newArray[defeatedEnemies.Length] = enemyID;
        defeatedEnemies = newArray;

        savedAtIsoUtc = DateTime.UtcNow.ToString("o");
    }

    // Check if a chest has already been opened
    public bool IsChestOpened(string chestID)
    {
        if (openedChests == null || string.IsNullOrEmpty(chestID)) return false;
        return System.Array.Exists(openedChests, id => id == chestID);
    }

    // Add an open chest to the list
    public void AddOpenedChest(string chestID)
    {
        // Initialize if needed
        if (openedChests == null)
        {
            openedChests = new string[] { chestID };
            savedAtIsoUtc = DateTime.UtcNow.ToString("o");
            return;
        }
        // Check if chest was already opened
        if (IsChestOpened(chestID)) return;

        // Add to array
        var newArray = new string[openedChests.Length + 1];
        openedChests.CopyTo(newArray, 0);
        newArray[openedChests.Length] = chestID;
        openedChests = newArray;

        savedAtIsoUtc = DateTime.UtcNow.ToString("o");
    }

    public Vector2 ToVector2() => new Vector2(posX, posY);
}