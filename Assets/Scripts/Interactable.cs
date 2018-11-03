using UnityEngine;

public class Interactable : MonoBehaviour {
    protected Transform player;
    public float radius = 2f;
    protected bool isInRange = false;
    public EnemyStats stats;
    public bool low = true;

    void Start()
    {
        player = PlayerManager.instance.player.transform;
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
            if ((player.position - transform.position).magnitude < radius)
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
            MenuScript.instance.addImteractables(this);
            //Debug.Log("In");
        }
        else
        {
            MenuScript.instance.removeImteractables(this);
            //Debug.Log("out");
        }
    }

}
