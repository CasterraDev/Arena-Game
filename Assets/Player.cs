using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventorySO inventory;
    public LayerMask itemLayer;

    private void Awake()
    {
        for (int i = 0; i < inventory.maxSlots; i++)
        {
            inventory.list.items[i] = new InventorySlot(-1, null, 0);
        }
    }

    public void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 1f, itemLayer);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var item = colliders[i].gameObject.GetComponent<GroundItem>();
                if (item)
                {
                    if (inventory.ItemInInventorySpaceAvailable(new Item(item.item)) || inventory.AnySpace())
                    {
                        inventory.AddItem(item);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        var item = collision.gameObject.GetComponent<GroundItem>();
        if (item)
        {
            inventory.AddItem(new Item(item.item), item.amount);
            Destroy(collision.gameObject);
        }
        */
    }

    private void OnApplicationQuit()
    {
    }
}
