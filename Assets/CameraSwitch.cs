using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;
    public Camera rearCamera;

    void Start()
    {
        rearCamera.enabled = false;
        mainCamera.enabled = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            mainCamera.enabled = false;
            rearCamera.enabled = true;
        }
        else
        {
            mainCamera.enabled = true;
            rearCamera.enabled = false;
        }
    }
}