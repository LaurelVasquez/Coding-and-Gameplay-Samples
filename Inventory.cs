using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    public int allSlots;
    private int enabledSlots;
    public GameObject[] slots;
    public GameObject slotHolder;
    public void Start()
    {
        //detect all available slots
        //access slot holder game object from inspector
        allSlots = 24;
        slots = new GameObject[allSlots];

        for(int i = 0; i < allSlots; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;

            if(slots[i].GetComponent<Slot>().item == null)
            {
                slots[i].GetComponent<Slot>().empty = true;
            }
        }
    }
    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            //run funciton to add item
            GameObject caughtItem = other.gameObject;
            Item item = caughtItem.GetComponent<Item>();
            AddItem(caughtItem, item.ID, item.type, item.description, item.icon);
        }
    }
    public void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {
        for(int i = 0; i < allSlots; i++)
        {
            //check each slot to see if they contain an item or if they're empty
            if(slots[i].GetComponent<Slot>().empty)
            {
                //add item to slot
                itemObject.GetComponent<Item>().pickedUp = true;

                slots[i].GetComponent<Slot>().item = itemObject;
                slots[i].GetComponent<Slot>().icon = itemIcon;
                slots[i].GetComponent<Slot>().type = itemType;
                slots[i].GetComponent<Slot>().description = itemDescription;
                slots[i].GetComponent<Slot>().ID = itemID;
                

                //move item literally into slot
                itemObject.SetActive(false);

                slots[i].GetComponent<Slot>().UpdateSlot();

                slots[i].GetComponent<Slot>().empty = false;

                
                return;
            }
        }
        Debug.Log("Unable to add item.");
        
    }
    public void RemoveItem(int indexToRemove)
    {
        GameObject itemToRemove = slots[indexToRemove];

        itemToRemove.GetComponent<Slot>().item = null;
        itemToRemove.GetComponent<Slot>().icon = null;
        itemToRemove.GetComponent<Slot>().type = null;
        itemToRemove.GetComponent<Slot>().description = null;
        itemToRemove.GetComponent<Slot>().ID = 0;

        itemToRemove.GetComponent<Slot>().empty = true;

        Debug.Log("Item was removed = " + itemToRemove.GetComponent<Slot>().empty);
    }
}
