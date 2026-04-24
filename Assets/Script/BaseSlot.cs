using UnityEngine;

public class BaseSlot : MonoBehaviour
{
    public bool isOccupied = false;
    public Tower currentTower;

    [Header("Visual")]
    public Renderer rend;

    public Color emptyColor = Color.white;

    [Tooltip("Lv0, Lv1, Lv2, Lv3, Lv4")]
    public Color[] levelColors;

    [Header("Preview")]
    public Color previewColor = Color.yellow; // 🔥 สีตอนกำลังเลือก

    bool isPreviewing = false; // 🔥 สถานะ preview

    void Start()
    {
        UpdateColor();
    }

    public void OnClick()
    {
        Debug.Log("Click Slot");

        if (TowerBuildUI.instance == null)
        {
            Debug.LogError("ไม่มี TowerBuildUI");
            return;
        }

        if (isOccupied)
        {
            Debug.Log("ช่องนี้มี Tower แล้ว");
            return;
        }

        // 🔥 เปิด UI + ทำช่องเป็นสีเหลือง
        BuildConfirmUI.instance.Show(this);
    }

    // 🔥 เรียกตอน Show()
    public void SetPreview(bool active)
    {
        isPreviewing = active;
        UpdateColor();
    }

    public void SetOccupied(Tower tower)
    {
        isOccupied = true;
        currentTower = tower;

        isPreviewing = false; // 🔥 ยกเลิก preview

        Debug.Log("SetOccupied: " + tower.name);

        UpdateColor();
    }

    public void ClearSlot()
    {
        isOccupied = false;
        currentTower = null;

        isPreviewing = false;

        UpdateColor();
    }

    public void UpdateColor()
    {
        if (rend == null)
        {
            Debug.LogWarning("❌ ยังไม่ได้ใส่ Renderer");
            return;
        }

        // 🔥 ถ้ากำลัง preview → สีเหลืองก่อนเลย
        if (isPreviewing)
        {
            rend.material.color = previewColor;
            return;
        }

        if (!isOccupied || currentTower == null)
        {
            rend.material.color = emptyColor;
            return;
        }

        int lv = currentTower.currentLevel;

        if (levelColors != null && levelColors.Length > 0)
        {
            lv = Mathf.Clamp(lv, 0, levelColors.Length - 1);
            rend.material.color = levelColors[lv];
        }
    }
}