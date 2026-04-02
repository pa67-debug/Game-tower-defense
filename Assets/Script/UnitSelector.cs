using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static UnitSelector instance;

    public RectTransform highlight;
    public UnitData selectedUnit;

    void Awake()
    {
        instance = this;
    }

    public void Select(UnitData data, RectTransform button)
    {
        selectedUnit = data;

        // 🔥 เพิ่มแค่บรรทัดนี้
        TowerBuildUI.instance.selectedUnit = data;

        highlight.gameObject.SetActive(true);
        highlight.position = button.position;
    }
}