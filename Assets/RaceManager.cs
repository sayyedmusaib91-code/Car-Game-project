using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class RaceManager : MonoBehaviour
{
    public int playerFinalRank = 0;
    [Header("Assign all cars here")]
    public List<Transform> allCars;           // Inspector me saari cars drag kare

    [Header("Assign rank texts here")]
    public List<TextMeshProUGUI> rankTexts;   // Car ke upar jo text dikhaye wo drag kare

    [Header("Assign GameManager here")]
    public GameManager gameManager;           // Inspector me GameManager assign kare

    [HideInInspector]
    public bool raceFinished = false; // Race finish hone par position lock


    void Update()
    {
        if (!raceFinished && allCars.Count > 0 && rankTexts.Count > 0)
        {
            CalculatePositions();
        }
    }

    void CalculatePositions()
    {
        // Cars ko Z-axis ke hisab se sort karo (jo aage hai wo 1st)
        var sortedCars = allCars
            .Select((car, index) => new { Car = car, OriginalIndex = index })
            .OrderByDescending(x => x.Car.position.z)
            .ToList();

        // Rank texts update karo
        for (int i = 0; i < sortedCars.Count; i++)
        {
            int rank = i + 1; // 1st, 2nd, ...
            int carIndex = sortedCars[i].OriginalIndex;

            rankTexts[carIndex].text = rank.ToString();
        }
        playerFinalRank = sortedCars.FindIndex(x => x.Car.CompareTag("Player")) + 1;
    }

    // Race finish hone ke baad ye function call hoga
    public void LockRace()
    {
        raceFinished = true;

        // Sab cars ko rok do
        foreach (var car in allCars)
        {
            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;
        }

        // Player rank calculate properly
        int playerRank = allCars
            .Select((car, index) => new { Car = car, Index = index })
            .OrderByDescending(x => x.Car.position.z)
            .ToList()
            .FindIndex(x => x.Car.CompareTag("Player")) + 1;

        // GameManager ko correct rank bhejo
        if (gameManager != null)
        {
            gameManager.OnRaceFinished(playerFinalRank);
        }    
    }
}