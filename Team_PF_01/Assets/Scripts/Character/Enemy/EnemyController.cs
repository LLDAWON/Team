using System.Collections;
using System.Collections.Generic;
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
    //��� ����
    [SerializeField]
    protected List<Vector3> pathes = new List<Vector3>();
    protected Vector3 _destPos;
    protected int _curPathNum = 0;
    protected bool _isFind = false;
    //Ÿ�� �׺���̼�
    protected Transform _target;
    protected NavMeshAgent _navigation;

    // �� ������ȭ ���Ѿ���
    protected float _detectionRange = 10.0f;

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
        CheckForward();
        SetPath();
        EnemyAiPattern();
    }


    protected void CheckForward()
    {
        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0; // y �� �̵��� �����Ͽ� ��� �̵��� �����ϰ� ��
        RaycastHit hit;

        if (Physics.Raycast(transform.position, targetDirection, out hit, _detectionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                _isFind = true;
            }
        }

        if(targetDirection.sqrMagnitude > _detectionRange * _detectionRange)
            _isFind=false;

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

    protected void SetPath()
    {
        if (_isFind)
            SetState(1);
        else
            SetState(0);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere (transform.position, _detectionRange);
    }

    protected void EnemyAiPattern()
    {

        switch(_enemyState)
        {
            case EnemyState.Patrol:
                {
                    _navigation.SetDestination(_destPos);
                }
                break;
            case EnemyState.Trace:
                {
                    _navigation.SetDestination(_target.position);
                }
                break;
            case EnemyState.Attack:
                {
                }
                break;
            case EnemyState.Stop:
                {
                }
                break;
        }
    }

    private void SetState(int state)
    {
        _enemyState = (EnemyState)state;
        print(_enemyState);
    }
}
