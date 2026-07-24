using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public GameObject button;

    void Start()
    {
        if (button != null)
            button.SetActive(false);
    }

    public void ShowButton()
    {
        if (button != null)
            button.SetActive(true);
    }

    public void GoToMenu()
    {
        Debug.Log("Switching to Main Menu");

#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalEval("window.location.href = '/main_menu';");
#endif
    }
}
