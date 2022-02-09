using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AmmoUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI weaponNameText;
    [SerializeField] TextMeshProUGUI currentBulletCountext;
    [SerializeField] TextMeshProUGUI totalBulletCountText;


    [SerializeField] WeaponComponent weaponComponent;


    private void OnEnable()
    {
        PlayerEvents.OnWeaponEquipped += OnWeaponEquipped;
    }
    private void OnDisable()
    {
        PlayerEvents.OnWeaponEquipped -= OnWeaponEquipped;
    }
    void OnWeaponEquipped(WeaponComponent _weaponComponent)
    {
        weaponComponent = _weaponComponent;
    }
    void Update()
    {
        if (!weaponComponent) return;
        weaponNameText.text = weaponComponent.weaponStats.weaponName;
        currentBulletCountext.text = weaponComponent.weaponStats.bulletsInClip.ToString();
        totalBulletCountText.text = weaponComponent.weaponStats.totalBullets.ToString();
    }
}
