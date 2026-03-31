using UnityEngine;

public class BaseSlot : MonoBehaviour
{
    public bool occupied = false;

    // 🔥 เรียกตอนคลิกจาก Raycast
    public void OnClick()
    {
        Debug.Log("CLICK SLOT"); // ✅ ต้องอยู่ในนี้

        if (occupied) return;

        if (UnitSelector.instance.selectedUnit == null)
        {
            Debug.Log("No unit selected");
            return;
        }

        UnitData data = UnitSelector.instance.selectedUnit;

        if (data.prefab == null)
        {
            Debug.LogError("No prefab assigned!");
            return;
        }

        int cost = data.prices[0];

        if (!PlayerMoney.instance.Spend(cost))
        {
            Debug.Log("Not enough money");
            return;
        }

        Instantiate(data.prefab, transform.position, Quaternion.identity);

        SetOccupied();
    }

    public void SetOccupied()
    {
        occupied = true;
    }
}