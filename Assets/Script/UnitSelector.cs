using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static UnitSelector instance;

    public UnitData selectedUnit;

    void Awake()
    {
        instance = this;
    }

    public void Select(UnitData data)
    {
        selectedUnit = data;
        Debug.Log("Selected: " + data.unitName);
    }
}