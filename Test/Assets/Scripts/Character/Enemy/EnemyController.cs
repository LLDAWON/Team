using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MoveableCharactorController
{

    protected enum EnemyState
    {
        Patrol,
        Trace,
        Attack,
        Stop
    }
    //경로 관련
    [SerializeField]
    protected List<Vector3> pathes = new List<Vector3>();
    protected Vector3 _destPos;
    protected int _curPathNum = 0;

    //타겟 네비게이션
    protected Transform _target;
    protected NavMeshAgent _navigation;

    // 타겟 부채꼴범위판별 //  벽투과방지용 레이저

    protected bool _isPlayerDetected = false; // 내가 플레이어를 찾음
    protected bool _isInCircularSector = true; // 부채꼴안에 들어옴 
    protected bool _rayzorHitPlayer = false; 

    //State
    protected EnemyState _enemyState = EnemyState.Patrol;

    //animator
    protected Animator _animator;

    



    protected override void Awake()
    {
        base.Awake();
        _navigation = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        gameObject.SetActive(false);
    }
    protected void Start()
    {
        _target = GameObject.Find("Player").transform;
        _destPos = pathes[0];
    }

     protected override void Update()
    {
        if (_enemyState == EnemyState.Attack)
            return;

        CheckPath();
        StateUpdate();
        EnemyAiPattern();
        base.Update();
    }


    virtual protected void StateUpdate()
    {

        // 타겟에서 타겟이 바라보는 방향으로 레이저를 쐈을때 내가 걸리면 Stop으로 변경해주고 
        //이거는 우는천사 몬스터일떄만 해야하니 자식으로 빼자


        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0; // y 축 이동을 방지하여 평면 이동만 가능하게 함

        //벽투과방지용 bool
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _characterData.DetectRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                _rayzorHitPlayer = true;
            }
        }

        if (targetDirection.magnitude< _characterData.DetectRange)
        {
            float dot = Vector3.Dot(targetDirection.normalized, transform.forward);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);

            // 각도와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            if (degree <= _angleRange )
            {
                _isInCircularSector = true;
                if (_rayzorHitPlayer)
                {
                    _isPlayerDetected = true;
                }
            }
            else
            {
                _isInCircularSector = false;
            }
        }


        if(_isPlayerDetected)
        {
            SetState(1);
        }
        else
        {
            if (targetDirection.magnitude > _characterData.DetectRange)
            {
                _isPlayerDetected = false;
                SetState(0);
            }
            else
            {
                SetState(1);
            }
        }
       


    }

   
    protected void CheckPath()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        _destPos.y = 0;
        Vector3 direction = _destPos - pos;

        if (direction.sqrMagnitude < 0.01f)
        {
            _curPathNum = (_curPathNum + 1) % pathes.Count;
            _destPos = pathes[_curPathNum];
        }
    }

   
    protected void EnemyAiPattern()
    {
        switch (_enemyState)
        {
            case EnemyState.Patrol:
                {
                    _navigation.SetDestination(_destPos);
                    _animator.speed = 0.5f;
                    _navigation.speed = _characterData.WalkSpeed;
                }
                break;
            case EnemyState.Trace:
                {
                    _navigation.SetDestination(_target.position);
                    _animator.speed =1.0f;
                    _navigation.speed = _characterData.RunSpeed;
                }
                break;
            case EnemyState.Attack:
                {
                    //공격애니메이션 재생
                    _navigation.speed = 0;
                    _animator.speed = 1.0f;
                    _animator.SetTrigger("Attack");
                    _isAttack= true;

                }
                break;
            case EnemyState.Stop:
                {
                    _navigation.velocity = Vector3.zero;
                    // 특정 프레임에서 애니메이션 멈추기
                    _navigation.speed = 0;
                    _animator.speed = 0.0f;
                }
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetState(2);
        }
    }
    public void SetState(int state)
    {
        _enemyState = (EnemyState)state;
    }


    protected void Spawn(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
    }
}
