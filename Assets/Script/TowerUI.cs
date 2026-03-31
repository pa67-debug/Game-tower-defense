using UnityEngine;

public class TowerUI : MonoBehaviour
{
    public GameObject panel;
    public Tower selectedTower;

    void Start()
    {
        panel.SetActive(false);
    }

    public void SelectTower(Tower tower)
    {
        selectedTower = tower;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
        selectedTower = null;
    }
}