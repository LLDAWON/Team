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

    //animator
    
    protected Animator _animator;

    



    protected override void Awake()
    {
        base.Awake();
        _navigation = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
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

        // Ÿ�ٿ��� Ÿ���� �ٶ󺸴� �������� �������� ������ ���� �ɸ��� Stop���� �������ְ� 
        //�̰Ŵ� ���õ�� �����ϋ��� �ؾ��ϴ� �ڽ����� ����


        // ��ä�÷� �������� ���� Ȯ������


        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0; // y �� �̵��� �����Ͽ� ��� �̵��� �����ϰ� ��

        bool playerDetected = false;
        float detectAngle = 45.0f; // ��ä�� ���� (90�� ���� ����)
        int rayCount = 10; // �� ������ ����
        float angleStep = detectAngle / (rayCount - 1); // ���� ����

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -detectAngle / 2 + angleStep * i; // �߾� �������� �¿�� �������� ����
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward; // ������ ���� ���� ���
            Debug.DrawRay(transform.position, direction,Color.blue);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, _characterData.DetectRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    playerDetected = true;
                    print("playerDetect");
                    _animator.speed = 0;
                    break;
                }
            }
        }

        if (playerDetected)
        {
            SetState(1);
        }
        else
        {
            if (targetDirection.sqrMagnitude > _characterData.DetectRange * _characterData.DetectRange)
            {
                playerDetected = false;
                SetState(0);
            }
            else
            {
                SetState(1);
            }
        }






        //RaycastHit enemyHit;
        //if (Physics.Raycast(transform.position, transform.forward, out enemyHit, _characterData.DetectRange))
        //{
        //    if (enemyHit.collider.CompareTag("Player"))
        //    {
        //        SetState(1);
        //    }
        //}

        //if(targetDirection.sqrMagnitude > _characterData.DetectRange * _characterData.DetectRange)
        //    SetState(0);

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

        //Gizmos.draw
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
                    //���ݾִϸ��̼� ���
                    _navigation.speed = 0;
                    _animator.speed = 1.0f;
                    _animator.SetTrigger("Attack");
                    _isAttack= true;

                }
                break;
            case EnemyState.Stop:
                {
                    _navigation.velocity = Vector3.zero;
                    // Ư�� �����ӿ��� �ִϸ��̼� ���߱�
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
}
