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

        TowerBuildUI.instance.Build(this);
    }

    public void SetOccupied(Tower tower)
    {
        isOccupied = true;
        currentTower = tower;

        Debug.Log("SetOccupied: " + tower.name); // 🔥 debug

        UpdateColor();
    }

    public void ClearSlot()
    {
        isOccupied = false;
        currentTower = null;

        UpdateColor();
    }

    public void UpdateColor()
    {
        if (rend == null)
        {
            Debug.LogWarning("❌ ยังไม่ได้ใส่ Renderer");
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