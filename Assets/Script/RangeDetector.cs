using UnityEngine;
using System.Collections.Generic;

public class RangeDetector : MonoBehaviour
{
    public Tower tower;

    private List<Enemy> enemies = new List<Enemy>();
    float timer;

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
        if (enemies.Count == 0) return;

        timer += Time.deltaTime;

        // 🔥 แก้ตรงนี้
        float attackCooldown = tower.data.GetAttackSpeed(tower.currentLevel);

        if (timer >= attackCooldown)
        {
            Enemy target = GetClosest();

            if (target != null)
            {
                float dmg = tower.data.GetDamage(tower.currentLevel);
                target.TakeDamage(dmg, tower.data.type);
            }

            timer = 0f;
        }
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
}