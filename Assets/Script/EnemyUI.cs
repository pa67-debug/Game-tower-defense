using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public static EnemyUI instance;

    [Header("UI")]
    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;

    [Header("HP Image")]
    public Image hpImage; // 🔥 ใช้แค่ตัวนี้พอ

    [Header("HP Sprites")]
    public Sprite greenSprite;   // 100%
    public Sprite yellowSprite;  // <=50%
    public Sprite redSprite;     // <=30%
    public Sprite blackSprite;   // 0%

    public Enemy currentEnemy;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public void Show(Enemy enemy)
    {
        currentEnemy = enemy;
        panel.SetActive(true);
        UpdateUI(enemy);
    }

    public void Hide()
    {
        panel.SetActive(false);
        currentEnemy = null;
    }

    public void UpdateUI(Enemy enemy)
    {
        if (enemy == null) return;

        nameText.text = enemy.enemyName;

        float hp = enemy.GetHP();
        float max = enemy.GetMaxHP();

        float percent = Mathf.Clamp01(hp / max);

        hpText.text = $"HP {(int)hp}/{(int)max}";

        // 🔥 เปลี่ยน "ภาพล้วน"
        if (percent <= 0f)
        {
            hpImage.sprite = blackSprite;
        }
        else if (percent <= 0.3f)
        {
            hpImage.sprite = redSprite;
        }
        else if (percent <= 0.5f)
        {
            hpImage.sprite = yellowSprite;
        }
        else
        {
            hpImage.sprite = greenSprite;
        }
    }

    void Update()
    {
        if (currentEnemy != null)
        {
            UpdateUI(currentEnemy);
        }
    }
}