using UnityEngine;
using UnityEngine.SceneManagement;

// Script to be placed on buttons to load scenes.
public class SceneLoaderButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.Play("Button Press");
                
                if (sceneToLoad == "HomeTown")
                {
                    audioManager.Stop("Title");
                    audioManager.Play("Theme");
                }
                else if (sceneToLoad == "MainMenuScreen")
                {
                    audioManager.Stop("Theme");
                    audioManager.Play("Title");
                }
            }
            
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name not set on " + gameObject.name);
        }
    }

    public void EndGame()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Stop("ButtonPress");
        }

        SaveSystem.Delete();        
        Application.Quit();
    }
}