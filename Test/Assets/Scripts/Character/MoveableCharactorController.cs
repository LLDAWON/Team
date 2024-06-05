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
    protected bool _isPlay = true;
    protected float _moveSpeed = 0.0f;   
    protected int _playMove;
    protected bool _isAttack = false;
    protected CharacterData _characterData;

    public CharacterData GetCharacterData() { return _characterData; }

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
        _playMove = _isPlay ? 1 : 0;
    }

    protected void FixedUpdate()
    {
        Move();
        CheckFoward();

    }


    protected void Move()
    {
        transform.Translate(_velocity * _playMove * Time.fixedDeltaTime);
    }

    protected void CheckFoward()
    {
        Debug.DrawRay(transform.position, transform.forward * _characterData.DetectRange, Color.red);
        
    }

}
