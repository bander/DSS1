using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensitive : MonoBehaviour {


    void OnCollisionEnter(Collision collision)
    { 

        Debug.Log("dd");
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
            Debug.Log("dd");
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("dd");
    }
}
