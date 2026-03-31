using UnityEngine;

public class Farm : MonoBehaviour
{
    public UnitData data;
    public int level = 0;

    public void GiveIncome()
    {
        int income = data.incomePerWave[level];
        PlayerMoney.instance.Add(income);
    }
}