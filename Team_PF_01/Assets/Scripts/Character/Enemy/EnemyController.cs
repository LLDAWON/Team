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



    //State
    protected EnemyState _enemyState = EnemyState.Patrol;

    



    protected override void Awake()
    {
        base.Awake();
        _navigation = GetComponent<NavMeshAgent>();
    }
    protected void Start()
    {
        _target = GameObject.Find("Player").transform;
        _destPos = pathes[0];
    }

     protected override void Update()
    {
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

        RaycastHit enemyHit;
        if (Physics.Raycast(transform.position, targetDirection, out enemyHit, _characterData.DetectRange))
        {
            if (enemyHit.collider.CompareTag("Player"))
            {
                SetState(1);
            }
        }

        if(targetDirection.sqrMagnitude > _characterData.DetectRange * _characterData.DetectRange)
            SetState(0);

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

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere (transform.position, _characterData.DetectRange);
    }

    protected void EnemyAiPattern()
    {
        switch (_enemyState)
        {
            case EnemyState.Patrol:
                {
                    _navigation.SetDestination(_destPos);
                    _navigation.speed = _characterData.WalkSpeed;
                }
                break;
            case EnemyState.Trace:
                {
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _characterData.RunSpeed;
                }
                break;
            case EnemyState.Attack:
                {
                    //공격애니메이션 재생
                    _navigation.speed = 0;
                    _isAttack= true;

                }
                break;
            case EnemyState.Stop:
                {
                    // 특정 프레임에서 애니메이션 멈추기
                    _navigation.speed = 0;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SetState(2);
        }
    }
    public void SetState(int state)
    {
        _enemyState = (EnemyState)state;
        print(_enemyState);
    }
}
