using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    // If you want multiple slots later, make this configurable.
    private const string FileName = "save1.json";

    public static string SavePath =>
        Path.Combine(Application.persistentDataPath, FileName);

    public static bool HasSave() => File.Exists(SavePath);

    public static void Save(SaveData data)
    {
        try
        {
            var existing = Load() ?? new SaveData();

            // Merge save fields to avoid overwriting values
            if (data.posX != null) existing.UpdatePosition(new Vector2(data.posX, data.posY), data.scene);
            if (data.playerMaxHP > 0)
            {
                existing.playerName = data.playerName;
                existing.playerLevel = data.playerLevel;
                existing.playerDamage = data.playerDamage;
                existing.playerMaxHP = data.playerMaxHP;
                existing.playerCurrentHP = data.playerCurrentHP;
            }
            ;

            // Preserve defeated enemies if present in new data
            if (data.defeatedEnemies != null && data.defeatedEnemies.Length > 0)
            {
                existing.defeatedEnemies = data.defeatedEnemies;
            }
            // Preserve opened chests if present in new data
            if (data.openedChests != null && data.openedChests.Length > 0)
            {
                existing.openedChests = data.openedChests;
            }

            var json = JsonUtility.ToJson(existing, true);

            // Ensure directory exists
            var dir = Path.GetDirectoryName(SavePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Atomic-ish write: write to a temp file, then replace
            var tmpPath = SavePath + ".tmp";
            File.WriteAllText(tmpPath, json);
            if (File.Exists(SavePath)) File.Delete(SavePath);
            File.Move(tmpPath, SavePath);

#if UNITY_EDITOR
            Debug.Log($"[SaveSystem] Saved: {SavePath}\n{json}");
#endif
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveSystem] Save failed: {ex}");
        }
    }

    public static SaveData Load()
    {
        try
        {
            if (!File.Exists(SavePath)) return null;
            var json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<SaveData>(json);

#if UNITY_EDITOR
            Debug.Log($"[SaveSystem] Loaded: {SavePath}\n{json}");
#endif

            return data;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveSystem] Load failed: {ex}");
            return null;
        }
    }

    public static void Delete()
    {
        try
        {
            if (File.Exists(SavePath)) File.Delete(SavePath);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveSystem] Delete failed: {ex}");
        }
    }
}