using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public GameObject enemyPrefabToBattle;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
