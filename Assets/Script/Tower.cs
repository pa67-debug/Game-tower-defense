using UnityEngine;

public class Tower : MonoBehaviour
{
    public UnitData data;
    public int currentLevel = 0;

    [Header("Range Visual (โชว์ตอนกด)")]
    public GameObject rangeIndicatorEx;

    [Header("Range Detector (ตัวจริง ยิง)")]
    public GameObject rangeDetectorObj;

    private SphereCollider rangeDetector;

    float range;
    float attackCooldown;
    float timer;

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
            rangeIndicatorEx.layer = LayerMask.NameToLayer("Ignore Raycast");

            Collider col = rangeIndicatorEx.GetComponent<Collider>();
            if (col != null) col.enabled = false;
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
        attackCooldown = data.attackSpeed > 0 ? data.attackSpeed : 1f;

        UpdateRangeVisual();
        UpdateRangeDetector();
    }

    void UpdateRangeVisual()
    {
        if (rangeIndicatorEx == null) return;

        float diameter = range * 2f;

        rangeIndicatorEx.transform.localScale =
            new Vector3(diameter, 0.01f, diameter);
    }

    public void ShowRange(bool show)
    {
        if (rangeIndicatorEx != null)
            rangeIndicatorEx.SetActive(show);
    }

    void UpdateRangeDetector()
    {
        if (rangeDetector == null) return;

        rangeDetector.radius = range;
    }

    void Attack()
    {
        if (rangeDetector == null) return;

        int layerMask = LayerMask.GetMask("Enemy");

        Collider[] hits = Physics.OverlapSphere(
            rangeDetector.transform.position,
            rangeDetector.radius,
            layerMask
        );

        if (hits.Length == 0) return;

        Enemy target = null;
        float closest = Mathf.Infinity;

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy == null) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);

            if (dist < closest)
            {
                closest = dist;
                target = enemy;
            }
        }

        if (target == null) return;

        float damage = data.GetDamage(currentLevel);
        target.TakeDamage(damage, data.type);
    }
}