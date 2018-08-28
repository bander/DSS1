using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvManager : MonoBehaviour {
    #region Singleton
    public static InvManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public Invs playerInvs;
    public Invs lootInvs;
    public Invs EquipInvs;


    public Item tm;
    public Item tm2;

    public List<Invent> invents = new List<Invent>();


    public delegate void OnInvChanged();
    public OnInvChanged OnInvChangedCallback;

    void Start()
    {
        playerInvs.SetNum(0);
        lootInvs.SetNum(1);
        EquipInvs.SetNum(2);

        GameObject invent= new GameObject();
        invent.AddComponent<Invent>();
        invent.GetComponent<Invent>().Init(8);
        invents.Add(invent.GetComponent<Invent>());

        GameObject invent2 = new GameObject();
        invent2.AddComponent<Invent>();
        invent2.GetComponent<Invent>().Init(8);
        invents.Add(invent2.GetComponent<Invent>());
        
        GameObject invent3 = new GameObject();
        invent3.AddComponent<Invent>();
        invent3.GetComponent<Invent>().Init(7);
        invents.Add(invent3.GetComponent<Invent>());
        //*/
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           /* invents[0].Add(tm);

           invents[1].Add(tm2);
           invents[2].Add(tm2);

            if (OnInvChangedCallback!=null)
            OnInvChangedCallback.Invoke();
            //*/
        }
    }

    public bool AddToInventory(Item item,int numInv)
    {
        if (true)
        {

            invents[0].Add(item);

            if (OnInvChangedCallback != null)
                OnInvChangedCallback.Invoke();

            return true;
        }
        return false;
    }

    public Invent GetInvent(int num)
    {
        if (invents.Count>num)
        {
            return invents[num];
        }
        return null;
    }
    
}
