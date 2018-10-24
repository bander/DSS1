using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : ConstructObject
{
    GameObject roof;
    public bool isLadderHole { get; protected set; }
    public bool isLadderExit { get; protected set; }
    public bool busy { get; protected set; }
    public bool Busy { set { busy = value; } }

    Dictionary<Vector3, WallObject> walls = new Dictionary<Vector3, WallObject>();

    public TileObject(Vector3 inKey, int inLevel, GameObject gObj = null) : base(inKey, inLevel, gObj)
    {
        prefs = home.tempFabs[0];
        if (level > 0) UpdateRoofMesh(4);
    }
    public override void Build(int n = 2)
    {
        base.Build();

        ConnectBuildAndWallsAround();
        RemoveWalls();
        RemoveRoof();
    }
    void ConnectBuildAndWallsAround()
    {
        int[] wallZKeys = new int[] { 0, 90, 180, 270 };
        Vector2[] keys = new Vector2[4] {new Vector3(key.x-1,key.y,0),
                                         new Vector3(key.x,key.y+1,0),
                                         new Vector3(key.x+1,key.y,0),
                                         new Vector3(key.x,key.y-1,0)
                                         };

        List<Building> nearBuildings = new List<Building>();
        List<Vector3> finalWallPositions = new List<Vector3>();
        int i = 0;
        foreach (Vector3 k in keys)
        {
            if (home.fl[level].ContainsKey(k))
            {
                TileObject t2 = home.fl[level][k];
               //if (t2.state == 1)
               if (t2.state >0)
                {
                    if (t2.InBuilding() && !nearBuildings.Contains(t2.building))
                    {
                        nearBuildings.Add(t2.building);
                    }
                }
                else
                    finalWallPositions.Add(new Vector3(k.x, k.y, wallZKeys[i]));
            }
            else
                finalWallPositions.Add(new Vector3(k.x, k.y, wallZKeys[i]));

            i++;
        }

        if (!InBuilding())
        {
            if (nearBuildings.Count == 0)
                home.CreateBuilding(this);
            else
            {
                nearBuildings[0].Add(this);
                int j = nearBuildings.Count;
                while (--j > 0)
                {
                    nearBuildings[0].Connect(nearBuildings[j]);
                    home.buildings[level].Remove(nearBuildings[j]);
                }
            }
        }

        foreach (Vector3 w in finalWallPositions)
        {
            MakeWallPosition(w);
        }
    }
    void RemoveWalls()
    {
        int[] wallZKeys = new int[] { 0, 90, 180, 270 };
        foreach (int z in wallZKeys)
            RemoveWallPosition(new Vector3(key.x, key.y, z));
    }
    void RemoveRoof()
    {
        if (level > 0 && roof != null)
        {
            GameObject.Destroy(roof);
        }
    }

    void MakeWallPosition(Vector3 key)
    {
        WallObject wall = new WallObject(key, level);
        building.AddWall(wall);
        home.wl[level][key] = wall;//.Add(key, wall);
    }
    void RemoveWallPosition(Vector3 key)
    {
        if (home.wl[level].ContainsKey(key))
        {
            WallObject wall = home.wl[level][key];
            home.wl[level].Remove(key);
            building.RemoveWall(wall);
            if (wall.obj != null) GameObject.Destroy(wall.obj);
            wall = null;
        }
    }

    public override bool IsEmpty()
    {
        if (base.IsEmpty() && !busy &&
            !isLadderExit &&
            !isLadderHole)
            return true;

        return false;
    }

    public void UpdateRoofMesh(int n)
    {
        GameObject.Destroy(roof);
        roof = GameObject.Instantiate(prefs.fab[n],
                        home.transform.position + new Vector3(key.x * 6 * home.scaleFactor,
                                                          (level - 1) * 4 * home.scaleFactor,
                                                          key.y * 6 * home.scaleFactor),
                        Quaternion.identity,
                        home.transform);
        roof.transform.parent = home.gameObject.transform;
        roof.GetComponent<ConstructElement>().construct = this;

    }

    public void StairUnderTile(StairObject stair)
    {
        isLadderExit = true;
        state = 2;
        ConnectBuildAndWallsAround();
        RemoveWalls();
        RemoveRoof();
        int oppositeStairRotation = RotateBy90(RotateBy90(stair.key.z, 1), 1);
        SetWallsAroundStairHole(oppositeStairRotation / 90);
    }
    public void SetWallsAroundStairHole(int num)
    {
        Vector3[] keys = new Vector3[4] {new Vector3(key.x-1,key.y,0),
                                         new Vector3(key.x,key.y+1,90),
                                         new Vector3(key.x+1,key.y,180),
                                         new Vector3(key.x,key.y-1,270)
                                         };
        foreach (Vector3 k in keys)
        {
            if (k == keys[num])
                home.wl[level][k].SetCanBeDoorOnly();//SetCanBeDoorOnly();
            else
                home.wl[level][k].SetNeverCanBeDoor();
        }
        
        
    }


    public override void Hide(bool revert = true)
    {
        base.Hide(revert);
        bool act = !revert;
        if (roof != null) roof.SetActive(act);
    }

}