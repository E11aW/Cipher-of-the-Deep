using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject battleUI;
    public GameObject chestUI;

    void Start()
    {
        // Hide both UIs first
        battleUI.SetActive(false);
        chestUI.SetActive(false);

        if (SceneController.nextMode == SceneController.SceneMode.Battle)
        {
            battleUI.SetActive(true);
        }
        else if (SceneController.nextMode == SceneController.SceneMode.Chest)
        {
            chestUI.SetActive(true);
        }
    }
}