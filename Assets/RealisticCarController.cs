using UnityEngine;
using System.Collections;

public class RealisticCarController : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration = 25f;
    public float maxSpeed = 30f;
    public float turnSpeed = 120f;

    [Header("Physics")]
    public float naturalDrag = 1.2f;
    public float brakeForce = 6f;

    [Header("Drift")]
    public float driftFactor = 0.95f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = 1200f;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 3f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.centerOfMass = new Vector3(0, -1.2f, 0);
    }

    void FixedUpdate()
    {
        // 🎮 GET AXIS INPUT
        float move = Input.GetAxis("Vertical");     // W/S ya Up/Down
        float turn = Input.GetAxis("Horizontal");   // A/D ya Left/Right

        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // 🚗 ACCELERATION (smooth)
        if (move != 0)
        {
            rb.AddForce(transform.forward * move * acceleration, ForceMode.Acceleration);
        }

        // 🚗 SPEED LIMIT
        if (flatVel.magnitude > maxSpeed)
        {
            rb.linearVelocity = flatVel.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;
        }

        // 🚗 TURNING
        if (flatVel.magnitude > 1f)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn * turnSpeed * Time.fixedDeltaTime, 0f));
        }

        // 🛑 BRAKE
        if (Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, brakeForce * Time.fixedDeltaTime);
        }
        else
        {
            // 🌊 NATURAL SLOWDOWN (REALISTIC)
            rb.linearVelocity = Vector3.Lerp(
                rb.linearVelocity,
                new Vector3(0, rb.linearVelocity.y, 0),
                naturalDrag * Time.fixedDeltaTime
            );
        }
    }
}