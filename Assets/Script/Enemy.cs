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

    // 🔥 NEW: Class Icon
    public Sprite classIcon;

    [Header("Stats")]
    public float maxHP = 15;
    float currentHP;

    public float speed = 2f;
    public int shieldHits = 0;
    public int reward = 3;

    // 🔥 NEW: Armor
    [Header("Armor")]
    public float armor = 0f; // เช่น 0.2 = ลดดาเมจ 20%

    [Header("Path")]
    public Transform[] waypoints;
    int currentWaypoint = 0;

    [Header("Rotate")]
    public float rotateSpeed = 10f;

    // 🔥 FX
    [Header("FX")]
    public GameObject bloodEffectPrefab;
    public GameObject deathEffectPrefab;

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

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                rot,
                Time.deltaTime * rotateSpeed
            );
        }

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypoint++;
        }
    }

    void ReachGoal()
    {
        int damage = Mathf.RoundToInt(currentHP);

        BaseHealth.instance.TakeDamage(damage);
        WaveManager.instance.EnemyDied();

        if (EnemyUI.instance != null && EnemyUI.instance.currentEnemy == this)
        {
            EnemyUI.instance.Hide();
        }

        Destroy(gameObject);
    }

    public void TakeDamage(float dmg, UnitType attackerType)
    {
        // 🔥 กัน Flying
        if (type == EnemyType.Flying)
        {
            if (attackerType != UnitType.Ranged && attackerType != UnitType.Magic)
                return;
        }

        // 🔥 Shield
        if (shieldHits > 0)
        {
            shieldHits--;
            return;
        }

        // =========================
        // 🔥 ARMOR SYSTEM
        // =========================
        float finalDamage = dmg;

        if (armor > 0)
        {
            finalDamage = dmg * (1f - armor);
        }

        currentHP -= finalDamage;

        // 🔥 ตาย
        if (currentHP <= 0)
        {
            Die();
            return;
        }

        // 🔥 ยังไม่ตาย
        SpawnBlood();

        if (EnemyUI.instance != null && EnemyUI.instance.currentEnemy == this)
        {
            EnemyUI.instance.UpdateUI(this);
        }
    }

    void SpawnBlood()
    {
        if (bloodEffectPrefab == null) return;

        Collider col = GetComponent<Collider>();
        Vector3 pos = (col != null) ? col.bounds.center : transform.position;

        GameObject fx = Instantiate(bloodEffectPrefab, pos, Quaternion.identity);

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Play();

        Destroy(fx, 1f);
    }

    void Die()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        PlayerMoney.instance.Add(reward);
        WaveManager.instance.EnemyDied();

        if (EnemyUI.instance != null && EnemyUI.instance.currentEnemy == this)
        {
            EnemyUI.instance.Hide();
        }

        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        if (EnemyUI.instance != null)
        {
            EnemyUI.instance.Show(this);
        }
    }

    // =========================
    // 🔥 UI GETTERS
    // =========================
    public float GetHP() => currentHP;
    public float GetMaxHP() => maxHP;

    public float GetArmor() => armor;

    public Sprite GetClassIcon() => classIcon;
}