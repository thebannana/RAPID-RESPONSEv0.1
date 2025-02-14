using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class LoadoutSelection : MonoBehaviour
{
    [System.Serializable]
    public class OfficerLoadout
    {
        public TMP_Dropdown mainGunDropdown;
        public TMP_Dropdown grenadeDropdown;

        public int mainGunId;
        public int grenadeId;
    }

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

    public OfficerLoadout[] officerLoadouts;

    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "loadout.json");

        if (officerLoadouts.Length != 4)
        {
            Debug.LogError("There should be exactly 4 officer loadouts.");
        }

        InitializeDropdowns();
        LoadLoadoutData();
    }

    private void InitializeDropdowns()
    {
        string[] mainGuns = { "Carbine 5.56", "Universal Machinen .45", "Machine Schalldampfer 9mm", "Pump-Shotgun 00 12G", "Avtomat 7.62" };
        string[] grenades = { "Flashbang", "Stinger" };

        foreach (OfficerLoadout loadout in officerLoadouts)
        {
            loadout.mainGunDropdown.ClearOptions();
            loadout.mainGunDropdown.AddOptions(new List<string>(mainGuns));

            loadout.grenadeDropdown.ClearOptions();
            loadout.grenadeDropdown.AddOptions(new List<string>(grenades));

            loadout.mainGunDropdown.onValueChanged.AddListener(delegate { SaveLoadoutData(); });
            loadout.grenadeDropdown.onValueChanged.AddListener(delegate { SaveLoadoutData(); });
        }
    }

    private void SaveLoadoutData()
    {
        LoadoutData data = new LoadoutData();

        foreach (OfficerLoadout loadout in officerLoadouts)
        {
            OfficerLoadoutData officerData = new OfficerLoadoutData
            {
                mainGunId = loadout.mainGunDropdown.value + 1, // Adjust according to your IDs
                grenadeId = loadout.grenadeDropdown.value + 1  // Adjust according to your IDs
            };
            data.officers.Add(officerData);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    private void LoadLoadoutData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LoadoutData data = JsonUtility.FromJson<LoadoutData>(json);

            for (int i = 0; i < officerLoadouts.Length; i++)
            {
                officerLoadouts[i].mainGunDropdown.value = data.officers[i].mainGunId - 1; // Adjust according to your IDs
                officerLoadouts[i].grenadeDropdown.value = data.officers[i].grenadeId - 1; // Adjust according to your IDs
                officerLoadouts[i].mainGunDropdown.RefreshShownValue();
                officerLoadouts[i].grenadeDropdown.RefreshShownValue();
            }
        }
        else
        {
            SaveLoadoutData(); // Create file if it doesn't exist
        }
    }

    public void PrintLoadoutSelections()
    {
        for (int i = 0; i < officerLoadouts.Length; i++)
        {
            string mainGun = officerLoadouts[i].mainGunDropdown.options[officerLoadouts[i].mainGunDropdown.value].text;
            string grenade = officerLoadouts[i].grenadeDropdown.options[officerLoadouts[i].grenadeDropdown.value].text;

            Debug.Log($"Officer {i + 1} Loadout - Main Gun: {mainGun}, Grenade: {grenade}");
        }
    }
}
