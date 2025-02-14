using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string doorOpenAnimName;
    public string doorCloseAnimName;
    Animator doorAnim;
    public bool isLocked = false;
    public bool isWedged = false;
    public bool isOpen = false;

    void Start()
    {
        doorAnim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (!isLocked && !isWedged)
        {
            this.gameObject.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
            doorAnim.ResetTrigger("Close");
            doorAnim.SetTrigger("Open");
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        this.gameObject.transform.GetChild(2).gameObject.GetComponent<AudioSource>().Play();
        doorAnim.ResetTrigger("Open");
        doorAnim.SetTrigger("Close");
        isOpen = false;
    }

    public void InteractWithDoor()
    {
        if (isOpen == false)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

}
