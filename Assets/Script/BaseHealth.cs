using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseHealth : MonoBehaviour
{
    public static BaseHealth instance;

    public int maxHP = 100;
    int currentHP;

    public TMP_Text hpText;
    public GameObject loseUI;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip hitSound;

    [Header("Lose Sound")]
    public AudioClip loseSound;   // 🔊 เสียงแพ้
    public AudioClip loseMusic;   // 🎼 เพลงแพ้ (optional)

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHP = maxHP;
        UpdateHPText();

        if (loseUI != null)
            loseUI.SetActive(false);
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP < 0) currentHP = 0;

        UpdateHPText();

        // 🔥 เสียงโดนตี
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHP <= 0)
        {
            GameOver();
        }
    }

    void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = $"Base\nHP {currentHP}/{maxHP}";
        }
    }

    void GameOver()
    {
        Debug.Log("YOU LOSE");

        if (loseUI != null)
            loseUI.SetActive(true);

        // 🔥 หยุดเสียงทั้งหมด
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in allAudio)
        {
            a.Stop();
        }

        // 🔥 เล่นเสียงแพ้
        if (audioSource != null && loseSound != null)
        {
            audioSource.ignoreListenerPause = true;
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(loseSound);
        }

        // 🔥 เล่นเพลงแพ้ (ถ้ามี)
        if (audioSource != null && loseMusic != null)
        {
            audioSource.clip = loseMusic;
            audioSource.loop = true;
            audioSource.PlayDelayed(0.2f);
        }

        // 🔥 ค่อยหยุดเกม
        Time.timeScale = 0f;
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}