using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(TeacherController))]
public class EnemyRange : Editor
{
    private float _angleRange = 30f;
    private float _detectRange;


    private void Start()
    {
        //_detectRange = _char
    }
     
    private void Update()
    {
        
    }


    private void OnSceneGUI()
    {
        //�̶��� Ÿ���� Editor����� ����ϱ� ���� Ÿ���̴�.

    }
}
