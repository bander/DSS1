using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataController : MonoBehaviour
{
    #region Singleton
    public static GameDataController instance;

    void Awake()
    {
        instance = this;
        LoadData();
    }
    #endregion

    public Item[] prefabItems;
    public static SaveInv saveData;
    
    [ContextMenu("Save Data")]
    public void SaveGame()
    {
        Debug.Log("Save data ");
        var data = JsonUtility.ToJson(saveData);
        Debug.Log("SAAAAVE " + data);
        PlayerPrefs.SetString("GameData2", data);

        int i = 0;
        if (saveData == null) return;

        foreach (Vector3 it in saveData.items)
        {
            if (it != null)
            {
                Debug.Log(i + " svsv " + it + " ");
            }
            else
            {
                Debug.Log(i + " svsv ");
            }
            i++;
        }
    }

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        var data = PlayerPrefs.GetString("GameData2");
        if (data.Length < 10) return;
        Debug.Log("LOOOAD "+ data);
        saveData = JsonUtility.FromJson<SaveInv>(data);
    }

    private void OnDisable()
    {
        SaveGame();
    }

    public static void GetItems()
    {

        if (saveData.items == null)
            return;
        if (saveData.items.Count == 0)
            return;

        int i = 0;
        foreach (Vector3 it in saveData.items)
        {
            if (it != null)
            {
                Debug.Log(i+" = "+it);
            }
            else
            {
                Debug.Log(i + " = ");
            }
            i++;
        }
    }
    public static List<Item> GetItems2()
    {
        if (saveData == null)
            return null;
        if (saveData.items == null)
            return null;
        if (saveData.items.Count == 0)
            return null;

        List<Item> itms = new List<Item>(8);
        int i = 0;
        foreach (Vector3 it in saveData.items)
        {
            if (it != null)
            {
                Debug.Log(i + " = " + it);
                int num = (int)it.x;
                itms[i] = GameDataController.instance.prefabItems[num].Clone();
                if (it.y > 0) itms[i].countInSlot = (int)it.y;
            }
            else
            {
                Debug.Log(i + " = ");
                itms[i] = null;
            }

            i++;
        }
        return itms;
    }
    public static void SetItems(List<Item> its)
    {
        Debug.Log("set");
        if (saveData.items == null)
            saveData.items = new List<Vector3>();
        if (saveData.items.Count < 8)
            CreateItemsCount();



        int i = 0;
        foreach (Item it in its)
        {
            if (it == null)
                saveData.items[i] = new Vector3(0, 0, 0);
            else {
                int x = 0;
                switch (it.name) {
                    case "Oil":
                        x = 1;
                        break;
                    case "Metall":
                        x = 2;
                        break;
                }
                int y = 0;
                if (it.countInSlot > 1) y = it.countInSlot;

                saveData.items[i] =new Vector3(x,y,0);//it.Clone();

                Debug.Log(i + " Set " + saveData.items[i]);
            }
            i++;
        }

        GameDataController go = GameObject.FindObjectOfType<GameDataController>();
        go.SaveGame();

    }
    static void CreateItemsCount()
    {
        for (int i = 0; i < 8; i++)
        {
            saveData.items.Add(new Vector3(0,0,0));
        }
    }
}