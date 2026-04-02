using UnityEngine;

public class UnitSelectUI : MonoBehaviour
{
    public GameObject highlight; // กรอบเหลือง

    static UnitSelectUI current; // ตัวที่เลือกอยู่

    public void Select()
    {
        // ปิดตัวเก่า
        if (current != null)
        {
            current.highlight.SetActive(false);
        }

        // เปิดตัวใหม่
        highlight.SetActive(true);
        current = this;

        Debug.Log("Selected: " + gameObject.name);
    }
}