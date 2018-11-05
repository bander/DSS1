using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameConstructor
{
    public List<Vector3> floors = new List<Vector3>();

    private static string _gameDataFileName = "data3.json";

    private static SaveGameConstructor _instance;
    public static SaveGameConstructor Instance
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
        _instance = FileManager.Load<SaveGameConstructor>(_gameDataFileName);
    }
}