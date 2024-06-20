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
    private GameObject _spotLight;

    //�߷������� �ڷ�ƾ�� ����
    private Coroutine _danceRoutineCoroutine;
    private Coroutine _drawCircleCoroutine;
    // �߷������� ���� ������ �����ϴ� ���� ���õ� 
    private LineRenderer _lineRenderer;
    private float _circleRadius; // ���� ������
    private float _circleGrowthRate; // ���� �����ϴ� �ӵ�
    private int _numSegments = 100; // ���� �׸� �� ����� ���׸�Ʈ ��


    override protected void Awake()
    {
        base.Awake();
        _mannequinPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Manequin");
        _spotLight = transform.GetChild(1).gameObject;
        //������
        CircleSetting();
    }
    protected override void Start()
    {
        base.Start();
        SpawnMannequins();
        _danceRoutineCoroutine = StartCoroutine(DanceRoutine());

    }

    // ����ŷ�� ���� ��ġ�� �����ϴ� �Լ�
    private void SpawnMannequins()
    {
        for (int i = 0; i < 30; i++)
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

    // ���� ��ġ�� ��ȯ�ϴ� �Լ�
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f));
    }

    // �� ��ƾ�� ó���ϴ� �ڷ�ƾ
    private IEnumerator DanceRoutine()
    {
        while (_isAttack == false)
        {
            yield return new WaitForSeconds(7.0f);

            StartDance();
        }
    }

    // ���� �����ϴ� �Լ�
    private void StartDance()
    {
        //���� ���ְ�
        //balletMusic.Play();

        //����ŷ�� ���� ���߱� ���� ��ġ�� �����ֱ�

        //�ٽ� ������ ����༭ �߷����� ��ü�� �Ű��ֱ�
        randomManequinIndex = Random.Range(0, _spawnedMannequins.Count);
        transform.position = _spawnedMannequins[randomManequinIndex].transform.position;


        Animator animator = _spawnedMannequins[randomManequinIndex].GetComponent<Animator>();
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(_savedAnimationHash, 0, _currentAnimatorState.normalizedTime);
        //Light���ֱ�
        _spotLight.SetActive(true);

        ///���ߴµ��� ���� �����ϸ鼭 �� �ȿ� ������ ����
        // �� ���� ���������� �÷����Ҷ��� ��������Ѵ�.
        //���ʱ�ȭ�� �ٽ� �׸���
        _circleRadius = 0f;
        _circleGrowthRate = 2.0f; // ���� �����ϴ� �ӵ� ����
        _lineRenderer.enabled = true;

        StartCoroutine(GrowCircle());
        Invoke("StopDance", 5.0f);
    }

    private void StopDance()
    {
        // ���� �ִϸ��̼� ���� ����
        _currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        _savedAnimationHash = _currentAnimatorState.fullPathHash;
        _savedAnimationTime = _currentAnimatorState.normalizedTime;
        // ����Ʈ���ֱ�
        _spotLight.SetActive(false);

        //���� ���� ����
        Quaternion currentRotation = transform.rotation;
        transform.rotation = currentRotation;

       // ���׸��°� �����ֱ�
        _lineRenderer.enabled = false;
        //�������� �ʰ� ���ְ� �ӵ� 0 �ִϸ��̼� �ӵ�0 ���߰��ϱ�
       
        //�߷����� �Ű��ֱ�
        //Invoke("TeleportBalerina", 1.0f);

        SetState(0);

    }
    // �߷������� ������ ����ŷ ��ġ�� �̵���Ű�� �Լ�
    
    private void TeleportBalerina()
    {

        _spawnedMannequins[randomManequinIndex].transform.position = transform.position;
        _spawnedMannequins[randomManequinIndex].transform.rotation = transform.rotation;
        float randomX = 0;
        float randomZ = 0;
        while ((randomX + randomZ) < 5.0f)
        {
            randomX = Random.Range(-5f, 5f);
            randomZ = Random.Range(-5f, 5f);
        }
        transform.position = _target.position + new Vector3(randomX, 0, randomZ);
    }
    private void OnDrawGizmos()
    {
        
    }
    private void DrawCircle(float radius)
    {
        float deltaTheta = (2.0f * Mathf.PI) / _numSegments;
        float theta = 0f;

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            _lineRenderer.SetPosition(i, new Vector3(x, 0, z));
            theta += deltaTheta;
        }
    }
    private IEnumerator GrowCircle()
    {
        while (_lineRenderer.enabled)
        {
            _circleRadius += _circleGrowthRate * Time.deltaTime;
            DrawCircle(_circleRadius);

            yield return null;
        }
    }
    override protected void StateUpdate()
    {
        //�����ϋ� ������Ʈ��ȭ x

        if (_enemyState == EnemyState.Attack)
            return;

        // �÷��̾ �� �ȿ� �ִ��� Ȯ��
        if (Vector3.Distance(transform.position, _target.position) <= _circleRadius)
        {
            SetState(1);
        }

    }
    protected override void EnemyAiPattern()
    {

        switch (_enemyState)
        {
            case EnemyState.Patrol:
                {
                    _animator.speed = 1.0f;
                    _navigation.speed = 1.0f;
                    Debug.Log("patrol");
                }
                break;
            case EnemyState.Trace:
                {
                    if (_danceRoutineCoroutine != null)
                    {
                        StopCoroutine(_danceRoutineCoroutine); // Attack ������ �� �ڷ�ƾ ����
                        _danceRoutineCoroutine = null;
                        _lineRenderer.enabled = false;
                    }
                    _animator.SetTrigger("Trace");
                    _animator.speed = 2.0f;
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _characterData.WalkSpeed;
                    Debug.Log("Trace");
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    _isAttack = true;
                    _animator.SetTrigger("Attack");
                    Debug.Log("Attack");

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
                    _navigation.speed = 0.0f;
                    Debug.Log("None");
                }
                break;
        }
    }

    private void CircleSetting()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.2f;
        _lineRenderer.endWidth = 0.2f;  
        _lineRenderer.positionCount = _numSegments + 1;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;
        _lineRenderer.enabled = false;
    }
}
