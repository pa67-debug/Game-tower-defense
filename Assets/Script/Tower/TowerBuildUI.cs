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
        Debug.Log("เลือก: " + data.unitName);
    }

    public void Build(BaseSlot slot)
    {
        if (selectedUnit == null)
        {
            Debug.Log("❌ ยังไม่ได้เลือกยูนิต");
            return;
        }

        if (slot.isOccupied)
        {
            Debug.Log("❌ ช่องนี้มีแล้ว");
            return;
        }

        // =========================
        // 🔥 NEW: เช็ค LIMIT ก่อน
        // =========================
        if (!WaveManager.instance.CanBuild(selectedUnit.type))
        {
            Debug.Log("❌ " + selectedUnit.type + " เต็มแล้ว!");
            return;
        }

        int cost = selectedUnit.GetPrice(0);

        if (!PlayerMoney.instance.Spend(cost))
        {
            Debug.Log("เงินไม่พอ");
            return;
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

            // 🔥 ผูก Slot
            tower.mySlot = slot;

            slot.SetOccupied(tower);

            // =========================
            // 🔥 NEW: นับจำนวน
            // =========================
            WaveManager.instance.RegisterTower(selectedUnit.type);
        }
        else
        {
            Debug.LogError("Prefab ไม่มี Tower Script!");
        }
    }
}