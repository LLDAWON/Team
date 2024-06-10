using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraController : MonoBehaviour
{

    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    private void Start()
    {

        GameObject _player = GameManager.Instance.GetPlayer();
        _virtualCamera.Follow = _player.transform.GetChild(0).transform;
        _virtualCamera.LookAt = _player.transform.GetChild(0).transform.GetChild(0).transform;

    }






}
