using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RaceAPI raceAPI;           // Inspector: RaceAPI object

    [HideInInspector]
    public int playerPosition;
    [HideInInspector]
    public int playerScore;

    private bool resultSent = false;

    // Called by RaceManager
    public void OnRaceFinished(int position)
    {
        if (resultSent) return;
        resultSent = true;

        playerPosition = position;

        if (position == 1) playerScore = 500;
        else if (position == 2) playerScore = 300;
        else if (position == 3) playerScore = 150;
        else playerScore = 50;

        Debug.Log("FINAL POSITION: " + playerPosition);
        Debug.Log("FINAL SCORE: " + playerScore);

        if (raceAPI != null)
            raceAPI.SendRaceResult(playerPosition, playerScore);
        else
            Debug.LogWarning("RaceAPI not assigned in GameManager!");
    }
}