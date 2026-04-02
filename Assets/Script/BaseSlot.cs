using UnityEngine;

public class BaseSlot : MonoBehaviour
{
    public bool isOccupied = false;
    public Tower currentTower;

    public void OnClick()
    {
        Debug.Log("Click Slot");

        if (TowerBuildUI.instance == null)
        {
            Debug.LogError("ไม่มี TowerBuildUI");
            return;
        }

        // 👉 ไม่เปิด UI แล้ว
        TowerBuildUI.instance.Build(this);
    }
    public void SetOccupied()
    {
        isOccupied = true;
    }
}