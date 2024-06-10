using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testobject : MonoBehaviour
{
    public ObjectManager objectmanager;
    private bool iscollision = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (!iscollision)
        //{
        //    objectmanager.Falling();
        //}
        objectmanager.BookCase();
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            iscollision = true;

        }
    }
}
