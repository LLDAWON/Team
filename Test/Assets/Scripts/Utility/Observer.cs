using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{
    public delegate void OnUseItem(float value);
    public static Dictionary<int, OnUseItem> OnEvents = new();

    public delegate void OnTargetEvent(GameObject target);
    public static Dictionary<int, OnTargetEvent> OnTargetEvents = new();

    public delegate void OnEquip();
    public static Dictionary<int, OnEquip> OnNoneEvents = new();

    //1¹ø : µðÁ¹ºê
    public delegate IEnumerator OnDesolve(float time);
    public static Dictionary<int, OnDesolve> OnDesolveEvents = new();
}
