using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TeacherController))]
public class EnemyRange : Editor
{
    private void OnSceneGUI()
    {
        EnemyController enemy = (EnemyController)target;

        Handles.color = new Color(1, 1, 1, 0.2f);

        Handles.DrawSolidDisc(enemy.transform.position, Vector3.up, enemy.GetCharacterData().DetectRange);

        Handles.color = new Color(0, 1, 0, 0.2f);

        float angle = enemy.transform.eulerAngles.y - 30.0f;

        Vector3 pos = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),
            0, Mathf.Cos(angle * Mathf.Deg2Rad));
        //Vector3 pos = enemy.transform.forward * enemy.GetCharacterData().DetectRange;

        Handles.DrawSolidArc(enemy.transform.position, Vector3.up, pos, 60.0f, enemy.GetCharacterData().DetectRange);
    }
}
