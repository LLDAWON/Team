using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneCameraController : MonoBehaviour
{
    
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void EndAnimator()
    {
        GameManager.Instance.RestoreMainCamera();
    }
}
