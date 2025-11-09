using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public Stats player;
    public Item item;

    public void yesClick()
    {
        player.damage += item.modifier;
        // Save updated stats
        var data = SaveData.FromStats(player);
        SaveSystem.Save(data);
        SceneController.Instance.ReturnToWorld();
    }

    public void noClick()
    {
        Debug.Log("Missed out on item: " + item.ID);
        SceneController.Instance.ReturnToWorld();
    }
}
