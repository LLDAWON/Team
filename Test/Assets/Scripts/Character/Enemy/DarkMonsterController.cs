using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DarkMonsterController : EnemyController
{

    // ��ũ���Ͱ� �����ϴ� ���°�
    private float _floatSpeed = 1.0f; //�����ϴ� �ӵ�
    private float _floatAmplitude = 0.3f; // ���� ����
    public float _darkMonsterSpeed;
    private float _initialY; //�ʱ� y��

    //��ũ���� �ֺ����ִ� �ذ��
    private Transform _skulPanel;
    private List<Light> _skulLights;

    //��ũ������ �̵��� �����ϴ� �кҵ��� �����ϴ� ����Ʈ
    //�к��� ó���ʿ� off�� ���¸� ����
    private List<CandleScript> candles;
    private Dictionary<CandleScript, bool> candlePrevStates;
    private int _count;

    override protected void Awake()
    {
        base.Awake();
        //�ذ� ������Ʈ
        _skulPanel = transform.GetChild(0);
        _skulLights = _skulPanel.GetComponentsInChildren<Light>().ToList();
    }

    override protected void Start()
    {
        base.Start();


        _initialY = transform.position.y;
        _darkMonsterSpeed = _characterData.WalkSpeed;


        // ��ųʸ��� �кҵ��� �ʱ� ���·� �ʱ�ȭ
        candles = new List<CandleScript>(FindObjectsOfType<CandleScript>()); 
        candlePrevStates = new Dictionary<CandleScript, bool>();
        foreach (CandleScript candle in candles)
        {
            candlePrevStates[candle] = candle.GetLit();
        }
    }
    override protected void Update()
    {

        SkulRotate();
        SpeedIncresePerGetSkul();
        FloatingAir();
        base.Update();
    }


    override protected void StateUpdate()
    {
        // �� ���ݽ� ���°� ����
        if (_enemyState == EnemyState.Attack)
            return;
        // �кҾȿ� ������ none���� ����
        //�ٵ� �̷��� ��� �ȿ� ���� ���� �ƴұ�?
        if (IsWithinAnyCandleLight())
        {
            SetState(3);
            return;
        }
        //�÷��̾ ������ ����
        bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(0);
            return;
        }


        PlayerController playerController = _target.GetComponent<PlayerController>();
        //�÷��̾��� ��ä�� Ž��
        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        float _detectRange = playerController.GetCharacterData().DetectRange  ;

        //���������ȿ� ��������
        if (_inPlayerSight.magnitude <= _detectRange )
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            //��ä�� �ȿ� ��������
            if (degree <= _angleRange)
            {
                //��������
                if(playerController.GetIsLightOn() == true)
                    SetState(5); //�ֱ�
                else
                    SetState(1); // ����
            }
            //��ä�þȿ� �ȵ�������
            else
                SetState(1); //����
        }
        // �������� ���϶��� �����ؿ���
        else
        {
            SetState(1);
        }


    }


    protected override void EnemyAiPattern()
    {
        switch (_enemyState)
        {
            case EnemyState.Trace:
                {
                    _animator.speed = 1.0f;
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _darkMonsterSpeed;
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    _isAttack = true;
                    _animator.SetTrigger("Attack");
                    Observer.OnTargetEvents[1](gameObject);
                }
               
                break;
            case EnemyState.Die:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("����");
                }
                break;
            case EnemyState.Stop:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("����");
                }
                break;
        }
        Debug.Log(_enemyState);
    }

    private void FloatingAir()
    {

        Vector3 pos = transform.position;
        pos.y = _initialY + Mathf.Sin(Time.time * _floatSpeed) * _floatAmplitude;
        transform.position = pos;
        Debug.Log("����");

    }

   

    private void SkulRotate()
    {
        float rotZ = 20.0f;
        _skulPanel.Rotate(0, 0, rotZ * Time.deltaTime);

        foreach (Transform child in _skulPanel)
        {
            child.Rotate(0, 0, -rotZ * Time.deltaTime);
        }

    }

    private void SpeedIncresePerGetSkul()
    {
        //�к�����Ʈ �к�ų������ ���ǵ�++;

        foreach (CandleScript candle in candles)
        {
            bool previousState = candlePrevStates[candle];
            bool currentState = candle.GetLit();

            // �к��� ������ �����־��� ���� �����ִٸ� �ӵ� ����
            if (!previousState && currentState)
            {
                _darkMonsterSpeed += 1.0f; // �к� �ϳ��� �ӵ��� 1 ����
            }

            int candleIndex = candles.IndexOf(candle);

            if (candleIndex >= 0 && candleIndex < _skulLights.Count)
            {
                Light skulLight = _skulLights[candleIndex];
                skulLight.intensity = currentState ? 1.0f : 0.0f; // �к��� ���� ������ ���� 1, ���� ������ ���� 0
            }
            // ���� ���¸� ��ųʸ��� ������Ʈ
            candlePrevStates[candle] = currentState;
        }


        //�ذ� �� ���ֱ�

    }

    private bool IsWithinAnyCandleLight()
    {
        if(candles.Count == 0) 
            return false;

        foreach (CandleScript candle in candles)
        {
            if (candle.IsWithinLight(transform.position))
                return true;
        }
        return false;
    }

    private void AvoidCandles()
    {
        // �к� �ֺ��� ���ϵ��� �̵� ��� ���
        
    }

    private void EndAttack()
    {
        StartCoroutine(Observer.OnDesolveEvents[1](gameObject));
    }
}
