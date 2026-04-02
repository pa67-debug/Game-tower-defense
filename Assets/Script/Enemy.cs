using UnityEngine;

public enum EnemyType
{
    Normal,
    Armored,
    Flying,
    Boss
}

public class Enemy : MonoBehaviour
{
    public EnemyType type;

    [Header("Info")]
    public string enemyName = "Enemy";

    [Header("Stats")]
    public float maxHP = 15;
    float currentHP;

    public float speed = 2f;
    public int shieldHits = 0;
    public int reward = 3;

    [Header("Path")]
    public Transform[] waypoints;
    int currentWaypoint = 0;

    void Start()
    {
        currentHP = maxHP;
        currentWaypoint = 0;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (currentWaypoint >= waypoints.Length)
        {
            ReachGoal();
            return;
        }

        Transform target = waypoints[currentWaypoint];

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypoint++;
        }
    }

    void ReachGoal()
    {
        BaseHealth.instance.TakeDamage(1);
        WaveManager.instance.EnemyDied();
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg, UnitType attackerType)
    {
        if (type == EnemyType.Flying)
        {
            if (attackerType != UnitType.Ranged && attackerType != UnitType.Magic)
                return;
        }

        if (shieldHits > 0)
        {
            shieldHits--;
            return;
        }

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        // 🔥 อัปเดต UI ถ้าถูกเลือกอยู่
        if (EnemyUI.instance != null && EnemyUI.instance.currentEnemy == this)
        {
            EnemyUI.instance.UpdateUI(this);
        }
    }

    void Die()
    {
        PlayerMoney.instance.Add(reward);
        WaveManager.instance.EnemyDied();

        // 🔥 ถ้ากำลังเลือกอยู่ → ปิด UI
        if (EnemyUI.instance != null && EnemyUI.instance.currentEnemy == this)
        {
            EnemyUI.instance.Hide();
        }

        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        // 🔥 คลิกศัตรู
        EnemyUI.instance.Show(this);
    }

    public float GetHP() => currentHP;
    public float GetMaxHP() => maxHP;
}