using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GrenadeDelete : MonoBehaviour
{
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer>=0.5)
        {
            Destroy(gameObject);
        }
    }
}
