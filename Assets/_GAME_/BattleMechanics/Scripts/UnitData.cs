using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "RPG/Unit")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public int damage;
    public int maxHP;
    public Sprite unitSprite;
}