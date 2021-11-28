using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsUpgradeController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;

    private bool[,] weaponUpgraded = new bool[7, 4];

    private const int pistolType = 0;
    private const int katanaType = 2;
    private const int assaultRifleType = 3;
    private const int shotgunType = 4;
    private const int sniperRifleType = 5;

    [SerializeField] private GameObject assaultRifleUpgrade2Image = null;
    [SerializeField] private GameObject assaultRifleUpgrade3Image = null;

    [SerializeField] private GameObject doublePistolImage = null;
    [SerializeField] private GameObject pistolImage = null;


    private void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                weaponUpgraded[i, j] = false;
            }
        }
    }

    public void UpgradePistol(int upgradeType)
    {
        if (upgradeType == 0 && !weaponUpgraded[pistolType, upgradeType])
        {
            //PlayerController.playerWeaponType = 1;

            PlayerWeaponStats.weaponAttackCooldowns[pistolType] /= 2;
            PlayerWeaponStats.weaponClipAmmoCount[pistolType] *= 2;
            PlayerWeaponStats.weaponReloadingCooldown[pistolType] *= 1.5f;

            PlayerWeaponStats.playerIdleAnimations[pistolType] = "PlayerDoublePistolIdleRight";
            PlayerWeaponStats.playerAdditionalIdleAnimations[pistolType] = "PlayerDoublePistolIdleLeft";
            PlayerWeaponStats.playerAttackAnimations[pistolType] = "PlayerDoublePistolAttackRight";
            PlayerWeaponStats.playerAdditionalAttackAnimations[pistolType] = "PlayerDoublePistolAttackLeft";
            PlayerWeaponStats.playerLegsWalkingAnimations[pistolType] = "PlayerDoublePistolLegsWalkRight";
            PlayerWeaponStats.playerAdditionalLegsWalkingAnimations[pistolType] = "PlayerDoublePistolLegsWalkLeft";
            PlayerWeaponStats.playerBodyAnimations[pistolType] = "PlayerDoublePistolBody";

            pistolImage.SetActive(false);
            doublePistolImage.SetActive(true);
            weaponUpgraded[pistolType, upgradeType] = true;
        }

        if (upgradeType == 1 && !weaponUpgraded[pistolType, upgradeType])
        {
            
        }

        if (upgradeType == 2 && !weaponUpgraded[pistolType, upgradeType])
        {
            
        }

        if (upgradeType == 3 && !weaponUpgraded[pistolType, upgradeType])
        {
            
        }
        playerController.SetWeaponSettings();
    }

    public void UpgradeAssaultRifle(int upgradeType)
    {
        if (upgradeType == 0 && !weaponUpgraded[assaultRifleType, upgradeType])
        {
            PlayerWeaponStats.weaponAttackCooldowns[assaultRifleType] /= 2;
            weaponUpgraded[assaultRifleType, upgradeType] = true;
        }

        if (upgradeType == 1 && !weaponUpgraded[assaultRifleType, upgradeType])
        {
            PlayerWeaponStats.weaponClipAmmoCount[assaultRifleType] *= 2;
            assaultRifleUpgrade2Image.SetActive(true);
            weaponUpgraded[assaultRifleType, upgradeType] = true;
        }

        if (upgradeType == 2 && !weaponUpgraded[assaultRifleType, upgradeType])
        {
            PlayerWeaponStats.weaponScatterAngles[assaultRifleType] /= 2;
            assaultRifleUpgrade3Image.SetActive(true);
            weaponUpgraded[assaultRifleType, upgradeType] = true;
        }

        if (upgradeType == 3 && !weaponUpgraded[assaultRifleType, upgradeType])
        {

        }
        playerController.SetWeaponSettings();
    }

    public void UpgradeShotgun(int upgradeType)
    {
        if (upgradeType == 0 && !weaponUpgraded[shotgunType, upgradeType])
        {
            PlayerWeaponStats.shotgunProjectiles = 5;
        }

        if (upgradeType == 1 && !weaponUpgraded[shotgunType, upgradeType])
        {

        }

        if (upgradeType == 2 && !weaponUpgraded[shotgunType, upgradeType])
        {

        }

        if (upgradeType == 3 && !weaponUpgraded[shotgunType, upgradeType])
        {
            
        }
        playerController.SetWeaponSettings();
    }

    public void UpgradeSniperRifle(int upgradeType)
    {
        if (upgradeType == 0 && !weaponUpgraded[sniperRifleType, upgradeType])
        {
            PlayerWeaponStats.weaponProjectileTypes[sniperRifleType] = 1;
            weaponUpgraded[sniperRifleType, upgradeType] = true;
        }

        if (upgradeType == 1 && !weaponUpgraded[sniperRifleType, upgradeType])
        {

        }

        if (upgradeType == 2 && !weaponUpgraded[sniperRifleType, upgradeType])
        {

        }

        if (upgradeType == 3 && !weaponUpgraded[sniperRifleType, upgradeType])
        {
            
        }
        playerController.SetWeaponSettings();
    }

    public void UpgradeKatana(int upgradeType)
    {
        if (upgradeType == 0 && !weaponUpgraded[katanaType, upgradeType])
        {
            PlayerWeaponStats.weaponsDamage[katanaType] *= 2;
            weaponUpgraded[katanaType, upgradeType] = true;
        }

        if (upgradeType == 1 && !weaponUpgraded[katanaType, upgradeType])
        {
            PlayerWeaponStats.weaponAttackCooldowns[katanaType] /= 1.5f;
            weaponUpgraded[katanaType, upgradeType] = true;
        }

        if (upgradeType == 2 && !weaponUpgraded[katanaType, upgradeType])
        {
            PlayerWeaponStats.maxKatanaDashSpeed *= 1.5f;
            weaponUpgraded[katanaType, upgradeType] = true;
        }

        if (upgradeType == 3 && !weaponUpgraded[katanaType, upgradeType])
        {

        }
        playerController.SetWeaponSettings();
    }
}
