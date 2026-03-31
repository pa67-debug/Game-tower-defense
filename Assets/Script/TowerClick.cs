using UnityEngine;

public class TowerClick : MonoBehaviour
{
    public Tower tower;

    void OnMouseDown()
    {
        TowerUI ui = FindObjectOfType<TowerUI>();

        if (ui != null)
        {
            ui.SelectTower(tower);
        }
    }
}