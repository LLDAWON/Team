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
    private LineRenderer _lineRenderer;
    private float _circleRadius; // 원의 반지름
    private float _circleGrowthRate; // 원이 증가하는 속도
    private int _numSegments = 100; // 원을 그릴 때 사용할 세그먼트 수


    override protected void Awake()
    {
        base.Awake();
        _mannequinPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Manequin");
        _spotLight = transform.GetChild(1).gameObject;
        //원관련
        CircleSetting();
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
        //음악 켜주고
        //balletMusic.Play();

        //마네킹을 음악 멈추기 전의 위치로 보내주기

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
        _circleGrowthRate = 2.0f; // 원이 증가하는 속도 설정
        _lineRenderer.enabled = true;

        StartCoroutine(GrowCircle());
        Invoke("StopDance", 5.0f);
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

       // 원그리는거 멈춰주기
        _lineRenderer.enabled = false;
        //움직이지 않게 해주고 속도 0 애니메이션 속도0 멈추게하기
       
        //발레리나 옮겨주기
        //Invoke("TeleportBalerina", 1.0f);

        SetState(0);

    }
    // 발레리나를 랜덤한 마네킹 위치로 이동시키는 함수
    
    private void TeleportBalerina()
    {

        _spawnedMannequins[randomManequinIndex].transform.position = transform.position;
        _spawnedMannequins[randomManequinIndex].transform.rotation = transform.rotation;
        float randomX = 0;
        float randomZ = 0;
        while ((randomX + randomZ) < 5.0f)
        {
            randomX = Random.Range(-5f, 5f);
            randomZ = Random.Range(-5f, 5f);
        }
        transform.position = _target.position + new Vector3(randomX, 0, randomZ);
    }
    private void OnDrawGizmos()
    {
        
    }
    private void DrawCircle(float radius)
    {
        float deltaTheta = (2.0f * Mathf.PI) / _numSegments;
        float theta = 0f;

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            _lineRenderer.SetPosition(i, new Vector3(x, 0, z));
            theta += deltaTheta;
        }
    }
    private IEnumerator GrowCircle()
    {
        while (_lineRenderer.enabled)
        {
            _circleRadius += _circleGrowthRate * Time.deltaTime;
            DrawCircle(_circleRadius);

            yield return null;
        }
    }
    override protected void StateUpdate()
    {
        //공격일떈 스테이트변화 x

        if (_enemyState == EnemyState.Attack)
            return;

        // 플레이어가 원 안에 있는지 확인
        if (Vector3.Distance(transform.position, _target.position) <= _circleRadius)
        {
            SetState(1);
        }

    }
    protected override void EnemyAiPattern()
    {

        switch (_enemyState)
        {
            case EnemyState.Patrol:
                {
                    _animator.speed = 1.0f;
                    _navigation.speed = 1.0f;
                    Debug.Log("patrol");
                }
                break;
            case EnemyState.Trace:
                {
                    if (_danceRoutineCoroutine != null)
                    {
                        StopCoroutine(_danceRoutineCoroutine); // Attack 상태일 때 코루틴 중지
                        _danceRoutineCoroutine = null;
                        _lineRenderer.enabled = false;
                    }
                    _animator.SetTrigger("Trace");
                    _animator.speed = 2.0f;
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _characterData.WalkSpeed;
                    Debug.Log("Trace");
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    _isAttack = true;
                    _animator.SetTrigger("Attack");
                    Debug.Log("Attack");

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
                    Debug.Log("None");
                }
                break;
        }
    }

    private void CircleSetting()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.2f;
        _lineRenderer.endWidth = 0.2f;  
        _lineRenderer.positionCount = _numSegments + 1;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;
        _lineRenderer.enabled = false;
    }
}
