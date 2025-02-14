using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    public string doorOpenAnimName;
    public string doorCloseAnimName;
    Animator doorAnim;
    public List<GameObject> peopleInCollider = new List<GameObject>();
    private bool isOpen = false;
    public float radius = 2.5f;

    void Start()
    {
        doorAnim = GetComponent<Animator>();
    }

    void Update()
    {
        Sensor();

        if(peopleInCollider.Count > 0 && isOpen == false)
        {
            OpenDoor();
        }
        else if(peopleInCollider.Count <= 0 && isOpen == true)
        {
            CloseDoor();
        }
    }

    private void Sensor()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        peopleInCollider.Clear();

        foreach(Collider nearbyObject in colliders)
        {
            if(nearbyObject.CompareTag("PoliceOfficer") || nearbyObject.CompareTag("Suspect") || nearbyObject.CompareTag("Civilian"))
            {
                GameObject personObj = nearbyObject.gameObject;
                peopleInCollider.Add(personObj);
            }
        }
    }

    private void OpenDoor()
    {
        doorAnim.ResetTrigger("Close");
        doorAnim.SetTrigger("Open");
        isOpen = true;
    }

    private void CloseDoor()
    {
        doorAnim.ResetTrigger("Open");
        doorAnim.SetTrigger("Close");
        isOpen = false;
    }

}
