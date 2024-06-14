using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalerinaController : EnemyController
{
    //public List<GameObject> _mannequinPrefabs; // 5���� ����ŷ ������ ����Ʈ
    private GameObject _mannequinPrefab;
    private List<GameObject> _spawnedMannequins = new List<GameObject>(); // ������ ����ŷ ��ü ����Ʈ
    private int _currentMannequinIndex; // ���� �߷������� ��ġ�� ����ŷ �ε���
    private int randomManequinIndex; // ���� ����ŷ �ε���
    private float _nextDanceTime; // ���� �� �ð�
    AnimatorStateInfo _currentAnimatorState;
    private int _savedAnimationHash; // �ִϸ��̼� ���� �ؽ� ����


    //public AudioSource balletMusic; // �߷����� ���� 3d����� ������ֱ�

    override protected void Awake()
    {
        base.Awake();        
        //for (int i = 0; i < 5; i++)
        //{
        //    GameObject _manequin = Resources.Load("Prefabs/Character/Enemy/Manequin") as GameObject;
        //    _mannequinPrefabs.Add(_manequin);
        //}
        _mannequinPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Manequin");
    }
    protected override void Start()
    {
        base.Start(); 
        SpawnMannequins();
        //StartCoroutine(DanceRoutine());
        //Invoke("StartDance", Random.ran;
        //StartCoroutine(DanceRoutine());
    }

    private void Update()
    {
        //base.Update();

        if(Input.GetMouseButtonDown(0))
        {
            EndDance();
        }

        if (Input.GetMouseButtonDown(1))
        {
            StartDance();
        }
    }

    // ����ŷ�� ���� ��ġ�� �����ϴ� �Լ�
    private void SpawnMannequins()
    {
        for (int i = 0; i < 5; i++)
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
        // ������ ��ġ�� ��ȯ (������ ������ ����)
        // ����: Ư�� ���� ������ ������ ��ġ�� ��ȯ
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }

    // �� ��ƾ�� ó���ϴ� �ڷ�ƾ
    private IEnumerator DanceRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 7f));

            StartDance();
        }        
    }

    // ���� �����ϴ� �Լ�
    private void StartDance()
    {
        //���� ���ְ�
        //balletMusic.Play();
        //_animator.SetTrigger("StartDance");

        //gameObject.SetActive(true);
        //SwitchMannequin();

        //_animator.Play(_savedAnimationHash, -1, _savedAnimationTime);             

        //Invoke("EndDance", Random.Range(4f, 7f));

         randomManequinIndex = Random.Range(0, _spawnedMannequins.Count);

        Animator animator = _spawnedMannequins[randomManequinIndex].GetComponent<Animator>();
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(_savedAnimationHash, 0, _currentAnimatorState.normalizedTime);
        _spawnedMannequins[randomManequinIndex].transform.position = transform.position;
        _spawnedMannequins[randomManequinIndex].transform.rotation = transform.rotation;


        _animator.speed = 1.0f;
        //transform.Translate(GetRandomPosition());

        randomManequinIndex = Random.Range(0, _spawnedMannequins.Count);
        transform.position = _spawnedMannequins[randomManequinIndex].transform.position;
    }

    private void EndDance()
    {
        // ���� �ִϸ��̼� ���� ����
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        //_savedAnimationHash = _currentAnimatorState.fullPathHash;
        //_savedAnimationTime = _currentAnimatorState.normalizedTime;
        //foreach (GameObject spawnedMannequin in _spawnedMannequins)
        //{
        //    Animator animator = spawnedMannequin.GetComponent<Animator>();
        //    animator.Play(_currentAnimatorState.fullPathHash, 0, _currentAnimatorState.normalizedTime);
        //    animator.speed = 0.0f;
        //}

        //gameObject.SetActive(false);
        //transform.GetComponentInChildren<GameObject>().SetActive(false);
        _animator.speed = 0.0f;
        // �ı� �� ��ġ�� ����ŷ�� ����
        _spawnedMannequins[_currentMannequinIndex].SetActive(true);

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

        // �÷��̾ �����ϱ����� ������Ʈ��ȭ  x
        CheckFirstMeetPlayer();
        if (!_isFirstMeet)
            return;

        SetState(1);
        
    }
}
