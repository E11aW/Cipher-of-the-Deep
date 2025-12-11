using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnCollision : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SaveSystem.Delete();        
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}