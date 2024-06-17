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


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string path;
    private void Awake()
    {
        Instance = this;

       

    }
    void Start()
    {
        path = Path.Combine(Application.dataPath + "/Data/", "database.json");
        
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
                GameManager.Instance._savePoint = saveData.savePoint;
                GameManager.Instance._curEvent = saveData.curEvent;
            }
        }
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
