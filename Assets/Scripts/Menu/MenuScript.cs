using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    PlayerController pController;

    public Sprite runIcon;
    public Sprite walkIcon;
    public GameObject RunWalkButton;

	void Start () {
        pController = PlayerController.instance;
        pController.onMainUIUpdate += UpdateUI;
	}
	void UpdateUI()
    {
        RunWalkButton.GetComponent<Image>().sprite =  (pController.isRunning())? runIcon:walkIcon;
    }

	void Update () {
		
	}
}
