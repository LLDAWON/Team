using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    public CameraManager cameraManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // cameraManager.StartFuzziness();
            cameraManager.StartVignette();
            gameObject.SetActive(false);
        }
    }
}
