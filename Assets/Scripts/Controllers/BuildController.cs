using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildController : MonoBehaviour
{
    public GameObject panel;

    public LayerMask layerMask;
    public GameObject tile;
    public Material green;
    public Material red;
    public Material invis;
    

    Material current;

    Vector3 currentV;

    public float divider = 3;

	void Start () {
		
	}
	
	void Update () {

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

                float rtx = panel.GetComponent<RectTransform>().rect.width / 4;
                float rty = -panel.GetComponent<RectTransform>().rect.height / 2;
                panel.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position)+new Vector3(rtx,rty,0);
//                    WorldToScreenPoint(star.coordinates) + labelOffset; ;
            }
            
        }
        if (Input.GetMouseButtonDown(0))
        {   
            float vx = Mathf.Round(hit.point.x / divider) * divider;
            float vz = Mathf.Round(hit.point.z / divider) * divider;
            Vector3 v = new Vector3(vx, tile.transform.position.y, vz);
            
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
    }
}
