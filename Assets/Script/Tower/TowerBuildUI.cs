using UnityEngine;

public class TowerBuildUI : MonoBehaviour
{
    public static TowerBuildUI instance;

    public UnitData selectedUnit;

    void Awake()
    {
        instance = this;
    }

    public void SelectUnit(UnitData data)
    {
        selectedUnit = data;

        if (data == null)
        {
            Debug.Log("ยกเลิกเลือก");
            return;
        }

        Debug.Log("เลือก: " + data.unitName);
    }

    // 🔥 เปลี่ยนจาก void → bool
    public bool Build(BaseSlot slot)
    {
        if (selectedUnit == null)
        {
            Debug.Log("❌ ยังไม่ได้เลือกยูนิต");
            return false;
        }

        if (slot.isOccupied)
        {
            Debug.Log("❌ ช่องนี้มีแล้ว");
            return false;
        }

        // 🔥 LIMIT
        if (!WaveManager.instance.CanBuild(selectedUnit.type))
        {
            Debug.Log("❌ " + selectedUnit.type + " เต็มแล้ว!");
            return false;
        }

        int cost = selectedUnit.GetPrice(0);

        // 🔥 ใช้ Spend แบบมี return
        if (!PlayerMoney.instance.Spend(cost))
        {
            Debug.Log("เงินไม่พอ");
            return false;
        }

        GameObject towerObj = Instantiate(
            selectedUnit.prefab,
            slot.transform.position,
            Quaternion.identity
        );

        Tower tower = towerObj.GetComponent<Tower>();

        if (tower != null)
        {
            tower.data = selectedUnit;

            // 🔥 ผูก slot
            tower.mySlot = slot;
            slot.SetOccupied(tower);

            // 🔥 นับจำนวน
            WaveManager.instance.RegisterTower(selectedUnit.type);
        }
        else
        {
            Debug.LogError("Prefab ไม่มี Tower Script!");
            return false;
        }

        return true; // ✔ สำเร็จ
    }
}