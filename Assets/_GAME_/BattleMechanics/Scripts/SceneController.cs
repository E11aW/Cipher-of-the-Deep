using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadBattle()
    {
        SceneManager.LoadScene("Battle");
    }

    public void ReturnToWorld()
    {
        SceneManager.LoadScene("HomeTown");
    }
}