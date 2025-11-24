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
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Stop("Theme");
            audioManager.Play("Combat");
        }
        SceneManager.LoadScene("BattleChest");
    }

    public void LoadChest()
    {
        nextMode = SceneMode.Chest;
        SceneManager.LoadScene("BattleChest");
    }

    public void ReturnToWorld()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Stop("Theme");
        }
        SceneManager.LoadScene("HomeTown");
    }
}