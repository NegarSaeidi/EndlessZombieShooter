using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType
{
    NONE,
        PISTOL,
        MACHINEGUN
}
[System.Serializable]
public struct WeaponStats
{
    WeaponType weapontype;
    public string weaponName;
    public float damage;
    public int bulletsInClip;
    public int clipSize;
    public int fireRate;
    public float fireStartDelay;
    public float fireDistance;
    public bool repeating;
    public LayerMask weaponHitLayers;
}

public enum weaponFiringPattern
{
    SEMIAUTO,FULLAUTO,THREESHOT,FIVESHOT
}
public class WeaponComponent : MonoBehaviour
{
    public Transform GripLocation;
    protected WeaponHolder weaponholder;
    [SerializeField]
    public WeaponStats weaponStats;
    public weaponFiringPattern pattern;
    public bool isFiring = false;
    public WeaponType weapontype;
    public bool isReloading = false;

    public Camera mainCamera;
    void Start()
    {
    }
    private void Awake()
    {
            mainCamera = Camera.main;
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialized(WeaponHolder _weaponholder)
    {
        weaponholder = _weaponholder;

    }

    public virtual void StartFiringWeapon()
    {
        isFiring = true;
        if (weaponStats.repeating)
        {
            InvokeRepeating(nameof(FireWeapon), weaponStats.fireStartDelay, weaponStats.fireRate);
        }
        else
        {
            FireWeapon();
        }
    }
    public virtual void StopFiringWeapon()
    {
        isFiring = false;
        CancelInvoke(nameof(FireWeapon));
        print("stop firing!");
    }
    protected virtual void FireWeapon()
    {
        weaponStats.bulletsInClip--;
        print("Firing weapon..!");
    }
}

