using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static UnitSelector instance;

    void Awake()
    {
        instance = this;
    }

    public void Select(UnitData data, RectTransform target)
    {
        gameObject.SetActive(true);
        transform.position = target.position;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}