using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    // 부채꼴 범위판별 관련
    protected float _angleRange = 30f;
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
    }


    protected void Move()
    {
        transform.Translate(_velocity * _playMove * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        //Handles.color = new Color(1, 1, 1, 0.2f);
        //
        //Handles.DrawSolidDisc(transform.position, Vector3.up, _characterData.DetectRange);
        //
        //Handles.color = new Color(0, 1, 0, 0.2f);
        //
        //float angle = transform.eulerAngles.y - _angleRange;
        //
        //Vector3 pos = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),
        //    0, Mathf.Cos(angle * Mathf.Deg2Rad));
        //
        //Handles.DrawSolidArc(transform.position, Vector3.up, pos, _angleRange * 2.0f, _characterData.DetectRange);
    }

}
