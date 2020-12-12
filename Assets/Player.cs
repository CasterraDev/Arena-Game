using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventorySO inventory;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var item = collision.gameObject.GetComponent<GroundItem>();
        if (item)
        {
            inventory.AddItem(new Item(item.item), item.amount);
            Destroy(collision.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
    }
}
