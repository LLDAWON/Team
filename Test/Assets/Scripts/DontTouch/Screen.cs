using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
    public Material screenMaterial; 

    void Start()
    {
        
        MeshRenderer screenMeshRenderer = GetComponent<MeshRenderer>();

        if (screenMeshRenderer != null && screenMaterial != null)
        {
            screenMeshRenderer.material = screenMaterial;
        }
        
    }
}
