using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScrollingText : MonoBehaviour
{
    [SerializeField] [TextArea] private string[] gameStart;
    [SerializeField] private float textSpeed = 0.08f; // slower so you can see it

    [SerializeField] private TextMeshProUGUI gameStartText;
    [SerializeField] private string nextSceneName = "MainScene"; // <-- set this in Inspector

    private int currentDisplayingText = 0;

    public void ActiveText()
    {
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        // Safety checks
        if (gameStart == null || gameStart.Length == 0)
        {
            Debug.LogError("ScrollingText: gameStart array is empty or not assigned.");
            yield break;
        }

        if (currentDisplayingText < 0 || currentDisplayingText >= gameStart.Length)
        {
            Debug.LogError("ScrollingText: currentDisplayingText index out of range.");
            yield break;
        }

        string fullText = gameStart[currentDisplayingText];

        // Start with empty text
        gameStartText.text = "";
        gameStartText.maxVisibleCharacters = 0;

        // Typewriter loop
        for (int i = 0; i <= fullText.Length; i++)
        {
            gameStartText.text = fullText.Substring(0, i);
            gameStartText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(textSpeed);
        }

        // Leave the full title up for a moment
        yield return new WaitForSeconds(0.75f);

        // Now load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("ScrollingText: nextSceneName is empty; not loading a scene.");
        }
    }
}
