using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinger : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 7.5f;
    private float countdown;
    private bool exploded = false;
    public GameObject explosionEffect;

    void Start()
    {
        countdown = delay;
    }


    void Update()
    {
        countdown -= Time.deltaTime;

        if(countdown <= 0 && exploded == false)
        { 
            exploded = true;
            Explode();
        }
    }

    private void Explode()
    {
        // Effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearbyObject in colliders)
        {
            if(nearbyObject.CompareTag("Suspect"))
            {
                GameObject suspectObj = nearbyObject.gameObject;

                suspectObj.GetComponent<Suspect>().BecomeConcussed();
            }
        }

        //  Remove stinger
        Destroy(gameObject);
    }
}
