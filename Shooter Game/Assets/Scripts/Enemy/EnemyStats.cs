using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyStats
{
    //walker
    public const int walkerType = 0;

    public const float maxWalkerHealth = 50;
    public const float walkerDamage = 2;


    public const float maxWalkerSpeed = 9;
    
    public const int walkerDestroyPrice = 30;
    public const float walkerTakingDamageResetTime = 0.25f;
    public const float walkerAttackPreparationTime = 0.25f;
    public const float walkerAttackTime = 0.5f;
    public const float walkerAttackCooldownTime = 1;

    //striker
    public const int strikerType = 1;

    public const float maxStrikerHealth = 50;
    public const float strikerDamage = 3;


    public const float maxStrikerSpeed = 7f;
    public const int strikerProjectileType = 0;
    public const float strikerProjectileSpeed = 1000;
    public const float strikerShootingCooldown = 0.5f;
    public const int strikerDestroyPrice = 10;
    public const float strikerTakingDamageResetTime = 0.1f;

    //berserk
    public const int berserkType = 2;

    public const float maxBerserkHealth = 100;
    public const float berserkDamage = 5;


    public const float maxBerserkSpeed = 2000;
    public const float berserkMeleeCooldown = 1;
    public const int berserkDestroyPrice = 30;
    public const int berserkMaxSpeedLimit = 8;
    public const float berserkTakingDamageResetTime = 0.25f;

    //kamikaze
    public const float maxKamikazeHealth = 10;
    public const float kamikazeDamageToPlayer = 10;
    public const float kamikazeDamageToEnemy = 50;

    public const int kamikazeType = 3;    
    public const float maxKamikazeSpeed = 7;
    public const float kamikazeDetonationTime = 1;
    public const int kamikazeDestroyPrice = 50;



    //cutter
    public const int cutterType = 4;

    public const float maxCutterHealth = 100;
    public const float cutterDamage = 1;



    public const float maxCutterSpeed = 8f;
    public const float cutterRotationBlockCooldown = 0.5f;
    public const float cutterAttackCooldown = 0.5f;
    public const int cutterDestroyPrice = 20;
    public const float cutterLaserDamageCooldown = 0.1f;
    public const float cutterLaserActivasionCooldown = 0.5f;
    public const float cutterTakingDamageResetTime = 0.25f;


    //grenadier
    public const int grenadierType = 5;

    public const float maxGrenadierHealth = 50;
    public const float grenadierDamage = 10;


    public const int grenadierProjectileType = 1;
    public const float grenadierThrowingCooldown = 7.5f;
    public const float minDropChance = 100;
    public const int grenadierDestroyPrice = 30;
}
