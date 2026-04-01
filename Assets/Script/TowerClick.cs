using UnityEngine;

public class TowerClick : MonoBehaviour
{
    public Tower tower;

    static Tower currentTower; // 🔥 จำตัวที่เลือกอยู่

    void OnMouseDown()
    {
        TowerUI ui = FindObjectOfType<TowerUI>();

        if (ui != null)
        {
            ui.SelectTower(tower);
        }

        // 🔥 ปิดตัวเก่าก่อน
        if (currentTower != null)
        {
            currentTower.ShowRange(false);
        }

        // 🔥 เปิดตัวใหม่
        tower.ShowRange(true);
        currentTower = tower;
    }
}