using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public List<int> itemData = new List<int>();
    public Vector3 savePoint;
    public int curEvent;
    public string sceneName;
}

public class SaveManager : Singleton<SaveManager>
{
    private string path;
    private SaveData loadedData;
    private void Awake()
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");
        Debug.Log("Save path: " + path);
    }

    public void JsonLoad()
    {
        SaveData saveData = new SaveData();

        if (File.Exists(path))
        {
            Debug.Log("Load file path: " + path);
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);
            if (saveData != null)
            {
                loadedData = saveData;
                // GameManager.Instance.GetPlayer().transform.position = saveData.savePoint;
                //  GameManager.Instance.GetPlayer().GetComponent<EventManager>().SetKey(saveData.curEvent);
                SceneManager.LoadScene(saveData.sceneName);
                Debug.Log("LoadPoint: " + saveData.sceneName);
                SceneManager.sceneLoaded += OnSceneLoaded;

            }
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadedData != null)
        {
            GameManager.Instance.GetPlayer().transform.position = loadedData.savePoint;
            GameManager.Instance.GetPlayer().GetComponent<EventManager>().SetKey(loadedData.curEvent);
            Debug.Log("OnLoadPoint: " + loadedData.savePoint);

            foreach(int data in loadedData.itemData)
            {
                ItemData key = DataManager.Instance.GetItemData(data);
                UIManager.Instance.GetInventory.AddItem(key);
            }
            
            loadedData = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    public bool GetData()
    {
        return File.Exists(path);
    }

    public void JsonSave(Vector3 savePoint, int curEvent, List<int> itemData)
    {
        SaveData saveData = new SaveData();
        saveData.savePoint = savePoint;
        saveData.sceneName = SceneManager.GetActiveScene().name;
        saveData.curEvent = curEvent;
         saveData.itemData = itemData;
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
        Debug.Log("Save path: " + path);
        Debug.Log("SavePoint: " + savePoint);
    }
}
