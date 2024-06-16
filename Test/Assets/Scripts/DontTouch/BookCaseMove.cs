using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class BookCaseMove : MonoBehaviour
{
    private enum MoveType
    {
        Front,Back,LeftTurn,RightTurn
    }
    private Collider _collider;


    private MoveType moveType;  

    private void Update()
    {
        StartCoroutine(Move());
    }


    private IEnumerator Move()
    {
        
        yield return new WaitForSeconds(3.0f);
    }

    //private IEnumerator Move()
    //{
    //    int random = Random.Range(1, 5);
    //    switch (random)
    //    {
    //        case 1:
    //            Front();
    //            moveType = MoveType.Front;
    //            break;
    //        case 2:
    //            moveType = MoveType.Back;
    //            break;
    //        case 3:
    //            moveType = MoveType.LeftTurn;
    //            break;
    //        case 4:
    //            moveType = MoveType.RightTurn;
    //            break;
    //    }
        
    //    yield return new WaitForSeconds(3.0f);

    //}

    //private void Front()
    //{
    //    if (moveType == MoveType.Front) return;

    //}












}
