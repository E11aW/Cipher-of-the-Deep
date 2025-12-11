using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnCollision : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {            
            var data = SaveSystem.Load();
            data.scene = sceneToLoad;

            if (sceneToLoad == "Level2")
            {
                data.posX = -6.35f;
                data.posY = 2.77f;
                data.checkpointX = -6.35f;
                data.checkpointY = 2.77f;
            }
            else if (sceneToLoad == "Level3")
            {
                data.posX = -8.03f;
                data.posY = -2.24f;
                data.checkpointX = -8.03f;
                data.checkpointY = -2.24f;
            }
            else if (sceneToLoad == "EndScreen")
            {
                SaveSystem.Delete();
            }
            
            SaveSystem.Save(data);

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}