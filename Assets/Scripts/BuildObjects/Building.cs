using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    HomeConstructor home;

    int level;
    public bool isolated { get; private set; }
    public bool hasDoor { get; private set; }
    public WallObject lastDoor { get; private set; }

    List<ConstructObject> tiles = new List<ConstructObject>();
    List<ConstructObject> walls = new List<ConstructObject>();
    List<ConstructObject> doors = new List<ConstructObject>();
    List<ConstructObject> corners = new List<ConstructObject>();
    List<ConstructObject> stairs = new List<ConstructObject>();

    public Color color { get; private set; }

    public Building(ConstructObject t)
    {
        home = HomeConstructor.instance;
        color = Random.ColorHSV();
        level = t.level;
        Add(t);
    }
    public void Add(ConstructObject t)
    {
        tiles.Add(t);
        t.SetBuilding(this);
    }
    public void AddWall(ConstructObject t)
    {
        walls.Add(t);
        t.SetBuilding(this);
        HomeConstructor.instance.wl[level][t.key] = t as WallObject;
    }
    public void AddCorner(ConstructObject t)
    {
        corners.Add(t);
        t.SetBuilding(this);
        //HomeConstructor.instance.cr[level][t.key] = t as CornerObject;
    }
    public void AddStair(ConstructObject t)
    {
        stairs.Add(t);
        t.SetBuilding(this);
        HomeConstructor.instance.st[level][t.key] = t as StairObject;
    }
    public void RemoveWall(ConstructObject t)
    {
        walls.Remove(t);
        HomeConstructor.instance.wl[level].Remove(t.key);
    }

    public List<ConstructObject> GetTiles() { return tiles; }
    public List<ConstructObject> GetWalls() { return walls; }
    public List<ConstructObject> GetCorners() { return corners; }
    public List<ConstructObject> GetStairs() { return stairs; }

    public void Connect(Building b)
    {
        foreach (TileObject t in b.GetTiles())
        {
            Add(t);
        }
        foreach (WallObject w in b.GetWalls())
        {
            AddWall(w);
        }
        foreach (ConstructObject c in b.GetCorners())
        {
            AddCorner(c);
        }

        foreach (StairObject s in b.GetStairs())
        {
            AddStair(s);
        }
    }

    public void CheckDoors()
    {
        if (walls.Count < 3) return;

        int canBeDoorsCount = 0;
        int emptyWalls = 0;
        WallObject lastDoor2 = null;
        int i = 0;
        foreach (WallObject w in walls)
        {
            w.CheckCanBeDoor();
            if (w.state < 1)
            {
                if (w.canBeDoor)
                {
                    canBeDoorsCount++;
                    lastDoor2 = w;
                }
                emptyWalls++;
            }
            else
                if (w.isDoor) hasDoor = true;
            i++;
        }

        if (emptyWalls == 0) Isolate();

        if (!hasDoor)
        {
            if (canBeDoorsCount == 1)
            {
                lastDoor = lastDoor2;
                if (home.wallType == WallType.Wall) lastDoor.SetRed();
            }
            else
                if (lastDoor != null)
            {
                lastDoor.Show();
                lastDoor = null;
            }
        }
        else
        {
            if (lastDoor != null)
            {
                lastDoor.Show();
                lastDoor = null;
            }
        }


    }

    void Isolate()
    {
        if (!isolated)
        {
            isolated = true;
            CreateNextLevelFloorPositions();
            CreateStairPositions();
        }
    }
    void CreateNextLevelFloorPositions()
    {
        foreach (TileObject t in tiles)//home.fl[level].Values)
        {
            if (t.state > 0)
            {
                TileObject to = new TileObject(t.key, level + 1);
                home.fl[level + 1].Add(t.key, to);

            }
        }
    }
    void CreateStairPositions()
    {
        foreach (TileObject t in tiles)//home.fl[level].Values)
        {
            Vector3[] key2 = new Vector3[4] { new Vector3(t.key.x+1,t.key.y,0),
                                              new Vector3(t.key.x,t.key.y-1,90),
                                              new Vector3(t.key.x-1,t.key.y,180),
                                              new Vector3(t.key.x,t.key.y+1,270)
                                              };
            Vector3[] key3 = new Vector3[4] { new Vector3(t.key.x-1,t.key.y,0),
                                              new Vector3(t.key.x,t.key.y+1,90),
                                              new Vector3(t.key.x+1,t.key.y,180),
                                              new Vector3(t.key.x,t.key.y-1,270)
                                              };

            if (t.state == 1)
            {
                int i = 0;
                for (i = 0; i < 4; i++)
                {
                    Vector3 keyz2 = key2[i];
                    keyz2.z = 0;
                    Vector3 keyz3 = key3[i];
                    keyz3.z = 0;
                    Vector3 keyStair = t.key;
                    keyStair.z = key2[i].z;
                    //if (home.fl[level].ContainsKey(keyz2) && home.fl[level][keyz2].state == 1)
                    int num = ContainsKey(tiles, keyz2);
                    if (num != -1 && tiles[num].state == 1)
                   {
                        num = ContainsKey(tiles, keyz3);
                        if (num != -1 && tiles[num].state == 1)
                        {
                            StairObject sObj = new StairObject(keyStair, level);
                            sObj.SetBuilding(this);
                            AddStair(sObj);
                            home.st[level][keyStair] = sObj;
                        }
                    }
                }
            }
        }
        
        foreach (WallObject w in walls)//home.wl[level].Values)
        {
            Vector3 tileKey = new Vector3(w.key.x,w.key.y,0);
            //if (level==0)
            if (w.state == 1 && w.isDoor == false)// && w.canBeDoor == true)
            {
                StairObject stair = new StairObject(w.key, level);
                stair.isAtWall = true;
                home.st[level].Add(w.key, stair);
                AddStair(stair);
                
            }
        }
        //*/
    }
    int ContainsKey(List<ConstructObject> list,Vector3 key)
    {
        int i= 0;
        foreach (ConstructObject co in list)
        {
            if (co.key == key) return i;
            i++;
        }
        return -1;
    }
}