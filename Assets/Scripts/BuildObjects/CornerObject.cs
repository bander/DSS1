using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerObject
{
    protected HomeConstructor home;
    protected TempPrefabsArray prefs;
    public Vector3 key;
    public int level { get; protected set; }
    public GameObject obj;
    public Building building { get; protected set; }
    public List<WallObject> walls = new List<WallObject>();

    public CornerObject(Vector3 inKey, bool outer)
    {
        key = inKey;
        home = HomeConstructor.instance;
        level = home.currentLevel;
        prefs = home.tempFabs[3];
        int n = (outer) ? 0 : 1;

        obj = GameObject.Instantiate(prefs.fab[n],
                        home.transform.position + new Vector3(key.x * 6 * home.scaleFactor,
                                                          level * 4 * home.scaleFactor,
                                                          key.y * 6 * home.scaleFactor),
                        Quaternion.identity,
                        home.transform);
        obj.transform.parent = home.gameObject.transform;
        obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, key.z, 0);
    }

    public void HideHalf(bool revert = true)
    {
        if (key.z != 270) return;

        bool act = !revert;

        if (obj == null) return;
        if (obj.transform.GetChild(0) == null) return;
        if (obj.transform.GetChild(0).GetChild(0) == null) return;
        obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(act);
        if (obj.transform.GetChild(0).GetChild(1) == null) return;
        obj.transform.GetChild(0).GetChild(1).gameObject.SetActive(!act);
    }
}
