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
    public GameObject equipUI;
    public GameObject closeButton;

    public void ShowInventory()
    {
        mainUI.SetActive(false);
        inventoryUI.SetActive(true);
        equipUI.SetActive(true);
        closeButton.SetActive(true);
    }

    public void ShowMain()
    {
        mainUI.SetActive(true);
        inventoryUI.SetActive(false);
        equipUI.SetActive(false);
        closeButton.SetActive(false);
    }
}
