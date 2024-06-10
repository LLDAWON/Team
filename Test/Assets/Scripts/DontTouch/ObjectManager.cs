using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager _instance;
    public static ObjectManager Instance
    { get { return _instance; } }


    public GameObject celing;
    public GameObject _bookCase;
    private Vector3 yPos;
    private float speed = 10.0f;
    public float maxRotationX = 45f;
    public void Falling( )
    {
        
       // celing.transform.Translate(Vector3.down * speed*Time.deltaTime );
    }
    public void BookCase()
    {

        Vector3 currentRotation = _bookCase.transform.rotation.eulerAngles;

        
        currentRotation.x = Mathf.Clamp(currentRotation.x, 0f, maxRotationX);

        
        _bookCase.transform.rotation = Quaternion.Euler(currentRotation);
       
    }
   



}
