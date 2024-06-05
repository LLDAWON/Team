using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : EnemyController
{

    protected override void StateUpdate()
    {
        PlayerController playerController = _target.GetComponent<PlayerController>();

        RaycastHit hit;
        if (Physics.Raycast(_target.position, _target.forward, out hit, playerController.GetCharacterData().DetectRange))
        {
            SetState(3);
            return;
        }
        base.StateUpdate();
    }
}
