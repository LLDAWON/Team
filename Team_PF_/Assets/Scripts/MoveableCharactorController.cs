using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableCharactorController : MonoBehaviour
{

    [SerializeField]
    protected int _characterKey;
    [SerializeField]
    protected float _gravityScale = 9.8f;





    // 확인용
    [SerializeField]
    protected Vector3 _velocity;

    // 움직임관련
    protected float _crawlingSpeed;
    protected float _walkSpeed;
    protected float _runSpeed;
    protected float _moveSpeed = 0.0f;
    protected bool _isJump = false;
    protected bool _isAttack = false;


    //받는 컴포넌트 
    protected Animator _animator;
    protected CharacterData _characterData;
    protected CapsuleCollider _collider;
    


    virtual protected void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();

        _characterData = DataManager.Instance.GetCharacterData(_characterKey);
    }

    virtual protected void Update()
    {
        
    }

    protected void FixedUpdate()
    {
        Move();
        Turn();
        //Jump();
    }


    protected void Move()
    {
        transform.Translate(_velocity * Time.fixedDeltaTime);
    }

    protected void Turn()
    {

    }
    //protected void Jump()
    //{
    //    transform.Translate(_velocity * Time.fixedDeltaTime);
    //}

}
