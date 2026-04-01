using UnityEngine;

public class Tower : MonoBehaviour
{
    public UnitData data;
    public int currentLevel = 0;

    [Header("Range Visual")]
    public GameObject rangeIndicator;

    float baseDamage;
    float range;
    float attackCooldown;

    float timer;
    float buffMultiplier = 1f;

    void Start()
    {
        ApplyStats(); // 👈 จะเรียก UpdateRangeVisual() ข้างในแล้ว

        if (rangeIndicator != null)
            rangeIndicator.SetActive(false);
    }

    void Update()
    {
        buffMultiplier = 1f;

        if (data.type == UnitType.Farm) return;

        timer += Time.deltaTime;

        if (timer >= attackCooldown)
        {
            if (data.type == UnitType.Support)
                Buff();
            else
                Attack();

            timer = 0f;
        }
    }

    void ApplyStats()
    {
        if (data == null) return;

        baseDamage = data.GetDamage(currentLevel);
        range = data.GetRange(currentLevel);

        // 🔥 DEBUG เช็คค่า
        Debug.Log("Range ตอนนี้ = " + range);

        if (data.attackSpeed > 0)
            attackCooldown = 1f / data.attackSpeed;
        else
            attackCooldown = 1f;

        UpdateRangeVisual(); // 👈 ย้ายมาไว้ตรงนี้ (สำคัญ!)
    }

    // 🔥 อัปเดตขนาดวง (แก้ให้ชัวร์)
    void UpdateRangeVisual()
    {
        if (rangeIndicator == null) return;

        float size = range * 2f;

        rangeIndicator.transform.localScale = new Vector3(
            size,
            0.05f,   // 👈 บางลง จะดูเหมือนวงมากขึ้น
            size
        );

        // 👉 บังคับตำแหน่งให้ตรงพื้นเสมอ
        rangeIndicator.transform.localPosition = new Vector3(0, 0.05f, 0);
    }

    public void ShowRange(bool show)
    {
        if (rangeIndicator != null)
            rangeIndicator.SetActive(show);
    }

    void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null) continue;

            float finalDamage = GetFinalDamage();

            if (data.type == UnitType.Magic)
            {
                float aoe = data.GetAOE(currentLevel);

                Collider[] aoeHits = Physics.OverlapSphere(hit.transform.position, aoe);

                foreach (var aoeHit in aoeHits)
                {
                    if (!aoeHit.CompareTag("Enemy")) continue;

                    Enemy e = aoeHit.GetComponent<Enemy>();
                    if (e != null)
                        e.TakeDamage(finalDamage, data.type);
                }
            }
            else
            {
                enemy.TakeDamage(finalDamage, data.type);
            }

            break;
        }
    }

    void Buff()
    {
        float buff = data.GetBuff(currentLevel);

        Collider[] towers = Physics.OverlapSphere(transform.position, range);

        foreach (var t in towers)
        {
            Tower other = t.GetComponent<Tower>();

            if (other == null || other == this) continue;
            if (other.data.type == UnitType.Support) continue;

            other.AddBuff(buff);
        }
    }

    public void AddBuff(float amount)
    {
        buffMultiplier += amount;
    }

    public float GetFinalDamage()
    {
        return baseDamage * buffMultiplier;
    }

    public bool CanUpgrade()
    {
        return currentLevel < data.maxLevel - 1;
    }

    public void Upgrade()
    {
        if (!CanUpgrade()) return;

        currentLevel++;
        ApplyStats(); // 👈 จะอัปเดตวงอัตโนมัติ
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = (data != null && data.type == UnitType.Support) ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}