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
    public GameObject droppedItemPrefab;
    public ItemDatabaseSO database;
    public Inventory list;

    public void AddItem(Item item,int amount)
    {
        for (int i = 0; i < list.items.Length; i++)
        {
            if (list.items[i].ID == item.Id && list.items[i].amount < item.maxQuantity)
            {
                if (list.items[i].amount + amount < item.maxQuantity)
                {
                    list.items[i].AddAmount(amount);
                }
                else
                {
                    var droppedItem = Instantiate(droppedItemPrefab, Vector3.zero, Quaternion.identity);
                    droppedItem.GetComponent<Transform>().position = new Vector3(10, 10, 0);
                    droppedItem.GetComponent<GroundItem>().item = database.items[item.Id];
                    droppedItem.GetComponent<GroundItem>().amount = (list.items[i].amount + amount) - item.maxQuantity;

                    list.items[i].AddAmount(item.maxQuantity - list.items[i].amount);
                }

                return;
            }
        }
        FindEmptySlot(item, amount);
    }

    public void AddItem(GroundItem gItem)
    {
        for (int i = 0; i < list.items.Length; i++)
        {
            if (list.items[i].ID == gItem.item.Id && list.items[i].amount < gItem.item.maxQuantity)
            {
                if ((list.items[i].amount + gItem.amount) <= gItem.item.maxQuantity)
                {
                    //Debug.Log("AMount " + list.items[i].amount + gItem.amount);
                    //Debug.Log("MA " + gItem.item.maxQuantity);
                    list.items[i].AddAmount(gItem.amount);
                    Destroy(gItem.gameObject);
                }
                else
                {
                    Debug.Log("LEFT " + ((list.items[i].amount + gItem.amount) - gItem.item.maxQuantity));
                    gItem.amount = (list.items[i].amount + gItem.amount) - gItem.item.maxQuantity;
                    return;
                }

                return;
            }
        }
        FindEmptySlot(new Item(gItem.item), gItem.amount);
        Destroy(gItem.gameObject);
    }

    public bool AnySpace()
    {
        for (int i = 0; i < list.items.Length; i++)
        {
            if (list.items[i].ID <= -1)
            {
                return true;
            }
        }
        return false;
    }

    public bool ItemInInventorySpaceAvailable(Item _item)
    {
        for (int i = 0; i < list.items.Length; i++)
        {
            if (list.items[i].ID == _item.Id && list.items[i].amount < _item.maxQuantity)
            {
                return true;
            }
        }
        return false;
    }

    public bool ItemInInventory(Item _item)
    {
        for (int i = 0; i < list.items.Length; i++)
        {
            if (list.items[i].ID == _item.Id)
            {
                return true;
            }
        }
        return false;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        var temp = new InventorySlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void DropItem(InventorySlot _invSlot, Item _item, int _amount)
    {
        _invSlot.ID = -1;
        _invSlot.item = null;
        _invSlot.amount = 0;

        var droppedItem = Instantiate(droppedItemPrefab,Vector3.zero,Quaternion.identity);
        droppedItem.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        droppedItem.GetComponent<GroundItem>().item = database.items[_item.Id];
        droppedItem.GetComponent<GroundItem>().amount = _amount;
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
