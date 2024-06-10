using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyController : MoveableCharactorController
{
    protected enum EnemyState
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
    protected EnemyState _enemyState = EnemyState.None;

    // Animator
    protected Animator _animator;
    private AnimatorStateInfo _animatorStateInfo;
    private float _animatorPlaybackTime;

    // ó�� ���� ���� Ȯ��
    private bool _isFirstSpawn = true;
    private int _defaultAnimatorStateHash;

    //�÷��̾�� ó�� �����ϱ������� ������ x
    protected bool _isFirstMeet = false;

    // ������ȿ�� �� ���̴� 
    protected Material _material;
    protected Shader _shader;
    protected float _desolveTime;
    protected float _desolveEndTime = 2.0f;
    protected bool _desolveStart = false;


    protected override void Awake()
    {
        base.Awake();
        _navigation = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _desolveTime = _desolveEndTime;

    }

    protected void Start()
    {
        _target = GameManager.Instance.GetPlayer().transform;
        _destPos = pathes[0];
        _defaultAnimatorStateHash = _animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
    }

    protected override void Update()
    {
        if (_enemyState == EnemyState.Attack)
            return;

        //ó���������
        CheckFirstMeetPlayer();


        CheckPath();

        
        StateUpdate();
        EnemyAiPattern();
        //DesolveEnemy();
        base.Update();
    }

    virtual protected void StateUpdate()
    {
        // �÷��̾ �����ϱ����� ������Ʈ��ȭ  x
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
            //����� ������� �Դٰ���
            //_curPathNum = (_curPathNum + 1) % pathes.Count;
            //_destPos = pathes[_curPathNum];
            //�������� �Դٰ���? �ϴ°� �� �����Ű���.
            int randompathNum = Random.Range(0, _curPathNum);
            _destPos = pathes[randompathNum];
        }
    }

    protected void EnemyAiPattern()
    {
        switch (_enemyState)
        {
            case EnemyState.Patrol:
                _animator.SetBool("IsTrace", false);
                _animator.speed = 0.5f;
                _navigation.SetDestination(_destPos);
                _navigation.speed = _characterData.WalkSpeed;
                break;
            case EnemyState.Trace:
                _animator.SetBool("IsTrace", true);
                _animator.speed = 2.0f;
                _navigation.SetDestination(_target.position);
                _navigation.speed = _characterData.RunSpeed;
                break;
            case EnemyState.Attack:
                _navigation.speed = 0;
                _animator.speed = 1.0f;
                _animator.SetTrigger("Attack");
                _isAttack = true;
                break;
            case EnemyState.Stop:
                _navigation.velocity = Vector3.zero;
                _navigation.speed = 0;
                _animator.speed = 0.0f;
                break;
            case EnemyState.None:
                _navigation.velocity = Vector3.zero;
                _navigation.speed = 0;
                break;
            case EnemyState.Die:
                _navigation.velocity = Vector3.zero;
                _navigation.speed = 0;
                DesolveEffect();


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

    public void Spawn(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;

        if (_isFirstSpawn)
        {
            // ó�� ���� �� �⺻ �ִϸ��̼� ���·� ����
            _animator.Play(_defaultAnimatorStateHash, -1, 0f);
            _isFirstSpawn = false;
        }
        else
        {
            // �ִϸ����� ���� ����
            _animator.Play(_animatorStateInfo.fullPathHash, -1, _animatorPlaybackTime);
        }
        _animator.speed = 1.0f;
    }

    public void DestroyMonster()
    {
        // �ִϸ����� ���� ����
        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _animatorPlaybackTime = _animatorStateInfo.normalizedTime % 1;

        gameObject.SetActive(false);
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

    //������ȿ���� n���� �����
    protected void DesolveEffect()
    {
        //1���� ������ ȿ���� �ݺ��Ǵ� �����ؾ���
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                _material = renderer.material;
                _shader = _material.shader;
                _shader = Shader.Find("Shader Graphs/Desolve");
                _material.shader = _shader;

            }
        }
        _desolveStart = true;
    }

    protected void DesolveEnemy()
    {
        if(_desolveStart)
        {
            _desolveTime--;
            if(_desolveTime<0)
            {
                gameObject.SetActive(false);
                _desolveTime += _desolveEndTime;
                _desolveStart = false;
            }
        }

        _material.SetFloat("DesloveTime", _desolveTime);
    }
}