using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons: MonoBehaviour
{ 
    //damage
    protected const float pistolDamage = 50;
    protected const float doublePistolDamage = 50;
    protected const float katanaDamage = 50;
    protected const float assaultRifleDamage = 30;
    protected const float shotgunDamage = 50;
    protected const float sniperRifleDamage = 200;
    protected const float grenadeLauncherDamage = 200;

    //attack cooldown
    protected const float pistolAttackCooldown = 0.5f;
    protected const float doublePistolAttackCooldown = 0.25f;
    protected const float katanaAttackCooldown = 0.75f;
    protected const float assaultRifleAttackCooldown = 0.2f;
    protected const float shotgunAttackCooldown = 0.75f;
    protected const float sniperRifleAttackCooldown = 0.75f;
    protected const float grenadeLauncherAttackCooldown = 1;

    //projectile type
    protected const int pistolProjectileType = 0;
    protected const int doublePistolProjectileType = 0;
    protected const int assaultRifleProjectileType = 0;
    protected const int shotgunProjectileType = 0;
    protected const int sniperRifleProjectileType = 0;
    protected const int grenadeLauncherProjectileType = 2;

    //projectile speed
    protected const float pistolProjectileSpeed = 20;
    protected const float doublePistolProjectileSpeed = 20;
    protected const float assaultRifleProjectileSpeed = 20;
    protected const float shotgunProjectileSpeed = 20;
    protected const float sniperRifleProjectileSpeed = 30;
    protected const float grenadeLauncherProjectileSpeed = 50;

    //scatter angle
    protected const float pistolScatterAngle = 3;
    protected const float doublePistolScatterAngle = 3;
    protected const float assaultRifleScatterAngle = 5;
    protected const float shotgunScatterAngle = 2;
    protected const float sniperRifleScatterAngle = 1;
    protected const float grenadeLauncherScatterAngle = 1;

    //weapon clip ammo count
    protected const int pistolWeaponClipAmmoCount = 10;
    protected const int doublePistolWeaponClipAmmoCount = 20;
    protected const int assaultRifleWeaponClipAmmoCount = 30;
    protected const int shotgunWeaponClipAmmoCount = 6;
    protected const int sniperRifleWeaponClipAmmoCount = 10;
    protected const int grenadeLauncherWeaponClipAmmoCount = 1;

    //weapon total ammo count
    protected const int assaultRifleWeaponTotalAmmoCount = 300;
    protected const int shotgunWeaponTotalAmmoCount = 50;
    protected const int sniperRifleWeaponTotalAmmoCount = 50;
    protected const int grenadeLauncherWeaponTotalAmmoCount = 20;

    //weapon reloading cooldown
    protected const float pistolReloadingCooldown = 2;
    protected const float doublePistolReloadingCooldown = 3;
    protected const float assaultRifleReloadingCooldown = 2;
    protected const float shotgunReloadingCooldown = 2;
    protected const float sniperRifleReloadingCooldown = 3;
    protected const float grenadeLauncherReloadingCooldown = 3;

    //weapon ammo drop count
    protected const int pistolAmmoDropCount = 0;
    protected const int doublePistolAmmoDropCount = 0;
    protected const int katanaAmmoDropCount = 0;
    protected const int assaultRifleAmmoDropCount = 30;
    protected const int shotgunAmmoDropCount = 6;
    protected const int sniperRifleAmmoDropCount = 10;
    protected const int grenadeLauncherAmmoDropCount = 1;

    //price
    protected const float pistolPrice = 0;
    protected const float doublePistolPrice = 0;
    protected const float katanaPrice = 25;
    protected const float assaultRiflePrice = 50;
    protected const float shotgunPrice = 50;
    protected const float sniperRiflePrice = 50;
    protected const float grenadeLauncherPrice = 0;


    //player body animation
    protected const string playerPistolBodyAnimation = "PlayerPistolBody";
    protected const string playerDoublePistolBodyAnimation = "PlayerDoublePistolBody";
    protected const string playerKatanaBodyAnimation = "PlayerKatanaBody";
    protected const string playerAssaultRifleBodyAnimation = "PlayerAssaultRifleBody";
    protected const string playerShotgunBodyAnimation = "PlayerAssaultRifleBody";
    protected const string playerSniperRifleBodyAnimation = "PlayerAssaultRifleBody";
    protected const string playerGrenadeLauncherBodyAnimation = "PlayerAssaultRifleBody";

    //player idle animation
    protected const string playerPistolIdleAnimation = "PlayerPistolIdle";
    protected const string playerDoublePistolIdleAnimation = "PlayerDoublePistolIdleRight";
    protected const string playerKatanaIdleAnimation = "PlayerKatanaIdleRight";
    protected const string playerAssaultRifleIdleAnimation = "PlayerAssaultRifleIdle";
    protected const string playerShotgunIdleAnimation = "PlayerAssaultRifleIdle";
    protected const string playerSniperRifleIdleAnimation = "PlayerAssaultRifleIdle";
    protected const string playerGrenadeLauncherIdleAnimation = "PlayerAssaultRifleIdle";

    //player attack animation
    protected const string playerPistolAttackAnimation = "PlayerPistolAttack";
    protected const string playerDoublePistolAttackAnimation = "PlayerDoublePistolAttackRight";
    protected const string playerKatanaAttackAnimation = "PlayerKatanaAttackRight";
    protected const string playerAssaultRifleAttackAnimation = "PlayerAssaultRifleAttack";
    protected const string playerShotgunAttackAnimation = "PlayerAssaultRifleAttack";
    protected const string playerSniperRifleAttackAnimation = "PlayerAssaultRifleAttack";
    protected const string playerGrenadeLauncherAttackAnimation = "PlayerAssaultRifleAttack";

    //player legs walking animation
    protected const string playerPistolLegsWalkingAnimation = "PlayerPistolLegsWalk";
    protected const string playerDoublePistolLegsWalkingAnimation = "PlayerDoublePistolLegsWalkRight";
    protected const string playerKatanaLegsWalkingAnimation = "PlayerKatanaLegsWalkRight";
    protected const string playerAssaultRifleLegsWalkingAnimation = "PlayerAssaultRifleLegsWalk";
    protected const string playerShotgunLegsWalkingAnimation = "PlayerAssaultRifleLegsWalk";
    protected const string playerSniperRifleLegsWalkingAnimation = "PlayerAssaultRifleLegsWalk";
    protected const string playerGrenadeLauncherLegsWalkingAnimation = "PlayerAssaultRifleLegsWalk";



    //player idle animation

    protected const string playerAdditionalPistolIdleAnimation = "PlayerPistolIdleLeft";
    //protected const string playerAdditionalDoublePistolIdleAnimation = "PlayerDoublePistolIdleLeft";
    protected const string playerAdditionalKatanaIdleAnimation = "PlayerKatanaIdleLeft";


    //player attack animation

    protected const string playerAdditionalPistolAttackAnimation = "PlayerPistolAttackLeft";
    //protected const string playerAdditionalDoublePistolAttackAnimation = "PlayerDoublePistolAttackLeft";
    protected const string playerAdditionalKatanaAttackAnimation = "PlayerKatanaAttackLeft";


    //player legs walking animation

    protected const string playerAdditionalPistolLegsWalkingAnimation = "PlayerPistolLegsWalkLeft";
    //protected const string playerAdditionalDoublePistolLegsWalkingAnimation = "PlayerDoublePistolLegsWalkLeft";
    protected const string playerAdditionalKatanaLegsWalkingAnimation = "PlayerKatanaLegsWalkLeft";

}

public class PlayerWeaponStats: PlayerWeapons
{
    public static float[] weaponsDamage = new float[7];
    public static float[] weaponAttackCooldowns = new float[7];
    public static int[] weaponProjectileTypes = new int[7];
    public static float[] weaponProjectilesSpeed = new float[7];
    public static float[] weaponScatterAngles = new float[7];
    public static int[] weaponClipAmmoCount = new int[7];
    public static int[] weaponTotalAmmoCount = new int[7];
    public static float[] weaponReloadingCooldown = new float[7];
    public static int[] weaponAmmoDropCount = new int[7];
    public static float[] weaponPrices = new float[7];
    public static string[] playerBodyAnimations = new string[7];
    public static string[] playerIdleAnimations = new string[7];
    public static string[] playerAttackAnimations = new string[7];
    public static string[] playerLegsWalkingAnimations = new string[7];

    public static string[] playerAdditionalIdleAnimations = new string[7];
    public static string[] playerAdditionalAttackAnimations = new string[7];
    public static string[] playerAdditionalLegsWalkingAnimations = new string[7];

    public static int shotgunProjectiles = 0;

    public static float katanaDashTime = 0.25f;
    public static float maxKatanaDashSpeed = 20;

    private void Start()
    {
        weaponsDamage[0] = PlayerWeapons.pistolDamage;
        weaponsDamage[1] = PlayerWeapons.doublePistolDamage;
        weaponsDamage[2] = PlayerWeapons.katanaDamage;
        weaponsDamage[3] = PlayerWeapons.assaultRifleDamage;
        weaponsDamage[4] = PlayerWeapons.shotgunDamage;
        weaponsDamage[5] = PlayerWeapons.sniperRifleDamage;
        weaponsDamage[6] = PlayerWeapons.grenadeLauncherDamage;

        weaponAttackCooldowns[0] = PlayerWeapons.pistolAttackCooldown;
        weaponAttackCooldowns[1] = PlayerWeapons.doublePistolAttackCooldown;
        weaponAttackCooldowns[2] = PlayerWeapons.katanaAttackCooldown;
        weaponAttackCooldowns[3] = PlayerWeapons.assaultRifleAttackCooldown;
        weaponAttackCooldowns[4] = PlayerWeapons.shotgunAttackCooldown;      
        weaponAttackCooldowns[5] = PlayerWeapons.sniperRifleAttackCooldown;
        weaponAttackCooldowns[6] = PlayerWeapons.grenadeLauncherAttackCooldown;

        weaponProjectileTypes[0] = PlayerWeapons.pistolProjectileType;
        weaponProjectileTypes[1] = PlayerWeapons.doublePistolProjectileType;
        weaponProjectileTypes[2] = -1;
        weaponProjectileTypes[3] = PlayerWeapons.assaultRifleProjectileType;
        weaponProjectileTypes[4] = PlayerWeapons.shotgunProjectileType;
        weaponProjectileTypes[5] = PlayerWeapons.sniperRifleProjectileType;
        weaponProjectileTypes[6] = PlayerWeapons.grenadeLauncherProjectileType;

        weaponProjectilesSpeed[0] = PlayerWeapons.pistolProjectileSpeed;
        weaponProjectilesSpeed[1] = PlayerWeapons.doublePistolProjectileSpeed;
        weaponProjectilesSpeed[2] = -1;
        weaponProjectilesSpeed[3] = PlayerWeapons.assaultRifleProjectileSpeed;
        weaponProjectilesSpeed[4] = PlayerWeapons.shotgunProjectileSpeed;
        weaponProjectilesSpeed[5] = PlayerWeapons.sniperRifleProjectileSpeed;
        weaponProjectilesSpeed[6] = PlayerWeapons.grenadeLauncherProjectileSpeed;

        weaponScatterAngles[0] = PlayerWeapons.pistolScatterAngle;
        weaponScatterAngles[1] = PlayerWeapons.doublePistolScatterAngle;
        weaponScatterAngles[2] = -1;
        weaponScatterAngles[3] = PlayerWeapons.assaultRifleScatterAngle;
        weaponScatterAngles[4] = PlayerWeapons.shotgunScatterAngle;
        weaponScatterAngles[5] = PlayerWeapons.sniperRifleScatterAngle;
        weaponScatterAngles[6] = PlayerWeapons.grenadeLauncherScatterAngle;

        weaponClipAmmoCount[0] = PlayerWeapons.pistolWeaponClipAmmoCount;
        weaponClipAmmoCount[1] = PlayerWeapons.doublePistolWeaponClipAmmoCount;
        weaponClipAmmoCount[2] = -1;
        weaponClipAmmoCount[3] = PlayerWeapons.assaultRifleWeaponClipAmmoCount;
        weaponClipAmmoCount[4] = PlayerWeapons.shotgunWeaponClipAmmoCount;
        weaponClipAmmoCount[5] = PlayerWeapons.sniperRifleWeaponClipAmmoCount;
        weaponClipAmmoCount[6] = PlayerWeapons.grenadeLauncherWeaponClipAmmoCount;

        weaponTotalAmmoCount[0] = -1;
        weaponTotalAmmoCount[1] = -1;
        weaponTotalAmmoCount[2] = -1;
        weaponTotalAmmoCount[3] = PlayerWeapons.assaultRifleWeaponTotalAmmoCount;
        weaponTotalAmmoCount[4] = PlayerWeapons.shotgunWeaponTotalAmmoCount;
        weaponTotalAmmoCount[5] = PlayerWeapons.sniperRifleWeaponTotalAmmoCount;
        weaponTotalAmmoCount[6] = PlayerWeapons.grenadeLauncherWeaponTotalAmmoCount;

        weaponReloadingCooldown[0] = PlayerWeapons.pistolReloadingCooldown;
        weaponReloadingCooldown[1] = PlayerWeapons.doublePistolReloadingCooldown;
        weaponReloadingCooldown[2] = -1;
        weaponReloadingCooldown[3] = PlayerWeapons.assaultRifleReloadingCooldown;
        weaponReloadingCooldown[4] = PlayerWeapons.shotgunReloadingCooldown;
        weaponReloadingCooldown[5] = PlayerWeapons.sniperRifleReloadingCooldown;
        weaponReloadingCooldown[6] = PlayerWeapons.grenadeLauncherReloadingCooldown;

        weaponAmmoDropCount[0] = PlayerWeapons.pistolAmmoDropCount;
        weaponAmmoDropCount[1] = PlayerWeapons.doublePistolAmmoDropCount;
        weaponAmmoDropCount[2] = PlayerWeapons.katanaAmmoDropCount;
        weaponAmmoDropCount[3] = PlayerWeapons.assaultRifleAmmoDropCount;
        weaponAmmoDropCount[4] = PlayerWeapons.shotgunAmmoDropCount;
        weaponAmmoDropCount[5] = PlayerWeapons.sniperRifleAmmoDropCount;
        weaponAmmoDropCount[6] = PlayerWeapons.grenadeLauncherAmmoDropCount;

        weaponPrices[0] = PlayerWeapons.pistolPrice;
        weaponPrices[1] = PlayerWeapons.doublePistolPrice;
        weaponPrices[2] = PlayerWeapons.katanaPrice;
        weaponPrices[3] = PlayerWeapons.assaultRiflePrice;
        weaponPrices[4] = PlayerWeapons.shotgunPrice;
        weaponPrices[5] = PlayerWeapons.sniperRiflePrice;
        weaponPrices[6] = PlayerWeapons.grenadeLauncherPrice;

        playerBodyAnimations[0] = PlayerWeapons.playerPistolBodyAnimation;
        playerBodyAnimations[1] = PlayerWeapons.playerDoublePistolBodyAnimation;
        playerBodyAnimations[2] = PlayerWeapons.playerKatanaBodyAnimation;
        playerBodyAnimations[3] = PlayerWeapons.playerAssaultRifleBodyAnimation;
        playerBodyAnimations[4] = PlayerWeapons.playerShotgunBodyAnimation;
        playerBodyAnimations[5] = PlayerWeapons.playerSniperRifleBodyAnimation;
        playerBodyAnimations[6] = PlayerWeapons.playerGrenadeLauncherBodyAnimation;

        playerIdleAnimations[0] = PlayerWeapons.playerPistolIdleAnimation;
        playerIdleAnimations[1] = PlayerWeapons.playerDoublePistolIdleAnimation;
        playerIdleAnimations[2] = PlayerWeapons.playerKatanaIdleAnimation;
        playerIdleAnimations[3] = PlayerWeapons.playerAssaultRifleIdleAnimation;
        playerIdleAnimations[4] = PlayerWeapons.playerShotgunIdleAnimation;
        playerIdleAnimations[5] = PlayerWeapons.playerSniperRifleIdleAnimation;
        playerIdleAnimations[6] = PlayerWeapons.playerGrenadeLauncherIdleAnimation;

        playerAttackAnimations[0] = PlayerWeapons.playerPistolAttackAnimation;
        playerAttackAnimations[1] = PlayerWeapons.playerDoublePistolAttackAnimation;
        playerAttackAnimations[2] = PlayerWeapons.playerKatanaAttackAnimation;
        playerAttackAnimations[3] = PlayerWeapons.playerAssaultRifleAttackAnimation;
        playerAttackAnimations[4] = PlayerWeapons.playerShotgunAttackAnimation;
        playerAttackAnimations[5] = PlayerWeapons.playerSniperRifleAttackAnimation;
        playerAttackAnimations[6] = PlayerWeapons.playerGrenadeLauncherAttackAnimation;

        playerLegsWalkingAnimations[0] = PlayerWeapons.playerPistolLegsWalkingAnimation;
        playerLegsWalkingAnimations[1] = PlayerWeapons.playerDoublePistolLegsWalkingAnimation;
        playerLegsWalkingAnimations[2] = PlayerWeapons.playerKatanaLegsWalkingAnimation;
        playerLegsWalkingAnimations[3] = PlayerWeapons.playerAssaultRifleLegsWalkingAnimation;
        playerLegsWalkingAnimations[4] = PlayerWeapons.playerShotgunLegsWalkingAnimation;
        playerLegsWalkingAnimations[5] = PlayerWeapons.playerSniperRifleLegsWalkingAnimation;
        playerLegsWalkingAnimations[6] = PlayerWeapons.playerGrenadeLauncherLegsWalkingAnimation;

        playerAdditionalIdleAnimations[0] = null;
        playerAdditionalIdleAnimations[1] = null;
        playerAdditionalIdleAnimations[2] = PlayerWeapons.playerAdditionalKatanaIdleAnimation;
        playerAdditionalIdleAnimations[3] = null;
        playerAdditionalIdleAnimations[4] = null;
        playerAdditionalIdleAnimations[5] = null;
        playerAdditionalIdleAnimations[6] = null;

        playerAdditionalAttackAnimations[0] = null;
        playerAdditionalAttackAnimations[1] = null;
        playerAdditionalAttackAnimations[2] = PlayerWeapons.playerAdditionalKatanaAttackAnimation;
        playerAdditionalAttackAnimations[3] = null;
        playerAdditionalAttackAnimations[4] = null;
        playerAdditionalAttackAnimations[5] = null;
        playerAdditionalAttackAnimations[6] = null;

        playerAdditionalLegsWalkingAnimations[0] = null;
        playerAdditionalLegsWalkingAnimations[1] = null;
        playerAdditionalLegsWalkingAnimations[2] = PlayerWeapons.playerAdditionalKatanaLegsWalkingAnimation; 
        playerAdditionalLegsWalkingAnimations[3] = null;
        playerAdditionalLegsWalkingAnimations[4] = null;
        playerAdditionalLegsWalkingAnimations[5] = null;
        playerAdditionalLegsWalkingAnimations[6] = null;

        shotgunProjectiles = 3;
    }
}

