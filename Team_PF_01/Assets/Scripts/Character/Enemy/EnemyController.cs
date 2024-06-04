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
        _targetRotation = transform.rotation; // 초기 목표 회전 설정
    }

    protected override void Update()
    {
        MoveControl();
        SmoothRotate();
    }


    virtual protected void MoveControl()
    {
        //플레이어의 카메라가 바라보고있다면 멈춰야한다.
        //안보고있다면 달려야한다.
        //rayCast로 설정해보자

        Vector3 direction = _target.transform.position - transform.position;
        direction.y = 0; // y 축 이동을 방지하여 평면 이동만 가능하게 함
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, _detectionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                _velocity = direction.normalized * _characterData.RunSpeed;
                //_animator.SetFloat("Speed", _characterData.RunSpeed);
                _isRotating = false; // 플레이어를 추적할 때는 회전하지 않음
                return;
            }
            else if (hit.collider.CompareTag("Wall"))
            { 
                // 벽이 범위 내에 있다면 서서히 Y축으로 90도 회전
                _targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0);
                _isRotating = true; // 회전 플래그 설정
                _velocity = Vector3.zero;
               // _animator.SetFloat("Speed", 0);
                return;
            }
        }

        _velocity = Vector3.zero;
        //_animator.SetFloat("Speed", 0);
        _isRotating = false; // 아무것도 없을 때는 회전하지 않음
    }
    private void SmoothRotate()
    {
        if (_isRotating)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * _rotateSpeed);
            // 회전이 거의 완료되면 회전 플래그를 해제
            if (Quaternion.Angle(transform.rotation, _targetRotation) < 1.0f)
            {
                transform.rotation = _targetRotation;
                _isRotating = false;
            }
        }
    }

}
