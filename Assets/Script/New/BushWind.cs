using UnityEngine;

public class BushWind : MonoBehaviour
{
    public float speed = 2f;      // §«“¡‡√Á«≈¡
    public float strength = 5f;   // ·√ß≈¡

    Vector3 startRot;

    void Start()
    {
        startRot = transform.eulerAngles;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * strength;
        transform.eulerAngles = startRot + new Vector3(0, angle, 0);
    }
}