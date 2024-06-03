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
    public float JumpPower;
    public float Stemina;
    public int Type;
    public int Room;
}
public struct ItemData
{
    public int Key;
    public string Name;
    public float Value;
    public int ImagePath;
    public int Type;

}


public class DataManager : Singleton<DataManager>
{   

    private Dictionary<int, CharacterData> characterDatas = new Dictionary<int, CharacterData>();
    private Dictionary<int, ItemData> itemDatas = new Dictionary<int, ItemData>();

    public CharacterData GetCharacterData(int key)
    {
        return characterDatas[key];
    }

    public ItemData GetItemData(int key)
    {
        return itemDatas[key];
    }


    private void Awake()
    {
        LoadData();
    }

    public void LoadData()
    {
        LoadCharacterDataTable();
        LoadItemDataTable();
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
            characterData.JumpPower = float.Parse(data[5]);
            characterData.Stemina = float.Parse(data[6]);
            characterData.Type = int.Parse(data[7]);
            characterData.Room = int.Parse(data[8]);

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
            itemData.ImagePath = int.Parse(data[3]);
            itemData.Type = int.Parse(data[4]);


            itemDatas.Add(itemData.Key, itemData);
        }
    }
}
