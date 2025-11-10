using System.Collections.Generic;
using UnityEngine;

public enum {
    StartScreen,
    
}

public class SplashScreenManager : MonoBehaviour
{
    public static SplashScreenManager Instance { get; private set; }

    [SerializeField] private List<SplashScreenEntry> splashScreens;
    private Dictionary<SplashType, GameObject> splashMap;
    private GameObject currentSplash;

    [System.Serializable]
    public class SplashScreenEntry
    {
        public SplashType type;
        public GameObject prefab;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        splashMap = new Dictionary<SplashType, GameObject>();
        foreach (var entry in splashScreens)
        {
            splashMap[entry.type] = entry.prefab;
        }
    }

    public void ShowSplash(SplashType type)
    {
        HideSplash();

        if (splashMap.ContainsKey(type))
        {
            currentSplash = Instantiate(splashMap[type]);
        }
    }

    public void HideSplash()
    {
        if (currentSplash != null)
        {
            Destroy(currentSplash);
            currentSplash = null;
        }
    }
}