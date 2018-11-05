using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHome
{
    public List<Vector3> v = new List<Vector3>();
    public List<Vector3> w = new List<Vector3>();

    private static string _gameDataFileName = "data5.json";

    private static SaveHome _instance;
    public static SaveHome Instance
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
        _instance = FileManager.Load<SaveHome>(_gameDataFileName);
    }
}