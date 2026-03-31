using UnityEngine;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney instance;

    public int money = 100;

    [Header("UI")]
    public TextMeshProUGUI moneyText;

    [Header("FX")]
    public GameObject floatingTextPrefab;
    public Transform fxSpawnPoint; // จุดเกิดข้อความ
    public AudioClip moneySound;

    AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        UpdateUI();
    }

    public void Add(int amount)
    {
        money += amount;
        UpdateUI();

        // 🔥 เล่นเสียง
        if (moneySound != null)
            audioSource.PlayOneShot(moneySound);

        // 🔥 แสดงข้อความลอย
        if (floatingTextPrefab != null && fxSpawnPoint != null)
        {
            GameObject obj = Instantiate(floatingTextPrefab, fxSpawnPoint.position, Quaternion.identity, fxSpawnPoint);

            FloatingText ft = obj.GetComponent<FloatingText>();
            ft.Setup(amount);
        }
    }

    public bool Spend(int amount)
    {
        if (money < amount) return false;

        money -= amount;
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }
}