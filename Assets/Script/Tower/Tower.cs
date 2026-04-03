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

    void Start()
    {
        if (rangeDetectorObj != null)
        {
            rangeDetector = rangeDetectorObj.GetComponent<SphereCollider>();

            rangeDetectorObj.layer = LayerMask.NameToLayer("Ignore Raycast");

            if (rangeDetector != null)
                rangeDetector.isTrigger = true;
        }

        ApplyStats();

        if (rangeIndicatorEx != null)
        {
            rangeIndicatorEx.SetActive(false);
        }
    }

    void Update()
    {
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

        totalCost += data.GetPrice(currentLevel);

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

        // 🔥 สำคัญ: อัปสีตามเลเวล
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

        float damage = data.GetDamage(currentLevel);
        target.TakeDamage(damage, data.type);
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

        currentLevel++;
        ApplyStats(); // 🔥 จะไปเรียก UpdateColor()
    }

    public void Sell()
    {
        int refund = totalCost / 2;

        Debug.Log("Sell ได้เงิน: " + refund);

        if (PlayerMoney.instance != null)
        {
            PlayerMoney.instance.Add(refund);
        }

        // 🔥 สำคัญ: รีเซ็ต Slot + สี
        if (mySlot != null)
        {
            mySlot.ClearSlot();
        }

        Destroy(gameObject);
    }
}