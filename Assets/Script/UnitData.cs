using UnityEngine;

public enum UnitType
{
    Melee,
    Ranged,
    Magic,
    Support,
    Farm
}

[CreateAssetMenu(fileName = "UnitData", menuName = "TD/Unit Data")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public UnitType type;

    [Header("UI")]
    public Sprite towerImage;
    public Sprite iconImage;
    public Sprite backgroundImage;

    [Header("Upgrade Stats")]
    public int[] prices;
    public float[] damages;
    public float[] ranges;
    public float[] attackSpeeds;

    public int maxLevel = 5;

    [Header("Farm")] // 🔥 เพิ่ม
    public int[] incomePerWave;

    [Header("Prefab")]
    public GameObject prefab;

    // ===== GETTERS =====
    public float GetDamage(int level)
    {
        if (damages == null || damages.Length == 0) return 0;
        return damages[Mathf.Clamp(level, 0, damages.Length - 1)];
    }

    public float GetRange(int level)
    {
        if (ranges == null || ranges.Length == 0) return 0;
        return ranges[Mathf.Clamp(level, 0, ranges.Length - 1)];
    }

    public int GetPrice(int level)
    {
        if (prices == null || prices.Length == 0) return 0;
        return prices[Mathf.Clamp(level, 0, prices.Length - 1)];
    }

    public float GetAttackSpeed(int level)
    {
        if (attackSpeeds == null || attackSpeeds.Length == 0) return 1f;
        return attackSpeeds[Mathf.Clamp(level, 0, attackSpeeds.Length - 1)];
    }

    public int GetIncome(int level) // 🔥 เพิ่ม
    {
        if (incomePerWave == null || incomePerWave.Length == 0) return 0;
        return incomePerWave[Mathf.Clamp(level, 0, incomePerWave.Length - 1)];
    }
}