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
    public Transform fxSpawnPoint;
    public AudioClip moneySound;

    AudioSource audioSource;

    void Awake()
    {
        // 🔥 กันซ้ำ (สำคัญมาก)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

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

        Debug.Log("เงินตอนนี้: " + money); // 👈 เช็ค

        UpdateUI();

        if (moneySound != null)
            audioSource.PlayOneShot(moneySound);

        if (floatingTextPrefab != null && fxSpawnPoint != null)
        {
            GameObject obj = Instantiate(
                floatingTextPrefab,
                fxSpawnPoint.position,
                Quaternion.identity,
                fxSpawnPoint
            );

            FloatingText ft = obj.GetComponent<FloatingText>();
            if (ft != null)
                ft.Setup(amount);
        }
    }

    public bool Spend(int amount)
    {
        if (money < amount) return false;

        money -= amount;

        Debug.Log("เงินตอนนี้: " + money); // 👈 เช็ค

        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
        else
        {
            Debug.LogWarning("moneyText ยังไม่ได้ใส่!");
        }
    }
}