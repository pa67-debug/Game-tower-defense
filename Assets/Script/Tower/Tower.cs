using UnityEngine;

public class Tower : MonoBehaviour
{
    public UnitData data;
    public int currentLevel = 0;

    public GameObject rangeIndicatorEx;
    public GameObject rangeDetectorObj;

    private SphereCollider rangeDetector;

    float range;
    int totalCost = 0;

    public BaseSlot mySlot;

    [Header("Buff UI")]
    public GameObject buffIcon;

    // 🔥 NEW: ปรับความสูงได้
    [Header("Placement Settings")]
    public float heightOffset = 0.5f;
    public bool autoDetectHeight = true;

    void Start()
    {
        if (rangeDetectorObj != null)
        {
            rangeDetector = rangeDetectorObj.GetComponent<SphereCollider>();

            rangeDetectorObj.layer = LayerMask.NameToLayer("Ignore Raycast");

            if (rangeDetector != null)
                rangeDetector.isTrigger = true;
        }

        totalCost = data.GetPrice(0);

        // 🔥 Auto คำนวณความสูงจาก Collider
        if (autoDetectHeight)
        {
            AutoSetHeight();
        }

        ApplyStats();

        if (rangeIndicatorEx != null)
            rangeIndicatorEx.SetActive(false);

        // 🔥 Snap ลง slot ตอนเริ่ม
        if (mySlot != null)
        {
            SnapToSlot();
        }
    }

    void ApplyStats()
    {
        if (data == null) return;

        range = data.GetRange(currentLevel);

        if (rangeIndicatorEx != null)
        {
            float diameter = range * 2f;
            rangeIndicatorEx.transform.localScale =
                new Vector3(diameter, 0.01f, diameter);
        }

        if (rangeDetector != null)
        {
            rangeDetector.radius = range;
        }

        if (mySlot != null)
        {
            mySlot.UpdateColor();
        }
    }

    // =========================
    // 🔥 Placement System
    // =========================

    public void SnapToSlot()
    {
        transform.position = mySlot.transform.position + Vector3.up * heightOffset;
    }

    void AutoSetHeight()
    {
        Collider col = GetComponentInChildren<Collider>();

        if (col != null)
        {
            heightOffset = col.bounds.extents.y;
        }
    }

    // =========================
    // 🔥 Support Buff
    // =========================
    public float GetSupportBuff()
    {
        float totalBuff = 0f;
        bool isBuffed = false;

        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        foreach (var hit in hits)
        {
            Tower t = hit.GetComponentInParent<Tower>();

            if (t != null && t != this && t.data.type == UnitType.Support)
            {
                float buff = t.data.GetBuff(t.currentLevel);
                totalBuff += buff;

                if (buff > 0)
                    isBuffed = true;
            }
        }

        if (buffIcon != null)
            buffIcon.SetActive(isBuffed);

        return totalBuff;
    }

    public float GetFinalDamage()
    {
        float baseDamage = data.GetDamage(currentLevel);
        float buff = GetSupportBuff();
        return baseDamage * (1f + buff);
    }

    void OnMouseDown()
    {
        ShowRange(true);

        if (TowerUIManager.instance != null)
        {
            TowerUIManager.instance.OpenUI(this);
        }
    }

    public void ShowRange(bool show)
    {
        if (rangeIndicatorEx != null)
            rangeIndicatorEx.SetActive(show);
    }

    public void Upgrade()
    {
        if (currentLevel >= data.maxLevel - 1) return;

        int cost = data.GetUpgradeCost(currentLevel);
        totalCost += cost;

        currentLevel++;
        ApplyStats();
    }

    public void Sell()
    {
        int refund = Mathf.RoundToInt(totalCost * data.sellPercent);

        if (PlayerMoney.instance != null)
            PlayerMoney.instance.Add(refund);

        if (mySlot != null)
            mySlot.ClearSlot();

        if (WaveManager.instance != null)
            WaveManager.instance.RemoveTower(data.type);

        Destroy(gameObject);
    }

    public int GetTotalCost()
    {
        return totalCost;
    }

    public void GiveFarmIncome()
    {
        if (data.type != UnitType.Farm) return;

        int income = data.GetIncome(currentLevel);

        if (PlayerMoney.instance != null)
        {
            PlayerMoney.instance.Add(income);
        }
    }

    public void RotateToEnemy(Transform target)
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                rot,
                Time.deltaTime * 10f
            );
        }
    }
}