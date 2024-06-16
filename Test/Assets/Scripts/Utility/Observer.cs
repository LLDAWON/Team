using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{

    public delegate void OnUseItem(float value);
    public static Dictionary<int, OnUseItem> OnEvents = new();

    //1�� �÷��̾� Die Cam
    public delegate void OnTargetEvent(GameObject target);
    public static Dictionary<int, OnTargetEvent> OnTargetEvents = new();

    public delegate void OnEquip();
    public static Dictionary<int, OnEquip> OnNoneEvents = new();

    //1�� : ������
    //2�� �ƾ� ī�޶� �̵�
    public delegate IEnumerator OnDesolve(GameObject target);
    public static Dictionary<int, OnDesolve> OnDesolveEvents = new();

}
