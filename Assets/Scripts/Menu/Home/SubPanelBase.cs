using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPanelBase : MonoBehaviour {
    protected HomeConstructor home;

    public List<GameObject> buttons;
    public GameObject rotateButton;
    public GameObject acceptButton;
    protected int type;
    protected int buildType;
    protected int objectType;


    void Start()
    {
        home = HomeConstructor.instance;
        home.selectComplete = Selected;
    }

    public virtual void SubClick()
    {
        DeactivateAll();
        ActSubButton(buttons[type]);
        home.SetBuildType(buildType, objectType);
    }

    public virtual void Selected() {
        acceptButton.SetActive(true);
    }
    protected void DeactivateAll()
    {
        foreach (GameObject go in buttons)
        {
            ActSubButton(go, false);
        }
        rotateButton.SetActive(false);
        acceptButton.SetActive(false);
    }
    protected void ActSubButton(GameObject go, bool b = true)
    {
        go.transform.GetChild(1).gameObject.SetActive(b);
    }

    public virtual void AcceptClick() { }
    public virtual void RotateClick() { }
    public virtual void CloseClick(){}
}
