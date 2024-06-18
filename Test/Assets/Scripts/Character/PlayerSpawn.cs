using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{   void Start()
    {
        GameObject player = GameManager.Instance.GetPlayer();

        player.transform.position = transform.position;
    }
}
