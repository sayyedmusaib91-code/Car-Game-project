using UnityEngine;

public class DriftSound : MonoBehaviour
{
    public AudioSource driftAudio;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            driftAudio.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            driftAudio.Stop();
        }
    }
}