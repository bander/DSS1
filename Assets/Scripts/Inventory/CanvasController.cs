using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    #region Singleton
    public static CanvasController instance;

	void Awake () {
        instance = this;
	}
    #endregion

    InvManager manager;

    public GameObject mainUI;
    public GameObject inventoryUI;
    public GameObject equipUI;
    public GameObject closeButton;
    public GameObject LootUI;
    public GameObject BuildUI;

    public GameObject useButton;
    public GameObject splitButton;
    public GameObject removeButton;
    public GameObject getAllButton;
    public InvSlot selectedSlot;

    int currentBuild = 0;

    void Start()
    {
        buttonsControl(false, false, false);

        manager = InvManager.instance;
        if (manager != null)
        {
            manager.OnInvChangedCallback += UpdateGetAllButton;
            manager.OnInvChangedCallback += updateBuildButtonsActivity;
        }

    }

    public void buttonsControl(bool use=true,bool split=true,bool rem=true)
    {
        activateButton(useButton, use);
        activateButton(splitButton, split);
        activateButton(removeButton, rem);
    }
    void activateButton(GameObject button,bool act=true)
    {
        Button b = button.GetComponent<Button>();
        b.interactable = act ;
        Color c = Color.white;
        c.a = (act)?  1f:0.2f;
        button.transform.GetChild(0).gameObject.GetComponent<Image>().color = c;
    }
    public void UpdateGetAllButton()
    {
        bool lootIsEmpty = InvManager.instance.IsLootEmpty();

        Button b = getAllButton.GetComponent<Button>();
        b.interactable = lootIsEmpty;
        Color c = Color.white;
        c.a = (lootIsEmpty) ? 1f : 0.2f;
        getAllButton.transform.GetChild(0).gameObject.GetComponent<Image>().color = c;
        getAllButton.transform.GetChild(1).gameObject.GetComponent<Image>().color = c;
    }

    public void ShowInventory()
    {
        closeAll();
        inventoryUI.SetActive(true);
        equipUI.SetActive(true);
        closeButton.SetActive(true);
    }
    public void ShowLoot()
    {
        closeAll();
        inventoryUI.SetActive(true);
        LootUI.SetActive(true);
        closeButton.SetActive(true);
        UpdateGetAllButton();
    }
    public void ShowBuild()
    {
        closeAll();
        BuildUI.SetActive(true);
    }


    public void ShowMain()
    {
        closeAll();
        mainUI.SetActive(true);
        updateBuildButtonsActivity();
    }
    void closeAll()
    {
        inventoryUI.SetActive(false);
        equipUI.SetActive(false);
        closeButton.SetActive(false);
        mainUI.SetActive(false);
        LootUI.SetActive(false);
        BuildUI.SetActive(false);
    }

    public void RemoveSelectedItem()
    {
        if (selectedSlot!=null && selectedSlot.GetItem()!=null)
        {
            int numInvent = selectedSlot.invIndex;

            InvManager.instance.invents[numInvent].RemoveItemByNum(selectedSlot.slotIndex);
        }
    }

    public void buildFloor()
    {
        updateBuildButtonsActivity();
        buildOn(BuildUI.transform.GetChild(1).gameObject, true);
    }
    public void buildWall()
    {
        updateBuildButtonsActivity();
        buildOn(BuildUI.transform.GetChild(2).gameObject, true);
    }
    public void buildBox()
    {
        updateBuildButtonsActivity();
        buildOn(BuildUI.transform.GetChild(3).gameObject, true);
    }

    void updateBuildButtonsActivity()
    {
        SetActiveBuildButton(BuildUI.transform.GetChild(1).gameObject, false);
        SetActiveBuildButton(BuildUI.transform.GetChild(2).gameObject, false);
        SetActiveBuildButton(BuildUI.transform.GetChild(3).gameObject, false);

        int summ =  manager.invents[0].GetSummMetall();
        if (summ > 0)
        {
            SetActiveBuildButton(BuildUI.transform.GetChild(1).gameObject, true);
            SetActiveBuildButton(BuildUI.transform.GetChild(2).gameObject, true);
        }
        if (summ > 5)
        {
            SetActiveBuildButton(BuildUI.transform.GetChild(3).gameObject, true);
        }
    }
    void SetActiveBuildButton(GameObject button,bool act=true)
    {
        Button b = button.GetComponent<Button>();
        b.interactable = act;
        Color c = Color.white;
        c.a = (act) ? 1f : 0.2f;
        button.transform.GetChild(0).gameObject.GetComponent<Image>().color = c;
        Color c2 = Color.white;
        c2.a =0.01f;
        button.transform.GetChild(1).gameObject.GetComponent<Image>().color = c2;
    }
    void buildOn(GameObject button, bool act = true)
    {
        Color c2 = Color.white;
        c2.a = (act) ? 1f : 0.01f;
        button.transform.GetChild(1).gameObject.GetComponent<Image>().color = c2;

    }
    
}
