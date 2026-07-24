
using UnityEngine;
using System.Collections;

public class BetterCarController : MonoBehaviour
{
    public float acceleration = 25f;
    public float maxSpeed = 30f;
    public float turnSpeed = 100f;
    public float brakeForce = 8f;
    public float naturalDrag = 1.5f;
    public float driftFactor = 0.95f;

    private Rigidbody rb;
    public bool raceFinished = false;

    private float normalAcceleration;
    private bool isBoosting = false;

    public TrailRenderer leftTrail;
    public TrailRenderer rightTrail;

    public ParticleSystem smokeLeft;
    public ParticleSystem smokeRight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.isKinematic = false;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 3f;

        normalAcceleration = acceleration;

        leftTrail.time = Mathf.Infinity;
        rightTrail.time = Mathf.Infinity;
        leftTrail.emitting = false;
        rightTrail.emitting = false;
    }

    void FixedUpdate()
    {
        if (raceFinished) return;

        // 🔥 UNITY 6 INPUT FIX
        float move = 0f;
        if (Input.GetKey(KeyCode.W)) move = 1f;
        if (Input.GetKey(KeyCode.S)) move = -1f;

        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        if (Input.GetKey(KeyCode.D)) turn = 1f;

        Vector3 forwardVel = transform.forward * move * acceleration;

        // 🚗 ADD FORCE (realistic)
        if (move != 0)
        {
            rb.AddForce(forwardVel, ForceMode.Acceleration);
        }

        // 🚗 SPEED LIMIT
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            rb.linearVelocity = flatVel.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;
        }

    }
}// 🚗