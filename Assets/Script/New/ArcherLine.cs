using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class ArcherLine : MonoBehaviour
{
    LineRenderer line;

    [Header("ยิงออกจากจุดนี้")]
    public Transform firePoint;

    [Header("ตั้งค่าเส้น")]
    public float lineDuration = 0.1f;
    public float lineWidth = 0.1f;
    public Color lineColor = Color.yellow;

    void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.positionCount = 2;
        line.enabled = false;

        // 🔥 กันพลาดทั้งหมด
        line.useWorldSpace = true;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        line.startColor = lineColor;
        line.endColor = lineColor;

        // 🔥 ใส่ material ถ้ายังไม่มี
        if (line.material == null)
        {
            line.material = new Material(Shader.Find("Unlit/Color"));
        }
    }

    public void ShootLine(Transform target)
    {
        if (target == null) return;

        StopAllCoroutines();
        StartCoroutine(ShowLine(target));
    }

    IEnumerator ShowLine(Transform target)
    {
        line.enabled = true;

        Vector3 startPos = (firePoint != null) ? firePoint.position : transform.position;
        Vector3 endPos = target.position + Vector3.up * 1f;

        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);

        yield return new WaitForSeconds(lineDuration);

        line.enabled = false;
    }
}