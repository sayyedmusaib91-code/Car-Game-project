using UnityEngine;

public class BotDrive : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 20f; // Yahan se speed badha sakte ho

    void FixedUpdate()
    {
        // 'FixedUpdate' physics ke liye best hai, isse car jhatke nahi maregi
        // 'fixedDeltaTime' use karne se movement smooth ho jati hai
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
    }
}
