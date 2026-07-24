using UnityEngine;
using System.Collections;
using TMPro; // TextMeshPro ke liye

public class RaceCountdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Inspector se drag karein
    public CarController playerControl;   // Player car (bugatti)

    // Yahan humne Array banaya hai taaki ek se zyada bots add ho sakein
    public BotDrive[] botControls;

    void Start()
    {
        // Shuruat mein sabko rok do
        playerControl.enabled = false;
        foreach (BotDrive bot in botControls)
        {
            if (bot != null) bot.enabled = false;
        }

        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1);

        countdownText.text = "2";
        yield return new WaitForSeconds(1);

        countdownText.text = "1";
        yield return new WaitForSeconds(1);

        countdownText.text = "GO!";

        // Race shuru: Sabko enable kar do
        playerControl.enabled = true;
        foreach (BotDrive bot in botControls)
        {
            if (bot != null) bot.enabled = true;
        }

        yield return new WaitForSeconds(1);
        countdownText.text = ""; // Text gayab kar do
    }
}
