using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

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

    [Header("FX Settings")]
    public float fxDelay = 1f; // 🔥 คูลดาว 1 วิ

    AudioSource audioSource;

    Queue<int> fxQueue = new Queue<int>();
    bool isPlayingFX = false;

    void Awake()
    {
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

        Debug.Log("เงินตอนนี้: " + money);

        UpdateUI();

        if (moneySound != null)
            audioSource.PlayOneShot(moneySound);

        // 🔥 ใส่เข้าคิว
        fxQueue.Enqueue(amount);

        // 🔥 ถ้ายังไม่ทำงาน → เริ่ม
        if (!isPlayingFX)
        {
            StartCoroutine(PlayFXQueue());
        }
    }

    IEnumerator PlayFXQueue()
    {
        isPlayingFX = true;

        while (fxQueue.Count > 0)
        {
            int amount = fxQueue.Dequeue();

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

            yield return new WaitForSeconds(fxDelay); // 🔥 หน่วง 1 วิ
        }

        isPlayingFX = false;
    }

    public bool Spend(int amount)
    {
        if (money < amount) return false;

        money -= amount;

        Debug.Log("เงินตอนนี้: " + money);

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