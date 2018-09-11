using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildController : MonoBehaviour
{
    #region Singleton
    public static BuildController instance;
    void Awake()
    {
        instance = this;
    }

    #endregion

    CanvasController canvas;
    InvManager manager;

    public GameObject panel;

    public LayerMask layerMask;
    public GameObject tile;
    public GameObject wall;
    public GameObject box;
    public Material green;
    public Material red;
    public Material invis;

    Material finalWall;
    Material finalFloor;
    Material finalBox;
    Material finalLedder;
    Material finalMaterial;


    Material current;
    Vector3 currentV;
    Vector3 wallCorrector=new Vector3(-2,0,1);

    public delegate void OnBuidUpdate();
    public OnBuidUpdate onBuildUpdate;

    float divider = 2;

    float wallRotation = 0;

    GameObject currentTarget;

    List<Vector3> tilesSetted = new List<Vector3>();
    public List<Vector3> AvailableForBuild = new List<Vector3>();

    void Start() {
        canvas = CanvasController.instance;
        manager = InvManager.instance;

        tile.transform.localScale = tile.transform.localScale * 2 / 3;
        finalFloor = tile.GetComponent<MeshRenderer>().material;
        wall.transform.localScale = wall.transform.localScale * 2 / 3;
        finalWall = wall.GetComponent<MeshRenderer>().material;
        wall.GetComponent<BoxCollider>().enabled = false;

        finalBox = box.GetComponent<MeshRenderer>().material;

        tile.transform.position = new Vector3(0, -20, 0);
        wall.transform.position = new Vector3(0, -20, 0);
    }

    public void setFloorState()
    {
        turnOffCurrent();
        currentTarget = tile;
        currentTarget.transform.position = new Vector3(0, 0.1f, 0);
        onBuildUpdate = FloorState;
        finalMaterial = finalFloor;

        if (onBuildUpdate != null) onBuildUpdate.Invoke();
    }
    public void setWallState()
    {
        turnOffCurrent();
        currentTarget = wall;
        currentTarget.transform.position = new Vector3(0, 0.1f, 0);
        onBuildUpdate = WallState;
        finalMaterial = finalWall;

        if (onBuildUpdate != null) onBuildUpdate.Invoke();
    }
    public void setBoxState()
    {
        turnOffCurrent();
        currentTarget = box;
        currentTarget.transform.position = new Vector3(0, 0.1f, 0);
        onBuildUpdate = BoxState;
        finalMaterial = finalBox;

        if (onBuildUpdate != null) onBuildUpdate.Invoke();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            turnOffCurrent();
            currentTarget = tile;
            currentTarget.transform.position = new Vector3(0, 0.1f, 0);
            onBuildUpdate = FloorState;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //turnOffCurrent();
            currentTarget = wall;
            //currentTarget.transform.position = new Vector3(0, 0.1f, 0);
            wallCorrector = new Vector3(2, 0, -1);// -2);
            onBuildUpdate = WallState;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            turnOffCurrent();
            currentTarget = box;
            //currentTarget.transform.position = new Vector3(0, 0.1f, 0);
            //onBuildUpdate = WallState;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            turnOffCurrent();
            onBuildUpdate = null;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            wall.transform.Rotate(new Vector3(0,90,0));

            //wallCorrector = (wall.transform.rotation.eulerAngles.y <100)? new Vector2(0, -2):new Vector2(0, 0); 

            int sw = (int)(wall.transform.rotation.eulerAngles.y / 90);

            switch (sw){
                case 0:
                    wallCorrector = new Vector3(2, 0, -1);// -2);
                    break;
                case 1:
                    wallCorrector = new Vector3(0, 0, -2);
                    break;
                case 2:
                    wallCorrector = new Vector3(0, 0, 0);
                    break;
                case 3:
                    wallCorrector = new Vector3(2, 0, 0);
                    break;
            }
            //wall.transform.position += wallCorrector;
        }

        //if (onBuildUpdate != null) onBuildUpdate.Invoke();

        if (Input.GetMouseButtonDown(0))
        {
            if (onBuildUpdate != null) onBuildUpdate.Invoke();
            //buildTarget();
        }
    }
    
    public void turnOffCurrent()
    {
        if (currentTarget != null && currentTarget.GetComponent<MeshRenderer>()!=null)
        {
            currentTarget.GetComponent<MeshRenderer>().material = invis;
            currentTarget.transform.position = new Vector3(0, -20, 0);
            currentTarget = null;
            onBuildUpdate = null;
        }
    }

    public void BuildTarget()
    {
        if (currentTarget != null)
        {
            if (currentTarget == tile)
            {
                tilesSetted.Add(currentTarget.transform.position);
                Debug.Log(tilesSetted.Count+"  // ");
            }

            if (currentTarget == box)
            {
                manager.spendItems(Item.ItemType.Metall, 5);
            }
            else
            {
                manager.spendItems(Item.ItemType.Metall, 1);
            }

            GameObject newObject = GameObject.Instantiate(currentTarget, currentTarget.transform.position , currentTarget.transform.rotation);
            newObject.GetComponent<MeshRenderer>().material = finalMaterial;

            if (finalMaterial != finalFloor) newObject.GetComponent<BoxCollider>().enabled = true;

            currentTarget.GetComponent<MeshRenderer>().material = red;

            canvas.showBuildPanel(false);
        }
    }
    public void RotateTarget()
    {
        wall.transform.Rotate(new Vector3(0, 90, 0));

        int sw = (int)(wall.transform.rotation.eulerAngles.y / 90);
        switch (sw) { 
            case 0:
                wall.transform.position += new Vector3(-1, 0, -1);//(0, 0, -2);
                wallCorrector = new Vector3(-1, 0, 0);// -2);
                break;
            case 1:
                wall.transform.position += new Vector3(-1,0, 1);//(-2,0, 0);
                wallCorrector = new Vector3(-2, 0, 1);
                break;
            case 2:
                wall.transform.position += new Vector3(1, 0, 1);//(0, 0, 2);
                wallCorrector = new Vector3(-1, 0, 2);
                break;
            case 3:
                wall.transform.position += new Vector3(1, 0, -1);//(2, 0, 0);
                wallCorrector = new Vector3(0, 0, 1);
                break;
        }
        
    }
    public void removeTarget()
    {
        if (currentTarget == tile)
        {
            removeTile();
        }
        if (currentTarget == wall)
        {

        }
    }

    void removeTile()
    {
        Vector3 v4 = tile.transform.position;
        if (tilesSetted.Contains(v4)) tilesSetted.Remove(v4);
        foreach (GameObject tileObj in GameObject.FindGameObjectsWithTag("Floor"))
        {
            if (tileObj.transform.position == v4 && tileObj!=tile)
            {
                GameObject.Destroy(tileObj);
                ShowFloor();   
                return;
            }
        }
    }
    void removeWall()
    {

    }
    void removeBox()
    {

    }

    void FloorState() {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            ShowFloor();
        }

    }
    void ShowFloor() { 

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 v=Vector2.zero;

        if (Physics.Raycast(ray,out hit,100, layerMask))
        {

            float vx = Mathf.Round((hit.point.x/divider)+0.5f)* divider;
            float vz = Mathf.Round((hit.point.z/ divider)- 0.5f) * divider;
            v = new Vector3(vx,tile.transform.position.y, vz);
            
            if (v != currentV)
            {
                tile.transform.position =  v;
                currentV = v;

                float rtx = -panel.GetComponent<RectTransform>().rect.width /4;
                float rty = panel.GetComponent<RectTransform>().rect.height;
                panel.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position) +new Vector3(rtx,rty,0);
            }
            
        }

        bool canBuild = false;
        bool isset = true;
        int i = 0;
        foreach (Vector3 v3 in AvailableForBuild)
        {
            if (v == v3) canBuild = true;
            i++;
        }

        if (canBuild)
        {
            i = 0;
            foreach (Vector3 v3 in tilesSetted)
            {
                if (v == v3)
                {
                    isset = false;
                    break;
                }
                i++;
            }
        }
        else
        {
            isset = false;
            ShowPanel(false, false, false, false);
            tile.GetComponent<MeshRenderer>().material = red;
            return;
        }

        current = (isset) ? green : red;

        tile.GetComponent<MeshRenderer>().material = current;
        if (current == green) {
            ShowPanel(true,true,false,false);
        } else
        {
            ShowPanel(true,false,false,true);
        }
    }
    void WallState()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            float vx = Mathf.Round((hit.point.x / divider) + 0.5f) * divider;// + wallCorrector.x;// -2;
            float vz = Mathf.Round((hit.point.z / divider) - 0.5f) * divider;// + wallCorrector.z;// +1f;
            Vector3 v = new Vector3(vx + wallCorrector.x, wall.transform.position.y, vz + wallCorrector.z);
            

            if (v != currentV)
            {
                wall.transform.position = v;
                currentV = v;

                float rtx = panel.GetComponent<RectTransform>().rect.width / 4;
                float rty = -panel.GetComponent<RectTransform>().rect.height / 2;
                panel.transform.position = Camera.main.WorldToScreenPoint(wall.transform.position - wallCorrector) + new Vector3(rtx, rty, 0);
                
            }

            bool isset = false;
            int i = 0;
            foreach (Vector3 v3 in tilesSetted)
            {
                if ((v-wallCorrector) == v3) isset = true;
                i++;
            }

            current = (isset) ? green : red;

            wall.GetComponent<MeshRenderer>().material = current;
        }
        if (current == green)
        {
            ShowPanel();
        }
        else
        {
            ShowPanel(false);
        }
    }
    void BoxState()
    {
        if (EventSystem.current.IsPointerOverGameObject())  return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            float vx = Mathf.Round((hit.point.x / divider) + 0.5f) * divider-1f;
            float vz = Mathf.Round((hit.point.z / divider) - 0.5f) * divider + 1f;
            Vector3 v2 = new Vector3(-1f, 0,1f);
            Vector3 v = new Vector3(vx, box.transform.position.y, vz);

            if (v != currentV)
            {
                box.transform.position = v;
                currentV = v;

                float rtx = panel.GetComponent<RectTransform>().rect.width / 6;
                float rty = -panel.GetComponent<RectTransform>().rect.height / 4;
                panel.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position) + new Vector3(rtx, rty, 0);
                //WorldToScreenPoint(star.coordinates) + labelOffset; ;
            }
            bool isset = false;
            int i = 0;

            foreach (Vector3 v3 in tilesSetted)
            {
                if ((v+v2) == v3) isset = true;
                i++;
            }
            current = (isset) ? green : red;
            box.GetComponent<MeshRenderer>().material = current;

            if (current == green)
            {
                ShowPanel();
            }
            else
            {
                ShowPanel(false);
            }
        }
    }

    void ShowPanel(bool act=true, bool buil = true, bool rot = true, bool del = true)
    {
        canvas.showBuildPanel(act,buil,rot,del);
    }
}
