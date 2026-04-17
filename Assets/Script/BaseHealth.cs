using UnityEngine;
using TMPro;

public class BaseHealth : MonoBehaviour
{
    public static BaseHealth instance;

    public int maxHP = 100;
    int currentHP;

    public TMP_Text hpText; // 🔥 เพิ่ม
    public GameObject loseUI;

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

        UpdateHPText(); // 🔥 อัปเดต UI

        if (currentHP <= 0)
        {
            GameOver();
        }
    }

    void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = $"HP: {currentHP} / {maxHP}";
        }
    }

    void GameOver()
    {
        Debug.Log("YOU LOSE");

        if (loseUI != null)
            loseUI.SetActive(true);

        Time.timeScale = 0f;
    }
}