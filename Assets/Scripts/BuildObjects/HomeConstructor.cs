using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HomeConstructor : MonoBehaviour {

    //List<Vector3> availablePoints = new List<Vector3>();
   
    public NavMeshSurface surface;
    public NavMeshModifierVolume modifier;
    
    Dictionary<Vector2, int>[] ob = new Dictionary<Vector2, int>[3] { new Dictionary<Vector2, int>(), new Dictionary<Vector2, int>(), new Dictionary<Vector2, int>()};
    Dictionary<Vector2, int>[] fl = new Dictionary<Vector2, int>[3] { new Dictionary<Vector2, int>(), new Dictionary<Vector2, int>(), new Dictionary<Vector2, int>() };
    Dictionary<Vector3, int>[] wl = new Dictionary<Vector3, int>[3] { new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>()};
    Dictionary<Vector3, int>[] cr = new Dictionary<Vector3, int>[3] { new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>() };
    Dictionary<Vector3, int>[] st = new Dictionary<Vector3, int>[3]{ new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(), new Dictionary<Vector3, int>() };
    Dictionary<Vector3, int>[] wlCuts = new Dictionary<Vector3, int>[3] { new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>(),new Dictionary<Vector3, int>()};

    Dictionary<Vector2, GameObject>[] flComplete = new Dictionary<Vector2, GameObject>[3] { new Dictionary<Vector2, GameObject>(), new Dictionary<Vector2, GameObject>(), new Dictionary<Vector2, GameObject>()};
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
    
    GameObject selected;
    int buildType;
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
                fl[currentLevel][new Vector2(i, j)] = 0;
                //availablePoints.Add(new Vector3(startX+i, 0, startY+j));
            }
        }
    }

    void ShowAvailableFloors()
    {
        RemoveAllPoints();
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 key=new Vector2(i, j);
                if (fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 0)
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
    }
    void ShowAvailableFloorsHight()
    {
        RemoveAllPoints();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 key = new Vector2(i, j);
                if (fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 0)
                {
                    bool act = true;

                    Vector3 keyWall;
                    for (int k = 0; k < 4; k++)
                    {
                        keyWall = new Vector3(i, j, k * 90);
                        if (wl[currentLevel].ContainsKey(keyWall) && wl[currentLevel][keyWall] > 0) act = false;
                    }
                    if (act == true) act = !IsDoorConnectedToTile(i, j);

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

    bool IsDoorConnectedToTile(int i, int j)
    {
        Vector3[] keys = { new Vector3{x= i + 1,y= j,z= 90 },
                           new Vector3{x= i + 1,y= j,z= 270 },
                           new Vector3{x= i - 1,y= j,z= 90 },
                           new Vector3{x= i - 1,y= j,z= 270 },
                           new Vector3{x= i ,y= j + 1,z= 0 },
                           new Vector3{x= i ,y= j + 1,z= 180 },
                           new Vector3{x= i ,y= j - 1,z= 0 },
                           new Vector3{x= i ,y= j - 1,z= 180 }};
        foreach (Vector3 key in keys)
        {
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] == 4) return true;
        }
        return false;
    }

    void ShowAvailableWalls()
    {
        RemoveAllPoints();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 keyFloor = new Vector2(i, j);
                if (fl[currentLevel].ContainsKey(keyFloor) && fl[currentLevel][keyFloor] == 1)
                {
                    Vector2 key = new Vector2(i + 1, j);
                    Vector3 key2 = new Vector3(i + 1, j,180);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 1))
                        if(!(wl[currentLevel].ContainsKey(key2) && wl[currentLevel][key2]>0))
                            CreateWallConstruct(i + 1, j - 1, 180);

                    key = new Vector2(i, j + 1);
                    key2 = new Vector3(i , j+1 , 90);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 1))
                        if (!(wl[currentLevel].ContainsKey(key2) && wl[currentLevel][key2] > 0))
                            CreateWallConstruct(i + 1, j , 90);


                    key = new Vector2(i-1, j);
                    key2 = new Vector3(i-1 , j , 0);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 1))
                        if (!(wl[currentLevel].ContainsKey(key2) && wl[currentLevel][key2] > 0))
                            CreateWallConstruct(i, j, 0);

                    key = new Vector2(i, j -1);
                    key2 = new Vector3(i , j - 1, 270);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 1))
                        if (!(wl[currentLevel].ContainsKey(key2) && wl[currentLevel][key2] > 0))
                            CreateWallConstruct(i , j - 1, 270);
                    
                }
            }
        }
        
    }
    void ShowAvailableDoors()
    {
        RemoveAllPoints();
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 Floor = new Vector2(i, j);
                if (fl[currentLevel].ContainsKey(Floor) && fl[currentLevel][Floor] == 1)
                {
                    Vector2 key = new Vector2(i + 1, j);
                    Vector2 key2 = new Vector2(i + 1, j+1);
                    Vector2 key3 = new Vector2(i + 1, j-1);
                    Vector3 key4 = new Vector3(i + 1, j,180);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 1) && !(fl[currentLevel].ContainsKey(key2) && fl[currentLevel][key2] == 1) && !(fl[currentLevel].ContainsKey(key3) && fl[currentLevel][key3] == 1))
                        if (!(wl[currentLevel].ContainsKey(key4) && wl[currentLevel][key4] > 0))
                            CreateWallConstruct(i + 1, j - 1, 180);

                    key = new Vector2(i, j+1);
                    key2 = new Vector2(i + 1, j + 1);
                    key3 = new Vector2(i -1, j + 1);
                    key4 = new Vector3(i , j+1, 90);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 1) && !(fl[currentLevel].ContainsKey(key2) && fl[currentLevel][key2] == 1) && !(fl[currentLevel].ContainsKey(key3) && fl[currentLevel][key3] == 1))
                        if (!(wl[currentLevel].ContainsKey(key4) && wl[currentLevel][key4] > 0))
                            CreateWallConstruct(i + 1, j, 90);

                    key = new Vector2(i - 1, j);
                    key2 = new Vector2(i - 1, j + 1);
                    key3 = new Vector2(i - 1, j - 1);
                    key4 = new Vector3(i - 1, j, 0);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 1) && !(fl[currentLevel].ContainsKey(key2) && fl[currentLevel][key2] == 1) && !(fl[currentLevel].ContainsKey(key3) && fl[currentLevel][key3] == 1))
                        if (!(wl[currentLevel].ContainsKey(key4) && wl[currentLevel][key4] > 0))
                            CreateWallConstruct(i, j, 0);

                    key = new Vector2(i, j - 1);
                    key2 = new Vector2(i + 1, j - 1);
                    key3 = new Vector2(i - 1, j - 1);
                    key4 = new Vector3(i , j-1, 270);
                    if (!(fl[currentLevel].ContainsKey(key) && fl[currentLevel][key] == 1) && !(fl[currentLevel].ContainsKey(key2) && fl[currentLevel][key2] == 1) && !(fl[currentLevel].ContainsKey(key3) && fl[currentLevel][key3] == 1))
                        if (!(wl[currentLevel].ContainsKey(key4) && wl[currentLevel][key4] > 0))
                            CreateWallConstruct(i, j - 1, 270);
                }
            }
        } 
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
                if (!(fl[currentLevel].ContainsKey(keys[0]) && fl[currentLevel][keys[0]] == 1)) act = false;
                if (!(fl[currentLevel].ContainsKey(keys[1]) && fl[currentLevel][keys[1]] == 1)) act = false;

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
        foreach (GameObject cons in constructs)
        {
            Destroy(cons);
        }
        constructs.Clear();
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
            if (selected != null)
            {
                setObj.Invoke();
            }
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
            GameObject parent = hit.collider.gameObject.transform.parent.gameObject;
            if (constructs.Contains(parent))
            {
                if(selectObj!=null) selectObj.Invoke(parent);
            }
        }
    }


    /// ////////                 Floors

    void SelectFloor(GameObject target)
    {
        if (selected != null) ClearFloor();

        constructs.Remove(target);
        GameObject construct = Instantiate(floorConstructFill, target.transform.position, target.transform.rotation, transform);
        Destroy(target);
        selected = construct;
    }
    void ClearFloor()
    {
        GameObject construct = Instantiate(floorConstruct, selected.transform.position, selected.transform.rotation, transform);
        constructs.Add(construct);
        Destroy(selected);
        selected = null;
    }
    void SetFloor()
    {
        int i = (int)Mathf.Round(selected.transform.localPosition.x/6);
        int j = (int)Mathf.Round(selected.transform.localPosition.z/6);
        Vector2 key = new Vector2(i,j);

        GameObject obj = Instantiate(floor, transform.position+ new Vector3(i*6*scaleFactor,currentLevel*4*scaleFactor, j * 6 * scaleFactor), selected.transform.rotation, transform);
        obj.transform.parent = transform;

        fl[currentLevel][key] = 1;
        flComplete[currentLevel][key] = obj;

        Destroy(selected);
        selected = null;

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
            if (!(fl[currentLevel-1].ContainsKey(check) && fl[currentLevel - 1][check]>0)) act = false;
            if (ob[currentLevel-1].ContainsKey(check) && fl[currentLevel - 1][check]==1) act = false;
            if (act)
            {
                fl[currentLevel][check] = 0;
                sumAct = true;
            }
        }
        if (sumAct)
        {
            ShowAvailableFloorsHight();
        }
    }
    
    /// ////////                 WALLS

    void SelectWall(GameObject target)
    {
        if (selected != null) ClearWall();
        
        constructs.Remove(target);
        GameObject construct = Instantiate(wallConstructFill, target.transform.position, target.transform.rotation, transform);
        Destroy(target);
        selected = construct;
    }
    void ClearWall()
    {
        GameObject construct = Instantiate(wallConstruct, selected.transform.position, selected.transform.rotation, transform);
        constructs.Add(construct);
        Destroy(selected);
        selected = null;
    }
    void SetWall()
    {
        Vector3 key=new Vector3();
        int i = (int)Mathf.Round(selected.transform.localPosition.x / 6);
        int j = (int)Mathf.Round(selected.transform.localPosition.z / 6);
        
        float angle = selected.transform.localRotation.eulerAngles.y;
        if (angle == 270 || angle==270)
            key = new Vector3(i, j, 270);
        else if (angle == 0)
            key = new Vector3(i - 1, j, 0);
        else if (angle == 90)
            key = new Vector3(i - 1, j+1, 90);
        else if (angle == 180)
            key = new Vector3(i , j+1, 180);
        

        Debug.Log("Wall "+key);

        GameObject obj = Instantiate(Wall, transform.position + new Vector3(i * 6 * scaleFactor, currentLevel * 4 * scaleFactor, j * 6 * scaleFactor), selected.transform.localRotation, transform);
        obj.transform.parent = transform;


        wl[currentLevel][key]= 1;
        wlComplete[currentLevel][key] = obj;

        Destroy(selected);
        selected = null;

        ConstructCorners(key,obj);

        //surface.BuildNavMesh();
    }
    void ConstructCorners(Vector3 wallc, GameObject  obj)
    {
        Vector3 key = new Vector3();
        int wallSkaler = 0;

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
                if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 90));
                }
            }
            
            key = new Vector3(wallc.x, wallc.y, 90);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x, key.y - 1, 270));
                UpdateWallSkaler(key, 2);
            }
            else
            {
                key = new Vector3(wallc.x - 1, wallc.y - 1, 270);
                if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x + 1, key.y, 180));
                }
            }
        }
        if (wallc.z == 270)
        {
            key = new Vector3(wallc.x, wallc.y, 180);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x, key.y, 0));
                UpdateWallSkaler(key, 2);
            }
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
        if (fl[currentLevel].ContainsKey(checkKey2) && fl[currentLevel][checkKey2] > 0) return;

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


    ///////////////////////  DOORS 
    void SelectDoor(GameObject target)
    {
        if (selected != null) ClearDoor();

        constructs.Remove(target);
        GameObject construct = Instantiate(wallConstructFill, target.transform.position, target.transform.rotation, transform);
        Destroy(target);
        selected = construct;
    }
    void ClearDoor()
    {
        GameObject construct = Instantiate(wallConstruct, selected.transform.position, selected.transform.rotation, transform);
        constructs.Add(construct);
        Destroy(selected);
        selected = null;
    }
    void SetDoor()
    {
        Vector3 key = new Vector3();
        int i = (int)Mathf.Round(selected.transform.localPosition.x / 6);
        int j = (int)Mathf.Round(selected.transform.localPosition.z / 6);

        float angle = selected.transform.localRotation.eulerAngles.y;
        if (angle == 270 || angle == 270)
            key = new Vector3(i, j, 270);
        else if (angle == 0)
            key = new Vector3(i - 1, j, 0);
        else if (angle == 90)
            key = new Vector3(i - 1, j + 1, 90);
        else if (angle == 180)
            key = new Vector3(i, j + 1, 180);


        Debug.Log("door "+key);

        GameObject obj = Instantiate(door, transform.position + new Vector3(i * 6 * scaleFactor, currentLevel * 4 * scaleFactor, j * 6 * scaleFactor), selected.transform.localRotation, transform);
        obj.transform.parent = transform;

        wl[currentLevel].Add(key, 4);
        wlComplete[currentLevel][key] = obj;

        /*if (wlComplete.ContainsKey(key) && wlComplete[key] != null)
            wlComplete[currentLevel][key] = obj;
        else
            wlComplete.Add(key, obj);
            //*/

        Destroy(selected);
        selected = null;

        ConstructCornersDoors(key, obj);

       //surface.BuildNavMesh();
    }
    void ConstructCornersDoors(Vector3 wallc, GameObject obj)
    {
        Vector3 key = new Vector3();
        int wallSkaler = 0;
        if (wallc.z == 0)
        {
            key = new Vector3(wallc.x + 1, wallc.y + 1, 90);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] >0)
            {
                CreateOutCorner(new Vector3(key.x, key.y - 1, 0));
            }
            key = new Vector3(wallc.x + 1, wallc.y - 1, 270);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x, key.y, 270));
            }
        }
        if (wallc.z == 90)
        {
            key = new Vector3(wallc.x - 1, wallc.y - 1, 0);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y, 0));
            }
            key = new Vector3(wallc.x + 1, wallc.y - 1, 180);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x, key.y, 90));
            }
        }

        if (wallc.z == 180)
        {
            key = new Vector3(wallc.x - 1, wallc.y + 1, 90);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 90));
            }
            key = new Vector3(wallc.x - 1, wallc.y - 1, 270);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y, 180));
            }
        }
        if (wallc.z == 270)
        {
            key = new Vector3(wallc.x - 1, wallc.y + 1, 0);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 270));
            }
            key = new Vector3(wallc.x + 1, wallc.y + 1, 180);
            if (wl[currentLevel].ContainsKey(key) && wl[currentLevel][key] > 0)
            {
                CreateOutCorner(new Vector3(key.x, key.y - 1, 180));
            }
        }

        
    }

    ///////               STAIRS 
    void SelectStair(GameObject target)
    {
        if (selected != null) ClearStair();

        constructs.Remove(target);
        GameObject construct = Instantiate(stairConstructFill, target.transform.position, target.transform.rotation, transform);
        Destroy(target);
        selected = construct;
    }
    void ClearStair()
    {
        GameObject construct = Instantiate(stairConstruct, selected.transform.position, selected.transform.rotation, transform);
        constructs.Add(construct);
        Destroy(selected);
        selected = null;
    }
    void SetStair()
    {
        int i = (int)Mathf.Round(selected.transform.localPosition.x / 6);
        int j = (int)Mathf.Round(selected.transform.localPosition.z / 6);
        Vector2[] keys = GetStairKeys(i,j);
        Vector3 key = new Vector3(i, j,currentRotation);

        GameObject obj = Instantiate(stair, transform.position + new Vector3(i * 6 * scaleFactor, currentLevel * 4 * scaleFactor, j* 6 * scaleFactor), selected.transform.rotation, transform);
        obj.transform.parent = transform;

        st[currentLevel][key] = 1;
        //ob.Add(key, 1);
        //stComplete.Add(key, obj);
        ob[currentLevel][keys[0]] = 1;
        stComplete[currentLevel][key] = obj;

        fl[currentLevel + 1][keys[1]] = 0;

        Destroy(selected);
        selected = null;

        //surface.BuildNavMesh();
    }
}

