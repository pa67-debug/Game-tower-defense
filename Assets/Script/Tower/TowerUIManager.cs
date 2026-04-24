using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("Sound")]
    public AudioClip upgradeSound;
    public AudioClip failSound;
    private AudioSource audioSource;

    private Tower currentTower;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CloseUI();
        }

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

        // 🔥 เคลียร์ selection ของ UI (กันต้องกด 2 ครั้ง)
        EventSystem.current.SetSelectedGameObject(null);
    }

    void UpdateUI()
    {
        var data = currentTower.data;
        int lv = currentTower.currentLevel;

        nameText.text = "Name: " + data.unitName;

        // =========================
        // 🔥 Farm / Support / Attack
        // =========================
        if (data.type == UnitType.Farm)
        {
            int income = data.GetIncome(lv);

            if (lv < data.maxLevel - 1)
                damageText.text = $"Money: {income} → {data.GetIncome(lv + 1)}";
            else
                damageText.text = $"Money: {income} (MAX)";
        }
        else if (data.type == UnitType.Support) // 🔥 นักบวช
        {
            float buff = data.GetBuff(lv) * 100f;

            if (lv < data.maxLevel - 1)
            {
                float nextBuff = data.GetBuff(lv + 1) * 100f;
                damageText.text = $"Buff: {buff}% → {nextBuff}%";
            }
            else
            {
                damageText.text = $"Buff: {buff}% (MAX)";
            }
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

        bool success = currentTower.Upgrade();

        if (!success)
        {
            PlaySound(failSound);
            return;
        }

        PlaySound(upgradeSound);
        UpdateUI();
    }

    public void Sell()
    {
        if (currentTower == null) return;

        // 🔥 เก็บ reference ก่อน (สำคัญมาก)
        Tower tower = currentTower;

        // 🔥 เคลียร์ UI focus
        EventSystem.current.SetSelectedGameObject(null);

        // 🔥 ปิด UI ก่อน (กัน click ซ้อน)
        CloseUI();

        // 🔥 ค่อยขาย
        tower.Sell();
    }
    void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }
}