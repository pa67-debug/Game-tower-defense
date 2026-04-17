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

    public TMP_Text upgradeCostText;
    public TMP_Text sellValueText;

    public Image towerImage;
    public Image iconImage;
    public Image backgroundImage;

    // 🔊 SOUND
    public AudioClip upgradeSound;
    private AudioSource audioSource;

    private Tower currentTower;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CloseUI();
        }

        // 🔥 อัปเดต real-time
        if (currentTower != null && panel.activeSelf)
        {
            UpdateUI();
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

        // =========================
        // 🔥 แยก Farm กับ Tower ปกติ
        // =========================
        if (data.type == UnitType.Farm)
        {
            int income = data.GetIncome(lv);

            if (lv < data.maxLevel - 1)
                damageText.text = $"Money: {income} → {data.GetIncome(lv + 1)}";
            else
                damageText.text = $"Money: {income} (MAX)";
        }
        else
        {
            float dmg = currentTower.GetFinalDamage();

            if (lv < data.maxLevel - 1)
                damageText.text = $"Damage: {dmg} → {data.GetDamage(lv + 1)}";
            else
                damageText.text = $"Damage: {dmg} (MAX)";
        }

        // =========================
        // Range
        // =========================
        float range = data.GetRange(lv);
        if (lv < data.maxLevel - 1)
            rangeText.text = $"Range: {range} → {data.GetRange(lv + 1)}";
        else
            rangeText.text = $"Range: {range} (MAX)";

        // =========================
        // Speed
        // =========================
        float spd = data.GetAttackSpeed(lv);
        if (lv < data.maxLevel - 1)
            speedText.text = $"Speed: {spd} → {data.GetAttackSpeed(lv + 1)}";
        else
            speedText.text = $"Speed: {spd} (MAX)";

        // =========================
        // Upgrade Cost
        // =========================
        if (lv < data.maxLevel - 1)
        {
            int cost = data.GetUpgradeCost(lv);
            upgradeCostText.text = $"G {cost}";
        }
        else
        {
            upgradeCostText.text = "MAX";
        }

        // =========================
        // Sell
        // =========================
        int totalCost = currentTower.GetTotalCost();
        int sellValue = Mathf.RoundToInt(totalCost * data.sellPercent);
        sellValueText.text = $"G {sellValue}";

        // =========================
        // Images
        // =========================
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

        // 🔊 เล่นเสียง
        if (upgradeSound != null)
        {
            audioSource.PlayOneShot(upgradeSound);
        }

        UpdateUI();
    }

    public void Sell()
    {
        if (currentTower == null) return;

        currentTower.Sell();
        CloseUI();
    }
}