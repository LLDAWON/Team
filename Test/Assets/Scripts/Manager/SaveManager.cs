using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public Vector3 savePoint;
    public int curEvent;
}


public class SaveManager : Singleton<SaveManager>
{
    private string path;
    private void Awake()
    {
        path = Path.Combine(Application.persistentDataPath + "/Data/", "database.json");
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
            }
        }
    }

    public bool GetData()
    {
        return File.Exists(path);
    }

    public void JsonSave(Vector3 savePoint,int curEvent)
    {
        SaveData saveData = new SaveData();
        saveData.savePoint = savePoint;
        saveData.curEvent = curEvent;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}
