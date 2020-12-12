using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapons Object", menuName = "Inventory System/Items/Weapons")]
public class WeaponsObject : ItemObject
{
    public int attackDamage;
    public void Awake()
    {
        type = ItemType.Weapons;
    }
}
