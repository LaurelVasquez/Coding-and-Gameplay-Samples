using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class fixedprocessfishinv : MonoBehaviour
{

    //when we click the inventory button, let's pretty much update the whole inventory and do the book keeping
    //This may change, was thinking instead of sending playerback to title or anything it should send player
    //back to inventory and do the inventory thing at that point. So let's instead try making just a function to call,
    //which makes it easy to change later if I want to

    //yo also this will not include the elements size since thatll be a specifically generated value for the caught fish, so ill need to save that with the ID later in the Save System

    //To start, let's load both inventories
    //when loading inventories, we recieve an instance of an InventoryData object which is basically just an array of IDs of items (actually, it contains the array: itemIDList)
    //So let's get those first

    //We need the Storage Inventory Object for the fish in order to operate on it
    public Inventory StorageInventoryFish;

    public GameObject FishInventoryScriptHolder;



    //an array of game objects with "Item" component that we reference according to the position in the array
    public GameObject[] IDs = new GameObject[1];

    void Start()
    {


        try
        {
            //SO we update the inventory if there is something new that we caught AND
            ProcessInventoryFunc();
        }
        //dont judge me ill figure it out later
        catch (System.Exception e)
        {

        }
    }

    public void ProcessInventoryFunc()
    {
        //set the inventory to the component of the fish inventoryScriptHolder
        StorageInventoryFish = FishInventoryScriptHolder.GetComponent<Inventory>();
        InventoryData catchInventory = null;
        InventoryData storageInventory = null;
        //load in catch inventory
        catchInventory = SaveSystem.LoadCatchInventoryData();
        //load in storage inventory
        storageInventory = SaveSystem.LoadStorageData();
        Debug.Log(catchInventory == null);
        Debug.Log(storageInventory == null);

        //NO ITEMS AT ALL
        if (catchInventory == null && storageInventory == null)
        {
            return;
        }
        //ONLY NEW ITEMS BUT NONE IN STORAGE
        else if (catchInventory != null && storageInventory == null)
        {
            //There is new stuff to load in AND there is nothing previously loaded into storage

            //then we just need to load in catch, delete catch and save the new stuff into storage
            for (int i = 0; i < catchInventory.itemIDList.Length - 1; i++)
            {
                int itemID = catchInventory.itemIDList[i];
                Item itemToAdd = IDs[itemID].GetComponent<Item>();
                StorageInventoryFish.AddItem(itemToAdd.gameObject, itemToAdd.ID, itemToAdd.type, itemToAdd.description, itemToAdd.icon);
            }
            SaveSystem.SaveStorageData(StorageInventoryFish);
            //delete catch
            string path = Application.persistentDataPath + "/catch.fun";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
            }
            return;
        }
        //ONLY STORED STUFF NOTHING NEW
        else if (catchInventory == null && storageInventory != null)
        {
            //This shouldn't be triggering until something is appearing in the storage
            //then just load in storage
            for (int i = 0; i < storageInventory.itemIDList.Length - 1; i++)
            {
                int itemID = storageInventory.itemIDList[i];
                Item itemToAdd = IDs[itemID].GetComponent<Item>();
                StorageInventoryFish.AddItem(itemToAdd.gameObject, itemToAdd.ID, itemToAdd.type, itemToAdd.description, itemToAdd.icon);
            }
            return;
        }
        //BOTH NEW AND OLD STUFF SO LOAD IN EVERYTHING
        else if (storageInventory != null && catchInventory != null)
        {
            //we need to load in the caught fish AND storage, then delete the catch inventory file and resave the new storage
            //This shouldn't be triggering until something is appearing in the storage
            //load in the catch inventory first
            for (int i = 0; i < catchInventory.itemIDList.Length - 1; i++)
            {

                int itemID = catchInventory.itemIDList[i];
                Item itemToAdd = IDs[itemID].GetComponent<Item>();
                StorageInventoryFish.AddItem(itemToAdd.gameObject, itemToAdd.ID, itemToAdd.type, itemToAdd.description, itemToAdd.icon);
            }
            //then load in the storage stuff
            for (int i = 0; i < storageInventory.itemIDList.Length - 1; i++)
            {
                int itemID = storageInventory.itemIDList[i];
                Item itemToAdd = IDs[itemID].GetComponent<Item>();
                StorageInventoryFish.AddItem(itemToAdd.gameObject, itemToAdd.ID, itemToAdd.type, itemToAdd.description, itemToAdd.icon);
            }

            //then, save the new storage
            SaveSystem.SaveStorageData(StorageInventoryFish);

            //delete the catch inventory file
            string path = Application.persistentDataPath + "/catch.fun";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
            }

            return;
        }
        else
        {

        }
    }
}
