using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    public LayerMask policeOfficer;
    public LayerMask ground;

    public GameObject groundMarker;

    private Camera cam;


    public LayerMask civilian;
    public LayerMask suspect;
    public LayerMask door;
    public LayerMask evidence;



    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }



    private void Start()
    {
        cam = Camera.main;
        DeselectAll();
    }



    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, policeOfficer))
            {

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultipleSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }

            }
            else
            {

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }

            }
        }


        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, civilian | door | suspect | evidence))
            {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                // Calculate grid positions for each unit
                List<Vector3> targetPositions = CalculateGridPositions(hit.point, unitsSelected.Count);
                EnableMovementForSelectedUnits(false); // To prevent issues with the grid function

                for (int i = 0; i < unitsSelected.Count; i++)
                {
                    var unit = unitsSelected[i];

                    if (unit.GetComponent<Unit>().isIncapacitated == false)
                    {
                        unit.GetComponent<Unit>().ResetState();
                        UnityEngine.AI.NavMeshAgent agent = unit.GetComponent<UnityEngine.AI.NavMeshAgent>();
                        Vector3 targetPos = targetPositions[i];
                        agent.SetDestination(targetPos);

                        // Debug log to check positions
                        //Debug.Log($"Unit {i} moving to {targetPos}");
                    }
                }

                EnableMovementForSelectedUnits(true); // To prevent issues with the grid function
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
            }
        }

        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, civilian))
            {

                if(hit.collider.gameObject.GetComponent<Civilian>().isDetained == false)
                {
                    if(unitsSelected.Count > 1)
                    {
                        EnableMovementForSelectedUnits(false);
                    }
                    
                    unitsSelected[0].GetComponent<Unit>().OrderToDetainCivilian(hit.collider.gameObject);

                    if(unitsSelected.Count > 1)
                    {
                        EnableMovementForSelectedUnits(true);
                    }
                }

            }

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, suspect))
            {

                if(hit.collider.gameObject.GetComponent<Suspect>().isInactive == false) // If a Suspect is inactive(killed or arrested) there is no need for interactions
                {
                    if(hit.collider.gameObject.GetComponent<Suspect>().isSurrendered == true) // Arrest surrendered suspect
                    {
                        if(unitsSelected.Count > 1)
                        {
                            EnableMovementForSelectedUnits(false);
                        }
                        
                        unitsSelected[0].GetComponent<Unit>().OrderToArrestSuspect(hit.collider.gameObject);

                        if(unitsSelected.Count > 1)
                        {
                            EnableMovementForSelectedUnits(true);
                        }
                    }
                    else // Attack target
                    {
                        foreach(var unit in unitsSelected)
                        {
                            unit.GetComponent<Unit>().OrderToAttackSuspect(hit.collider.gameObject);
                        }
                    }

                }

            }

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, door))
            {

                if(hit.collider.transform.root.gameObject.GetComponent<Door>().isLocked == false)
                {
                    if(unitsSelected.Count > 1)
                    {
                        EnableMovementForSelectedUnits(false);
                    }
                    
                    unitsSelected[0].GetComponent<Unit>().OpenCloseDoor(hit.collider.transform.root.gameObject);

                    if(unitsSelected.Count > 1)
                    {
                        EnableMovementForSelectedUnits(true);
                    }
                }

            }

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, evidence))
            {

                if(hit.collider.transform.root.gameObject.GetComponent<Evidence>().isCollected == false)
                {
                    if(unitsSelected.Count > 1)
                    {
                        EnableMovementForSelectedUnits(false);
                    }
                    
                    unitsSelected[0].GetComponent<Unit>().CollectEvidence(hit.collider.transform.root.gameObject);

                    if(unitsSelected.Count > 1)
                    {
                        EnableMovementForSelectedUnits(true);
                    }
                }

            }
            
        }

        if (Input.GetKeyDown(KeyCode.G) && unitsSelected.Count == 1)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                unitsSelected[0].transform.LookAt(hit.point);

                unitsSelected[0].GetComponent<Unit>().ThrowThrowable();
            }
        }

    }



    private List<Vector3> CalculateGridPositions(Vector3 targetPosition, int unitCount)
    {
        List<Vector3> positions = new List<Vector3>();
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(unitCount)); // Size of the grid (e.g., 3x3 for 9 units)
        float spacing = 2.0f; // Distance between units

        for (int i = 0; i < unitCount; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;

            // Center the grid around the target position
            float xOffset = (col - (gridSize - 1) / 2.0f) * spacing;
            float zOffset = (row - (gridSize - 1) / 2.0f) * spacing;

            Vector3 offset = new Vector3(xOffset, 0, zOffset);
            positions.Add(targetPosition + offset);
        }

        // Debug log to check calculated positions
        //for (int i = 0; i < positions.Count; i++)
        //{
        //    Debug.Log($"Calculated position {i}: {positions[i]}");
        //}

        return positions;
    }



    private void EnableSelectionIndicator(GameObject unit, bool trigger)
    {
        unit.transform.GetChild(0).gameObject.SetActive(trigger);
    }



    private void EnableUnitMovement(GameObject unit, bool trigger)
    {
        unit.GetComponent<UnitMovement>().enabled = trigger;
    }

    private void EnableMovementForSelectedUnits(bool trigger)
    {
        foreach(var unit in unitsSelected)
        {
            EnableUnitMovement(unit, trigger);
        }
    }


    private void DeselectAll()
    {
        foreach(var unit in unitsSelected)
        {
            EnableUnitMovement(unit, false);
            EnableSelectionIndicator(unit, false);
        }

        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }



    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();

        unitsSelected.Add(unit);

        EnableSelectionIndicator(unit, true);
        EnableUnitMovement(unit, true);
    }



    private void MultipleSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            EnableSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
        }
        else
        {
            EnableUnitMovement(unit, false);
            EnableSelectionIndicator(unit, false);
            unitsSelected.Remove(unit);
        }
    }



    internal void DragSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            EnableSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
        }
    }

}
