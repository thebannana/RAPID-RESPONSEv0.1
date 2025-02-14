using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public enum Upwards
    {
        X,
        Z
    }
    private Transform localTrans;
    public Camera facingCamera;
    public Upwards direction = Upwards.X;

    private void Start()
    {
        localTrans = GetComponent<Transform>();
        if (facingCamera == null)
        {
            facingCamera = Camera.main; 
        }
    }

    private void Update()
    {
        
        switch(direction)
        {
            case Upwards.X:
                if (facingCamera)
                {
                    Vector3 directionToCamera = facingCamera.transform.position - localTrans.position;
                    directionToCamera.x = 0; // Keep the y component zero to remain horizontal
                    localTrans.rotation = Quaternion.LookRotation(-directionToCamera);
                }
                break;
            case Upwards.Z:
                if (facingCamera)
                {
                    Vector3 directionToCamera = facingCamera.transform.position - localTrans.position;
                    directionToCamera.z = 0; // Keep the y component zero to remain horizontal
                    localTrans.rotation = Quaternion.LookRotation(-directionToCamera);
                }
                break;
        }

    }
    
}
