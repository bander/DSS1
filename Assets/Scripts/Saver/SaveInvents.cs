using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInvents : MonoBehaviour {
    //public Vector3[] inv1 = new Vector3[8];
    public Vector3 inv0;
    public Vector3 inv1;
    public Vector3 inv2;
    public Vector3 inv3;
    public Vector3 inv4;
    public Vector3 inv5;
    public Vector3 inv6;
    public Vector3 inv7;

    private static string _gameDataFileName = "data20.json";

    private static SaveInvents _instance;
    public static SaveInvents Instance
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
        _instance = FileManager.Load<SaveInvents>(_gameDataFileName);
    }
}
