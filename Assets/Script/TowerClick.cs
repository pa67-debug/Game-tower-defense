using UnityEngine;

public class TowerClick : MonoBehaviour
{
    public Tower tower;

    // 🔥 ต้องเป็น public เพราะ ClickDetect เรียก
    public void OnMouseDown()
    {
        // ปิดวงของทุกตัวก่อน
        Tower[] all = FindObjectsOfType<Tower>();

        foreach (var t in all)
            t.ShowRange(false);

        // เปิดของตัวที่กด
        if (tower != null)
            tower.ShowRange(true);

        Debug.Log("✅ Select Tower: " + gameObject.name);
    }
}