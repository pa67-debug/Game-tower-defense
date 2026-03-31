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

    [Header("Upgrade Stats")]
    public int[] prices;
    public float[] damages;
    public float[] ranges;

    public float attackSpeed;
    public int maxLevel = 5;

    [Header("Support")]
    public float[] buffAmounts;

    [Header("Mage")]
    public float[] aoeRadii;

    [Header("Farm")]
    public int[] incomePerWave;

    [Header("Prefab")]
    public GameObject prefab; // 🔥 ตัว Tower จริงที่จะ spawn

    // 🔥 Safe Getter (กัน error)
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

    public float GetBuff(int level)
    {
        if (buffAmounts == null || buffAmounts.Length == 0) return 0;
        return buffAmounts[Mathf.Clamp(level, 0, buffAmounts.Length - 1)];
    }

    public float GetAOE(int level)
    {
        if (aoeRadii == null || aoeRadii.Length == 0) return 0;
        return aoeRadii[Mathf.Clamp(level, 0, aoeRadii.Length - 1)];
    }

    public int GetIncome(int level)
    {
        if (incomePerWave == null || incomePerWave.Length == 0) return 0;
        return incomePerWave[Mathf.Clamp(level, 0, incomePerWave.Length - 1)];
    }
}