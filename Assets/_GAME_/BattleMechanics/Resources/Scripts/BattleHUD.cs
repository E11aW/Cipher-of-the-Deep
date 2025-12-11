using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Slider hpSlider;
    public Text hpText;
    private int maxHp;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        hpText.text = unit.currentHP + "/" + unit.maxHP;
        maxHp = unit.maxHP;
    }

    public void SetHP(int hp)
    {
        if (hp < 0) hp = 0;

        hpSlider.value = hp;
        hpText.text = hp + "/" + maxHp;
    }
}
