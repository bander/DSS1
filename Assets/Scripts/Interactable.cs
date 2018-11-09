using UnityEngine;

public class Interactable : MonoBehaviour {
    protected PlayerInteractions player;
    public float radius = 2f;
    public float stopping = 0.5f;
    protected bool isInRange = false;
    public EnemyStats stats;
    public bool low = true;
    public bool closed = false;


    void Start()
    {
        player = PlayerInteractions.instance;
        stats = GetComponent<EnemyStats>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Update()
    {
        SecondUpdate();

        if (player != null)
        {
            if ((player.transform.position - transform.position).magnitude < radius)
            {
                if (!isInRange)
                {
                    isInRange = true;
                    inRange();
                }
            }
            else
            {
                if (isInRange)
                {

                    isInRange = false;
                    inRange();
                }

            }
        }
        else
        {
           // Debug.Log("null "+name  );
        }
    }
    public virtual void SecondUpdate()
    {

    } 

    public virtual void Interact()
    {
       
    }
    public virtual void inRange()
    {
        if (isInRange)
        {
            player.addInteractables(this);
        }
        else
        {
            player.removeInteractables(this);
        }
    }

}
