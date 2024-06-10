using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testobject : MonoBehaviour
{
    public ObjectManager objectmanager;
    private bool iscollision = false;
    void Start()
    {
        //objectmanager.BookCase();
    }

    
    void Update()
    {
        objectmanager.BookCase();

    }



   
}
