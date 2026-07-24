using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -6);
    public float smoothTime = 0.15f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. World Position calculate karo (TransformPoint ki jagah)
        // Isse car ke barik vibrations camera mein nahi aayenge
        Vector3 targetPos = target.position + (target.rotation * offset);

        // 2. SmoothDamp se follow karo
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // 3. Camera ko car ki taraf focus rakho
        transform.LookAt(target.position + Vector3.up);
    }
}