using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalerinaController : EnemyController
{
    private GameObject _mannequinPrefab;
    private List<GameObject> _spawnedMannequins = new List<GameObject>(); // 생성된 마네킹 객체 리스트
    private int _currentMannequinIndex; // 현재 발레리나가 위치한 마네킹 인덱스
    private int randomManequinIndex; // 랜덤 마네킹 인덱스
    private float _nextDanceTime; // 다음 춤 시간
    AnimatorStateInfo _currentAnimatorState;
    private int _savedAnimationHash; // 애니메이션 상태 해시 저장
    private float _savedAnimationTime;


    //public AudioSource balletMusic; // 발레리나 음악 3d사운드로 만들어주기

    override protected void Awake()
    {
        base.Awake();        
        _mannequinPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Manequin");
    }
    protected override void Start()
    {
        base.Start(); 
        SpawnMannequins();
        StartCoroutine(DanceRoutine());
    }

    override protected void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            StopDance();
        }

        if (Input.GetMouseButtonDown(1))
        {
            StartDance();
        }

        base.Update();
    }

    // 마네킹을 랜덤 위치에 스폰하는 함수
    private void SpawnMannequins()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPosition = transform.position + GetRandomPosition();
            GameObject mannequin = Instantiate(_mannequinPrefab, randomPosition, Quaternion.identity);
            Animator animator = mannequin.GetComponent<Animator>();
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            _savedAnimationHash = info.fullPathHash;
            animator.Play(info.fullPathHash, 0, Random.Range(0.0f, 1.0f));
            animator.speed = 0.0f;
            _spawnedMannequins.Add(mannequin);
        }        
    }

    private void SwitchMannequin()
    {
        _currentMannequinIndex = Random.Range(0, _spawnedMannequins.Count);
        Transform currentMannequinTransform = _spawnedMannequins[_currentMannequinIndex].transform;
        transform.position = currentMannequinTransform.position;
        transform.rotation = currentMannequinTransform.rotation;
        _spawnedMannequins[_currentMannequinIndex].SetActive(false);
    }

    // 랜덤 위치를 반환하는 함수
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }

    // 춤 루틴을 처리하는 코루틴
    private IEnumerator DanceRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(6.0f);

            StartDance();
        }        
    }

    // 춤을 시작하는 함수
    private void StartDance()
    {
        //음악 켜주고
        //balletMusic.Play();
        //_animator.SetTrigger("StartDance");
    

         randomManequinIndex = Random.Range(0, _spawnedMannequins.Count);

        Animator animator = _spawnedMannequins[randomManequinIndex].GetComponent<Animator>();
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(_savedAnimationHash, 0, _currentAnimatorState.normalizedTime);
        _spawnedMannequins[randomManequinIndex].transform.position = transform.position;
        _spawnedMannequins[randomManequinIndex].transform.rotation = transform.rotation;

        SetState(1);
        //transform.Translate(GetRandomPosition());

        randomManequinIndex = Random.Range(0, _spawnedMannequins.Count);
        transform.position = _spawnedMannequins[randomManequinIndex].transform.position;
        Invoke("StopDance", 5.0f);
    }

    private void StopDance()
    {
        // 현재 애니메이션 상태 저장
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        _savedAnimationHash = _currentAnimatorState.fullPathHash;
        _savedAnimationTime = _currentAnimatorState.normalizedTime;

        //현재 각도 유지
        Quaternion currentRotation = transform.rotation;
        transform.rotation = currentRotation;

        //움직이지 않게 해주고 속도 0 애니메이션 속도0 멈추게하기
        SetState(4);

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


    override protected void StateUpdate()
    {
        //공격일떈 스테이트변화 x
        if (_enemyState == EnemyState.Attack)
            return;
        if (_enemyState == EnemyState.None)
            return;

        // 플레이어가 조우하기전엔 스테이트변화  x
        CheckFirstMeetPlayer();
        if (!_isFirstMeet)
            return;

        SetState(1);
        
    }
    protected override void EnemyAiPattern()
    {

        switch (_enemyState)
        {
            case EnemyState.Trace:
                {
                    _animator.speed = 0.5f;
                    transform.Rotate(0, 1.0f, 0);
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _characterData.WalkSpeed;
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    _isAttack = true;

                    //_animator.SetTrigger("Attack");
                    Debug.Log("EnemyState.Attack");

                    Observer.OnTargetEvents[1](gameObject);
                }

                break;
            case EnemyState.Die:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    gameObject.SetActive(false);
                }
                break;
            case EnemyState.None:
                {
                    _animator.speed = 0.0f;
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("None");
                }
                break;
        }
    }
}
