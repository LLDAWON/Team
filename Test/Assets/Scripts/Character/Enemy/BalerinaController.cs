using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalerinaController : EnemyController
{
    private GameObject _mannequinPrefab;
    private List<GameObject> _spawnedMannequins = new List<GameObject>(); // ������ ����ŷ ��ü ����Ʈ
    private int _currentMannequinIndex; // ���� �߷������� ��ġ�� ����ŷ �ε���
    private int randomManequinIndex; // ���� ����ŷ �ε���
    private float _nextDanceTime; // ���� �� �ð�
    AnimatorStateInfo _currentAnimatorState;
    private int _savedAnimationHash; // �ִϸ��̼� ���� �ؽ� ����
    private float _savedAnimationTime;


    //public AudioSource balletMusic; // �߷����� ���� 3d����� ������ֱ�

    override protected void Awake()
    {
        base.Awake();        
        _mannequinPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Manequin");
    }
    protected override void Start()
    {
        base.Start(); 
        SpawnMannequins();
        StartCoroutine(DanceRoutine());
    }

    override protected void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            StopDance();
        }

        if (Input.GetMouseButtonDown(1))
        {
            StartDance();
        }

        base.Update();
    }

    // ����ŷ�� ���� ��ġ�� �����ϴ� �Լ�
    private void SpawnMannequins()
    {
        for (int i = 0; i < 10; i++)
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

    private void SwitchMannequin()
    {
        _currentMannequinIndex = Random.Range(0, _spawnedMannequins.Count);
        Transform currentMannequinTransform = _spawnedMannequins[_currentMannequinIndex].transform;
        transform.position = currentMannequinTransform.position;
        transform.rotation = currentMannequinTransform.rotation;
        _spawnedMannequins[_currentMannequinIndex].SetActive(false);
    }

    // ���� ��ġ�� ��ȯ�ϴ� �Լ�
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }

    // �� ��ƾ�� ó���ϴ� �ڷ�ƾ
    private IEnumerator DanceRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(6.0f);

            StartDance();
        }        
    }

    // ���� �����ϴ� �Լ�
    private void StartDance()
    {
        //���� ���ְ�
        //balletMusic.Play();
        //_animator.SetTrigger("StartDance");
    

         randomManequinIndex = Random.Range(0, _spawnedMannequins.Count);

        Animator animator = _spawnedMannequins[randomManequinIndex].GetComponent<Animator>();
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(_savedAnimationHash, 0, _currentAnimatorState.normalizedTime);
        _spawnedMannequins[randomManequinIndex].transform.position = transform.position;
        _spawnedMannequins[randomManequinIndex].transform.rotation = transform.rotation;

        SetState(1);
        //transform.Translate(GetRandomPosition());

        randomManequinIndex = Random.Range(0, _spawnedMannequins.Count);
        transform.position = _spawnedMannequins[randomManequinIndex].transform.position;
        Invoke("StopDance", 5.0f);
    }

    private void StopDance()
    {
        // ���� �ִϸ��̼� ���� ����
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        _savedAnimationHash = _currentAnimatorState.fullPathHash;
        _savedAnimationTime = _currentAnimatorState.normalizedTime;

        //���� ���� ����
        Quaternion currentRotation = transform.rotation;
        transform.rotation = currentRotation;

        //�������� �ʰ� ���ְ� �ӵ� 0 �ִϸ��̼� �ӵ�0 ���߰��ϱ�
        SetState(4);

    }
    // �߷������� ������ ����ŷ ��ġ�� �̵���Ű�� �Լ�
    private void MoveToRandomMannequin()
    {

        _spawnedMannequins[_currentMannequinIndex].SetActive(true);

        _currentMannequinIndex = Random.Range(0, _spawnedMannequins.Count);
        Transform newMannequinTransform = _spawnedMannequins[_currentMannequinIndex].transform;
        transform.position = newMannequinTransform.position;
        transform.rotation = newMannequinTransform.rotation;

        _spawnedMannequins[_currentMannequinIndex].SetActive(false);
    }


    override protected void StateUpdate()
    {
        //�����ϋ� ������Ʈ��ȭ x
        if (_enemyState == EnemyState.Attack)
            return;
        if (_enemyState == EnemyState.None)
            return;

        // �÷��̾ �����ϱ����� ������Ʈ��ȭ  x
        CheckFirstMeetPlayer();
        if (!_isFirstMeet)
            return;

        SetState(1);
        
    }
    protected override void EnemyAiPattern()
    {

        switch (_enemyState)
        {
            case EnemyState.Trace:
                {
                    _animator.speed = 0.5f;
                    transform.Rotate(0, 1.0f, 0);
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _characterData.WalkSpeed;
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    _isAttack = true;

                    //_animator.SetTrigger("Attack");
                    Debug.Log("EnemyState.Attack");

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
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("None");
                }
                break;
        }
    }
}
