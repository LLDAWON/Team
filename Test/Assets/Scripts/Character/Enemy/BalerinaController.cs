using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalerinaController : EnemyController
{
    public List<GameObject> _mannequinPrefabs; // 5개의 마네킹 프리팹 리스트
    private List<GameObject> _spawnedMannequins = new List<GameObject>(); // 생성된 마네킹 객체 리스트
    private int _currentMannequinIndex; // 현재 발레리나가 위치한 마네킹 인덱스
    private float _nextDanceTime; // 다음 춤 시간
    private bool _isDancing = false; // 춤 상태
    AnimatorStateInfo _currentAnimatorState;
    private float _savedAnimationTime = 0f; // 애니메이션 시간 저장
    private int _savedAnimationHash; // 애니메이션 상태 해시 저장


    //public AudioSource balletMusic; // 발레리나 음악 3d사운드로 만들어주기

    override protected void Awake()
    {
        base.Awake();
        for (int i = 0; i < 5; i++)
        {
            GameObject _manequin = Resources.Load("Prefabs/Character/Enemy/Manequin") as GameObject;
            _mannequinPrefabs.Add(_manequin);
        }
    }
    protected override void Start()
    {
        base.Start(); 
        SpawnMannequins();
        StartCoroutine(DanceRoutine());
    }

    // 마네킹을 랜덤 위치에 스폰하는 함수
    private void SpawnMannequins()
    {
        for (int i = 0; i < _mannequinPrefabs.Count; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject mannequin = Instantiate(_mannequinPrefabs[i], randomPosition, Quaternion.identity);
            mannequin.SetActive(true);
            _spawnedMannequins.Add(mannequin);
        }

        _currentMannequinIndex = Random.Range(0, _spawnedMannequins.Count);
        Transform currentMannequinTransform = _spawnedMannequins[_currentMannequinIndex].transform;
        transform.position = currentMannequinTransform.position;
        transform.rotation = currentMannequinTransform.rotation;
        _spawnedMannequins[_currentMannequinIndex].SetActive(false);
    }

    // 랜덤 위치를 반환하는 함수
    private Vector3 GetRandomPosition()
    {
        // 임의의 위치를 반환 (임의의 로직을 구현)
        // 예시: 특정 범위 내에서 랜덤한 위치를 반환
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }

    // 춤 루틴을 처리하는 코루틴
    private IEnumerator DanceRoutine()
    {
        while (true)
        {
            if (!_isDancing)
            {
                yield return new WaitForSeconds(Random.Range(4f, 7f));
                StartDance();
            }
            else
            {
                yield return null;
            }
        }
    }

    // 춤을 시작하는 함수
    private void StartDance()
    {
        //음악 켜주고
        //balletMusic.Play();
        //_animator.SetTrigger("StartDance");
        _isDancing = true;

        _animator.Play(_savedAnimationHash, -1, _savedAnimationTime);

        _nextDanceTime = Time.time + Random.Range(4f, 7f);
        Invoke("EndDance", _nextDanceTime - Time.deltaTime);
    }

    // 춤을 종료하는 함수
    private void EndDance()
    {
        //음악꺼주고
        //balletMusic.Stop();
        //_animator.SetTrigger("EndDance");
        MoveToRandomMannequin();
        _isDancing = false;
    }

    // 발레리나를 랜덤한 마네킹 위치로 이동시키는 함수
    private void MoveToRandomMannequin()
    {

        _spawnedMannequins[_currentMannequinIndex].SetActive(true);

        _currentMannequinIndex = Random.Range(0, _spawnedMannequins.Count);
        Transform newMannequinTransform = _spawnedMannequins[_currentMannequinIndex].transform;
        transform.position = newMannequinTransform.position;
        transform.rotation = newMannequinTransform.rotation;


        _spawnedMannequins[_currentMannequinIndex].SetActive(false);
    }

    private  void DestroyMonster()
    {
        // 현재 애니메이션 상태 저장
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        _savedAnimationHash = _currentAnimatorState.fullPathHash;
        _savedAnimationTime = _currentAnimatorState.normalizedTime;

        gameObject.SetActive(false);

        // 파괴 전 위치에 마네킹을 생성
        GameObject mannequin = Instantiate(_mannequinPrefabs[_currentMannequinIndex], transform.position, transform.rotation);
        mannequin.SetActive(true);
        _spawnedMannequins[_currentMannequinIndex] = mannequin;
    }

    override protected void StateUpdate()
    {
        //공격일떈 스테이트변화 x
        if (_enemyState == EnemyState.Attack)
            return;

        // 플레이어가 조우하기전엔 스테이트변화  x
        CheckFirstMeetPlayer();
        if (!_isFirstMeet)
            return;

        SetState(1);
        
    }
}
