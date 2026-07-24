using UnityEngine;

public class RearCameraFollow : MonoBehaviour
{
    public Transform car;

    public float distance = 6f;
    public float height = 2f;

    public float positionSmooth = 5f;
    public float rotationSmooth = 5f;

    void LateUpdate()
    {
        // Target position (car ke piche)
        Vector3 targetPosition = car.position - car.forward * distance + Vector3.up * height;

        // Smooth position
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionSmooth * Time.deltaTime);

        // Target rotation (180 for rear view)
        Quaternion targetRotation = car.rotation * Quaternion.Euler(0f, 180f, 0f);

        // Smooth rotation 🔥
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth * Time.deltaTime);
    }
}