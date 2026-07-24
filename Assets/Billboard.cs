using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        // Text hamesha camera ki taraf dekhega
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }
}
