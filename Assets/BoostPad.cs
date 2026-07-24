using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float boostMultiplier = 2f;
    public float boostTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        CarController car = other.GetComponent<CarController>();

        if (car != null)
        {
            car.StartBoost(boostMultiplier, boostTime);
        }
    }
}
