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
    //��� ����
    [SerializeField]
    protected List<Vector3> pathes = new List<Vector3>();
    protected Vector3 _destPos;
    protected int _curPathNum = 0;
    //Ÿ�� �׺���̼�
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

        // Ÿ�ٿ��� Ÿ���� �ٶ󺸴� �������� �������� ������ ���� �ɸ��� Stop���� �������ְ� 
        //�̰Ŵ� ���õ�� �����ϋ��� �ؾ��ϴ� �ڽ����� ����
       

            Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0; // y �� �̵��� �����Ͽ� ��� �̵��� �����ϰ� ��

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
                    //���ݾִϸ��̼� ���
                    _navigation.speed = 0;
                    _isAttack= true;

                }
                break;
            case EnemyState.Stop:
                {
                    // Ư�� �����ӿ��� �ִϸ��̼� ���߱�
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
