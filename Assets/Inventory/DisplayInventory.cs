using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DisplayInventory : MonoBehaviour
{
    public InventorySO inventory;
    public GameObject slotPrefab;

    public int amountOfSlots;
    public float X_START;
    public float Y_START;
    public float X_Space_Between_Slots;

    protected MouseItem mouseItem = new MouseItem();
    Vector3 objSize;
    protected Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = slotPrefab.GetComponent<RectTransform>();
        objSize = new Vector3(rt.rect.width * rt.localScale.x, rt.rect.height * rt.localScale.y, 0);
        amountOfSlots = inventory.maxSlots;
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        float centerScreen = Screen.width / 2;
        RectTransform rt = slotPrefab.GetComponent<RectTransform>();
        float width = rt.rect.width * rt.localScale.x;

        Vector3 spaceTakenByObj = new Vector3(rt.rect.width * rt.localScale.x, rt.rect.height * rt.localScale.y, 0);
        float xSpaceTaken = (spaceTakenByObj.x * amountOfSlots) + (X_Space_Between_Slots * (amountOfSlots - 1));
        X_START = centerScreen - (xSpaceTaken/2);

        //X_START = centerScreen - (amountOfSlots * (width/2)) - (X_Space_Between_Slots * (amountOfSlots-1))/2;
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.maxSlots; i++)
        {

            GameObject obj = Instantiate(slotPrefab, GetPosition(i), Quaternion.identity, transform);
            //obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragBegin(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragExit(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsDisplayed.Add(obj, inventory.list.items[i]);
        }
    }

    public void UpdateDisplay()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.ID >= 0)
            {
                try
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.Value.ID].itemSprite;
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                    slot.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
                }
                catch (KeyNotFoundException)
                {
                    Debug.LogError("You need to add all of your scriptable ItemObjects into a ItemDatabase || Add a ItemDatabase then lock the inspecter then select and drag all of your itemObjects into the Items array");
                    throw;
                }
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }
    public void OnDragBegin(GameObject obj)
    {
        var mouseObj = new GameObject();
        var rt = mouseObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(objSize.x, objSize.y);
        mouseObj.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObj.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].itemSprite;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObj;
        mouseItem.item = itemsDisplayed[obj];
    }
    public void OnDragExit(GameObject obj)
    {
        if (mouseItem.hoverObj)
        {
            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
            inventory.DropItem(itemsDisplayed[obj], itemsDisplayed[obj].item, itemsDisplayed[obj].amount);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }
    public void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + ((objSize.x + X_Space_Between_Slots) * i), 20, 0f);
    }
}

public class MouseItem
{
    public GameObject obj;
    public GameObject hoverObj;
    public InventorySlot item;
    public InventorySlot hoverItem;
}
