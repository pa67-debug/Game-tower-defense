using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager instance;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        PlayByScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayByScene(scene.name);
    }

    void PlayByScene(string sceneName)
    {
        if (sceneName == "MainMenu")
        {
            PlayMusic(menuMusic);
        }
        else
        {
            PlayMusic(gameplayMusic);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        // 🔥 กันเล่นซ้ำเพลงเดิม
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        StopAllCoroutines();
        StartCoroutine(FadeSwitch(clip));
    }

    System.Collections.IEnumerator FadeSwitch(AudioClip newClip)
    {
        // fade out
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.unscaledDeltaTime * 1.5f;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        // fade in
        while (audioSource.volume < 0.5f)
        {
            audioSource.volume += Time.unscaledDeltaTime * 1.5f;
            yield return null;
        }
    }
}