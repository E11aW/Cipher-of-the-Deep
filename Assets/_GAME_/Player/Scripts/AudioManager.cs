using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip fightMusic;
    public AudioClip chest;

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainScene")
        {
            musicSource.clip = background;
            musicSource.Play();
        }

        else if (sceneName == "BattleScene")
        {
            musicSource.clip = fightMusic;
            musicSource.Play();
        }
        else if (sceneName == "Chest")
        {
            SFXSource.PlayOneShot(chest);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
