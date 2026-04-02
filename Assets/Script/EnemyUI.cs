using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public static EnemyUI instance;

    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public Slider hpBar;

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

        hpText.text = $"HP {(int)hp}/{(int)max}";
        hpBar.value = hp / max;
    }

    void Update()
    {
        // 🔥 auto update ตลอดถ้ายังเลือกอยู่
        if (currentEnemy != null)
        {
            UpdateUI(currentEnemy);
        }
    }
}