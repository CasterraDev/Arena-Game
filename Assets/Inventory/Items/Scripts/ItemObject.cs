﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapons,
    Food,
    Default,
    Equipment,
}

public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public int maxQuantity = 1;
    public Sprite itemSprite;
    public ItemType type;
    [TextArea(15,20)]
    public string Description;
}

[System.Serializable]
public class Item
{
    public int Id;
    public int maxQuantity;
    public Sprite sprite;
    public string name;

    public Item(ItemObject item)
    {
        Id = item.Id;
        maxQuantity = item.maxQuantity;
        sprite = item.itemSprite;
        name = item.name;
    }
}
