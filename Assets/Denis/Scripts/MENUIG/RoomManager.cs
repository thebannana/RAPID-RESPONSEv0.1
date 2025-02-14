using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] rooms; // Array of room GameObjects in the scene
    private int currentRoomIndex = 0; // Index of the current room

    private void Start()
    {
        // Disable all cameras except for the first room's camera
        DisableAllCameras();
    }

    private void DisableAllCameras()
    {
        foreach (GameObject room in rooms)
        {
            foreach (Transform child in room.transform)
            {
                if (child.CompareTag("MainCamera"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        // Enable the main camera of the first room
        rooms[currentRoomIndex].transform.Find("MainCamera").gameObject.SetActive(true);
    }

    private void SwitchToRoom(int newIndex)
    {
        // Disable the current room's camera
        rooms[currentRoomIndex].transform.Find("MainCamera").gameObject.SetActive(false);

        // Enable the new room's camera
        rooms[newIndex].transform.Find("MainCamera").gameObject.SetActive(true);

        // Update the current room index
        currentRoomIndex = newIndex;
    }

    public void ToggleRoom(GameObject targetRoom)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i] == targetRoom)
            {
                // Toggle to the target room if it's different from the current room
                if (i != currentRoomIndex)
                {
                    SwitchToRoom(i);
                }
                else
                {
                    // Toggle back to the previous room if the target room is the current room
                    int previousIndex = (currentRoomIndex - 1 + rooms.Length) % rooms.Length;
                    SwitchToRoom(previousIndex);
                }
                return;
            }
        }

        Debug.LogWarning("Target room not found in rooms array.");
    }
}


