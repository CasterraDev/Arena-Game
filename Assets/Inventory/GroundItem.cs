using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;
    public int amount = 1;

    private void Awake()
    {
        Debug.Log("ID " + item.Id);
        GetComponent<SpriteRenderer>().sprite = item.itemSprite;
    }
}
