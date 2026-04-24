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
    public Image hpImage;

    [Header("HP Sprites")]
    public Sprite greenSprite;
    public Sprite yellowSprite;
    public Sprite redSprite;
    public Sprite blackSprite;

    // 🔥 NEW: Class Icon
    public Image classIcon;

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

        // =========================
        // 🔥 HP + SHIELD
        // =========================
        int shd = enemy.shieldHits;

        if (shd > 0)
            hpText.text = $"HP {(int)hp}/{(int)max}  SHD {shd}";
        else
            hpText.text = $"HP {(int)hp}/{(int)max}";

        // =========================
        // 🔥 HP COLOR
        // =========================
        if (percent <= 0f)
            hpImage.sprite = blackSprite;
        else if (percent <= 0.3f)
            hpImage.sprite = redSprite;
        else if (percent <= 0.5f)
            hpImage.sprite = yellowSprite;
        else
            hpImage.sprite = greenSprite;

        // =========================
        // 🔥 CLASS ICON
        // =========================
        if (classIcon != null)
        {
            classIcon.sprite = enemy.GetClassIcon();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Hide();
        }

        if (currentEnemy != null)
        {
            UpdateUI(currentEnemy);
        }
    }
}