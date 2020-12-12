using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName = "New Inventory Object",menuName = "Inventory System/Inventory")]
public class InventorySO : ScriptableObject
{
    public string savePath;
    public int maxSlots = 5;
    public ItemDatabaseSO database;
    public Inventory list;
    public void AddItem(Item item,int amount)
    {
        for (int i = 0; i < list.items.Length; i++)
        {
            if (list.items[i].item == item)
            {
                list.items[i].AddAmount(amount);
                return;
            }
        }
        FindEmptySlot(item, amount);
    }

    public InventorySlot FindEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < list.items.Length; i++)
        {
            if (list.items[i].ID <= -1)
            {
                list.items[i].UpdateSlot(item.Id, item, amount);
                return list.items[i];
            }
        }
        return null;
    }

    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(this,true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath),FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] items;
}

[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amount;

    public InventorySlot(int _id,Item _item,int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int _amount)
    {
        amount += _amount;
    }
}
