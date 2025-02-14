using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LoadoutReader : MonoBehaviour
{
    public static LoadoutReader Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();

    [System.Serializable]
    public class LoadoutData
    {
        public List<OfficerLoadoutData> officers = new List<OfficerLoadoutData>();
    }

    [System.Serializable]
    public class OfficerLoadoutData
    {
        public int mainGunId;
        public int grenadeId;
    }

    private string filePath;
    private bool areEquiped = false;

    private Dictionary<int, string> mainGunMap = new Dictionary<int, string>
    {
        { 1, "Tac. Assault Rifle" },
        { 2, "Submachine Gun" },
        { 3, "Silenced Rifle" },
        { 4, "Shotgun" },
        { 5, "Assault Rifle" }
    };

    private Dictionary<int, string> grenadeMap = new Dictionary<int, string>
    {
        { 1, "Flashbang" },
        { 2, "Stinger" }
    };

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
        filePath = Path.Combine(Application.persistentDataPath, "loadout.json");
    }

    private void Update()
    {
        if(allUnitsList.Count == 4 && areEquiped == false)
        {
            PrintLoadouts();
            areEquiped = true;
        }
    }

    private void PrintLoadouts()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LoadoutData data = JsonUtility.FromJson<LoadoutData>(json);

            for (int i = 0; i < allUnitsList.Count; i++)
            {
                OfficerLoadoutData officerData = data.officers[i];
                string mainGun = mainGunMap.ContainsKey(officerData.mainGunId) ? mainGunMap[officerData.mainGunId] : "Unknown Main Gun";
                string grenade = grenadeMap.ContainsKey(officerData.grenadeId) ? grenadeMap[officerData.grenadeId] : "Unknown Grenade";

                EquipOfficer(allUnitsList[i], officerData.mainGunId, officerData.grenadeId);

                Debug.Log($"Officer {i + 1} Loadout - Main Gun: {mainGun}, Grenade: {grenade}");
            }
        }
        else
        {
            Debug.LogError("Loadout file not found.");
        }
    }

    private void EquipOfficer(GameObject officerObj, int weaponID, int throwableID)
    {
        // Template: GetEquiped(bool _isLethal, float _damage, float _rateOfFire, float _range, float _precision, int _throwableID)

        switch(weaponID)
        {
            case 1: // Carbine 5.56 (stats for 5.56 rifles)
                officerObj.GetComponent<Unit>().GetEquiped(true, 33.4f, 2.0f, 10.0f, 90.0f, throwableID, weaponID);
                break;
            case 2: // Universal Machinen .45 (stats for .45 smg)
                officerObj.GetComponent<Unit>().GetEquiped(true, 25.0f, 2.0f, 10.0f, 75.0f, throwableID, weaponID);
                break;
            case 3: // Machinen Schalldampfer 9mm (stats for 9mm smg)
                officerObj.GetComponent<Unit>().GetEquiped(true, 20.0f, 2.0f, 10.0f, 75.0f, throwableID, weaponID);
                break;
            case 4: // Pump-Shotgun 00 12G (stats for 00 12 gauge pump-action shotgun)
                officerObj.GetComponent<Unit>().GetEquiped(true, 100.0f, 5.0f, 10.0f, 70.0f, throwableID, weaponID);
                break;
            case 5: // Avtomat 7.62 (stats for 7.62 rifle)
                officerObj.GetComponent<Unit>().GetEquiped(true, 50.0f, 3.0f, 10.0f, 85.0f, throwableID, weaponID);
                break;
        }

    }

    
}
