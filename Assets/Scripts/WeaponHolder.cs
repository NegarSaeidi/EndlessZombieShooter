using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Header("weaponToSpawn"), SerializeField]
    GameObject weaponToSpawn;

    PlayerController playerController;
    Sprite crosshairImage;

    [SerializeField]
    GameObject weaponSocket;


    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnWeapon = Instantiate(weaponToSpawn, weaponSocket.transform.position, weaponSocket.transform.rotation, weaponSocket.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
