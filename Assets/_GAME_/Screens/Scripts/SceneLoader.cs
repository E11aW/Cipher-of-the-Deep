using UnityEngine;
using UnityEngine.SceneManagement;

// Script to be placed on buttons to load scenes.
public class SceneLoaderButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public void LoadScene()
    {   
        Debug.Log(sceneToLoad);
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name not set on " + gameObject.name);
        }
    }

    public void EndGame()
    {
        SaveSystem.Delete();        
        Application.Quit();
    }
}