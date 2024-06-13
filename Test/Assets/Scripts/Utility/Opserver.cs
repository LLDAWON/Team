using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opserver
{
    public delegate void OnUseItem(float value);
    public static Dictionary<int, OnUseItem> OnEvents = new();


}
