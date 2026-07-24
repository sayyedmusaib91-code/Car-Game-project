using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class RaceAPI : MonoBehaviour
{
    public string serverURL = "http://127.0.0.1:5000/save_race_result"; // Inspector me change possible

    public void SendRaceResult(int position, int score)
    {
        Debug.Log("SENDING TO FLASK → Position: " + position + " Score: " + score);
        StartCoroutine(PostRaceResult(position, score));
    }

    IEnumerator PostRaceResult(int position, int score)
    {
        RaceData data = new RaceData();
        data.position = position;
        data.score = score;

        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Race result sent successfully!");
        else
            Debug.Log("Error sending race result: " + request.error);
    }

    [System.Serializable]
    public class RaceData
    {
        public int position;
        public int score;
    }
}