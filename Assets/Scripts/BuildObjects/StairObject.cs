using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairObject : ConstructObject
{
    public StairObject(Vector3 inKey, int inLevel, GameObject gObj = null) : base(inKey, inLevel, gObj)
    {
        prefs = home.tempFabs[2];
    }
    int rotate(float angle, int dir)
    {
        angle += dir * 90;
        if (angle < 0) angle = 270;
        if (angle > 270) angle = 0;
        return (int)angle;
    }

    public override void Build(int n = 3)
    {
        base.Build(n);
        ClearRoffUpTile();

    }
    void ClearRoffUpTile()
    {
        Vector3 keyTile = key;
        keyTile.z = 0;
        home.fl[level + 1][keyTile].StairUnderTile(this);
    }

    public override void UpdateMesh(int n)
    {
        base.UpdateMesh(n);
        obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, key.z, 0);
    }
}