using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public Stats player;
    public Item item;

    public void yesClick()
    {
        player.damage += item.modifier;
        Debug.Log(player.damage);
        SceneController.Instance.ReturnToWorld();
    }

    public void noClick()
    {
        Debug.Log("Missed out on item: " + item.ID);
        SceneController.Instance.ReturnToWorld();
    }
}
