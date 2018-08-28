using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    #region Singleton
    public static CanvasController canvasController;

	void Awake () {
        canvasController = this;
	}
    #endregion

    public GameObject mainUI;
    public GameObject inventoryUI;

    public void ShowInventory()
    {
        mainUI.SetActive(false);
        inventoryUI.SetActive(true);
    }

    public void ShowMain()
    {
        mainUI.SetActive(true);
        inventoryUI.SetActive(false);
    }
}
