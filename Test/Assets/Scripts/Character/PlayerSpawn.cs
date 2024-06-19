using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UIElements;

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

        EventManager _eventManager = player.GetComponent<EventManager>();
        List<int> item = UIManager.Instance.GetInventory.SaveItem;
        SaveManager.Instance.JsonSave(transform.position, _eventManager.CurKey, item);

    }
}
