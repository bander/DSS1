using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour {
    delegate void OnIntersectChecking();
    OnIntersectChecking onIntersectChecking;

    public delegate void OnIntersect();
    public OnIntersect onIntersect;

    Collider targetCollider;
    PlayerControl player;
    Collider collider;

    void Start()
    {
        player = PlayerManager.instance.player.GetComponent<PlayerControl>();
        collider = GetComponent<Collider>();
    }
    void Update()
    {
        if (onIntersectChecking != null) onIntersectChecking.Invoke();
    }
    public void StartTargetChecking(GameObject target)
    {
        if (target != null)
        {
            targetCollider = target.GetComponent<Collider>();
            onIntersectChecking += CheckIntersectWithTarget;
        }
    }
    public void StopTargetChecking()
    {
        targetCollider = null;
        onIntersectChecking -= CheckIntersectWithTarget;
    }
    void CheckIntersectWithTarget()
    {
        if (targetCollider != null)
        {
            if (collider.bounds.Intersects(targetCollider.bounds))
            {
                if (onIntersect != null) onIntersect.Invoke();
                onIntersectChecking -= CheckIntersectWithTarget;
            }
        }
    }
}
