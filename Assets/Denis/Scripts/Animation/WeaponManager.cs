using System;
using UnityEngine;

[Serializable]
public class WeaponObject
{
    public int weaponID;
    public GameObject weaponObject;
}

[Serializable]
public class ThrowableObject
{
    public Unit.Throwables throwableType;
    public GameObject throwableObject;
}

public class WeaponManager : MonoBehaviour
{
    public WeaponObject[] weapons;
    public ThrowableObject[] throwables;

    private Unit unitScript;

    void Start()
    {
        // Get the Unit script from the parent object
        unitScript = GetComponentInParent<Unit>();
        if (unitScript == null)
        {
            Debug.LogError("WeaponManager: No Unit script found in the parent object!");
            return;
        }
    }

    void Update()
    {
        // Enable current weapon and throwable during runtime
        EnableWeapon(unitScript.weaponID);
        EnableThrowable(unitScript.equipedThrowable);
    }

    public void EnableWeapon(int weaponID)
    {
        foreach (var weapon in weapons)
        {
            weapon.weaponObject.SetActive(weapon.weaponID == weaponID);
        }
    }

    public void EnableThrowable(Unit.Throwables throwableType)
    {
        foreach (var throwable in throwables)
        {
            throwable.throwableObject.SetActive(throwable.throwableType == throwableType);
        }
    }
}
