using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventsOnHover : MonoBehaviour
{
    // Add references to the scripts or components that you want to trigger
    public MonoBehaviour[] scriptsToTrigger;

    private void OnMouseEnter()
    {
        // Check if the array of scripts to trigger is not null
        if (scriptsToTrigger != null)
        {
            // Iterate through each script in the array and trigger their enabled status
            foreach (var script in scriptsToTrigger)
            {
                script.enabled = true;
            }
        }
        else
        {
            Debug.LogWarning("No scripts to trigger assigned!");
        }
    }

    private void OnMouseExit()
    {
        // Check if the array of scripts to trigger is not null
        if (scriptsToTrigger != null)
        {
            // Iterate through each script in the array and disable them
            foreach (var script in scriptsToTrigger)
            {
                script.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("No scripts to trigger assigned!");
        }
    }
}

