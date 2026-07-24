using UnityEngine;

public class WebBridge : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("HTML se PLAY signal mil gaya!");

        RaceCountdown countdownScript = FindObjectOfType<RaceCountdown>();

        if (countdownScript != null)
        {
            countdownScript.enabled = true;
        }
        else
        {
            Debug.LogError("RaceCountdown script nahi mili!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
