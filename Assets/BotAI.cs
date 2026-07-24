using UnityEngine;

public class BotAI : MonoBehaviour
{
    public Transform[] waypoints; // Raste ke points
    public float speed = 15f;
    private int currentPoint = 0;

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        // Target ki taraf dekhna
        Vector3 direction = waypoints[currentPoint].position - transform.position;
        direction.y = 0; // Bot ko hawa mein udne se rokne ke liye

        if (direction.magnitude > 1f)
        {
            // Bot ko move aur rotate karna
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        }
        else
        {
            // Agle point par jaana
            currentPoint = (currentPoint + 1) % waypoints.Length;
        }
    }
}
