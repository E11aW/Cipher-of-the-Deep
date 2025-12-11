using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    public enum SceneMode { Battle, Chest };
    public static SceneMode nextMode;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadBattle()
    {
        nextMode = SceneMode.Battle;
        SceneManager.LoadScene("BattleScene");
    }

    public void LoadChest()
    {
        nextMode = SceneMode.Chest;
        SceneManager.LoadScene("ChestScene");
    }

    public void ReturnToWorld()
    {   
        var data = SaveSystem.Load();

        SceneManager.LoadScene(data.scene);
    }
}