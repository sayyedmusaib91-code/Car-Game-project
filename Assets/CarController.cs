using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    public float speed = 20f;
    public float turnSpeed = 100f;
    public float driftFactor = 0.9f;

    private Rigidbody rb;
    [HideInInspector]
    public bool raceFinished = false; // Coroutine me set karenge

    private float normalSpeed;
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
        normalSpeed = speed;

        // ✅ Trail Renderer ko permanent mark ke liye setup
        leftTrail.time = Mathf.Infinity;   // fade na ho
        rightTrail.time = Mathf.Infinity;  // fade na ho
        leftTrail.emitting = false;        // initially off
        rightTrail.emitting = false;       // initially off
    }

    void FixedUpdate()
    {
        if (raceFinished)
        {
            // Smooth freeze FixedUpdate me nahi, coroutine me karenge
            return;
        }

        float move = Input.GetAxis("Vertical") * speed;
        float turn = Input.GetAxis("Horizontal") * turnSpeed;

        Vector3 newVelocity = transform.forward * move;

        if (Input.GetKey(KeyCode.Space))
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, newVelocity, driftFactor * Time.fixedDeltaTime);
        else
            rb.linearVelocity = newVelocity;

        if (Mathf.Abs(move) > 0.1f) // car move ho rahi ho tabhi turn allow
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn * Time.fixedDeltaTime, 0f));
        }

        // TYRE MARKS (only drift)
        bool isDrifting = Input.GetKey(KeyCode.Space) && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f;

        if (isDrifting && rb.linearVelocity.magnitude > 5)
        {
            leftTrail.emitting = true;
            rightTrail.emitting = true;

            smokeLeft.Play();
            smokeRight.Play();
        }
        else
        {
            leftTrail.emitting = false;
            rightTrail.emitting = false;

            smokeLeft.Stop();
            smokeRight.Stop();
        }
    }

    public void StartBoost(float multiplier, float duration)
    {
        if (!isBoosting)
            StartCoroutine(BoostRoutine(multiplier, duration));
    }

    IEnumerator BoostRoutine(float multiplier, float duration)
    {
        isBoosting = true;
        speed = normalSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        speed = normalSpeed;
        isBoosting = false;
    }
}