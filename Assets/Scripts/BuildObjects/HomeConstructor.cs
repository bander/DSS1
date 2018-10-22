using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public NavMeshSurface surface;
    public NavMeshModifierVolume modifier;
    
    Dictionary<Vector2, int>[] ob = new Dictionary<Vector2, int>[3] { new Dictionary<Vector2, int>(), new Dictionary<Vector2, int>(), new Dictionary<Vector2, int>()};
    public Dictionary<Vector2, TileObject>[] fl = new Dictionary<Vector2, TileObject>[3] { new Dictionary<Vector2, TileObject>(), new Dictionary<Vector2, TileObject>(), new Dictionary<Vector2, TileObject>() };
    public Dictionary<Vector3, WallObject>[] wl = new Dictionary<Vector3, WallObject>[3] { new Dictionary<Vector3, WallObject>(),new Dictionary<Vector3, WallObject>(),new Dictionary<Vector3, WallObject>()};
    //Dictionary<Vector3, int>[] wl = new Dictionary<Vector3, int>[3] { new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>()};
    Dictionary<Vector3, int>[] cr = new Dictionary<Vector3, int>[3] { new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>() };
    Dictionary<Vector3, int>[] st = new Dictionary<Vector3, int>[3]{ new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(), new Dictionary<Vector3, int>() };
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
    public int buildType { get; protected set; }
    int currentLevel;

    delegate void SelectObj(GameObject target);
    SelectObj selectObj;
    delegate void SetObj();
    SetObj setObj;
    delegate void ClearObj();
    ClearObj clearObj;

    void Start () {

        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        SetAvailablePoints(4,4,-2,-1);

        ShowAvailableFloors();
        selectObj = SelectFloor;
        clearObj = ClearFloor;
        setObj = SetFloor;

        Destroy(modifier);
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
            }
        }
        foreach (Building b in buildings[currentLevel])
        {
            Debug.Log("biff");
            if (b.lastDoor!=null)
            {
                Debug.Log("IGG ");
                b.lastDoor.SetRed();
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
    void ShowAvailableStairs()
    {
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

    void SetBuildType(int n)
    {
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
        buildType = n;
    }
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SetBuildType(3);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetBuildType(2);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SetBuildType(1);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetBuildType(0);
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentLevel = 0;
            SetBuildType(buildType);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentLevel = 1;
            SetBuildType(buildType);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentLevel = 2;
            SetBuildType(buildType);
        }


        if (Input.GetMouseButtonDown(0))
        {
            GetCLickObject();
        }
    }

    void BuildObject()
    {
        if (select != null)
        {
            select.Build();
            select = null;

            //setObj.Invoke();
            //CheckTileConnectToBuilding(select);
        }
    }

    void RotatePositive(bool plus=true)
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
    }

    void GetCLickObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,100))
        {
            ConstructElement parent = hit.collider.transform.parent.GetComponent<ConstructElement>();
            if(parent==null) if(hit.collider.transform.parent.parent!=null) parent = hit.collider.transform.parent.parent.GetComponent<ConstructElement>();
            if (parent == null) return;

                if (parent.construct.state==0)
                {
                if (select != null)
                {
                    select.Show();
                }
                parent.construct.Select();
                select = parent.construct;
            }
        }
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
    }
    void CreateInCorner(Vector3 key)
    {
        if (!(cr[currentLevel].ContainsKey(key) && cr[currentLevel][key] == 1))
        {
            cr[currentLevel].Add(key, 1);
            Instantiate(inCorner, transform.position + new Vector3(key.x * 6 * scaleFactor, currentLevel * 4 * scaleFactor, key.y * 6 * scaleFactor), Quaternion.Euler(0, key.z, 0), transform);
        }
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

        st[currentLevel][key] = 1;
        ob[currentLevel][keys[0]] = 1;
        stComplete[currentLevel][key] = obj;

 //       fl[currentLevel + 1][keys[1]] = new TileObject(keys[1],1, currentLevel + 1, obj);

 //       Destroy(selected);
 //       selected = null;

        //surface.BuildNavMesh();
    }
}

public class ConstructObject
{
    protected HomeConstructor home;
    protected TempPrefabsArray prefs;
    public Vector3 key;
    public int state { get; protected set; }
    public int level { get; protected set; }
    public GameObject obj;
    public Building building { get; protected set; }

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
}

public class TileObject:ConstructObject
{
    public bool busy { get; protected set; }
    public bool Busy { set { busy = value; }}

    Dictionary<Vector3, WallObject> walls = new Dictionary<Vector3, WallObject>();

    public TileObject(Vector3 inKey, int inLevel, GameObject gObj = null) : base(inKey, inLevel, gObj)
    {
        prefs = home.tempFabs[0];
    }
    public override void Build(int n=2)
    {
        base.Build();

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
                }else
                    finalWallPositions.Add(new Vector3(k.x, k.y, wallZKeys[i]));
            }else
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

        foreach (int z in wallZKeys)
        {
            RemoveWallPosition(new Vector3(key.x, key.y, z));
        }
        foreach (Vector3 w in finalWallPositions)
        {
            MakeWallPosition(w);
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
        if (base.IsEmpty() && !busy) return true;
            return false;
    }
}

public class WallObject:ConstructObject
{
    public bool canBeDoor { get; protected set;}
    public bool isDoor { get; protected set; }
    public List<CornerObject> walls = new List<CornerObject>();


    public WallObject(Vector3 inKey, int inLevel, GameObject gObj = null) : base(inKey, inLevel, gObj)
    {
        prefs = home.tempFabs[1];
        CheckCanBeDoor();
    }
    void CheckCanBeDoor()
    {
        Vector3 testKey = new Vector3(key.x, key.y, rotate(key.z, 1));
        Vector3 testKey2 = new Vector3(key.x, key.y, rotate(key.z, -1));
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
        if (home.wl[level].ContainsKey(testKey) && home.wl[level][testKey].state>0)
        {
            home.wl[level][testKey].canBeDoor = false;

            return true;
        }
        return false;
    }
    int rotate(float angle,int dir)
    {
        angle += dir * 90;
        if (angle < 0) angle = 270;
        if (angle >270) angle = 0;
        return (int)angle;
    }

    public override void Build(int n=2)
    {
        int bType = home.buildType;
        base.Build(bType+2);
        SetBuilding(building);

        Vector3 floorKey = new Vector3(key.x, key.y, 0);
        if(home.fl[level].ContainsKey(floorKey)) home.fl[level][floorKey].Busy=true;

        if (bType == 2)
        {
            isDoor = true;
            disableTilesNearDoor();
        }
        building.CheckDoors();
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
        Vector3 testKey = new Vector3(key.x, key.y, rotate(key.z, 1));
        Vector3 testKey2 = new Vector3(key.x, key.y, rotate(key.z, -1));
        if (CheckCompleted(testKey))
        {
            CornerObject corn = new CornerObject(testKey, false);
        }
        if (CheckCompleted(testKey2))
        {
            CornerObject corn = new CornerObject(key, false);
        }
    }

    public override void UpdateMesh(int n)
    {
        base.UpdateMesh(n);
        obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, key.z, 0);
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

    public CornerObject(Vector3 key,bool outer)
    {
        home = HomeConstructor.instance;
        prefs = home.tempFabs[3];
        int n = (outer) ? 0 : 1;

        GameObject.Destroy(obj);

        obj = GameObject.Instantiate(prefs.fab[n],
                        home.transform.position + new Vector3(key.x * 6 * home.scaleFactor,
                                                          level * 4 * home.scaleFactor,
                                                          key.y * 6 * home.scaleFactor),
                        Quaternion.identity,
                        home.transform);
        obj.transform.parent = home.gameObject.transform;
        obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, key.z, 0);
    }

}


public class Building
{
    public bool isolated{ get; private set; }
    public bool hasDoor{ get; private set; }
    public WallObject lastDoor { get; private set; }

    List<ConstructObject> tiles = new List<ConstructObject>();
    List<ConstructObject> walls = new List<ConstructObject>();
    List<ConstructObject> doors = new List<ConstructObject>();
    public Color color{ get; private set; }

    public Building(ConstructObject t)
    {
        color = Random.ColorHSV();
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
        CheckDoors();
    }
    public void RemoveWall(ConstructObject t)
    {
        walls.Remove(t);
        CheckDoors();
    }
    public List<ConstructObject> GetTiles()
    {
        return tiles;
    }
    public List<ConstructObject> GetWalls()
    {
        return walls;
    }
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
    }

    public void CheckDoors()
    {
        if (walls.Count < 3) return;

        int canBeDoorsCount = 0;
        int emptyWalls = 0;
        WallObject lastDoor2=null;
        foreach (WallObject w in walls)
        {
            if(w.state <1)
            {
                if (w.canBeDoor)
                {
                    canBeDoorsCount++;
                    lastDoor2 = w;
                }
                emptyWalls++;
            }
            if (w.isDoor) hasDoor = true;
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
                lastDoor = null;
        }
        else
            lastDoor = null;

    }

    void Isolate()
    {
        isolated = true;
        color = Color.red;
        foreach (ConstructObject w in walls)
        {
            w.SetBuilding(this);
        }
        foreach (ConstructObject w in tiles)
        {
            w.SetBuilding(this);
        }
    }
}


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
