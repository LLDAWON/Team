using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraController : MonoBehaviour
{
    private GameObject _player;
    private PlayerController _playerController;
    private CinemachineVirtualCamera _virtualCamera;

    private bool _isFirstTime = false;
    private Vector3 _ventDownPos = new Vector3(0, -1.5f, 0);

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();

        Observer.OnTargetEvents.Add(1, PlayerDieCam);
        Observer.OnDesolveEvents.Add(2, MoveToCameraForCutScene);
    }


    private void Start()
    {

        //_monsters = MonsterManager.Instance.GetSpawnMonster();

        _player = GameManager.Instance.GetPlayer();
        _playerController = _player.GetComponent<PlayerController>();
        PlayerFollowCam();

    }

    private void Update()
    {
        if(_playerController.GetIsPlayerVant() ==true )
        {
            if(!_isFirstTime)
            {
                _player.transform.GetChild(0).transform.position += _ventDownPos;
                _isFirstTime = true;
                Debug.Log("카메라내려감");
            }

        }
        else if(_playerController.GetIsPlayerVant() == false)
        {
            if (_isFirstTime)
            {
                _player.transform.GetChild(0).transform.position -= _ventDownPos;
                _isFirstTime = false;
                Debug.Log("카메라올라감");
            }

        }


        //foreach (GameObject monster in _monsters)
        //{
        //    if (monster.GetComponent<MiniMonsterController>().GetEnemyCurState() == EnemyController.EnemyState.Attack)
        //    {
        //            PlayerDieCam(monster);
        //            return;
        //    }
        //}

        //if (_player.GetComponent<PlayerController>().GetIsPlayerDie() == false)
        //{
        //    PlayerFollowCam();
        //}


    }

    private void PlayerFollowCam()
    {

        _virtualCamera.Follow = _player.transform.GetChild(0).transform;
        _virtualCamera.LookAt = _player.transform.GetChild(0).transform.GetChild(0).transform;
        //StopCoroutine("MoveToCameraForCutScene");
    }

    public void PlayerDieCam(GameObject attackingMonster)
    {
        _virtualCamera.Follow = attackingMonster.transform.GetChild(0).transform;
        _virtualCamera.LookAt = attackingMonster.transform.GetChild(0).transform.GetChild(0).transform;
    }


    private IEnumerator MoveToCameraForCutScene(GameObject gameObject)
    {
        float duration = 2.0f;  // 2초동안이동
        float elapsed = 0.0f;

        Vector3 _initialRotatePosition = _virtualCamera.Follow.position;
        Vector3 _initialLookPosition = _virtualCamera.LookAt.position;


        Vector3 _rotatePosition = gameObject.transform.GetChild(0).transform.position;
        Vector3 _lookPosition = gameObject.transform.GetChild(0).transform.GetChild(0).position;


        while (elapsed < duration)
        {
            _virtualCamera.Follow.position = Vector3.Slerp(_initialRotatePosition, _rotatePosition, elapsed / duration);
            _virtualCamera.LookAt.position = Vector3.Slerp(_initialLookPosition, _lookPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 끝난다음에 다시 플레이어 로 이동

        Invoke("PlayerFollowCam", 6);
        
    }

}
