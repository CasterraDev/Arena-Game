using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventorySO inventory;
    public GameObject slotPrefab;

    public int amountOfSlots;
    public float X_START;
    public float Y_START;
    public float X_Space_Between_Slots;

    Vector3 objSize;

    // Start is called before the first frame update
    void Start()
    {
        objSize = new Vector3(slotPrefab.GetComponent<RectTransform>().rect.width, slotPrefab.GetComponent<RectTransform>().rect.height, 0);
        amountOfSlots = inventory.maxSlots;
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateDisplay()
    {
        float centerScreen = Screen.width / 2;
        RectTransform rt = slotPrefab.GetComponent<RectTransform>();
        float width = rt.rect.width * rt.localScale.x;

        Vector3 spaceTakenByObj = new Vector3(slotPrefab.GetComponent<RectTransform>().rect.width, slotPrefab.GetComponent<RectTransform>().rect.height, 0);
        float leftSide = Screen.width / -2f;
        leftSide += (spaceTakenByObj.x) / 2; //Add the objects extents so half of it isn't off the parent
        float spaceUsed = (amountOfSlots * spaceTakenByObj.x) + ((amountOfSlots - 1) * X_Space_Between_Slots);//Spacing is inbetween the objects so there will be one less than the numOfColumns of objects we have
        float spaceRemaining = Screen.width - spaceUsed;
        X_START = leftSide + (spaceRemaining / 2);

        //X_START = centerScreen - (amountOfSlots * (width/2)) - (X_Space_Between_Slots * (amountOfSlots-1))/2;
        for (int i = 0; i < amountOfSlots; i++)
        {
            var slot = Instantiate(slotPrefab,Vector3.zero, Quaternion.identity, this.transform);
            slot.GetComponent<RectTransform>().localPosition = GetPosition(i);
            if (inventory.list.items[i].ID == -1)
            {
                slot.GetComponentInChildren<Image>().sprite = null;
                slot.GetComponentInChildren<Image>().color = new Color(1,1,1,0);
                slot.GetComponentInChildren<TextMeshPro>().text = "";
            }
            else
            {
                slot.GetComponentInChildren<Image>().sprite = inventory.list.items[i].item.sprite;
                slot.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.GetComponentInChildren<TextMeshPro>().text = inventory.list.items[i].amount == 1 ? "" : inventory.list.items[i].amount.ToString("0n");
            }

        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (objSize.x + X_Space_Between_Slots) * (i % amountOfSlots), 10, 0f);
    }
}
