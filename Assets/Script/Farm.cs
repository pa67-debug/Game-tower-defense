using UnityEngine;

public class Farm : MonoBehaviour
{
    public UnitData data;
    public int currentLevel = 0;

    float timer;
    public float incomeInterval = 5f; // ทุกกี่วิให้เงิน

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= incomeInterval)
        {
            timer = 0f;

            // 🔥 ใช้ Getter แทน
            int income = data.GetIncome(currentLevel);

            PlayerMoney.instance.Add(income);

            Debug.Log("Farm ได้เงิน: " + income);
        }
    }
}