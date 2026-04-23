using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangeDetector : MonoBehaviour
{
    public Tower tower;

    private List<Enemy> enemies = new List<Enemy>();
    float timer;

    Animator anim;
    bool isAttacking = false;

    ArcherLine archerLine; // 🔥 เพิ่ม

    void Start()
    {
        if (tower != null)
        {
            anim = tower.GetComponentInChildren<Animator>();
            archerLine = tower.GetComponent<ArcherLine>(); // 🔥 หา ArcherLine
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Enemy e = other.GetComponentInParent<Enemy>();
        if (e != null && !enemies.Contains(e))
            enemies.Add(e);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Enemy e = other.GetComponentInParent<Enemy>();
        if (e != null)
            enemies.Remove(e);
    }

    void Update()
    {
        if (tower == null) return;

        CleanList();

        if (enemies.Count == 0) return;

        timer += Time.deltaTime;

        float attackCooldown = tower.data.GetAttackSpeed(tower.currentLevel);

        Enemy target = GetClosest();
        if (target == null) return;

        tower.RotateToEnemy(target.transform);

        if (timer >= attackCooldown && !isAttacking)
        {
            StartCoroutine(AttackRoutine(target, attackCooldown));
        }
    }

    IEnumerator AttackRoutine(Enemy target, float delay)
    {
        isAttacking = true;

        // 🔥 เล่นอนิเมชั่น
        if (anim != null)
            anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.2f); // timing ฟัน

        if (target != null)
        {
            float dmg = tower.GetFinalDamage();
            target.TakeDamage(dmg, tower.data.type);

            // 🔥 ยิงเส้นตรงนี้!!
            if (archerLine != null)
                archerLine.ShootLine(target.transform);
        }

        yield return new WaitForSeconds(delay - 0.2f);

        isAttacking = false;
        timer = 0f;
    }

    Enemy GetClosest()
    {
        Enemy closest = null;
        float min = Mathf.Infinity;

        foreach (var e in enemies)
        {
            if (e == null) continue;

            float d = Vector3.Distance(transform.position, e.transform.position);

            if (d < min)
            {
                min = d;
                closest = e;
            }
        }

        return closest;
    }

    void CleanList()
    {
        enemies.RemoveAll(e => e == null);
    }
}