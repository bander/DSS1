using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : ConstructObject
{
    public int wallScaler;

    public bool canBeDoor { get; protected set; }
    public bool canBeDoorOnly { get; protected set;}
    public void SetCanBeDoorOnly() { canBeDoorOnly = true; }
    public bool neverCanBeDoor { get; protected set; }
    public void SetNeverCanBeDoor() { neverCanBeDoor = true; }
    public bool neverCanBe { get; protected set; }
    public bool isDoor { get; protected set; }

    public List<CornerObject> walls = new List<CornerObject>();

    protected WallType wallType;

    public WallObject(Vector3 inKey, int inLevel, GameObject gObj = null) : base(inKey, inLevel, gObj)
    {
        prefs = home.tempFabs[1];
        CheckCanBeDoor();
    }

    public override void Show(bool b = true)
    {
        base.Show(b);

        if (!b) return;
        if (home.buildType == BuildType.Wall && home.wallType==WallType.Door)
        {
            if (neverCanBeDoor)
                Show(false);
            return;
        }
        if (canBeDoorOnly) SetRed();
        if (building.lastDoor == this) SetRed();
    }
    public void CheckCanBeDoor()
    {
        if (canBeDoorOnly)
        {
            canBeDoor = true;
            return;
        }
        if (neverCanBeDoor)
        {
            canBeDoor = false;
            return;
        }
        if (level != 0)
        {
            Vector3 testKeyFloor = new Vector3(key.x, key.y, 0);
            if (!home.fl[level].ContainsKey(testKeyFloor))
            {
                canBeDoor = false;
                return;
            }

        }

        Vector3 testKey = new Vector3(key.x, key.y, RotateBy90(key.z, 1));
        Vector3 testKey2 = new Vector3(key.x, key.y, RotateBy90(key.z, -1));
        if (CheckNear(testKey) || CheckNear(testKey2)) canBeDoor = false;
        else canBeDoor = true;
    }
    
   
    bool CheckNear(Vector3 testKey)
    {
        if (home.wl[level].ContainsKey(testKey))
        {
            home.wl[level][testKey].canBeDoor = false;
            return true;
        }
        return false;
    }
    bool CheckCompleted(Vector3 testKey)
    {
        if (home.wl[level].ContainsKey(testKey) && home.wl[level][testKey].state > 0)
        {
            return true;
        }
        return false;
    }

    public bool goToNextFloor = false;
    public override void Build(int meshNum=3)
    {
        wallType = home.wallType;
        switch (wallType)
        {
            case WallType.Wall:
                meshNum = 3;
                break;
            case WallType.SmallWindow:
                meshNum = 4;
                break;
            case WallType.BigWindow:
                meshNum = 5;
                break;
            case WallType.Door:
                meshNum = 6;
                isDoor = true;
                break;
        }

        base.Build(meshNum);
        //base.Build((int)bType + 2);

        SetBuilding(building);

        Vector3 floorKey = new Vector3(key.x, key.y, 0);
        if (home.fl[level].ContainsKey(floorKey)) home.fl[level][floorKey].Busy = true;

        if (home.wallType == WallType.Door)
        {
            isDoor = true;
            disableTilesNearDoor();
        }

        BuildCornersIn();

        HomeConstructor.instance.AddWallToSave(key);

        if (goToNextFloor)
        {
            goToNextFloor = false;
            HomeConstructor.instance.LevelUp();
        }
    }

    void disableTilesNearDoor()
    {
        Vector3 floorKey;
        if (key.z == 0 || key.z == 180)
        {
            floorKey = new Vector3(key.x, key.y - 1, 0);
            if (home.fl[level].ContainsKey(floorKey)) home.fl[level][floorKey].Busy = true;
            floorKey = new Vector3(key.x, key.y + 1, 0);
            if (home.fl[level].ContainsKey(floorKey)) home.fl[level][floorKey].Busy = true;
        }
        else
        {
            floorKey = new Vector3(key.x + 1, key.y, 0);
            if (home.fl[level].ContainsKey(floorKey)) home.fl[level][floorKey].Busy = true;
            floorKey = new Vector3(key.x - 1, key.y, 0);
            if (home.fl[level].ContainsKey(floorKey)) home.fl[level][floorKey].Busy = true;
        }
    }
    void BuildCornersIn()
    {
        BuildCornersOut();
        if (isDoor) return;

        CornerObject corn = null;
        int localScaler = 0;
        Vector3 testKey = new Vector3(key.x, key.y, RotateBy90(key.z, 1));
        Vector3 testKey2 = new Vector3(key.x, key.y, RotateBy90(key.z, -1));
        if (CheckCompleted(testKey))
        {
            corn = new CornerObject(testKey, false);
            home.cr[level][testKey] = corn;

            home.wl[level][testKey].UpdateMeshByScaler(2);
            localScaler += 1;
        }
        if (CheckCompleted(testKey2))
        {
            corn = new CornerObject(key, false);
            home.cr[level][testKey2] = corn;

            home.wl[level][testKey2].UpdateMeshByScaler(1);
            localScaler += 2;
        }
        UpdateMeshByScaler(localScaler);
    }
    void BuildCornersOut()
    {
        Vector3 key1 = new Vector3();
        Vector3 key2 = new Vector3();
        Vector3 key3 = new Vector3();
        Vector3 key4 = new Vector3();
        int z = (int)(key.z / 90);
        switch (z)
        {
            case 0:
                key1 = new Vector3(key.x + 1, key.y + 1, 90);
                key2 = new Vector3(key.x + 1, key.y - 1, 270);
                key3 = new Vector3(key.x, key.y + 1, 90);
                key4 = new Vector3(key.x, key.y - 1, 0);
                break;
            case 1:
                key1 = new Vector3(key.x + 1, key.y - 1, 180);
                key2 = new Vector3(key.x - 1, key.y - 1, 0);
                key3 = new Vector3(key.x + 1, key.y, 180);
                key4 = new Vector3(key.x - 1, key.y, 90);
                break;
            case 2:
                key1 = new Vector3(key.x - 1, key.y + 1, 90);
                key2 = new Vector3(key.x - 1, key.y - 1, 270);
                key3 = new Vector3(key.x, key.y + 1, 180);
                key4 = new Vector3(key.x, key.y - 1, 270);
                break;
            case 3:
                key1 = new Vector3(key.x + 1, key.y + 1, 180);
                key2 = new Vector3(key.x - 1, key.y + 1, 0);
                key3 = new Vector3(key.x + 1, key.y, 270);
                key4 = new Vector3(key.x - 1, key.y, 0);
                break;
        }

        if (CheckCompleted(key1))
        {
            CornerObject corn = new CornerObject(key3, true);
            home.cr[level][key3] = corn;
        }

        if (CheckCompleted(key2))
        {
            CornerObject corn = new CornerObject(key4, true);
            home.cr[level][key4] = corn;
        }
    }

    public void UpdateMeshByScaler(int num)
    {
        wallScaler += num;
        if (num > 0)
        {
            UpdateMesh(wallScaler + 6+3*(int)wallType);
        }
    }
    public override void UpdateMesh(int n)
    {
        base.UpdateMesh(n);
        obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, key.z, 0);
    }

    public override void Hide(bool revert = true)
    {
        //base.Hide(revert);
        bool act = !revert;
        if (obj == null) return;
            obj.transform.GetChild(0).GetChild(0).GetComponentInChildren<MeshRenderer>().enabled = act;//.SetActive(act);
        if (!isDoor) return;
       obj.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = act;//.SetActive(act);
       obj.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Renderer>().enabled = act;//.SetActive(act);

    }
    public override void HideHalf(bool revert = true)
    {
        if (key.z == 0 || key.z == 90) return;
        if (isDoor) return;
        base.HideHalf(revert);
    }
}