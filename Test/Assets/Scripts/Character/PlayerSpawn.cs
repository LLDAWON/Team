using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        if(GameManager.Instance == null)
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/GameManager");
            Instantiate(obj);
        }        
    }
    void Start()
    {
        GameObject player = GameManager.Instance.GetPlayer();

        player.transform.position = transform.position;
        
    }
}
