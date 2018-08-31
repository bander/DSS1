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
    public Material final;


    Material current;
    Vector3 currentV;
    Vector3 wallCorrector=new Vector3(2,0,-2);

    public delegate void OnBuidUpdate();
    public OnBuidUpdate onBuildUpdate;

    float divider = 2;

    float wallRotation = 0;

    GameObject currentTarget;

    List<Vector3> tilesSetted = new List<Vector3>();

    void Start() {
        canvas = CanvasController.instance;
        manager = InvManager.instance;

        tile.transform.localScale = tile.transform.localScale * 2 / 3;
        wall.transform.localScale = wall.transform.localScale * 2 / 3;
        tile.transform.position = new Vector3(0, -20, 0);
       // wall.transform.position = new Vector3(0, -20, 0);
    }

    public void setFloorState()
    {
        turnOffCurrent();
        currentTarget = tile;
        currentTarget.transform.position = new Vector3(0, 0.1f, 0);
        onBuildUpdate = FloorState;

        if (onBuildUpdate != null) onBuildUpdate.Invoke();
    }
    public void setWallState()
    {
        turnOffCurrent();
        currentTarget = wall;
        currentTarget.transform.position = new Vector3(0, 0.1f, 0);
        onBuildUpdate = WallState;

        if (onBuildUpdate != null) onBuildUpdate.Invoke();
    }
    public void setBoxState()
    {
        turnOffCurrent();
        currentTarget = box;
        currentTarget.transform.position = new Vector3(0, 0.1f, 0);
        onBuildUpdate = BoxState;

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
            wallCorrector = new Vector3(2,0, -2);
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
                    wallCorrector = new Vector3(2, 0, -2);
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
    
    void turnOffCurrent()
    {
        if (currentTarget != null)
        {
            currentTarget.GetComponent<MeshRenderer>().material = invis;
            currentTarget.transform.position = new Vector3(0, -20, 0);
            currentTarget = null;
        }
    }

    public void BuildTarget()
    {
        if (currentTarget != null)
        {
            if (currentTarget == tile)
            {
                tilesSetted.Add(currentTarget.transform.position);
            }
            if (currentTarget == box)
            {
                manager.spendItems(Item.ItemType.Metall, 5);
            }
            else
            {
                manager.spendItems(Item.ItemType.Metall, 1);
            }

            currentTarget.transform.position = currentTarget.transform.position + Vector3.up;
            GameObject newObject = GameObject.Instantiate(currentTarget, currentTarget.transform.position - Vector3.up, currentTarget.transform.rotation);
            newObject.GetComponent<MeshRenderer>().material = final;

            currentTarget.transform.position = currentTarget.transform.position - Vector3.up;
            currentTarget.GetComponent<MeshRenderer>().material = invis;

            canvas.showBuildPanel(false);
        }
    }
    public void RotateTarget()
    {
        wall.transform.Rotate(new Vector3(0, 90, 0));

        int sw = (int)(wall.transform.rotation.eulerAngles.y / 90);
        switch (sw) { 
            case 0:
                wall.transform.position += new Vector3(0, 0, -2);
                wallCorrector = new Vector3(2, 0, -2);
                break;
            case 1:
                wall.transform.position += new Vector3(-2,0, 0);
                wallCorrector = new Vector3(0, 0, -2);
                break;
            case 2:
                wall.transform.position += new Vector3(0, 0, 2);
                wallCorrector = new Vector3(0, 0, 0);
                break;
            case 3:
                wall.transform.position += new Vector3(2, 0, 0);
                wallCorrector = new Vector3(2, 0, 0);
                break;
        }
        
    }
    public void removeTarget()
    {

    }

    void FloorState() { 
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray,out hit,100, layerMask))
        {
            float vx = Mathf.Round((hit.point.x/divider)-0.5f)* divider;
            float vz = Mathf.Round((hit.point.z/ divider) + 0.5f) * divider;
            Vector3 v = new Vector3(vx,tile.transform.position.y, vz);

            if (v != currentV)
            {
                tile.transform.position = v;
                currentV = v;

                float rtx = panel.GetComponent<RectTransform>().rect.width / 6;
                float rty = -panel.GetComponent<RectTransform>().rect.height / 4;
                panel.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position)+new Vector3(rtx,rty,0);
//                    WorldToScreenPoint(star.coordinates) + labelOffset; ;
            }
            
        }

        current = green;
        foreach (Collider col in tile.GetComponent<TileCollider>().cols)
        {
            if (col.GetComponent<BuildLocker>() != null)
            {
                current = red;
                break;
            }
        }

        tile.GetComponent<MeshRenderer>().material = current;
        if (current == green) {
            ShowPanel();
        } else {
            ShowPanel(false);
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
            float vx = Mathf.Round((hit.point.x / divider) - 0.5f) * divider + wallCorrector.x;
            float vz = Mathf.Round((hit.point.z / divider) + 0.5f) * divider + wallCorrector.z;
            Vector3 v = new Vector3(vx, wall.transform.position.y, vz);

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

            if (isset)
            {
                current = green;
            }
            else
            {
                current = red;
            }
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            float vx = Mathf.Round((hit.point.x / divider) - 0.5f) * divider;
            float vz = Mathf.Round((hit.point.z / divider) + 0.5f) * divider;
            Vector3 v = new Vector3(vx, box.transform.position.y, vz);

            if (v != currentV)
            {
                box.transform.position = v;
                currentV = v;

                float rtx = panel.GetComponent<RectTransform>().rect.width / 6;
                float rty = -panel.GetComponent<RectTransform>().rect.height / 4;
                panel.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position) + new Vector3(rtx, rty, 0);
                //                    WorldToScreenPoint(star.coordinates) + labelOffset; ;
            }

        }

        current = red;
        foreach (Collider col in tile.GetComponent<TileCollider>().cols)
        {
            if (col.GetComponent<TileCollider>() != null)
            {
                current = green;
                break;
            }
            current = red;
        }
        box.GetComponent<MeshRenderer>().material = current;
    }

    void ShowPanel(bool act=true)
    {
        canvas.showBuildPanel(act);
    }
}
