using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableCharactorController : MonoBehaviour
{

    [SerializeField]
    protected int _characterKey;

    // 확인용
    [SerializeField]
    protected Vector3 _velocity;

    

    // 움직임관련        
    protected float _moveSpeed = 0.0f;    
    protected bool _isAttack = false;
    protected CharacterData _characterData;

    //받는 컴포넌트 
    protected CapsuleCollider _collider;
    protected Rigidbody _rigidBody;


    virtual protected void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidBody = GetComponent<Rigidbody>();

        _characterData = DataManager.Instance.GetCharacterData(_characterKey);
    }

    virtual protected void Update()
    {
        
    }

    protected void FixedUpdate()
    {
        Move();        
    }


    protected void Move()
    {
        transform.Translate(_velocity * Time.fixedDeltaTime);
    }

    protected void Turn()
    {
       

        //// 방법 1
        //if (_velocity.x <= _eliespedValue && _velocity.z <= _eliespedValue)
        //    return;
        //Vector3 dir = new Vector3(_velocity.x, 0, _velocity.z);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.fixedDeltaTime * _rotateSpeed);


        //방법 2
        //if (_velocity.x <= _eliespedValue && _velocity.z <= _eliespedValue)
        //    return;

        //Quaternion turnRotation = Quaternion.LookRotation(_velocity);
        //_rigidBody.rotation = Quaternion.Slerp(_rigidBody.rotation, turnRotation, _rotateSpeed);


    }
    protected void Floor()
    {
        //if (_velocity.y > 0.0f) return;
        //_isJump = false;
        //_velocity.y = 0.0f;
        //Debug.Log("Floor");
    }

    protected void StopToWall()
    {

        //Debug.DrawRay(transform.position, transform.forward * 2.5f, Color.red);
        //
        //_isWallCollision = Physics.Raycast(transform.position, transform.forward, 2.5f, LayerMask.GetMask("Wall"));
    }

    protected void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Floor();
        }
    }



    //protected void GravityPress()
    //{
    //    _velocity.y -= _gravityScale * Time.fixedDeltaTime;
    //}

}
