using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMonsterController : EnemyController
{
    private bool _isMeet = false;
    private bool _isDie =false;
    //������ ���°�
    protected float _desolveSpeed = 0.3f;

    override protected void Start()
    {
       base.Start();
    }
    


    protected override void StateUpdate()
    {
        //���ݻ����϶� ����
        if (_enemyState == EnemyState.Attack)
            return;
        if (_isDie)
            return;

        //�÷��̾ ������ �ǵ�����
        if (_target.GetComponent<PlayerController>().GetIsPlayerDie() == true)
            return;

        PlayerController playerController = _target.GetComponent<PlayerController>();

        if (playerController.GetIsPlayerDie() == true)
            SetState((int)EnemyState.None);
        //�÷��̾��� ��ä�� Ž��
        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        float _detectRange = playerController.GetCharacterData().DetectRange;

        //���������ȿ� ��������
        if (_inPlayerSight.magnitude <= _detectRange)
        {

            _target.GetComponent<PlayerController>().GetHeartBeatSound().volume = 1.0f;
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            //��ä�� �ȿ� ��������
            if (degree <= _angleRange)
            {
                
                //��������
                if (playerController.GetIsFlashLight() == true)
                {

                    if (!_isMeet)
                    {
                        _isMeet = true;
                        SetState(5); //�ֱ�
                    }
                } 
                    
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
            _target.GetComponent<PlayerController>().GetHeartBeatSound().volume = 0.0f;
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
                    _navigation.speed = _characterData.RunSpeed;
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    _isAttack = true;

                    _animator.SetTrigger("Attack");
                    SoundManager.Instance.Stop3D("Mini_Trace");
                    Debug.Log("EnemyState.Attack");

                    Observer.OnTargetEvents[1](gameObject);
                }

                break;
            case EnemyState.Die:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    _isDie = true;
                    Debug.Log("����");
                    SoundManager.Instance.Stop3D("Mini_Trace"); 
                    _collider.enabled = false;
                    DesolveAndTeleport();
                }
                break;
            case EnemyState.None:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("None");
                }
                break;
        }
    }
    private void DesolveAndTeleport()
    {
        // �к� �ֺ��� ���ϵ��� �̵� ��� ���
        StartCoroutine(DisolveEffect());
        SetState((int)EnemyState.None);
    }
    public IEnumerator DisolveEffect()
    {

        Renderer[] _renderers = transform.GetComponentsInChildren<Renderer>();

        float _time = 0.0f;

        while (_time < 1.0f)
        {
            _time += _desolveSpeed * Time.deltaTime;

            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_DesolveTime", _time);
                renderer.material.SetColor("DesolveColor", Color.white);
            }

            yield return null;
        }
        if (_time > 1.0f)
        {
            foreach (Renderer renderer in _renderers)
            {
                //path�� ������ ��ҿ��� �¾�� ���ְ� ������ �ʱⰪ�� ���� �ʱⰪ���� ����
                //
                renderer.material.SetFloat("_DesolveTime", 0.0f);
                renderer.material.SetColor("DesolveColor", Color.red);

            }

            int random = Random.Range(0, 4);
            _collider.enabled = true;
            _isDie = false;
            _isMeet = false;
            transform.position = pathes[random];
            SetState((int)EnemyState.Trace);

        }
    }
}
