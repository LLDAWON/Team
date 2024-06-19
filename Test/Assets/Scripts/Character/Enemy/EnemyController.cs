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
    
    // 경로 관련
    [SerializeField]
    protected List<Vector3> pathes = new List<Vector3>();
    protected Vector3 _destPos;
    protected int _curPathNum = 0;

    // 타겟 네비게이션
    protected Transform _target;
    protected NavMeshAgent _navigation;

    // 타겟 부채꼴범위판별 
    protected bool _isPlayerDetected = false; // 내가 플레이어를 찾음
    protected bool _isInCircularSector = true; // 부채꼴안에 들어옴 
    protected bool _rayzorHitPlayer = false;   //벽투과방지용 레이저

    // State
    protected EnemyState _enemyState = EnemyState.Patrol;
    public EnemyState GetEnemyCurState() { return _enemyState; }

    // Animator
    protected Animator _animator;
    protected AnimatorStateInfo _animatorStateInfo;

    // 처음 스폰 여부 확인
    protected bool _isFirstSpawn = true;

    //플레이어와 처음 조우하기전까진 움직임 x
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
        //공격일떈 스테이트변화 x
    if (_enemyState == EnemyState.Attack)
        return;

    // 플레이어가 조우하기전엔 스테이트변화  x
    CheckFirstMeetPlayer();
    if (!_isFirstMeet)
        return;
    //캐비넷같이 숨을수 있는곳 들어갔을때는 인식못하게 조절하기위해 숨으면 순찰
    bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(0);
            return;
        }

        // 부채꼴 판별 관련 코드
        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0; // y 축 이동을 방지하여 평면 이동만 가능하게 함

            // 벽투과방지용 bool 
            // 부채꼴안에 들어왔고, 사정거리 원안에 들어왔지만 사이에 벽이 있을때는 추격상태 x , 순찰모드
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
                    Debug.Log("공격중");
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
                    Debug.Log("죽음");
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
                Debug.Log("플레이어가 적을 찾았습니다.");
                return;
            }
        }
    }


    protected void SoundController()
    {
        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0; // y 축 이동을 방지하여 평면 이동만 가능하게 함

        //사운드 추가
        if (targetDirection.magnitude < _target.GetComponent<PlayerController>().GetCharacterData().DetectRange)
        {
            //심장소리 넣어주고
            SoundManager.Instance.SameStateJustOnePlay3D("Heart", transform, true, 1.0f);
        }
        else
        {
            SoundManager.Instance.Stop3D("Heart");
        }

    }

}