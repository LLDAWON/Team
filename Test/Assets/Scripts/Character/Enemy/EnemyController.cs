using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyController : MoveableCharactorController
{

    public enum EnemyState
    {
        Patrol,
        Trace,
        Attack,
        Stop,
        None,
        Die
    }
    
    // ��� ����
    [SerializeField]
    protected List<Vector3> pathes = new List<Vector3>();
    protected Vector3 _destPos;
    protected int _curPathNum = 0;

    // Ÿ�� �׺���̼�
    protected Transform _target;
    protected NavMeshAgent _navigation;

    // Ÿ�� ��ä�ù����Ǻ� 
    protected bool _isPlayerDetected = false; // ���� �÷��̾ ã��
    protected bool _isInCircularSector = true; // ��ä�þȿ� ���� 
    protected bool _rayzorHitPlayer = false;   //������������ ������

    // State
    protected EnemyState _enemyState = EnemyState.Patrol;
    public EnemyState GetEnemyCurState() { return _enemyState; }

    // Animator
    protected Animator _animator;
    protected AnimatorStateInfo _animatorStateInfo;

    // ó�� ���� ���� Ȯ��
    protected bool _isFirstSpawn = true;

    //�÷��̾�� ó�� �����ϱ������� ������ x
    protected bool _isFirstMeet = false;


    protected override void Awake()
    {

        base.Awake();
        _navigation = GetComponent<NavMeshAgent>();


        _animator = GetComponent<Animator>();





    }

    virtual protected  void Start()
    {
        _target = GameManager.Instance.GetPlayer().transform;
        _destPos = pathes[0];
       
    }
   
    protected override void Update()
    {
        SoundController();
        CheckPath();
        StateUpdate();
        EnemyAiPattern();
       
        base.Update();
    }

    virtual protected void StateUpdate()
    {
        //�����ϋ� ������Ʈ��ȭ x
    if (_enemyState == EnemyState.Attack)
        return;

    // �÷��̾ �����ϱ����� ������Ʈ��ȭ  x
    CheckFirstMeetPlayer();
    if (!_isFirstMeet)
        return;
    //ĳ��ݰ��� ������ �ִ°� �������� �νĸ��ϰ� �����ϱ����� ������ ����
    bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(0);
            return;
        }

        // ��ä�� �Ǻ� ���� �ڵ�
        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0; // y �� �̵��� �����Ͽ� ��� �̵��� �����ϰ� ��

            // ������������ bool 
            // ��ä�þȿ� ���԰�, �����Ÿ� ���ȿ� �������� ���̿� ���� �������� �߰ݻ��� x , �������
            RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _characterData.DetectRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                _rayzorHitPlayer = true;
            }
        }

        if (targetDirection.magnitude < _characterData.DetectRange)
        {

            float dot = Vector3.Dot(targetDirection.normalized, transform.forward);
                float theta = Mathf.Acos(dot);
                float degree = Mathf.Rad2Deg * theta;

            if (degree <= _angleRange)
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

        if (_isPlayerDetected)
            {
                SetState(1);
                _animator.SetBool("IsTrace", true);

            }
        else
        {
            if (targetDirection.magnitude > _characterData.DetectRange)
            {
                _isPlayerDetected = false;
                SetState(0);
                _animator.SetBool("IsTrace", false);
        }
            else
            {
                SetState(1);
                _animator.SetBool("IsTrace", true);
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
            
            int randompathNum = Random.Range(0, _curPathNum);
            _destPos = pathes[randompathNum];
        }
    }

    virtual protected void EnemyAiPattern()
    {
        switch (_enemyState)
        {
            case EnemyState.Patrol:
                {

                    _animator.speed = 0.5f;
                    _navigation.SetDestination(_destPos);
                    _navigation.speed = _characterData.WalkSpeed;
                    CameraManager.Instance.StopVignette();

                }
                break;
            case EnemyState.Trace:
                {
                    _animator.speed = 2.0f;
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _characterData.RunSpeed;

                    CameraManager.Instance.StartVignette();
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    if(!_isAttack)
                    {
                        _animator.SetTrigger("Attack");

                        SoundManager.Instance.SameStateJustOnePlay3D("FollowAttack", transform, false, 2.0f); 
                        Observer.OnTargetEvents[1](gameObject);
                        Debug.Log(_isAttack);
                    }
                    _isAttack = true;
                    Debug.Log("������");
                }
                break;
            case EnemyState.Stop:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    _animator.speed = 0.0f;
                }
                break;
            case EnemyState.None:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                }
                break;
            case EnemyState.Die:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("����");
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetState(2);
        }
    }

    public void SetState(int state)
    {
        _enemyState = (EnemyState)state;
    }

    public void Spawn(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        _animator.speed = 1.0f;
    }

    virtual protected void CheckFirstMeetPlayer()
    {
        PlayerController playerController = _target.GetComponent<PlayerController>();

        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        if (_inPlayerSight.magnitude <= playerController.GetCharacterData().DetectRange && !_isFirstMeet)
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            if (degree <= _angleRange)
            {
                _isFirstMeet = true;
                _animator.SetTrigger("MeetPlayer");
                Debug.Log("�÷��̾ ���� ã�ҽ��ϴ�.");
                return;
            }
        }
    }


    protected void SoundController()
    {
        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0; // y �� �̵��� �����Ͽ� ��� �̵��� �����ϰ� ��

        //���� �߰�
        if (targetDirection.magnitude < _target.GetComponent<PlayerController>().GetCharacterData().DetectRange)
        {
            //����Ҹ� �־��ְ�
            SoundManager.Instance.SameStateJustOnePlay3D("Heart", transform, true, 1.0f);
        }
        else
        {
            SoundManager.Instance.Stop3D("Heart");
        }

    }

}