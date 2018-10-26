using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructObject
{
    protected HomeConstructor home;
    protected TempPrefabsArray prefs;
    public Vector3 key;
    public int state { get; protected set; }
    public int level { get; protected set; }
    public GameObject obj;
    public Building building { get; protected set; }

    protected int RotateBy90(float angle, int dir)
    {
        angle += dir * 90;
        if (angle < 0) angle = 270;
        if (angle > 270) angle = 0;
        return (int)angle;
    }

    public ConstructObject(Vector3 inKey, int inLevel, GameObject gObj = null)
    {
        home = HomeConstructor.instance;
        key = inKey;
        level = inLevel;
        obj = gObj;
    }

    public virtual bool IsEmpty()
    {
        if (state < 1) return true;
        return false;
    }
    public bool InBuilding()
    {
        if (building != null) return true;
        return false;
    }
    
    public void SetBuilding(Building b)
    {
        building = b;

        if (true) return;
        if (obj != null)
        {
            GameObject child = obj.gameObject.transform.GetChild(0).gameObject;
            if (child != null)
            {
                if (child.GetComponent<MeshRenderer>() == null) return;
                Material m = child.GetComponent<MeshRenderer>().material;
                m.color = b.color;
                child.GetComponent<MeshRenderer>().material = m;
            }
        }
    }

    public virtual void Show(bool b = true)
    {
        if (state < 1)
        {
            state = 0;
            if (b)
            {
                UpdateMesh(0);
                home.tempObjects.Add(this);
            }
            else
            {
                if (obj != null)
                    GameObject.Destroy(obj);
            }
        }
    }
    public bool Select(bool b = true)
    {
        if (state == 0 && obj!=null && obj.activeSelf)
        {
            UpdateMesh(1);
            return true;
        }
        return false;
    }
    public virtual void Build(int meshNUm = 3)
    {
        UpdateMesh(meshNUm);
        state = 1;// meshNUm - 2;

        if(building !=null)
            building.CheckDoors();
        //CheckDoorsAllBuildings();
    }
    public virtual void UpdateMesh(int n)
    {
        if (home.tempObjects.Contains(this)) home.tempObjects.Remove(this);

        GameObject.Destroy(obj);
        obj = GameObject.Instantiate(prefs.fab[n],
                        home.transform.position + new Vector3(key.x * 6 * home.scaleFactor,
                                                          level * 4 * home.scaleFactor,
                                                          key.y * 6 * home.scaleFactor),
                        Quaternion.identity,
                        home.transform);
        obj.transform.parent = home.gameObject.transform;
        obj.GetComponent<ConstructElement>().construct = this;
    }

    public void SetRed()
    {
        UpdateMesh(2);
        state = -1;
        home.tempObjects.Add(this);
    }

    public void CheckDoorsAllBuildings()
    {
        //building.CheckDoors();
        /*foreach (Building b in home.buildings[level])
        {
            b.CheckDoors();
        }
        //*/
    }

    public virtual void Hide(bool revert = true)
    {
        bool act = !revert;
        if (obj != null) obj.transform.GetChild(0).GetComponentInChildren<MeshRenderer>().enabled = act;//.SetActive(act);
        
    }

    public virtual void HideHalf(bool revert = true)
    {
        bool act = !revert;

        if (obj == null)
            return;
        else obj.SetActive(true);
        
        if (obj.transform.GetChild(0) == null) return;
        if (obj.transform.GetChild(0).GetChild(0) == null) return;
        obj.transform.GetChild(0).GetChild(0).GetComponentInChildren<Renderer>().enabled = act;//gameObject.SetActive(act);
        if (obj.transform.GetChild(0).GetChild(1) == null) return;
        obj.transform.GetChild(0).GetChild(1).GetComponentInChildren<Renderer>().enabled = !act;//.gameObject.SetActive(!act);
    }
}
