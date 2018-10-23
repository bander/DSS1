using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeConstrPanel : MonoBehaviour {
    HomeConstructor home;

    public GameObject panelBuild;
    public GameObject panelObjects;

    public GameObject rotateButton;
    public GameObject acceptButton;

    public int currentPanel;
    List<GameObject> panels=new List<GameObject>();

    void Start()
    {
        home = HomeConstructor.instance;
        panels.Add(panelBuild);
        panels.Add(panelObjects);
    }

    public void Show(bool act=true)
    {
        gameObject.SetActive(act);
        if(act) OpenBuilding();
    }

    public void OpenBuilding()
    {
        currentPanel = 0;
        panelBuild.SetActive(true);
        panelObjects.SetActive(false);
        rotateButton.SetActive(false);
        acceptButton.SetActive(false);
    }
    public void OpenObjects()
    {
        currentPanel = 1;
        panelBuild.SetActive(false);
        panelObjects.SetActive(true);
        rotateButton.SetActive(false);
        acceptButton.SetActive(false);
    }

    public void AcceptButton()
    {
        panels[currentPanel].GetComponent<SubPanelBase>().AcceptClick();
        home.BuildObject();
    }
    public void RotateButton()
    {
        panels[currentPanel].GetComponent<SubPanelBase>().RotateClick();
        home.RotatePositive();
    }
    public void CloseButton()
    {
        panels[currentPanel].GetComponent<SubPanelBase>().CloseClick();
    }
}
