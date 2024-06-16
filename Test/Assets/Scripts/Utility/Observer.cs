using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{

    public delegate void OnUseItem(float value);
    public static Dictionary<int, OnUseItem> OnEvents = new();

    //1번 플레이어 Die Cam
    public delegate void OnTargetEvent(GameObject target);
    public static Dictionary<int, OnTargetEvent> OnTargetEvents = new();

    public delegate void OnEquip();
    public static Dictionary<int, OnEquip> OnNoneEvents = new();

    //1번 : 디졸브
    //2번 컷씬 카메라 이동
    public delegate IEnumerator OnDesolve(GameObject target);
    public static Dictionary<int, OnDesolve> OnDesolveEvents = new();

}
