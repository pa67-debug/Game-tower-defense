using UnityEngine;

public class Tower : MonoBehaviour
{
    public UnitData data;
    public int currentLevel = 0;

    public GameObject rangeIndicatorEx;
    public GameObject rangeDetectorObj;

    private SphereCollider rangeDetector;

    float range;
    float attackCooldown;
    float timer;

    int totalCost = 0;

    public BaseSlot mySlot;

    [Header("Buff UI")]
    public GameObject buffIcon; // 🔥 ลากไอคอนดาบมาใส่

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

        ApplyStats();

        if (rangeIndicatorEx != null)
        {
            rangeIndicatorEx.SetActive(false);
        }
    }

    void Update()
    {
        // 🔥 Farm + Support ไม่ยิง
        if (data.type == UnitType.Farm || data.type == UnitType.Support)
        {
            GetSupportBuff(); // 🔥 ให้มันเช็ค buff ด้วย
            return;
        }

        // 🔥 เช็ค buff ตลอด
        GetSupportBuff();

        timer += Time.deltaTime;

        if (timer >= attackCooldown)
        {
            timer = 0f;
            Attack();
        }
    }

    void ApplyStats()
    {
        if (data == null) return;

        range = data.GetRange(currentLevel);
        attackCooldown = data.GetAttackSpeed(currentLevel);

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

    void Attack()
    {
        int layerMask = LayerMask.GetMask("Enemy");

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            range,
            layerMask
        );

        if (hits.Length == 0) return;

        Enemy target = hits[0].GetComponentInParent<Enemy>();

        if (target == null) return;

        float baseDamage = data.GetDamage(currentLevel);

        // 🔥 รับบัพจาก Support
        float buff = GetSupportBuff();

        float finalDamage = baseDamage * (1f + buff);

        target.TakeDamage(finalDamage, data.type);
    }

    // =========================
    // 🔥 NEW: รับบัพจาก Support
    // =========================
    float GetSupportBuff()
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

        // 🔥 เปิด/ปิด UI
        if (buffIcon != null)
            buffIcon.SetActive(isBuffed);

        return totalBuff;
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

        Debug.Log("Sell ได้เงิน: " + refund);

        if (PlayerMoney.instance != null)
        {
            PlayerMoney.instance.Add(refund);
        }

        if (mySlot != null)
        {
            mySlot.ClearSlot();
        }

        if (WaveManager.instance != null)
        {
            WaveManager.instance.RemoveTower(data.type);
        }

        if (!WaveManager.instance.CanBuild(data.type))
        {
            Debug.Log("เต็มแล้ว!");
            return;
        }

        // Instantiate...

        WaveManager.instance.RegisterTower(data.type);
        Destroy(gameObject);
    }

    public int GetTotalCost()
    {
        return totalCost;
    }

    // =========================
    // 🔥 Farm ให้เงิน
    // =========================
    public void GiveFarmIncome()
    {
        if (data.type != UnitType.Farm) return;

        int income = data.GetIncome(currentLevel);

        Debug.Log("Farm +" + income);

        if (PlayerMoney.instance != null)
        {
            PlayerMoney.instance.Add(income);
        }
    }
    public float GetFinalDamage()
    {
        float baseDamage = data.GetDamage(currentLevel);

        // 🔥 เอาบัพจริง
        float buff = GetSupportBuff();

        return baseDamage * (1f + buff);
    }
}