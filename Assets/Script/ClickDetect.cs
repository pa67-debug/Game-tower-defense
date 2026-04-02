using UnityEngine;

public class ClickDetect : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("No Main Camera!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cam == null) return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit: " + hit.collider.name);

                BaseSlot slot = hit.collider.GetComponent<BaseSlot>();

                if (slot != null)
                {
                    slot.OnClick();
                }
            }
        }
    }
}