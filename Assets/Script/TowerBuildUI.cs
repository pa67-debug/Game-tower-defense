using UnityEngine;

public class TowerBuildUI : MonoBehaviour
{
    public static TowerBuildUI instance;

    public GameObject panel;
    BaseSlot currentSlot;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public void Open(BaseSlot slot)
    {
        currentSlot = slot;
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    // 🔥 เรียกตอนกดเลือกตัวละคร
    public void Build(UnitData data)
    {
        int cost = data.prices[0];

        if (!PlayerMoney.instance.Spend(cost))
        {
            Debug.Log("Not enough money");
            return;
        }

        GameObject tower = Instantiate(data.prefab, currentSlot.transform.position, Quaternion.identity);

        currentSlot.SetOccupied(); // 🔥 กันวางซ้ำ
        Close();
    }
}