using UnityEngine;

public class Tower : MonoBehaviour
{
    public UnitData data;
    public int currentLevel = 0;

    float baseDamage;
    float range;
    float attackCooldown;

    float timer;
    float buffMultiplier = 1f;

    void Start()
    {
        ApplyStats();
    }

    void Update()
    {
        // 🔥 รีเซ็ตบัพทุกเฟรม
        buffMultiplier = 1f;

        // 💰 Farm ไม่โจมตี
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
        baseDamage = data.GetDamage(currentLevel);
        range = data.GetRange(currentLevel);

        if (data.attackSpeed > 0)
            attackCooldown = 1f / data.attackSpeed;
        else
            attackCooldown = 1f;
    }

    // 🔥 ยิงปกติ + Mage AOE + รองรับ Flying
    void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null) continue;

            float finalDamage = GetFinalDamage();

            // 💥 Mage AOE
            if (data.type == UnitType.Magic)
            {
                float aoe = data.GetAOE(currentLevel);

                Collider[] aoeHits = Physics.OverlapSphere(hit.transform.position, aoe);

                foreach (var aoeHit in aoeHits)
                {
                    if (!aoeHit.CompareTag("Enemy")) continue;

                    Enemy e = aoeHit.GetComponent<Enemy>();
                    if (e != null)
                    {
                        e.TakeDamage(finalDamage, data.type); // 🔥 ส่ง type
                    }
                }
            }
            else
            {
                enemy.TakeDamage(finalDamage, data.type); // 🔥 ส่ง type
            }

            break;
        }
    }

    // 🟢 Support Buff
    void Buff()
    {
        float buff = data.GetBuff(currentLevel);

        Collider[] towers = Physics.OverlapSphere(transform.position, range);

        foreach (var t in towers)
        {
            Tower other = t.GetComponent<Tower>();

            if (other == null || other == this) continue;

            // ❌ ไม่บัพ Support กันเอง
            if (other.data.type == UnitType.Support) continue;

            other.AddBuff(buff);
        }
    }

    // 🔥 รวมบัพหลายตัว
    public void AddBuff(float amount)
    {
        buffMultiplier += amount;
    }

    public float GetFinalDamage()
    {
        return baseDamage * buffMultiplier;
    }

    // 🔼 Upgrade
    public bool CanUpgrade()
    {
        return currentLevel < data.maxLevel - 1;
    }

    public void Upgrade()
    {
        if (!CanUpgrade()) return;

        currentLevel++;
        ApplyStats();
    }

    // 🎯 Debug วงระยะ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = (data != null && data.type == UnitType.Support) ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}