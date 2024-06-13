using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalerinaController : EnemyController
{
    public List<GameObject> _mannequinPrefabs; // 5���� ����ŷ ������ ����Ʈ
    private List<GameObject> _spawnedMannequins = new List<GameObject>(); // ������ ����ŷ ��ü ����Ʈ
    private int _currentMannequinIndex; // ���� �߷������� ��ġ�� ����ŷ �ε���
    private float _nextDanceTime; // ���� �� �ð�
    private bool _isDancing = false; // �� ����
    AnimatorStateInfo _currentAnimatorState;
    private float _savedAnimationTime = 0f; // �ִϸ��̼� �ð� ����
    private int _savedAnimationHash; // �ִϸ��̼� ���� �ؽ� ����


    //public AudioSource balletMusic; // �߷����� ���� 3d����� ������ֱ�

    override protected void Awake()
    {
        base.Awake();
        for (int i = 0; i < 5; i++)
        {
            GameObject _manequin = Resources.Load("Prefabs/Character/Enemy/Manequin") as GameObject;
            _mannequinPrefabs.Add(_manequin);
        }
    }
    protected override void Start()
    {
        base.Start(); 
        SpawnMannequins();
        StartCoroutine(DanceRoutine());
    }

    // ����ŷ�� ���� ��ġ�� �����ϴ� �Լ�
    private void SpawnMannequins()
    {
        for (int i = 0; i < _mannequinPrefabs.Count; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject mannequin = Instantiate(_mannequinPrefabs[i], randomPosition, Quaternion.identity);
            mannequin.SetActive(true);
            _spawnedMannequins.Add(mannequin);
        }

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
            if (!_isDancing)
            {
                yield return new WaitForSeconds(Random.Range(4f, 7f));
                StartDance();
            }
            else
            {
                yield return null;
            }
        }
    }

    // ���� �����ϴ� �Լ�
    private void StartDance()
    {
        //���� ���ְ�
        //balletMusic.Play();
        //_animator.SetTrigger("StartDance");
        _isDancing = true;

        _animator.Play(_savedAnimationHash, -1, _savedAnimationTime);

        _nextDanceTime = Time.time + Random.Range(4f, 7f);
        Invoke("EndDance", _nextDanceTime - Time.deltaTime);
    }

    // ���� �����ϴ� �Լ�
    private void EndDance()
    {
        //���ǲ��ְ�
        //balletMusic.Stop();
        //_animator.SetTrigger("EndDance");
        MoveToRandomMannequin();
        _isDancing = false;
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

    private  void DestroyMonster()
    {
        // ���� �ִϸ��̼� ���� ����
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        _savedAnimationHash = _currentAnimatorState.fullPathHash;
        _savedAnimationTime = _currentAnimatorState.normalizedTime;

        gameObject.SetActive(false);

        // �ı� �� ��ġ�� ����ŷ�� ����
        GameObject mannequin = Instantiate(_mannequinPrefabs[_currentMannequinIndex], transform.position, transform.rotation);
        mannequin.SetActive(true);
        _spawnedMannequins[_currentMannequinIndex] = mannequin;
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
