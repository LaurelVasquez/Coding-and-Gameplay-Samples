
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveMoneyData(Money money)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/money.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        //converts the relevant money data into a format that can actually be turned into a bit stream
        MoneyData data = new MoneyData(money);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static MoneyData LoadMoneyData()
    {
        string path = Application.persistentDataPath + "/money.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            MoneyData data = (MoneyData) formatter.Deserialize(stream);

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    
    //TO SAVE CATCH INVENTORY: I take inventory, and I reduce it down into "InventoryData" an object that contains only a simple array
    //this array contains  a list of IDs which each represent a quantity of one of that item(ID tells us which item)
    public static void SaveCatchInventoryData(Inventory inv)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/catch.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        //converts the inventory into a data structure that is the list of IDs of the items
        InventoryData data = new InventoryData(inv);

        formatter.Serialize(stream, data);

        stream.Close();
    }
    

    public static InventoryData LoadCatchInventoryData()
    {
        string path = Application.persistentDataPath + "/catch.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            InventoryData data = (InventoryData)formatter.Deserialize(stream);

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveStorageData(Inventory inv)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/fishstorage.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        //converts the relevantdata into a format that can actually be turned into a bit stream
        InventoryData data = new InventoryData(inv);

        formatter.Serialize(stream, data);

        stream.Close();
    }
    public static InventoryData LoadStorageData()
    {
        string path = Application.persistentDataPath + "/fishstorage.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            InventoryData data = (InventoryData)formatter.Deserialize(stream);

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
