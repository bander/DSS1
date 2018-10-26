using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class HomeConstructor : MonoBehaviour {
    #region Singleton
    public static HomeConstructor instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    //List<Vector3> availablePoints = new List<Vector3>();
    public TempPrefabsArray[] tempFabs;

    public List<ConstructObject> tempObjects=new List<ConstructObject>();
    public List<ConstructObject> toBeDestroyedObjects=new List<ConstructObject>();

    public NavMeshSurface surface;
    public NavMeshModifierVolume modifier;
    
    Dictionary<Vector2, int>[] ob = new Dictionary<Vector2, int>[3] { new Dictionary<Vector2, int>(), new Dictionary<Vector2, int>(), new Dictionary<Vector2, int>()};
    public Dictionary<Vector2, TileObject>[] fl = new Dictionary<Vector2, TileObject>[3] { new Dictionary<Vector2, TileObject>(), new Dictionary<Vector2, TileObject>(), new Dictionary<Vector2, TileObject>() };
    public Dictionary<Vector3, WallObject>[] wl = new Dictionary<Vector3, WallObject>[3] { new Dictionary<Vector3, WallObject>(),new Dictionary<Vector3, WallObject>(),new Dictionary<Vector3, WallObject>()};
    //Dictionary<Vector3, int>[] wl = new Dictionary<Vector3, int>[3] { new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>()};
    public Dictionary<Vector3, CornerObject>[] cr = new Dictionary<Vector3, CornerObject>[3] { new Dictionary<Vector3, CornerObject>(),new Dictionary<Vector3, CornerObject>(),new Dictionary<Vector3, CornerObject>() };
    public Dictionary<Vector3, StairObject>[] st = new Dictionary<Vector3, StairObject>[3]{ new Dictionary<Vector3, StairObject>(),new Dictionary<Vector3, StairObject>(), new Dictionary<Vector3, StairObject>() };
    public Dictionary<Vector3, StairObject>[] tr = new Dictionary<Vector3, StairObject>[3]{ new Dictionary<Vector3, StairObject>(),new Dictionary<Vector3, StairObject>(), new Dictionary<Vector3, StairObject>() };
    //Dictionary<Vector3, int>[] st = new Dictionary<Vector3, int>[3]{ new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(), new Dictionary<Vector3, int>() };
    Dictionary<Vector3, int>[] wlCuts = new Dictionary<Vector3, int>[3] { new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>()};

    public List<Building>[] buildings = new List<Building>[3] { new List<Building>(),new List<Building>(),new List<Building>(), };

    Dictionary<Vector3, GameObject>[] wlComplete = new Dictionary<Vector3, GameObject>[3] { new Dictionary<Vector3, GameObject>(),new Dictionary<Vector3, GameObject>(),new Dictionary<Vector3, GameObject>()};
    Dictionary<Vector3, GameObject>[] crComplete = new Dictionary<Vector3, GameObject>[3] { new Dictionary<Vector3, GameObject>(),new Dictionary<Vector3, GameObject>(),new Dictionary<Vector3, GameObject>() };
    Dictionary<Vector3, GameObject>[] stComplete = new Dictionary<Vector3, GameObject>[3] { new Dictionary<Vector3, GameObject>(), new Dictionary<Vector3, GameObject>(), new Dictionary<Vector3, GameObject>() };
    
    List<GameObject> constructs = new List<GameObject>();
    
    public float scaleFactor = 0.75f;

    public GameObject floorConstruct;
    public GameObject floorConstructFill;
    public GameObject floorConstructRed;
    public GameObject wallConstruct;
    public GameObject wallConstructFill;
    public GameObject stairConstruct;
    public GameObject stairConstructFill;

    public GameObject floor;
    public GameObject door;
    public GameObject Wall;
    public GameObject Wall3;
    public GameObject Wall3r;
    public GameObject Wall6;
    public GameObject inCorner;
    public GameObject outCorner;
    public GameObject stair;

    GameObject currentPrefab;
    GameObject currentFillPrefab;
    int currentRotation;
    
    ConstructObject select;
    public BuildType buildType { get; protected set; }
    public WallType wallType { get; protected set; }
    public TileType tileType { get; protected set; }
    public int currentLevel { get; protected set; }

    delegate void SelectObj(GameObject target);
    SelectObj selectObj;
    delegate void SetObj();
    SetObj setObj;
    delegate void ClearObj();
    ClearObj clearObj;


    public delegate void SelectComplete();
    public SelectComplete selectComplete;

    void Start () {

        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        SetAvailablePoints(8,5,-2,-1);

        selectObj = SelectFloor;
        clearObj = ClearFloor;
        setObj = SetFloor;

        Destroy(modifier);

        //ShowAvailableFloors();
        TestBuilding();
	}

    void TestBuildingLow()
    {
        fl[0][new Vector2(0, 0)].Build(2);
        fl[0][new Vector2(1, 0)].Build(2);
        fl[0][new Vector2(0, 1)].Build(2);
        fl[0][new Vector2(1, 1)].Build(2);

        buildType = BuildType.Wall;
        wl[0][new Vector3(0, -1, 270)].Build();
        wl[0][new Vector3(1, -1, 270)].Build();

        wl[0][new Vector3(-1, 0, 0)].Build();
        wl[0][new Vector3(-1, 1, 0)].Build();

        wl[0][new Vector3(0, 2, 90)].Build();
        wl[0][new Vector3(1, 2, 90)].Build();

        wl[0][new Vector3(2, 0, 180)].Build();
        //wl[0][new Vector3(2, 0, 180)].HideHalf();
        buildType = BuildType.Wall;
        wallType = WallType.Door;
        wl[0][new Vector3(2, 1, 180)].Build();
        
        currentLevel = 1;

        fl[1][new Vector2(0, 0)].Build(2);
       // fl[1][new Vector2(1, 0)].Build(2);
       // fl[1][new Vector2(0, 1)].Build(2);
        //fl[1][new Vector2(1, 1)].Build(2);

        buildType = BuildType.Wall;
        wallType = WallType.Wall;
        wl[1][new Vector3(0, -1, 270)].Build();
        wl[1][new Vector3(-1, 0, 0)].Build();

        //cr[1][new Vector3(3, -1, 270)].HideHalf();
    }
    void TestBuilding()
    {
        fl[0][new Vector2(0, 0)].Build(2);
        fl[0][new Vector2(1, 0)].Build(2);
        fl[0][new Vector2(0, 1)].Build(2);
        fl[0][new Vector2(1, 1)].Build(2);
        fl[0][new Vector2(0, 2)].Build(2);
        fl[0][new Vector2(1, 2)].Build(2);
        fl[0][new Vector2(2,2)].Build(2);
        fl[0][new Vector2(2, 1)].Build(2);
        fl[0][new Vector2(2, 0)].Build(2);

        buildType = BuildType.Wall;
        wallType = WallType.Wall;
        wl[0][new Vector3(0,-1,270)].Build();
        wl[0][new Vector3(1,-1,270)].Build();
        wl[0][new Vector3(2,-1,270)].Build();

        wl[0][new Vector3(-1,0,0)].Build();
        wl[0][new Vector3(-1,1,0)].Build();
        wl[0][new Vector3(-1,2,0)].Build();

        wl[0][new Vector3(0,3,90)].Build();
        wl[0][new Vector3(1,3,90)].Build();
        wl[0][new Vector3(2,3,90)].Build();

        wl[0][new Vector3(3,0,180)].Build();
        wl[0][new Vector3(3,2,180)].Build();
        buildType = BuildType.Wall;
        wallType = WallType.Door;
        wl[0][new Vector3(3,1,180)].Build();
        
        st[0][new Vector3(1, 1, 180)].Build();
        currentLevel = 1;

        //fl[1][new Vector2(0, 1)].Build();
        //fl[1][new Vector2(2, 2)].Build();
        //wl[1][new Vector3(1, 2, 0)].Build();


        buildType = BuildType.Floor;
        fl[1][new Vector2(0, 0)].Build(2);
        fl[1][new Vector2(1, 0)].Build(2);
        fl[1][new Vector2(0, 1)].Build(2);
       // fl[1][new Vector2(1, 1)].Build(2);
        fl[1][new Vector2(0, 2)].Build(2);
        fl[1][new Vector2(1, 2)].Build(2);
        //fl[1][new Vector2(2, 2)].Build(2);
        //fl[1][new Vector2(2, 1)].Build(2);
        fl[1][new Vector2(2, 0)].Build(2);

        buildType = BuildType.Wall;
        wallType = WallType.Wall;
        wl[1][new Vector3(0, -1, 270)].Build();
        wl[1][new Vector3(1, -1, 270)].Build();
        wl[1][new Vector3(2, -1, 270)].Build();
        

        wl[1][new Vector3(-1, 0, 0)].Build();
        wl[1][new Vector3(-1, 1, 0)].Build();
        wl[1][new Vector3(-1, 2, 0)].Build();

        wl[1][new Vector3(0, 3, 90)].Build();
        wl[1][new Vector3(1, 3, 90)].Build();
        wl[1][new Vector3(2, 1, 90)].Build();

        wl[1][new Vector3(3, 0, 180)].Build();
        wl[1][new Vector3(2, 1, 180)].Build();
        buildType = BuildType.Wall;
        wallType = WallType.Door;
        wl[1][new Vector3(2, 2, 180)].Build();

        /*
        st[1][new Vector3(1, 0, 0)].Build();

        currentLevel = 2;

        buildType = 0;
        fl[2][new Vector2(0, 0)].Build(2);
        buildType = 1;

        wl[2][new Vector3(0, -1, 270)].Build();

        wl[2][new Vector3(-1, 0, 0)].Build();
        //*/
        currentLevel = 0;
        ChangeLevel();
    }

    void SetAvailablePoints(int x,int y,int startX,int startY)
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Vector2 key = new Vector2(i, j);
                fl[currentLevel][key] = new TileObject(key,currentLevel);
            }
        }
    }

    void ShowAvailableFloors()
    {
        RemoveAllPoints();
        foreach (TileObject t in fl[currentLevel].Values)
        {
            if (t.IsEmpty())
            {
                t.Show();
            }
        }
        /*
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 key=new Vector2(i, j);
                if (fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 0)
                {
                    bool act = true;

                    Vector3 keyWall;
                    for (int k = 0; k < 4; k++)
                    {
                        keyWall = new Vector3(i, j, k * 90);
                        if (wl[currentLevel].ContainsKey(keyWall) && wl[currentLevel][keyWall] >0) act = false;
                    }
                    if(act==true) act = !IsDoorConnectedToTile(i, j);
                    
                    if (act)
                    {
                        GameObject construct = Instantiate(floorConstruct, transform.position + new Vector3(i * 6 * scaleFactor, currentLevel*4 * scaleFactor, j * 6 * scaleFactor), Quaternion.identity, transform);
                        construct.transform.parent = transform;
                        constructs.Add(construct);
                    }
                }
            }
        }
        //*/
    }
    void ShowAvailableFloorsHight()
    {
        RemoveAllPoints();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 key = new Vector2(i, j);
                if (fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 0)
                {
                    bool act = true;

                    Vector3 keyWall;
                    for (int k = 0; k < 4; k++)
                    {
                        keyWall = new Vector3(i, j, k * 90);
 //                       if (wl[currentLevel].ContainsKey(keyWall) && wl[currentLevel][keyWall] > 0) act = false;
                    }
 //                   if (act == true) act = !IsDoorConnectedToTile(i, j);

                    if (act)
                    {
                        GameObject construct = Instantiate(floorConstruct, transform.position + new Vector3(i * 6 * scaleFactor, currentLevel * 4 * scaleFactor, j * 6 * scaleFactor), Quaternion.identity, transform);
                        construct.transform.parent = transform;
                        constructs.Add(construct);
                    }
                }
            }
        }
    }
    
    void ShowAvailableWalls()
    {
        RemoveAllPoints();
        foreach (WallObject w in wl[currentLevel].Values)
        {
            if (w.IsEmpty())
            {
                w.Show();
                //if (buildType ==1 && w.canBeDoorOnly) w.SetRed(); 
            }
        }
        foreach (Building b in buildings[currentLevel])
        {
            if (b.lastDoor!=null)
            {
               // b.lastDoor.SetRed();
            }
        }

        /*
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                Vector2 keyFloor = new Vector2(i, j);
                if (fl[currentLevel].ContainsKey(keyFloor) && fl[currentLevel][keyFloor].state == 1)
                {
                    Vector2 key = new Vector2(i + 1, j);
                    Vector3 key2 = new Vector3(i + 1, j,180);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 1))
                        if(!(wl[currentLevel].ContainsKey(key2) && wl[currentLevel][key2]>0))
                            CreateWallConstruct(i + 1, j - 1, 180);

                    key = new Vector2(i, j + 1);
                    key2 = new Vector3(i , j+1 , 90);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 1))
                        if (!(wl[currentLevel].ContainsKey(key2) && wl[currentLevel][key2] > 0))
                            CreateWallConstruct(i + 1, j , 90);


                    key = new Vector2(i-1, j);
                    key2 = new Vector3(i-1 , j , 0);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 1))
                        if (!(wl[currentLevel].ContainsKey(key2) && wl[currentLevel][key2] > 0))
                            CreateWallConstruct(i, j, 0);

                    key = new Vector2(i, j -1);
                    key2 = new Vector3(i , j - 1, 270);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 1))
                        if (!(wl[currentLevel].ContainsKey(key2) && wl[currentLevel][key2] > 0))
                            CreateWallConstruct(i , j - 1, 270);
                    
                }
            }
        }
        //*/
    }
    void ShowAvailableDoors()
    {
        RemoveAllPoints();
        foreach (WallObject w in wl[currentLevel].Values)
        {
            if (w.IsEmpty() && w.canBeDoor)
            {
                w.Show();
            }
        }
        /*

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 Floor = new Vector2(i, j);
                if (fl[currentLevel].ContainsKey(Floor) && fl[currentLevel][Floor].state == 1)
                {
                    Vector2 key = new Vector2(i + 1, j);
                    Vector2 key2 = new Vector2(i + 1, j+1);
                    Vector2 key3 = new Vector2(i + 1, j-1);
                    Vector3 key4 = new Vector3(i + 1, j,180);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 1) && !(fl[currentLevel].ContainsKey(key2) && fl[currentLevel][key2].state == 1) && !(fl[currentLevel].ContainsKey(key3) && fl[currentLevel][key3].state == 1))
 //                       if (!(wl[currentLevel].ContainsKey(key4) && wl[currentLevel][key4] > 0))
                            CreateWallConstruct(i + 1, j - 1, 180);

                    key = new Vector2(i, j+1);
                    key2 = new Vector2(i + 1, j + 1);
                    key3 = new Vector2(i -1, j + 1);
                    key4 = new Vector3(i , j+1, 90);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 1) && !(fl[currentLevel].ContainsKey(key2) && fl[currentLevel][key2].state == 1) && !(fl[currentLevel].ContainsKey(key3) && fl[currentLevel][key3].state == 1))
 //                       if (!(wl[currentLevel].ContainsKey(key4) && wl[currentLevel][key4] > 0))
                            CreateWallConstruct(i + 1, j, 90);

                    key = new Vector2(i - 1, j);
                    key2 = new Vector2(i - 1, j + 1);
                    key3 = new Vector2(i - 1, j - 1);
                    key4 = new Vector3(i - 1, j, 0);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 1) && !(fl[currentLevel].ContainsKey(key2) && fl[currentLevel][key2].state == 1) && !(fl[currentLevel].ContainsKey(key3) && fl[currentLevel][key3].state == 1))
 //                       if (!(wl[currentLevel].ContainsKey(key4) && wl[currentLevel][key4] > 0))
                            CreateWallConstruct(i, j, 0);

                    key = new Vector2(i, j - 1);
                    key2 = new Vector2(i + 1, j - 1);
                    key3 = new Vector2(i - 1, j - 1);
                    key4 = new Vector3(i , j-1, 270);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key].state == 1) && !(fl[currentLevel].ContainsKey(key2) && fl[currentLevel][key2].state == 1) && !(fl[currentLevel].ContainsKey(key3) && fl[currentLevel][key3].state == 1))
 //                       if (!(wl[currentLevel].ContainsKey(key4) && wl[currentLevel][key4] > 0))
                            CreateWallConstruct(i, j - 1, 270);
                }
            }
        } 
        //*/
    }

    void ShowAvailableTurrets()
    {
        RemoveAllPoints();
        foreach (TileObject fl in fl[currentLevel].Values)
        {
            if (fl.IsEmpty())
            {
                StairObject turret = new StairObject(fl.key, currentLevel);
                turret.Show();
                tempObjects.Add(turret);
                toBeDestroyedObjects.Add(turret);
            }
        }
    }
    void ShowAvailableStairs()
    {
        RemoveAllPoints();
        foreach (StairObject str in st[currentLevel].Values)
        {
            if (str.IsEmpty() && str.key.z==currentRotation)
            {
                str.Show();
            }
        }
        /*
        RemoveAllPoints();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2[] keys = GetStairKeys(i, j);
                
                bool act = true;
                if (ob[currentLevel].ContainsKey(keys[0]) && ob[currentLevel][keys[0]] > 0) act = false;
                if (ob[currentLevel].ContainsKey(keys[1]) && ob[currentLevel][keys[1]] == 1) act = false;
                if (!(fl[currentLevel].ContainsKey(keys[0]) && fl[currentLevel][keys[0]].state == 1)) act = false;
                if (!(fl[currentLevel].ContainsKey(keys[1]) && fl[currentLevel][keys[1]].state == 1)) act = false;

                if (act)
                {
                    GameObject construct = Instantiate(stairConstruct, transform.position + new Vector3(i * 6 * scaleFactor, currentLevel*4*scaleFactor, j * 6 * scaleFactor), Quaternion.Euler(0,currentRotation,0), transform);
                    construct.transform.parent = transform;
                    constructs.Add(construct);
                }
            }
        }
        //*/
    }
    Vector2[] GetStairKeys(int i, int j)
    {
        Vector2 key = new Vector2(i, j);
        Vector2 key2 = new Vector2(i - 1, j);
        int rot = currentRotation / 90;
        switch (rot)
        {
            case 1:
                key = new Vector2(i - 1, j);
                key2 = new Vector2(i - 1, j + 1);
                break;
            case 2:
                key = new Vector2(i - 1, j + 1);
                key2 = new Vector2(i, j + 1);
                break;
            case 3:
                key = new Vector2(i, j + 1);
                key2 = new Vector2(i, j);
                break;
        }
        return new Vector2[2]{key, key2};
    }

    void CreateWallConstruct(int i, int j, int turn)
    {
        GameObject construct = Instantiate(wallConstruct, transform.position + new Vector3((i) * 6 * scaleFactor, currentLevel * 4 * scaleFactor, (j) * 6 * scaleFactor), Quaternion.Euler(0, turn, 0), transform);
        construct.transform.parent = transform;
        constructs.Add(construct);
    }

    void RemoveAllPoints()
    {
        foreach (ConstructObject dest in toBeDestroyedObjects)
        {
            GameObject.Destroy(dest.obj);
        }
        toBeDestroyedObjects.Clear();

        foreach (ConstructObject c in tempObjects)
        {
           c.Show(false);
        }
        tempObjects.Clear();

        if(select!=null)
        {
            select.Show(false);
            select = null;
        }
        /*foreach (GameObject cons in constructs)
        {
            Destroy(cons);
        }
        constructs.Clear();
        //*/
    }

    int oldLevel;
    void ChangeLevel()
    {
        //if (oldLevel == currentLevel) return;
        oldLevel = currentLevel;

        int i = 0;
        while (i< fl.Length)
        {
            if (i < oldLevel)
            {
                foreach (TileObject t in fl[i].Values)
                {
                    t.Hide(false);
                }
                foreach (WallObject w in wl[i].Values)
                {
                    w.HideHalf(false);
                }
                foreach (CornerObject c in cr[i].Values)
                {
                    c.Hide(false);
                    c.HideHalf(false);
                }
                foreach (StairObject s in st[i].Values)
                {
                    s.Hide(false);
                }
            }
            else if(i==oldLevel)
            {
                foreach (TileObject t in fl[i].Values)
                {
                    t.Hide(false);
                }
                foreach (WallObject w in wl[i].Values)
                {
                    w.Hide(false);
                    w.HideHalf();
                }
                foreach (CornerObject c in cr[i].Values)
                {
                    c.Hide(false);
                    c.HideHalf();
                }
                foreach (StairObject s in st[i].Values)
                {
                    s.Hide(false);
                }
            }
            else
            {
                foreach (TileObject t in fl[i].Values)
                {
                    t.Hide();
                }
                foreach (WallObject w in wl[i].Values)
                {
                    w.HideHalf(false);
                    w.Hide();
                }
                foreach (CornerObject c in cr[i].Values)
                {
                    c.Hide();
                }
                foreach (StairObject s in st[i].Values)
                {
                    s.Hide();
                }
            }
            i++;
        }

        /*
        if (currentLevel == 0)
        {
            foreach (TileObject t in fl[1].Values)
            {
                t.Hide();
            }
            foreach (WallObject w in wl[1].Values)
            {
                w.Hide();
            }
            foreach (WallObject w in wl[0].Values)
            {
                w.HideHalf();
            }
            foreach(CornerObject c in cr[0].Values)
            {
                c.HideHalf();
            }
        }
        if (currentLevel == 1)
        {
            foreach (TileObject t in fl[1].Values)
            {
                t.Hide(false);
            }
            foreach (WallObject w in wl[1].Values)
            {
                w.Hide(false);
            }
            foreach (WallObject w in wl[0].Values)
            {
                w.HideHalf(false);
            }
            foreach (CornerObject c in cr[0].Values)
            {
                c.HideHalf(false);
            }
        }
        //*/
    }
    public void SetBuildType(BuildType _buildType,WallType _wallType=WallType.Wall,TileType _tyleType=TileType.Floor)
    {
        ChangeLevel();

        buildType = _buildType;
        wallType = _wallType;
        tileType = _tyleType;

        switch (_buildType)
        {
            case BuildType.Floor:
                ShowAvailableFloors();
                break;
            case BuildType.Wall:
                switch (wallType)
                {
                    case WallType.Door:
                        ShowAvailableDoors();
                        break;
                    default:
                        ShowAvailableWalls();
                        break;
                }

                break;
            case BuildType.OnFloor:
                switch (tileType) {
                    case TileType.Stair:
                        ShowAvailableStairs();
                        break;
                    case TileType.Turret:
                        ShowAvailableTurrets();
                        break;
                }


                break;
        }
        /*
        if (currentLevel == 0)
        {
            switch (n)
            {
                case 0:
                    ShowAvailableFloors();
                    break;
                case 1:
                    ShowAvailableWalls();

                    break;
                case 2:
                    ShowAvailableDoors();
                    break;
                case 3:
                    ShowAvailableStairs();
                    break;
            }
        }
        else
        {
            switch (n)
            {
                case 0:
                    ShowAvailableFloorsHight();
                    break;
                case 1:
                    ShowAvailableWalls();
                    break;
                case 2:
                    ShowAvailableDoors();
                    break;
                case 3:
                    ShowAvailableStairs();
                    break;
            }
        }

        switch (n)
        {
            case 0:
                selectObj = SelectFloor;
                clearObj = ClearFloor;
                setObj = SetFloor;
                break;
            case 1:
                selectObj = SelectWall;
                clearObj = ClearWall;
                setObj = SetWall;

                break;
            case 2:
                selectObj = SelectDoor;
                clearObj = ClearDoor;
                setObj = SetDoor;
                break;
            case 3:
                selectObj = SelectStair;
                clearObj = ClearStair;
                setObj = SetStair;
                break;
        }
        //*/
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            wl[1][new Vector3(0, -1, 270)].HideHalf();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            wl[1][new Vector3(0, -1, 270)].HideHalf(false);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {

            SetBuildType(BuildType.OnFloor, 0);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetBuildType(BuildType.Wall, 0);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SetBuildType(BuildType.Wall, 0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetBuildType(BuildType.Floor, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BuildObject();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            RotatePositive();
            ShowAvailableStairs();
            selectObj = SelectStair;
            clearObj = ClearStair;
            setObj = SetStair;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            RotatePositive(false);
            ShowAvailableStairs();
            selectObj = SelectStair;
            clearObj = ClearStair;
            setObj = SetStair;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentLevel = 0;
            ChangeLevel();
            clearObj();
            //SetBuildType(buildType, 0);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            currentLevel = 1;
            ChangeLevel();
            clearObj();
            //SetBuildType(buildType, 0);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            currentLevel = 2;
            ChangeLevel();
            clearObj();
            //SetBuildType(buildType, 0);
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            buttonDownPosition = Input.mousePosition;
            buttonDownTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if ((buttonDownPosition - Input.mousePosition).magnitude > 4f) return;
            if (Time.time - buttonDownTime > 0.5f) return;
            buttonDownTime = 0;
            buttonDownPosition = new Vector3(-10000, -10000, -10000);
            GetCLickObject();
        }
    }
    float buttonDownTime;
    Vector3 buttonDownPosition;

    public void BuildObject()
    {
        if (select != null)
        {
            select.Build();
            select = null;

            //setObj.Invoke();
            //CheckTileConnectToBuilding(select);
        }
    }

    public void RotatePositive(bool plus=true)
    {
        if (plus)
        {
            currentRotation += 90;
            if (currentRotation > 270) currentRotation = 0;
        }
        else
        {
            currentRotation -= 90;
            if (currentRotation < 0) currentRotation = 270;
        }

        switch (buildType)
        {
            case BuildType.Floor:
                ShowAvailableFloors();
                break;
            case BuildType.Wall:
                switch (wallType)
                {
                    case WallType.Door:
                        ShowAvailableDoors();

                        break;
                    default:
                        ShowAvailableWalls();
                        break;
                }
                break;
            case BuildType.OnFloor:
                ShowAvailableStairs();
                break;
        }

    }

    void GetCLickObject()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100,LayerMask.GetMask("House")))
        {
            ConstructElement parent = hit.collider.transform.parent.GetComponent<ConstructElement>();
            //Debug.Log("Click " + parent+" // "+ hit.collider+" __ "+ hit.collider.transform.parent);
            if (parent == null)
            {
                if (hit.collider.transform.parent.parent!=null)
                parent = hit.collider.transform.parent.parent.GetComponent<ConstructElement>();
            }
            //Debug.Log("22  " + parent);
            if (parent == null) return;
            if (parent.construct == null) return;
            //Debug.Log(" not return ");
            if (parent.construct.state == 0)
            {
                /*if (select != null)
                {
                    select.Show();
                }
                if (parent.construct.Select())
                {
                    select = parent.construct;
                    if (selectComplete != null) selectComplete.Invoke();
                }
                //*/
                parent.construct.Build();
            }
        }
    }

    public void ChangeFloor(bool up=true)
    {
        if (up && currentLevel < 2) currentLevel++;
        if (!up && currentLevel > 0) currentLevel--;

        SetBuildType(buildType,0);
    }

    ////  //////////////     BUILDINGS
    
    public void CreateBuilding(ConstructObject t)
    {
        Building b = new Building(t);
        buildings[currentLevel].Add(b);
    }

    /// ////////                 Floors

    void SelectFloor(GameObject target)
    {
 //       if (selected != null) ClearFloor();

        constructs.Remove(target);
        GameObject construct = Instantiate(floorConstructFill, target.transform.position, target.transform.rotation, transform);
        Destroy(target);
 //       selected = construct;
    }
    void ClearFloor()
    {
 //       GameObject construct = Instantiate(floorConstruct, selected.transform.position, selected.transform.rotation, transform);
 //       constructs.Add(construct);
 //       Destroy(selected);
 //       selected = null;
    }
    void SetFloor()
    {
        int i=0;//= (int)Mathf.Round(selected.transform.localPosition.x/6);
       int j=0;//= (int)Mathf.Round(selected.transform.localPosition.z/6);
        Vector2 key = new Vector2(i,j);
        TileObject t = fl[currentLevel][key];

        GameObject obj = null;// Instantiate(floor, transform.position+ new Vector3(i*6*scaleFactor,currentLevel*4*scaleFactor, j * 6 * scaleFactor), selected.transform.rotation, transform);
        obj.transform.parent = transform;

 //     t.Build();
        CheckTileConnectToBuilding(t);

        //        Destroy(selected);
        //       selected = null;

        if (currentLevel!=0)
        {
            ExpandFloor(key);
        }

        //surface.BuildNavMesh();
    }
    void ExpandFloor(Vector2 key)
    {
        Vector2[] checkKeys = new Vector2[4] {new Vector2(key.x-1,key.y),
                                              new Vector2(key.x+1,key.y),
                                              new Vector2(key.x,key.y-1),
                                              new Vector2(key.x,key.y+1)};
        bool sumAct = false;
        foreach (Vector2 check in checkKeys)
        {
            bool act = true;
            if (fl[currentLevel].ContainsKey(check)) act = false;
            if (!(fl[currentLevel-1].ContainsKey(check) && fl[currentLevel - 1][check].state > 0)) act = false;
            if (ob[currentLevel-1].ContainsKey(check) && fl[currentLevel - 1][check].state == 1) act = false;
            if (act)
            {
                fl[currentLevel][check] = new TileObject(check,currentLevel);
                sumAct = true;
            }
        }
        if (sumAct)
        {
            ShowAvailableFloorsHight();
        }
    }

    void CheckTilesConnections()
    {
        foreach (TileObject t in fl[currentLevel].Values)
        {
            CheckTileConnectToBuilding(t);
        }
    }
    void CheckTileConnectToBuilding(ConstructObject t)
    {

        if (t.state == 1 && !t.InBuilding())
        {
            Vector2[] keys = new Vector2[4] {new Vector3(t.key.x+1,t.key.y,0),
                                         new Vector3(t.key.x-1,t.key.y,0),
                                         new Vector3(t.key.x,t.key.y+1,0),
                                         new Vector3(t.key.x,t.key.y-1,0)
                                         };
            List<Building> nearBuildings = new List<Building>();
            foreach (Vector3 k in keys)
            { 
                if (fl[currentLevel].ContainsKey(k)) { 
                    TileObject t2 = fl[currentLevel][k];
                    if (t2.state == 1 && t2.InBuilding())
                    {
                        if (!nearBuildings.Contains(t2.building))
                        {
                            nearBuildings.Add(t2.building);
                        }
                    }
                }
            }

            if (nearBuildings.Count == 0)
                CreateBuilding(t);
            else
            {
                nearBuildings[0].Add(t);
                int i = nearBuildings.Count;
                while (--i > 0)
                {
                    nearBuildings[0].Connect(nearBuildings[i]);
                    buildings[currentLevel].Remove(nearBuildings[i]);
                }
            }

        }
    }
    
    /// ////////                 WALLS

    void SelectWall(GameObject target)
    {
//        if (selected != null) ClearWall();
        
        constructs.Remove(target);
        GameObject construct = Instantiate(wallConstructFill, target.transform.position, target.transform.rotation, transform);
        Destroy(target);
//        selected = construct;
    }
    void ClearWall()
    {
        GameObject construct=null;//= Instantiate(wallConstruct, selected.transform.position, selected.transform.rotation, transform);
        constructs.Add(construct);
//        Destroy(selected);
 //       selected = null;
    }
    void SetWall()
    {
        Vector3 key=new Vector3();
        int i=0;//= (int)Mathf.Round(selected.transform.localPosition.x / 6);
        int j=0;//= (int)Mathf.Round(selected.transform.localPosition.z / 6);

        float angle=0;// = selected.transform.localRotation.eulerAngles.y;
        if (angle == 270 || angle==270)
            key = new Vector3(i, j, 270);
        else if (angle == 0)
            key = new Vector3(i - 1, j, 0);
        else if (angle == 90)
            key = new Vector3(i - 1, j+1, 90);
        else if (angle == 180)
            key = new Vector3(i , j+1, 180);
        

        Debug.Log("Wall "+key);

        GameObject obj=null;// = Instantiate(Wall, transform.position + new Vector3(i * 6 * scaleFactor, currentLevel * 4 * scaleFactor, j * 6 * scaleFactor), selected.transform.localRotation, transform);
        obj.transform.parent = transform;


        //wl[currentLevel][key]= 1;
        wlComplete[currentLevel][key] = obj;

 //       Destroy(selected);
  //      selected = null;

        ConstructCorners(key,obj);

        //surface.BuildNavMesh();
    }
    void ConstructCorners(Vector3 wallc, GameObject  obj)
    {
        Vector3 key = new Vector3();
        int wallSkaler = 0;
        /*
        if (wallc.z == 0)
        {
            key = new Vector3(wallc.x, wallc.y, 270);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x + 1, key.y, 90));
                UpdateWallSkaler(key, 2);
            }
            else
            {
                key = new Vector3(wallc.x + 1, wallc.y + 1, 90);
               if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x, key.y - 1, 0));
                }
            }

            key = new Vector3(wallc.x, wallc.y, 90);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                wallSkaler += 2;
                CreateInCorner(new Vector3(key.x + 1, key.y - 1, 180));
                UpdateWallSkaler(key, 1);
           }
            else
            {
                key = new Vector3(wallc.x + 1, wallc.y - 1, 270);
                if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x, key.y, 270));
               }
            }
        }
        if (wallc.z == 90)
        {
            key = new Vector3(wallc.x, wallc.y, 180);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                wallSkaler += 2;
                CreateInCorner(new Vector3(key.x, key.y - 1, 270));
                UpdateWallSkaler(key, 1);
            }
            else
            {
                key = new Vector3(wallc.x - 1, wallc.y - 1, 0);
                if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x + 1, key.y, 0));
                }
            }
            
            key = new Vector3(wallc.x, wallc.y, 0);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x + 1, key.y - 1, 180));
                UpdateWallSkaler(key, 2);
           }
            else
            {
                key = new Vector3(wallc.x + 1, wallc.y - 1, 180);
               if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x, key.y, 90));
                }
            }
        }

        if (wallc.z == 180)
        {
            key = new Vector3(wallc.x, wallc.y, 270);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                wallSkaler += 2;
                CreateInCorner(new Vector3(key.x, key.y, 0));
                UpdateWallSkaler(key, 1);
            }
            else
            {
                key = new Vector3(wallc.x - 1, wallc.y + 1, 90);
  //              if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
   //             {
                    CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 90));
  //              }
            }
            
            key = new Vector3(wallc.x, wallc.y, 90);
 //           if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
 //           {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x, key.y - 1, 270));
                UpdateWallSkaler(key, 2);
 //           }
            else
            {
                key = new Vector3(wallc.x - 1, wallc.y - 1, 270);
   //             if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
  //              {
                    CreateOutCorner(new Vector3(key.x + 1, key.y, 180));
  //              }
            }
        }
        if (wallc.z == 270)
        {
            key = new Vector3(wallc.x, wallc.y, 180);
  //          if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
   //         {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x, key.y, 0));
                UpdateWallSkaler(key, 2);
 //           }
            else
            {
                key = new Vector3(wallc.x - 1, wallc.y + 1, 0);
                if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 270));
                }
            }

            key = new Vector3(wallc.x, wallc.y, 0);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                wallSkaler += 2;
                CreateInCorner(new Vector3(key.x + 1, key.y, 90));
                UpdateWallSkaler(key, 1);
            }
            else
            {
                key = new Vector3(wallc.x + 1, wallc.y + 1, 180);

                if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x, key.y - 1, 180));
                }
                
            }
        }

        if (wallSkaler != 0)
        {
            UpdateWallSkaler(wallc, wallSkaler);
        }
        /*
            GameObject prefab = Wall;
            switch (wallSkaler)
            {
                case 1:
                    prefab = Wall3r;
                    break;
                case 2:
                    prefab = Wall3;
                    break;
                case 3:
                    prefab = Wall6;
                    break;
            }

            GameObject updObject = Instantiate(prefab, obj.transform.position, obj.transform.rotation, transform);
            Destroy(obj);
            wlCuts.Add(wallc, wallSkaler);
        
        //*/
    }
    void UpdateWallSkaler(Vector3 wallc,int skaler)
    {
//        if(wlComplete.ContainsKey(wallc)) Debug.Log("wall comp "+wlComplete[wallc]);
        if (wlComplete[currentLevel].ContainsKey(wallc) && wlComplete[currentLevel][wallc]!=null)
        {
             GameObject obj = wlComplete[currentLevel][wallc];

            int prevScaler = 0;
            if(wlCuts[currentLevel].ContainsKey(wallc)) prevScaler = wlCuts[currentLevel][wallc];

            skaler += prevScaler;

            GameObject prefab = Wall;
            switch (skaler)
            {
                case 1:
                    prefab = Wall3r;
                    break;
                case 2:
                    prefab = Wall3;
                    break;
                case 3:
                    prefab = Wall6;
                    break;
            }

            GameObject updObject = Instantiate(prefab, obj.transform.position, obj.transform.rotation, transform);
            Destroy(obj);

            wlComplete[currentLevel][wallc] = updObject;
            wlCuts[currentLevel][wallc] = skaler;
        }
    }

    void CreateOutCorner(Vector3 key)
    {
        /*
        Vector2 checkKey2 = new Vector2();
        if (key.z==0)
        {
            checkKey2 = new Vector2(key.x-1,key.y+1);
        }else if (key.z == 90)
        {
            checkKey2 = new Vector2(key.x, key.y + 1);
        }
        else if (key.z == 180)
        {
            checkKey2 = new Vector2(key.x, key.y);
        }
        else if (key.z == 270)
        {
            checkKey2 = new Vector2(key.x-1, key.y);
        }
        if (fl[currentLevel].ContainsKey(checkKey2) && fl[currentLevel][checkKey2].state > 0) return;

        if (!(cr[currentLevel].ContainsKey(key) && cr[currentLevel][key]==1))
        {
            cr[currentLevel].Add(key, 1);
            Instantiate(outCorner, transform.position+new Vector3(key.x * 6 * scaleFactor, currentLevel * 4 * scaleFactor, key.y * 6 * scaleFactor), Quaternion.Euler(0,key.z,0), transform);
        }
        //*/
    }
    void CreateInCorner(Vector3 key)
    {
        /*
        if (!(cr[currentLevel].ContainsKey(key) && cr[currentLevel][key] == 1))
        {
            cr[currentLevel].Add(key, 1);
            Instantiate(inCorner, transform.position + new Vector3(key.x * 6 * scaleFactor, currentLevel * 4 * scaleFactor, key.y * 6 * scaleFactor), Quaternion.Euler(0, key.z, 0), transform);
        }
        //*/
    }

    void CheckIsolation()
    {

    }

    ///////////////////////  DOORS 
    void SelectDoor(GameObject target)
    {
 //       if (selected != null) ClearDoor();

        constructs.Remove(target);
        GameObject construct = Instantiate(wallConstructFill, target.transform.position, target.transform.rotation, transform);
        Destroy(target);
 //       selected = construct;
    }
    void ClearDoor()
    {
        GameObject construct = null;// Instantiate(wallConstruct, selected.transform.position, selected.transform.rotation, transform);
        constructs.Add(construct);
        //       Destroy(selected);
        //       selected = null;
    }
    void SetDoor()
    {
        Vector3 key = new Vector3();
        int i = 0;// (int)Mathf.Round(selected.transform.localPosition.x / 6);
        int j = 0;//(int)Mathf.Round(selected.transform.localPosition.z / 6);

        float angle = 0;//selected.transform.localRotation.eulerAngles.y;
        if (angle == 270 || angle == 270)
            key = new Vector3(i, j, 270);
        else if (angle == 0)
            key = new Vector3(i - 1, j, 0);
        else if (angle == 90)
            key = new Vector3(i - 1, j + 1, 90);
        else if (angle == 180)
            key = new Vector3(i, j + 1, 180);


        Debug.Log("door "+key);

        GameObject obj = null;// Instantiate(door, transform.position + new Vector3(i * 6 * scaleFactor, currentLevel * 4 * scaleFactor, j * 6 * scaleFactor), selected.transform.localRotation, transform);
        obj.transform.parent = transform;

        //wl[currentLevel].Add(key, 4);
        wlComplete[currentLevel][key] = obj;

 //       Destroy(selected);
 //       selected = null;

        ConstructCornersDoors(key, obj);

       //surface.BuildNavMesh();
    }
    void ConstructCornersDoors(Vector3 wallc, GameObject obj)
    {
        Vector3 key = new Vector3();
 //       int wallSkaler = 0;
/*        if (wallc.z == 0)
        {
            key = new Vector3(wallc.x + 1, wallc.y + 1, 90);
 //           if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] >0)
            {
                CreateOutCorner(new Vector3(key.x, key.y - 1, 0));
            }
            key = new Vector3(wallc.x + 1, wallc.y - 1, 270);
   //         if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x, key.y, 270));
            }
        }
        if (wallc.z == 90)
        {
            key = new Vector3(wallc.x - 1, wallc.y - 1, 0);
 //           if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y, 0));
            }
            key = new Vector3(wallc.x + 1, wallc.y - 1, 180);
 //           if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x, key.y, 90));
            }
        }

        if (wallc.z == 180)
        {
            key = new Vector3(wallc.x - 1, wallc.y + 1, 90);
 //           if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 90));
            }
            key = new Vector3(wallc.x - 1, wallc.y - 1, 270);
  //          if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y, 180));
            }
        }
        if (wallc.z == 270)
        {
            key = new Vector3(wallc.x - 1, wallc.y + 1, 0);
 //           if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 270));
            }
            key = new Vector3(wallc.x + 1, wallc.y + 1, 180);
 //           if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x, key.y - 1, 180));
            }
        }

       //*/ 
    }

    ///////               STAIRS 
    void SelectStair(GameObject target)
    {
 //       if (selected != null) ClearStair();

        constructs.Remove(target);
        GameObject construct = Instantiate(stairConstructFill, target.transform.position, target.transform.rotation, transform);
        Destroy(target);
//        selected = construct;
    }
    void ClearStair()
    {
        GameObject construct = null;// Instantiate(stairConstruct, selected.transform.position, selected.transform.rotation, transform);
        constructs.Add(construct);
 //       Destroy(selected);
 //       selected = null;
    }
    void SetStair()
    {
        int i = 0;// (int)Mathf.Round(selected.transform.localPosition.x / 6);
        int j = 0;//(int)Mathf.Round(selected.transform.localPosition.z / 6);
        Vector2[] keys = GetStairKeys(i,j);
        Vector3 key = new Vector3(i, j,currentRotation);

        GameObject obj = null;// Instantiate(stair, transform.position + new Vector3(i * 6 * scaleFactor, currentLevel * 4 * scaleFactor, j* 6 * scaleFactor), selected.transform.rotation, transform);
        obj.transform.parent = transform;

        //st[currentLevel][key].state = 1;
        ob[currentLevel][keys[0]] = 1;
        stComplete[currentLevel][key] = obj;

 //       fl[currentLevel + 1][keys[1]] = new TileObject(keys[1],1, currentLevel + 1, obj);

 //       Destroy(selected);
 //       selected = null;

        //surface.BuildNavMesh();
    }
}

/*
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

    // Отключить индикацию здания
    public void SetBuilding(Building b)
    {
        building = b;

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

    public void Show(bool b = true)
    {
        if (state< 1) {
            state = 0;
            if (b)
            {
                UpdateMesh(0);
                home.tempObjects.Add(this);
            }
            else {
                if (obj != null)
                    GameObject.Destroy(obj);
            }
        }
    }
    public void Select(bool b = true)
    {
        if(state==0)
            UpdateMesh(1);
    }
    public virtual void Build(int n=3)
    {
        UpdateMesh(n);
        state = n-2;

        CheckDoorsAllBuildings();
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
        foreach(Building b in home.buildings[level]){
            b.CheckDoors();
        }
    }

    public virtual void Hide(bool revert = true)
    {
        bool act = !revert;
        if (obj!=null)obj.SetActive(act);
    }

    public virtual void HideHalf(bool revert = true)
    {
        bool act = !revert;

        if (obj == null) return;
        if (obj.transform.GetChild(0) == null) return;
        if (obj.transform.GetChild(0).GetChild(0) == null) return;
        obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(act);
        if (obj.transform.GetChild(0).GetChild(1) == null) return;
        obj.transform.GetChild(0).GetChild(1).gameObject.SetActive(!act);
    }
}

public class TileObject:ConstructObject
{
    GameObject roof;
    public bool isLadderHole { get; protected set; }
    public bool isLadderExit { get; protected set; }
    public bool busy { get; protected set; }
    public bool Busy { set { busy = value; }}

    Dictionary<Vector3, WallObject> walls = new Dictionary<Vector3, WallObject>();

    public TileObject(Vector3 inKey, int inLevel, GameObject gObj = null) : base(inKey, inLevel, gObj)
    {
        prefs = home.tempFabs[0];
        if(level>0) UpdateRoofMesh(4);
    }
    public override void Build(int n=2)
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
                if (t2.state == 1)
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
        if(level>0 && roof != null)
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
                                                          (level-1) * 4 * home.scaleFactor,
                                                          key.y * 6 * home.scaleFactor),
                        Quaternion.identity,
                        home.transform);
        roof.transform.parent = home.gameObject.transform;
        roof.GetComponent<ConstructElement>().construct = this;

    }

    public void StairUnderTile(StairObject stair)
    {
        Debug.Log("UP "+key);
        state = 2;
        ConnectBuildAndWallsAround();
        RemoveWalls();
        RemoveRoof();
        int oppositeStairRotation = RotateBy90(RotateBy90(stair.key.z,1), 1);
        SetWallDoorOnly(oppositeStairRotation/90);
    }
    public void SetWallDoorOnly(int num)
    {
        Vector2[] keys = new Vector2[4] {new Vector3(key.x-1,key.y,0),
                                         new Vector3(key.x,key.y+1,90),
                                         new Vector3(key.x+1,key.y,180),
                                         new Vector3(key.x,key.y-1,270)
                                         };
        if (home.wl[level].ContainsKey(keys[num]))//.SetCanBeDoorOnly();
            home.wl[level][keys[num]].SetCanBeDoorOnly();
        else
            Debug.Log("Bad key ");
    }


    public override void Hide(bool revert = true)
    {
        base.Hide(revert);
        bool act = !revert;
       if (roof != null) roof.SetActive(act);
    }

}

public class WallObject : ConstructObject
{
    public int wallScaler;
    public bool canBeDoor { get; protected set; }
    public bool canBeDoorOnly { get; protected set; }
    public bool isDoor { get; protected set; }
    public List<CornerObject> walls = new List<CornerObject>();


    public WallObject(Vector3 inKey, int inLevel, GameObject gObj = null) : base(inKey, inLevel, gObj)
    {
        prefs = home.tempFabs[1];
        CheckCanBeDoor();
    }

    public void CheckCanBeDoor()
    {
        if(canBeDoorOnly)
        {
            canBeDoor = true;
            return;
        }
        Vector3 testKey = new Vector3(key.x, key.y, RotateBy90(key.z, 1));
        Vector3 testKey2 = new Vector3(key.x, key.y, RotateBy90(key.z, -1));
        if (CheckNear(testKey) || CheckNear(testKey2)) canBeDoor = false;
        else canBeDoor = true;
    }
    public void SetCanBeDoorOnly()
    {
        canBeDoorOnly = true;
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

    public override void Build(int n = 2)
    {
        int bType = home.buildType;
        base.Build(bType + 2);
        SetBuilding(building);

        Vector3 floorKey = new Vector3(key.x, key.y, 0);
        if (home.fl[level].ContainsKey(floorKey)) home.fl[level][floorKey].Busy = true;

        if (bType == 2)
        {
            isDoor = true;
            disableTilesNearDoor();
        }

        BuildCornersIn();
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
            UpdateMesh(wallScaler + 4);
        }
    }
    public override void UpdateMesh(int n)
    {
        base.UpdateMesh(n);
        obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, key.z, 0);
    }

    public override void Hide(bool revert = true)
    {
        base.Hide(revert);
    }
    public override void HideHalf(bool revert = true)
    {
        if (key.z == 0 || key.z == 90) return;
        if (isDoor) return;
        base.HideHalf(revert);
    }
}

public class CornerObject
{
    protected HomeConstructor home;
    protected TempPrefabsArray prefs;
    public Vector3 key;
    public int level { get; protected set; }
    public GameObject obj;
    public Building building { get; protected set; }
    public List<WallObject> walls=new List<WallObject>();

    public CornerObject(Vector3 inKey,bool outer)
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


public class Building
{
    HomeConstructor home;

    int level;
    public bool isolated{ get; private set; }
    public bool hasDoor{ get; private set; }
    public WallObject lastDoor { get; private set; }

    List<ConstructObject> tiles = new List<ConstructObject>();
    List<ConstructObject> walls = new List<ConstructObject>();
    List<ConstructObject> doors = new List<ConstructObject>();
    List<ConstructObject> corners = new List<ConstructObject>();
    List<ConstructObject> stairs = new List<ConstructObject>();

    public Color color{ get; private set; }

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

    public List<ConstructObject> GetTiles(){return tiles;}
    public List<ConstructObject> GetWalls(){return walls;}
    public List<ConstructObject> GetCorners() { return corners; }
    public List<ConstructObject> GetStairs() { return stairs; }

    public void Connect(Building b)
    {
        foreach(TileObject t in b.GetTiles())
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
        WallObject lastDoor2=null;
        int i = 0;
        foreach (WallObject w in walls)
        {
            w.CheckCanBeDoor();
            if(w.state <1)
            {
                if (w.canBeDoor)
                {
                    canBeDoorsCount++;
                    lastDoor2 = w;
                }
                emptyWalls++;
            }else
                if (w.isDoor) hasDoor = true;
            i++;
        }

        if (emptyWalls == 0) Isolate();

        if (!hasDoor)
        {
            if (canBeDoorsCount == 1)
            {
                lastDoor = lastDoor2;
                if (HomeConstructor.instance.buildType == 1) lastDoor.SetRed();
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
        foreach (TileObject t in home.fl[level].Values)
        {
            if (t.state == 1) home.fl[level + 1].Add(t.key,new TileObject(t.key,level+1));
        }
    }
    void CreateStairPositions()
    {
        foreach (TileObject t in home.fl[level].Values)
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
                for(i=0; i < 4; i++)
                {
                    Vector3 keyz2 = key2[i];
                    keyz2.z = 0;
                    Vector3 keyz3 = key3[i];
                    keyz3.z = 0;
                    Vector3 keyStair = t.key;
                    keyStair.z = key2[i].z;
                    if (home.fl[level].ContainsKey(keyz2) && home.fl[level][keyz2].state == 1)
                        if (home.fl[level].ContainsKey(keyz3) && home.fl[level][keyz3].state == 1)
                        {
                            StairObject sObj = new StairObject(keyStair, level);
                            //sObj.SetBuilding(this);
                            AddStair(sObj);
                            home.st[level][keyStair]= sObj;
                        }
                            
                }
            }
            /*
                foreach (Vector3 k in key2)
                {
                    Vector3 keyz = k;
                    keyz.z = 0;
                    Vector3 keyStair = t.key;
                    keyStair.z = k.z;
                    if (home.fl[level].ContainsKey(keyz) && home.fl[level][keyz].state == 1)
                        if (home.fl[level].ContainsKey(keyz) && home.fl[level][keyz].state == 1)
                            home.st[level].Add(keyStair, new StairObject(keyStair, level));
                }
                //*/
/*
        }
        foreach (WallObject w in home.wl[level].Values)
        {
            if (w.state == 1 && w.isDoor == false && w.canBeDoor == true)
                home.st[level].Add(w.key, new StairObject(w.key, level));
        }
    }
    
}

//*/


public enum BuildType { Floor,Wall,OnFloor};
public enum WallType { Wall,SmallWindow,BigWindow,Door};
public enum TileType { Floor,Stair,Turret};

[System.Serializable]
public class TempPrefabsArray
{
    public GameObject[] fab;

}
/*
public class TempObject
{
    GameObject go;
    GameObject prefab;

    Vector3 key;
    int level;
    int type;
    public TempObject(Vector3 inKey,int currentLevel,int inType, GameObject home)
    {
        prefab = home.GetComponent<HomeConstructor>().tempFabs[type].fab[0];
        float scaleFactor = home.GetComponent<HomeConstructor>().scaleFactor;

        key = inKey;
        level = currentLevel;
        type = inType;

        go = GameObject.Instantiate(prefab, home.transform.position + new Vector3(key.x * 6 * scaleFactor, level * 4 * scaleFactor, key.y * 6 * scaleFactor), Quaternion.identity, home.transform);
    }
}
public class TempFloor:TempObject
{
    public TempFloor(Vector3 key,int currentLevel,int type, GameObject home): base(key,currentLevel,type, home)
    {  }
}
public class Temp
{
    HomeConstructor home;
    int level;
    int type;
    List<GameObject> objs = new List<GameObject>();
    public Temp()
    {
        home=HomeConstructor.instance;
    }

    public void ShowFloors()
    {
        foreach (TileObject t in home.fl[level].Values)
        {

        }
    }
    void MakeObject(Vector3 key)
    {
        TempObject o = new TempFloor(key, level, type, home.gameObject);
    }

    public void Clear()
    {
        foreach (GameObject o in objs)
        {
            GameObject.Destroy(o);
        }
        objs.Clear();
    }
    public void SetLevel(int n)
    {
        level = n;
    }
    public void SetType(int n)
    {
        type = n;
    }
    
}
//*/
