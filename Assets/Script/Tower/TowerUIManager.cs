using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    public static TowerUIManager instance;

    public GameObject panel;

    public TMP_Text nameText;
    public TMP_Text damageText;
    public TMP_Text rangeText;
    public TMP_Text speedText;

    public Image towerImage;
    public Image iconImage;
    public Image backgroundImage;

    private Tower currentTower;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CloseUI();
        }
    }

    public void OpenUI(Tower tower)
    {
        currentTower = tower;
        panel.SetActive(true);
        UpdateUI();
    }

    void UpdateUI()
    {
        var data = currentTower.data;
        int lv = currentTower.currentLevel;

        nameText.text = "Name: " + data.unitName;

        // DAMAGE
        float dmg = data.GetDamage(lv);
        if (lv < data.maxLevel - 1)
            damageText.text = $"Damage: {dmg} → {data.GetDamage(lv + 1)}";
        else
            damageText.text = $"Damage: {dmg} (MAX)";

        // RANGE
        float range = data.GetRange(lv);
        if (lv < data.maxLevel - 1)
            rangeText.text = $"Range: {range} → {data.GetRange(lv + 1)}";
        else
            rangeText.text = $"Range: {range} (MAX)";

        // SPEED 🔥
        float spd = data.GetAttackSpeed(lv);
        if (lv < data.maxLevel - 1)
            speedText.text = $"Speed: {spd} → {data.GetAttackSpeed(lv + 1)}";
        else
            speedText.text = $"Speed: {spd} (MAX)";

        // IMAGE
        towerImage.sprite = data.towerImage;
        iconImage.sprite = data.iconImage;
        backgroundImage.sprite = data.backgroundImage;
    }

    public void CloseUI()
    {
        panel.SetActive(false);

        if (currentTower != null)
        {
            currentTower.ShowRange(false);
            currentTower = null;
        }
    }

    public void Upgrade()
    {
        if (currentTower == null) return;

        if (currentTower.currentLevel >= currentTower.data.maxLevel - 1)
        {
            Debug.Log("MAX LEVEL");
            return;
        }

        currentTower.Upgrade();
        UpdateUI();
    }

    public void Sell()
    {
        if (currentTower == null) return;

        currentTower.Sell();
        CloseUI();
    }
}