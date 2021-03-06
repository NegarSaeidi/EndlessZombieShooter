using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("weaponToSpawn"), SerializeField]
    GameObject weaponToSpawn;

    public PlayerController playerController;
    Animator animator;
    Sprite crosshairImage;

    [SerializeField]
    GameObject weaponSocket;

    bool wasFiring = false;
    bool firisngPressed = false;


    [SerializeField]
    Transform GripSocket;
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");

    WeaponComponent equippedWeapon;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        GameObject spawnWeapon = Instantiate(weaponToSpawn, weaponSocket.transform.position, weaponSocket.transform.rotation, weaponSocket.transform);
        equippedWeapon = spawnWeapon.GetComponent<WeaponComponent>();
        equippedWeapon.Initialized(this);
        PlayerEvents.InvokeOnWeaponEquipped(equippedWeapon);
        GripSocket = equippedWeapon.GripLocation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, GripSocket.position);

    }
    public void OnFire(InputValue value)
    {

        firisngPressed = value.isPressed;
      
        if (firisngPressed)
        {
           StartFiring();
        }
        else
        {
            StopFiring();
        }
     


    }
    public void StartFiring()
    {
        if (equippedWeapon.weaponStats.bulletsInClip <= 0)
        {
            StartReloading();
            return;
        }
        animator.SetBool(isFiringHash,true);
        playerController.isFiring = true;
        equippedWeapon.StartFiringWeapon();
    }
    public void StopFiring()
    {
        animator.SetBool(isFiringHash, false);
        playerController.isFiring = false;
        equippedWeapon.StopFiringWeapon();
    }
    public void OnReload(InputValue value)
    {
        playerController.isReloading = value.isPressed;
      
        StartReloading();
    }
    public void StartReloading()
    {
       
        if (playerController.isFiring)
        {
            StopFiring();
        }
        if (equippedWeapon.weaponStats.totalBullets <= 0) return;
        animator.SetBool(isReloadingHash, true);
        equippedWeapon.StartReloading();

        InvokeRepeating(nameof(StopReloading), 0, 0.1f);

    }
    public void StopReloading()
    {
        if (animator.GetBool(isReloadingHash)) return;
        playerController.isReloading = false;
        equippedWeapon.StopReloading();
        animator.SetBool(isReloadingHash, false);
        CancelInvoke(nameof(StopReloading));
       
    }
}
