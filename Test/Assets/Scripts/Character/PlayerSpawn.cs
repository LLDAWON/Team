using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

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
        GameObject player = NetWork.Instance.player;

        player.transform.position = transform.position;

        EventManager _eventManager = player.GetComponent<EventManager>();
        List<int> item = UIManager.Instance.GetInventory.SaveItem;
        SaveManager.Instance.JsonSave(transform.position, SceneManager.GetActiveScene().buildIndex, item);

    }
}
