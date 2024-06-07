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

    // Ÿ�� ��ä�ù����Ǻ� //  ������������ ������

    protected bool _isPlayerDetected = false; // ���� �÷��̾ ã��
    protected bool _isInCircularSector = true; // ��ä�þȿ� ���� 
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

        // Ÿ�ٿ��� Ÿ���� �ٶ󺸴� �������� �������� ������ ���� �ɸ��� Stop���� �������ְ� 
        //�̰Ŵ� ���õ�� �����ϋ��� �ؾ��ϴ� �ڽ����� ����


        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0; // y �� �̵��� �����Ͽ� ��� �̵��� �����ϰ� ��

        //������������ bool
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
            // �� ���� ��� ���� �����̹Ƿ� ���� ����� cos�� ���� ���ؼ� theta�� ����
            float theta = Mathf.Acos(dot);

            // ������ ���ϱ� ���� degree�� ��ȯ
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


    protected void Spawn(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
    }
}
