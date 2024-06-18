using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WallCollision : MonoBehaviour
{
    public CameraManager cameraManager;
    private void Start()
    {
        Spawn();
       
    }
    private void Spawn()
    {
        
       // SoundManager.Instance.Play3D("Door", transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{

        //    // cameraManager.ShakeCamera(10.0f, 3.0f);
        //    cameraManager.Shake(0.3f, 3.0f);
        ////    cameraManager.StartVignette();  
        //    gameObject.SetActive(false);
        //}
    }
}
