using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : EnemyController
{



   
   

    protected override void StateUpdate()
    {
        //√π¡∂øÏ¿¸ø£ ∏ÿ√„
        if (!_isFirstMeet)
            return;
        //º˚æÓ¿÷¿∏∏È ∏ÿ√„
        bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(3);
            return;
        }
        //«√∑π¿ÃæÓ∞° πŸ∂Û∫∏∏È ∏ÿ√„
        PlayerController playerController = _target.GetComponent<PlayerController>();
        

        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        float _detectRange = playerController.GetCharacterData().DetectRange * 2.0f;

        if (_inPlayerSight.magnitude <= _detectRange)
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            if (degree <= _angleRange)
            {
                SetState(3);
                return;
            }
            else
            {
                SetState(1);
            }
        }
        else
        {
            SetState(1);
        }


        



        //PlayerController playerController = _target.GetComponent<PlayerController>();

        //Vector3 _inPlayerSight  = transform.position - _target.transform.position;
        //_inPlayerSight.y = 0;


        //if (_inPlayerSight.magnitude <= _characterData.DetectRange)
        //{
        //    float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

        //    float theta = Mathf.Acos(dot);

        //    float degree = Mathf.Rad2Deg * theta;

        //    if (degree <= _angleRange)
        //    {
        //        SetState(3);
        //        return;
        //    }
        //    else
        //    {
        //        base.StateUpdate();
        //    }
        //}
        //else
        //{
        //    base.StateUpdate();
        //}



    }
}
