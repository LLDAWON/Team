using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MoveableCharactorController
{

    protected float _detectionRange = 5.0f;
    protected Quaternion _targetRotation;
    protected bool _isRotating = false;


    protected Transform _target;


    protected void Start()
    {
        _target = GameObject.Find("Player").transform;
        _targetRotation = transform.rotation; // �ʱ� ��ǥ ȸ�� ����
    }

    protected override void Update()
    {
        MoveControl();
        SmoothRotate();
    }


    virtual protected void MoveControl()
    {
        //�÷��̾��� ī�޶� �ٶ󺸰��ִٸ� ������Ѵ�.
        //�Ⱥ����ִٸ� �޷����Ѵ�.
        //rayCast�� �����غ���

        Vector3 direction = _target.transform.position - transform.position;
        direction.y = 0; // y �� �̵��� �����Ͽ� ��� �̵��� �����ϰ� ��
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, _detectionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                _velocity = direction.normalized * _characterData.RunSpeed;
                //_animator.SetFloat("Speed", _characterData.RunSpeed);
                _isRotating = false; // �÷��̾ ������ ���� ȸ������ ����
                return;
            }
            else if (hit.collider.CompareTag("Wall"))
            { 
                // ���� ���� ���� �ִٸ� ������ Y������ 90�� ȸ��
                _targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0);
                _isRotating = true; // ȸ�� �÷��� ����
                _velocity = Vector3.zero;
               // _animator.SetFloat("Speed", 0);
                return;
            }
        }

        _velocity = Vector3.zero;
        //_animator.SetFloat("Speed", 0);
        _isRotating = false; // �ƹ��͵� ���� ���� ȸ������ ����
    }
    private void SmoothRotate()
    {
        if (_isRotating)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * _rotateSpeed);
            // ȸ���� ���� �Ϸ�Ǹ� ȸ�� �÷��׸� ����
            if (Quaternion.Angle(transform.rotation, _targetRotation) < 1.0f)
            {
                transform.rotation = _targetRotation;
                _isRotating = false;
            }
        }
    }

}
