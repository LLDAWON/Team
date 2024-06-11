using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class IphoneSeting : MonoBehaviour
{
    public Camera renderCamera; 
    //public RenderTexture renderTexture;
   

    
    void Start()
    {
        //renderCamera.targetTexture = renderTexture;
        renderCamera.backgroundColor = Color.clear;
        renderCamera.clearFlags = CameraClearFlags.SolidColor;     

    }
}
