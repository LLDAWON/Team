using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections;

[System.Serializable]
public class SaveData
{
    public List<int> itemList = new List<int>();
   
    public Vector3 savePoint;
    public int curEvent;
}


public class SaveManager : Singleton<SaveManager>
{
    private string path;
    private void Awake()
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");
    }

    public void JsonLoad()
    {
        SaveData saveData = new SaveData();

        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);
            if (saveData != null)
            {
                GameManager.Instance.GetPlayer().transform.position = saveData.savePoint;
                GameManager.Instance.GetPlayer().GetComponent<EventManager>().SetKey(saveData.curEvent);
                foreach(int data in saveData.itemList)
                {
                    ItemData itemdata = DataManager.Instance.GetItemData(data);
                    UIManager.Instance.GetInventory.AddItem(itemdata);
                }
            }
        }

        UIManager.Instance.White.SetActive(false);
        PlayerController playerController = GameManager.Instance.GetPlayer().GetComponent<PlayerController>();
        //playerController.GetHand().SetActive(true);
        playerController.SetIsPlayerDie(false); 
        Observer.OnNoneEvents[10]();
    }

    public bool GetData()
    {
        return File.Exists(path);
    }

    public void JsonSave(Vector3 savePoint,int curEvent,List<int>data)
    {
        SaveData saveData = new SaveData();
        saveData.savePoint = savePoint;
        saveData.curEvent = curEvent;
        saveData.itemList = data;
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}
