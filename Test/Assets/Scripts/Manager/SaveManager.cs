using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public List<int> itemList = new List<int>();
   
    public Vector3 savePoint;
    public int curScene;
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
                SceneManager.LoadScene(saveData.curScene);

                foreach(int data in saveData.itemList)
                {
                    ItemData itemdata = DataManager.Instance.GetItemData(data);
                    UIManager.Instance.GetInventory.AddItem(itemdata);
                }
            }
        }
    }

    public bool GetData()
    {
        return File.Exists(path);
    }

    public void JsonSave(Vector3 savePoint,int curEvent,List<int>data)
    {
        SaveData saveData = new SaveData();
        saveData.savePoint = savePoint;
        saveData.curScene = curEvent;
        saveData.itemList = data;
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}
