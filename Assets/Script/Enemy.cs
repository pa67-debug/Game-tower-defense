using UnityEngine;
using TMPro;

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
    public string enemyName = "Enemy"; // 🔥 ชื่อศัตรู

    [Header("Stats")]
    public float maxHP = 15;
    float currentHP;

    public float speed = 2f;
    public int shieldHits = 0;
    public int reward = 3;

    [Header("Path")]
    public Transform[] waypoints;
    int currentWaypoint = 0;

    [Header("UI")]
    public TextMeshPro hpText;

    void Start()
    {
        currentHP = maxHP;
        UpdateUI();
    }

    void Update()
    {
        Move();
        UpdateUI();
    }

    void Move()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 dir = (target.position - transform.position).normalized;

        // 🔥 หันหน้า
        if (dir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                ReachGoal();
            }
        }
    }

    void ReachGoal()
    {
        BaseHealth.instance.TakeDamage(1); // 🔥 บ้านโดนตี

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
            UpdateUI();
            return;
        }

        currentHP -= dmg;
        UpdateUI();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerMoney.instance.Add(reward);

        WaveManager.instance.EnemyDied(); // 🔥 แจ้งว่า ตายแล้ว

        Destroy(gameObject);
    }

    // 🔥 UI
    void UpdateUI()
    {
        if (hpText == null) return;

        // 🔥 เอาชื่อจาก Type มาเลย
        string name = type.ToString();

        string text = name + "\n";

        text += $"HP {(int)currentHP}/{(int)maxHP}";

        if (shieldHits > 0)
        {
            text += $" SHD{shieldHits}";
        }

        hpText.text = text;
    }
}
