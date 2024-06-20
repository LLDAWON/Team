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
    private GameObject _spotLight;

    //발레리나의 코루틴을 관리
    private Coroutine _danceRoutineCoroutine;
    private Coroutine _drawCircleCoroutine;
    // 발레리나의 공격 패턴인 증가하는 원과 관련된 
    private float _circleRadius; // 원의 반지름
    private float _circleGrowthRate = 2.0f; // 원이 증가하는 속도
    private Material _floorMaterial;


    override protected void Awake()
    {
        base.Awake();
        _mannequinPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Manequin");
        _spotLight = transform.GetChild(1).gameObject;
        //원관련
        _floorMaterial = Resources.Load<Material>("Materials/Floor/Floor");
        _circleRadius = 0f;
    }
    protected override void Start()
    {
        base.Start();
        SpawnMannequins();
        _danceRoutineCoroutine = StartCoroutine(DanceRoutine());

    }

    // 마네킹을 랜덤 위치에 스폰하는 함수
    private void SpawnMannequins()
    {
        for (int i = 0; i < 30; i++)
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

    // 랜덤 위치를 반환하는 함수
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f));
    }

    // 춤 루틴을 처리하는 코루틴
    private IEnumerator DanceRoutine()
    {
        while (_isAttack == false)
        {
            yield return new WaitForSeconds(7.0f);

            StartDance();
        }
    }

    // 춤을 시작하는 함수
    private void StartDance()
    {
        //다시 랜덤값 잡아줘서 발레리나 본체를 옮겨주기
        randomManequinIndex = Random.Range(0, _spawnedMannequins.Count);
        transform.position = _spawnedMannequins[randomManequinIndex].transform.position;


        Animator animator = _spawnedMannequins[randomManequinIndex].GetComponent<Animator>();
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(_savedAnimationHash, 0, _currentAnimatorState.normalizedTime);
        //Light켜주기
        _spotLight.SetActive(true);
        ///춤추는동안 원이 증가하면서 원 안에 들어오면 죽음

        // 이 원은 가시적으로 플레이할때도 보여줘야한다.
        //원초기화및 다시 그리기
        _circleRadius = 0f; 
        _circleRadius += _circleGrowthRate * Time.deltaTime;

        StartCoroutine(GrowCircle());

        StartCoroutine(StopDanceCoroutine(5.0f));
    }
    private IEnumerator StopDanceCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopDance();
    }
    private void StopDance()
    {
       
        // 현재 애니메이션 상태 저장
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        _savedAnimationHash = _currentAnimatorState.fullPathHash;
        _savedAnimationTime = _currentAnimatorState.normalizedTime;
        // 라이트꺼주기
        _spotLight.SetActive(false);

        //현재 각도 유지
        Quaternion currentRotation = transform.rotation;
        transform.rotation = currentRotation;


        //발레리나 랜덤위치 옮겨주기
        StartCoroutine(TeleportBalerinaCoroutine(1.0f));
        SetState(0);

    }
    // 발레리나를 랜덤한 마네킹 위치로 이동시키는 함수

    private IEnumerator TeleportBalerinaCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        TeleportBalerina();
    }
    private void TeleportBalerina()
    {
        if (Vector3.Distance(transform.position, _target.position) <= _circleRadius)
        {
            _spotLight.SetActive(true);
            SetState(1);
            return;
        }
        float randomX = 0;
        float randomZ = 0;
        while ((randomX + randomZ) < 5.0f)
        {
            randomX = Random.Range(-5f, 5f);
            randomZ = Random.Range(-5f, 5f);
        }
        transform.position = _target.position + new Vector3(randomX, 0, randomZ);
    }
   
    private IEnumerator GrowCircle()
    {
        float time = 0.0f;        
        while (time < 3.0f)
        {
            time += Time.deltaTime;
            //_circleRadius = 0f;
            _circleRadius += _circleGrowthRate * Time.deltaTime;

            yield return null;
        }
    }
    override protected void StateUpdate()
    {
        //공격일떈 스테이트변화 x

        if (_enemyState == EnemyState.Attack)
            return;

        _floorMaterial.SetVector("_WorldPos", transform.position);
        _floorMaterial.SetFloat("_Range", _circleRadius);
        _floorMaterial.SetColor("_Color", Color.red);


    }
    protected override void EnemyAiPattern()
    {

        switch (_enemyState)
        {
            case EnemyState.Patrol:
                {
                    _animator.speed = 1.0f;
                    _navigation.speed = 1.0f;
                }
                break;
            case EnemyState.Trace:
                {
                    if (_danceRoutineCoroutine != null)
                    {
                        StopCoroutine(_danceRoutineCoroutine); // Attack 상태일 때 코루틴 중지
                        _danceRoutineCoroutine = null;
                        //_lineRenderer.enabled = false;
                    }
                    _animator.SetTrigger("Trace");
                    _animator.speed = 2.0f;
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
                    _animator.SetTrigger("Attack");

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
                    _navigation.speed = 0.0f;
                }
                break;
        }
    }

}
