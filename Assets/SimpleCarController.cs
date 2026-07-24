using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public float speed = 15f;
    public float turnSpeed = 100f;
    public float brakeForce = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        // Move forward/backward
        Vector3 movement = transform.forward * move * speed;
        rb.AddForce(movement, ForceMode.Acceleration);

        // Turn
        if (move != 0)
        {
            float rotation = turn * turnSpeed * Time.deltaTime;
            transform.Rotate(0, rotation, 0);
        }

        // Brake
        if (Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity *= (1f - brakeForce * Time.deltaTime);
        }
    }
}