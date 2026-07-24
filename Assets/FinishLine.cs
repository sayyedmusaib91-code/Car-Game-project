using UnityEngine;
using TMPro;
using System.Collections;

public class FinishLine : MonoBehaviour
{
    public TextMeshProUGUI winText;
    public float delayBeforeStop = 1.5f;
    public GameManager gameManager;
    public RaceManager raceManager;
    public BackToMenu backToMenu;

    private bool raceFinished = false;

    void Start()
    {
        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject rootCar = other.transform.root.gameObject;

        if (!raceFinished && rootCar.CompareTag("Player"))
        {
            if (winText != null)
                winText.gameObject.SetActive(true);

            CarController playerController = rootCar.GetComponent<CarController>();
            if (playerController != null)
                StartCoroutine(SmoothFinish(playerController));

            StartCoroutine(StopCarAndLockRace(rootCar));

            raceFinished = true; // Race lock after trigger
        }
    }

    IEnumerator SmoothFinish(CarController player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

        // Car thoda aage cross kare 1 sec
        yield return new WaitForSeconds(1f);

        float timer = 0f;
        float duration = 1f;
        Vector3 initialVel = rb.linearVelocity;
        Vector3 initialAngVel = rb.angularVelocity;

        while (timer < duration)
        {
            timer += Time.fixedDeltaTime;
            float t = timer / duration;

            rb.linearVelocity = Vector3.Lerp(initialVel, Vector3.zero, t);
            rb.angularVelocity = Vector3.Lerp(initialAngVel, Vector3.zero, t);

            yield return new WaitForFixedUpdate();
        }

        // Final freeze
        player.raceFinished = true;
        rb.isKinematic = true;
    }

    IEnumerator StopCarAndLockRace(GameObject car)
    {
        yield return new WaitForSeconds(delayBeforeStop);

        if (raceManager != null)
            raceManager.LockRace();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int finalRank = raceManager.playerFinalRank;
        if (gameManager != null)
            gameManager.OnRaceFinished(finalRank);

        if (backToMenu != null)
            backToMenu.ShowButton();

        Debug.Log("Player finished race! Final Rank: " + finalRank);
    }
}