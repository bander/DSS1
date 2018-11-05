using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameInventory
{
    public List<Item> itemsInv = new List<Item>();
    public List<Item> itemsQuip = new List<Item>();
    public List<Item> itemsLoot1 = new List<Item>();
    public List<Item> itemsLoot2 = new List<Item>();

    public Item[] itemInv = new Item[8];

    private static string _gameDataFileName = "data2.json";

    public Item trash;

    private static SaveGameInventory _instance;
    public static SaveGameInventory Instance
    {
        get
        {
            if (_instance == null)
                Load();
            return _instance;
        }
    }
    public static void Save()
    {
        FileManager.Save(_gameDataFileName, _instance);
    }
    public static void Load()
    {
        _instance = FileManager.Load<SaveGameInventory>(_gameDataFileName);
    }
}