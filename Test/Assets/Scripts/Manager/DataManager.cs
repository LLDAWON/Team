using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterData
{
    public int Key;
    public string Name;
    public float WalkSpeed;
    public float RunSpeed;
    public float CrawlingSpeed;
    public float Stemina;
    public int Type;
    public int Room;
    public float DetectRange;
    public float RotateSpeed;
}
public struct ItemData
{
    public int Key;
    public string Name;
    public float Value;
    public string ImagePath;
    public int Type;
}
public struct EventData
{
    public int Key;
    public int Condition;
    public string EventTag;
    public int EventCount;
    public int TextDataKey;
    public int GetItemKey;
    public int Type;
}

public struct TextData
{
    public int Key;
    public string Text;
    public int Type;
}

public class DataManager : Singleton<DataManager>
{   
    private Dictionary<int, CharacterData> characterDatas = new Dictionary<int, CharacterData>();
    private Dictionary<int, ItemData> itemDatas = new Dictionary<int, ItemData>();
    private Dictionary<int, TextData> textDatas = new Dictionary<int, TextData>();
    private Dictionary<string, EventData> eventDatas = new Dictionary<string, EventData>();

    public CharacterData GetCharacterData(int key)
    {
        return characterDatas[key];
    }

    public ItemData GetItemData(int key)
    {
        return itemDatas[key];
    }

    public TextData GetTextData(int key)
    {
        return textDatas[key];
    }

    public EventData GetEventData(string tag)
    {
        if (eventDatas.ContainsKey(tag))
            return eventDatas[tag];
        else
            return eventDatas["None"];
    }

    private void Awake()
    {
        LoadData();
    }

    public void LoadData()
    {
        LoadCharacterDataTable();
        LoadItemDataTable();
        LoadTextDataTable();
        LoadEventDataTable();
    }

    private void LoadCharacterDataTable()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/CharacterDataTable");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] str = temp.Split('\n');

        for(int i = 1; i < str.Length; i++)
        {
            string[] data = str[i].Split(',');

            if (data.Length < 2) return;

            CharacterData characterData;

            characterData.Key = int.Parse(data[0]);
            characterData.Name = data[1];
            characterData.WalkSpeed = float.Parse(data[2]);
            characterData.RunSpeed = float.Parse(data[3]);
            characterData.CrawlingSpeed = float.Parse(data[4]);
            characterData.Stemina = float.Parse(data[5]);
            characterData.Type = int.Parse(data[6]);
            characterData.Room = int.Parse(data[7]);
            characterData.DetectRange = float.Parse(data[8]);
            characterData.RotateSpeed = float.Parse(data[9]);

            characterDatas.Add(characterData.Key, characterData);
        }
    }

    private void LoadItemDataTable()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/ItemDataTable");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] str = temp.Split('\n');

        for (int i = 1; i < str.Length; i++)
        {
            string[] data = str[i].Split(',');

            if (data.Length <2) return;

            ItemData itemData;


            itemData.Key = int.Parse(data[0]);
            itemData.Name = data[1];
            itemData.Value = float.Parse(data[2]);
            itemData.ImagePath = data[3];
            itemData.Type = int.Parse(data[4]);


            itemDatas.Add(itemData.Key, itemData);
        }
    }

    private void LoadEventDataTable()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/EventTable");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] str = temp.Split('\n');

        for (int i = 1; i < str.Length; i++)
        {
            string[] data = str[i].Split(',');

            if (data.Length < 2) return;

            EventData eventData;


            eventData.Key = int.Parse(data[0]);
            eventData.Condition = int.Parse(data[1]);
            eventData.EventTag = data[2];
            eventData.EventCount = int.Parse(data[3]);
            eventData.TextDataKey = int.Parse(data[4]);
            eventData.GetItemKey = int.Parse(data[5]);
            eventData.Type = int.Parse(data[6]);

            eventDatas.Add(eventData.EventTag, eventData);
        }
    }

    private void LoadTextDataTable()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/TextTable");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] str = temp.Split('\n');

        for (int i = 1; i < str.Length; i++)
        {
            string[] data = str[i].Split(',');

            if (data.Length < 2) return;

            TextData textData;


            textData.Key = int.Parse(data[0]);
            string txt = data[1];
            textData.Text = txt.Replace("@", "\n");
            textData.Type = int.Parse(data[2]);


            textDatas.Add(textData.Key, textData);
        }
    }
}
