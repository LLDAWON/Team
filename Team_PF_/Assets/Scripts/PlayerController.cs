using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MoveableCharactorController
{





    protected override void Update()
    {
        MoveController();
        ItemUseController();
    }


    private void MoveController()
    {
        //
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.z = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _moveSpeed = _characterData.RunSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            _moveSpeed = _characterData.CrawlingSpeed;
        }
        else
        {
            _moveSpeed = _characterData.WalkSpeed;
        }
        _velocity.x *= _moveSpeed;
        _velocity.z *= _moveSpeed;
    }

    private void ItemUseController()
    {
        if (Input.GetKey(KeyCode.F))
        {
            // 아이템 상호작용 이떄의 아이템 코드에 따라서 행동이 달라지게 
            // 장비템일경우 (Type == 1) 장착하고
            // 키일경우 (Type == 2) 범위 측정해서 해당키와 문이 일치할경우 
            // 소비템일경우 (Type == 3) 게이지 회복하고
            //
        }

    }

}
