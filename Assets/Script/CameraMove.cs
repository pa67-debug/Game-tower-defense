using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float verticalSpeed = 20f;

    [Header("Map Bounds")]
    public float minX = -100f;
    public float maxX = 100f;
    public float minZ = -100f;
    public float maxZ = 100f;

    [Header("Height Bounds")]
    public float minY = 10f;
    public float maxY = 100f;

    [Header("Look Settings")]
    public float lookSpeed = 3f;
    public float minPitch = 20f;   // ก้มสุด
    public float maxPitch = 80f;   // เงยสุด

    float currentPitch;

    void Start()
    {
        currentPitch = transform.eulerAngles.x;
    }

    void Update()
    {
        // 🔥 Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float moveY = 0f;

        if (Input.GetKey(KeyCode.Space))
            moveY = 1f;

        if (Input.GetKey(KeyCode.LeftShift))
            moveY = -1f;

        Vector3 move = new Vector3(moveX, moveY * verticalSpeed, moveZ) * moveSpeed * Time.deltaTime;
        Vector3 newPos = transform.position + move;

        // 🔒 Clamp ตำแหน่ง
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.z = Mathf.Clamp(newPos.z, minZ, maxZ);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        transform.position = newPos;

        // 🔥 Right Click = หมุนกล้องขึ้น/ลง
        if (Input.GetMouseButton(1))
        {
            float mouseY = Input.GetAxis("Mouse Y");

            currentPitch -= mouseY * lookSpeed;

            // 🔒 ล็อคองศา
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(currentPitch, transform.eulerAngles.y, 0f);
        }
    }
}