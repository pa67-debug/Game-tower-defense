using UnityEngine;

public class UnitButton : MonoBehaviour
{
    public UnitData data;

    public void Click()
    {
        // 🔥 เลือกยูนิต
        TowerBuildUI.instance.SelectUnit(data);

        // 🔥 ขยับกรอบเหลือง
        UnitSelector.instance.Select(data, GetComponent<RectTransform>());
    }
}