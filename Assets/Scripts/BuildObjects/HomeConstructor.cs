using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeConstructor : MonoBehaviour {

    //List<Vector3> availablePoints = new List<Vector3>();
   
    Dictionary<Vector2, int> fl = new Dictionary<Vector2, int>();
    Dictionary<Vector3, int> wl = new Dictionary<Vector3, int>();
    Dictionary<Vector3, int> cr = new Dictionary<Vector3, int>();
    Dictionary<Vector3, int> st = new Dictionary<Vector3, int>();

    Dictionary<Vector3, int> wlCuts = new Dictionary<Vector3, int>();


    // List<Vector3> availablePoints2=new List<Vector3>();
    //List<Vector3> availablePoints3=new List<Vector3>();
    List<GameObject> constructs = new List<GameObject>();
    
    Dictionary<Vector2, GameObject> flComplete = new Dictionary<Vector2, GameObject>();
    Dictionary<Vector3, GameObject> wlComplete = new Dictionary<Vector3, GameObject>();
    Dictionary<Vector3, GameObject> crComplete = new Dictionary<Vector3, GameObject>();
    Dictionary<Vector3, GameObject> stComplete = new Dictionary<Vector3, GameObject>();

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
    
    GameObject selected;

    delegate void SelectObj(GameObject target);
    SelectObj selectObj;
    delegate void SetObj();
    SetObj setObj;
    delegate void ClearObj();
    ClearObj clearObj;

    void Start () {
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        SetAvailablePoints(4,4,-2,-1);

        ShowAvailableFloors(0);
	}

    void SetAvailablePoints(int x,int y,int startX,int startY)
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                fl[new Vector2(i, j)] = 0;
                //availablePoints.Add(new Vector3(startX+i, 0, startY+j));
            }
        }
    }

    void ShowAvailableFloors(int level)
    {
        RemoveAllPoints();
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (fl[new Vector2(i, j)] == 0)
                {
                    bool act = true;

                    Vector3 keyWall;
                    for (int k = 0; k < 4; k++)
                    {
                        keyWall = new Vector3(i, j, k * 90);
                        if (wl.ContainsKey(keyWall) && wl[keyWall] >0) act = false;
                    }
                    if(act==true) act = !IsDoorConnectedToTile(i, j);
                    
                    if (act)
                    {
                        GameObject construct = Instantiate(floorConstruct, transform.position + new Vector3(i * 6 * scaleFactor, 0, j * 6 * scaleFactor), Quaternion.identity, transform);
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
            if (wl.ContainsKey(key) && wl[key] == 4) return true;
        }
        return false;
    }

    void ShowAvailableWalls(int level)
    {
        RemoveAllPoints();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (fl[new Vector2(i, j)] == 1)
                {
                    Vector2 key = new Vector2(i + 1, j);
                    Vector3 key2 = new Vector3(i + 1, j,180);
                    if (!(fl.ContainsKey(key) && fl[key] == 1))
                        if(!(wl.ContainsKey(key2) && wl[key2]>0))
                            CreateWallConstruct(i + 1, j - 1, 180);

                    key = new Vector2(i, j + 1);
                    key2 = new Vector3(i , j+1 , 90);
                    if (!(fl.ContainsKey(key) && fl[key] == 1))
                        if (!(wl.ContainsKey(key2) && wl[key2] > 0))
                            CreateWallConstruct(i + 1, j , 90);


                    key = new Vector2(i-1, j);
                    key2 = new Vector3(i-1 , j , 0);
                    if (!(fl.ContainsKey(key) && fl[key] == 1))
                        if (!(wl.ContainsKey(key2) && wl[key2] > 0))
                            CreateWallConstruct(i, j, 0);

                    key = new Vector2(i, j -1);
                    key2 = new Vector3(i , j - 1, 270);
                    if (!(fl.ContainsKey(key) && fl[key] == 1))
                        if (!(wl.ContainsKey(key2) && wl[key2] > 0))
                            CreateWallConstruct(i , j - 1, 270);
                    
                }
            }
        }
        
    }
    void ShowAvailableDoors(int level)
    {
        RemoveAllPoints();
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (fl[new Vector2(i, j)] == 1)
                {
                    Vector2 key = new Vector2(i + 1, j);
                    Vector2 key2 = new Vector2(i + 1, j+1);
                    Vector2 key3 = new Vector2(i + 1, j-1);
                    Vector3 key4 = new Vector3(i + 1, j,180);
                    if (!(fl.ContainsKey(key) && fl[key] == 1) && !(fl.ContainsKey(key2) && fl[key2] == 1) && !(fl.ContainsKey(key3) && fl[key3] == 1))
                        if (!(wl.ContainsKey(key4) && wl[key4] > 0))
                            CreateWallConstruct(i + 1, j - 1, 180);

                    key = new Vector2(i, j+1);
                    key2 = new Vector2(i + 1, j + 1);
                    key3 = new Vector2(i -1, j + 1);
                    key4 = new Vector3(i , j+1, 90);
                    if (!(fl.ContainsKey(key) && fl[key] == 1) && !(fl.ContainsKey(key2) && fl[key2] == 1) && !(fl.ContainsKey(key3) && fl[key3] == 1))
                        if (!(wl.ContainsKey(key4) && wl[key4] > 0))
                            CreateWallConstruct(i + 1, j, 90);

                    key = new Vector2(i - 1, j);
                    key2 = new Vector2(i - 1, j + 1);
                    key3 = new Vector2(i - 1, j - 1);
                    key4 = new Vector3(i - 1, j, 0);
                    if (!(fl.ContainsKey(key) && fl[key] == 1) && !(fl.ContainsKey(key2) && fl[key2] == 1) && !(fl.ContainsKey(key3) && fl[key3] == 1))
                        if (!(wl.ContainsKey(key4) && wl[key4] > 0))
                            CreateWallConstruct(i, j, 0);

                    key = new Vector2(i, j - 1);
                    key2 = new Vector2(i + 1, j - 1);
                    key3 = new Vector2(i - 1, j - 1);
                    key4 = new Vector3(i , j-1, 270);
                    if (!(fl.ContainsKey(key) && fl[key] == 1) && !(fl.ContainsKey(key2) && fl[key2] == 1) && !(fl.ContainsKey(key3) && fl[key3] == 1))
                        if (!(wl.ContainsKey(key4) && wl[key4] > 0))
                            CreateWallConstruct(i, j - 1, 270);
                }
            }
        } 
    }
    void ShowAvailableStairs(int level)
    {
        RemoveAllPoints();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (fl[new Vector2(i, j)] == 1)
                {
                    GameObject construct = Instantiate(stairConstruct, transform.position + new Vector3(i * 6 * scaleFactor, 0, j * 6 * scaleFactor), Quaternion.identity, transform);
                    construct.transform.parent = transform;
                    constructs.Add(construct);
                }
            }
        }
    }

    void CreateWallConstruct(int i, int j, int turn)
    {
        GameObject construct = Instantiate(wallConstruct, transform.position + new Vector3((i) * 6 * scaleFactor, 0, (j) * 6 * scaleFactor), Quaternion.Euler(0, turn, 0), transform);
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

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ShowAvailableStairs(0);
            selectObj = SelectStair;
            clearObj = ClearStair;
            setObj = SetStair;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ShowAvailableDoors(0);
            selectObj = SelectDoor;
            clearObj = ClearDoor;
            setObj = SetDoor;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ShowAvailableWalls(0);
            selectObj = SelectWall;
            clearObj = ClearWall;
            setObj = SetWall;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShowAvailableFloors(0);
            selectObj = SelectFloor;
            clearObj = ClearFloor;
            setObj = SetFloor;
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
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            GetCLickObject();
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
        int i = (int)selected.transform.localPosition.x/6;
        int j = (int)selected.transform.localPosition.z/6;
        Vector2 key = new Vector2(i,j);

        GameObject obj = Instantiate(floor, transform.position+ new Vector3(i*6*scaleFactor,0, j * 6 * scaleFactor), selected.transform.rotation, transform);
        obj.transform.parent = transform;

        fl[key] = 1;
        flComplete.Add(key, obj);

        Destroy(selected);
        selected = null;
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
        int i = (int)selected.transform.localPosition.x / 6;
        int j = (int)selected.transform.localPosition.z / 6;
        
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

        GameObject obj = Instantiate(Wall, transform.position + new Vector3(i * 6 * scaleFactor, 0, j * 6 * scaleFactor), selected.transform.localRotation, transform);
        obj.transform.parent = transform;


        wl.Add(key, 1);
        if(wlComplete.ContainsKey(key) && wlComplete[key]!=null)
            wlComplete[key]= obj;
        else
            wlComplete.Add(key, obj);

        Destroy(selected);
        selected = null;

        ConstructCorners(key,obj);
    }
    void ConstructCorners(Vector3 wallc, GameObject  obj)
    {
        Vector3 key = new Vector3();
        int wallSkaler = 0;

        if (wallc.z == 0)
        {
            key = new Vector3(wallc.x, wallc.y, 270);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x + 1, key.y, 90));
                UpdateWallSkaler(key, 2);
            }
            else
            {
                key = new Vector3(wallc.x + 1, wallc.y + 1, 90);
                if (wl.ContainsKey(key) && wl[key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x, key.y - 1, 0));
                }
            }

            key = new Vector3(wallc.x, wallc.y, 90);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                wallSkaler += 2;
                CreateInCorner(new Vector3(key.x + 1, key.y - 1, 180));
                UpdateWallSkaler(key, 1);
            }
            else
            {
                key = new Vector3(wallc.x + 1, wallc.y - 1, 270);
                if (wl.ContainsKey(key) && wl[key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x, key.y, 270));
                }
            }
        }
        if (wallc.z == 90)
        {
            key = new Vector3(wallc.x, wallc.y, 180);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                wallSkaler += 2;
                CreateInCorner(new Vector3(key.x, key.y - 1, 270));
                UpdateWallSkaler(key, 1);
            }
            else
            {
                key = new Vector3(wallc.x - 1, wallc.y - 1, 0);
                if (wl.ContainsKey(key) && wl[key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x + 1, key.y, 0));
                }
            }
            
            key = new Vector3(wallc.x, wallc.y, 0);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x + 1, key.y - 1, 180));
                UpdateWallSkaler(key, 2);
            }
            else
            {
                key = new Vector3(wallc.x + 1, wallc.y - 1, 180);
                if (wl.ContainsKey(key) && wl[key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x, key.y, 90));
                }
            }
        }

        if (wallc.z == 180)
        {
            key = new Vector3(wallc.x, wallc.y, 270);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                wallSkaler += 2;
                CreateInCorner(new Vector3(key.x, key.y, 0));
                UpdateWallSkaler(key, 1);
            }
            else
            {
                key = new Vector3(wallc.x - 1, wallc.y + 1, 90);
                if (wl.ContainsKey(key) && wl[key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 90));
                }
            }
            
            key = new Vector3(wallc.x, wallc.y, 90);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x, key.y - 1, 270));
                UpdateWallSkaler(key, 2);
            }
            else
            {
                key = new Vector3(wallc.x - 1, wallc.y - 1, 270);
                if (wl.ContainsKey(key) && wl[key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x + 1, key.y, 180));
                }
            }
        }
        if (wallc.z == 270)
        {
            key = new Vector3(wallc.x, wallc.y, 180);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                wallSkaler += 1;
                CreateInCorner(new Vector3(key.x, key.y, 0));
                UpdateWallSkaler(key, 2);
            }
            else
            {
                key = new Vector3(wallc.x - 1, wallc.y + 1, 0);
                if (wl.ContainsKey(key) && wl[key] > 0)
                {
                    CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 270));
                }
            }

            key = new Vector3(wallc.x, wallc.y, 0);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                wallSkaler += 2;
                CreateInCorner(new Vector3(key.x + 1, key.y, 90));
                UpdateWallSkaler(key, 1);
            }
            else
            {
                key = new Vector3(wallc.x + 1, wallc.y + 1, 180);
                 if (wl.ContainsKey(key)) Debug.Log("keys "+ wl[key]);
                if (wl.ContainsKey(key) && wl[key] > 0)
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
        if (wlComplete.ContainsKey(wallc) && wlComplete[wallc]!=null)
        {
             GameObject obj = wlComplete[wallc];

            int prevScaler = 0;
            if(wlCuts.ContainsKey(wallc)) prevScaler = wlCuts[wallc];

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

            wlComplete[wallc] = updObject;

            if (wlCuts.ContainsKey(wallc))
                wlCuts[wallc] = skaler;
            else
                wlCuts.Add(wallc, skaler);
        }
    }

    void CreateOutCorner(Vector3 key)
    {
        Debug.Log("corn create " + key + " .. " + cr.ContainsKey(key));
        if (cr.ContainsKey(key)) Debug.Log("corn 22 " + cr[key]);

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
        if (fl.ContainsKey(checkKey2) && fl[checkKey2] > 0) return;

        if (!(cr.ContainsKey(key) && cr[key]==1))
        {
            cr.Add(key, 1);
            Instantiate(outCorner, transform.position+new Vector3(key.x * 6 * scaleFactor, 0, key.y * 6 * scaleFactor), Quaternion.Euler(0,key.z,0), transform);
        }
    }
    void CreateInCorner(Vector3 key)
    {
        if (!(cr.ContainsKey(key) && cr[key] == 1))
        {
            cr.Add(key, 1);
            Instantiate(inCorner, transform.position + new Vector3(key.x * 6 * scaleFactor, 0, key.y * 6 * scaleFactor), Quaternion.Euler(0, key.z, 0), transform);
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
        int i = (int)selected.transform.localPosition.x / 6;
        int j = (int)selected.transform.localPosition.z / 6;

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

        GameObject obj = Instantiate(door, transform.position + new Vector3(i * 6 * scaleFactor, 0, j * 6 * scaleFactor), selected.transform.localRotation, transform);
        obj.transform.parent = transform;

        wl.Add(key, 4);
        if (wlComplete.ContainsKey(key) && wlComplete[key] != null)
            wlComplete[key] = obj;
        else
            wlComplete.Add(key, obj);

        Destroy(selected);
        selected = null;

        ConstructCornersDoors(key, obj);
    }
    void ConstructCornersDoors(Vector3 wallc, GameObject obj)
    {
        Vector3 key = new Vector3();
        int wallSkaler = 0;
        if (wallc.z == 0)
        {
            key = new Vector3(wallc.x + 1, wallc.y + 1, 90);
            if (wl.ContainsKey(key) && wl[key] >0)
            {
                CreateOutCorner(new Vector3(key.x, key.y - 1, 0));
            }
            key = new Vector3(wallc.x + 1, wallc.y - 1, 270);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                CreateOutCorner(new Vector3(key.x, key.y, 270));
            }
        }
        if (wallc.z == 90)
        {
            key = new Vector3(wallc.x - 1, wallc.y - 1, 0);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y, 0));
            }
            key = new Vector3(wallc.x + 1, wallc.y - 1, 180);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                CreateOutCorner(new Vector3(key.x, key.y, 90));
            }
        }

        if (wallc.z == 180)
        {
            key = new Vector3(wallc.x - 1, wallc.y + 1, 90);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 90));
            }
            key = new Vector3(wallc.x - 1, wallc.y - 1, 270);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y, 180));
            }
        }
        if (wallc.z == 270)
        {
            key = new Vector3(wallc.x - 1, wallc.y + 1, 0);
            if (wl.ContainsKey(key) && wl[key] > 0)
            {
                CreateOutCorner(new Vector3(key.x + 1, key.y - 1, 270));
            }
            key = new Vector3(wallc.x + 1, wallc.y + 1, 180);
            if (wl.ContainsKey(key) && wl[key] > 0)
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
        int i = (int)selected.transform.localPosition.x / 6;
        int j = (int)selected.transform.localPosition.z / 6;
        Vector2 key = new Vector2(i, j);

        GameObject obj = Instantiate(stair, transform.position + new Vector3(i * 6 * scaleFactor, 0, j * 6 * scaleFactor), selected.transform.rotation, transform);
        obj.transform.parent = transform;

        st[key] = 1;
        stComplete.Add(key, obj);

        Destroy(selected);
        selected = null;
    }
}

