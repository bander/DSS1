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

    public TileType tileType;

    public override void Show(bool b = true)
    {
        tileType = home.tileType;

        int updatedMesh = 0;
        if (tileType == TileType.Turret) updatedMesh = 4;

        if (state < 1)
        {
            state = 0;
            if (b)
            {
                UpdateMesh(updatedMesh);
                home.tempObjects.Add(this);
            }
            else
            {
                if (obj != null)
                    GameObject.Destroy(obj);
            }
        }
    }

    public override void Build(int n = 3)
    {
        tileType = home.tileType;
        int buildMesh=3;
        switch (tileType)
        {
            case TileType.Stair:
                buildMesh = 3;
                break;
            case TileType.Turret:
                buildMesh = 7;
                break;
        }

        base.Build(buildMesh);

        if(buildMesh==3) ClearRoffUpTile();

    }
    void ClearRoffUpTile()
    {
        Vector3 keyTile = key;
        keyTile.z = 0;
        if (home.fl[level + 1].ContainsKey(keyTile)) 
        home.fl[level + 1][keyTile].StairUnderTile(this);
    }

    public override void UpdateMesh(int n)
    {
        base.UpdateMesh(n);
        obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, key.z, 0);
    }
    
    
    
}