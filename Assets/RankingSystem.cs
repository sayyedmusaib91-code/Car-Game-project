using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class RankingSystem : MonoBehaviour
{
    public List<Transform> racers; // Isme Car aur saare Bots dalo
    public TextMeshProUGUI[] rankingTexts; // UI wale Text objects yahan dalo
    public CarController playerCar; // Inspector me apni player car assign karna

    void Update()
    {
        if (playerCar != null && playerCar.raceFinished)
        {
            return; // Race khatam ho gaya → ranking update band
        }
        // Racers ko unki Z-axis (ya distance) ke hisaab se sort karo
        var sortedRacers = racers.OrderByDescending(r => r.position.z).ToList();

        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (i < sortedRacers.Count)
            {
                rankingTexts[i].text = (i + 1) + ". " + sortedRacers[i].name;

                // Agar ye asli Player hai toh color badal do (19663.jpg jaisa)
                if (sortedRacers[i].CompareTag("Player"))
                    rankingTexts[i].color = Color.cyan;
                else
                    rankingTexts[i].color = Color.white;
            }
        }
    }
}
