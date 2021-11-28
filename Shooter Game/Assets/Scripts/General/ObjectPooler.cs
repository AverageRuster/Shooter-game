using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //enemies
    private static List<GameObject> walkers = null;
    public GameObject walker = null;
    public int walkersToPool = 0;

    private static List<GameObject> strikers = null;
    public GameObject striker = null;
    public int strikersToPool = 0;

    private static List<GameObject> berserks = null;
    public GameObject berserk = null;
    public int berserksToPool = 0;

    private static List<GameObject> kamikazes = null;
    public GameObject kamikaze = null;
    public int kamikazesToPool = 0;

    private static List<GameObject> cutters = null;
    public GameObject cutter = null;
    public int cuttersToPool = 0;

    private static List<GameObject> grenadiers = null;
    public GameObject grenadier = null;
    public int grenadiersToPool = 0;

    //projectiles
    private static List<GameObject> playerGunProjectiles = null;
    public GameObject playerGunProjectile = null;
    public int playerGunProjectilesToPool = 0;

    private static List<GameObject> explosivePlayerGunProjectiles = null;
    public GameObject explosivePlayerGunProjectile = null;
    public int explosivePlayerGunProjectilesToPool = 0;

    private static List<GameObject> strikerProjectiles = null;
    public GameObject strikerProjectile = null;
    public int strikerProjectilesToPool = 0;

    private static List<GameObject> grenadeProjectiles = null;
    public GameObject grenadeProjectile = null;
    public int grenadeProjectilesToPool = 0;

    /*
    //drop 
    private static List<GameObject> ammoDrops = null;
    public GameObject ammoDrop = null;
    public int ammoDropsToPool = 0;

    private static List<GameObject> immortalityDrops = null;
    public GameObject immortalityDrop = null;
    public int immortalityDropsToPool = 0;

    private static List<GameObject> healthDrops = null;
    public GameObject healthDrop = null;
    public int healthDropsToPool = 0;

    
    */

    //floor
    private static List<GameObject> floors = null;
    public GameObject floor = null;
    public int floorToPool = 0;

    //souls
    private static List<GameObject> souls = null;
    public GameObject soul = null;
    public int soulsToPool = 0;


    private void Start()
    {
        walkers = new List<GameObject>();
        strikers = new List<GameObject>();
        berserks = new List<GameObject>();
        kamikazes = new List<GameObject>();
        cutters = new List<GameObject>();
        grenadiers = new List<GameObject>();
        playerGunProjectiles = new List<GameObject>();
        explosivePlayerGunProjectiles = new List<GameObject>();
        strikerProjectiles = new List<GameObject>();
        grenadeProjectiles = new List<GameObject>();
        //ammoDrops = new List<GameObject>();
        //immortalityDrops = new List<GameObject>();
        //healthDrops = new List<GameObject>();
        souls = new List<GameObject>();
        floors = new List<GameObject>();

        InstantiateEnemies();
        InstantiateProjectiles();
        //InstantiateDrops();
        InstantiateFloor();
        InstantiateSouls();
    }

    //enemies
    public static GameObject GetPooledEnemy(int enemyType)
    {
        if (enemyType == 0)
        {
            for (int i = 0; i < walkers.Count; i++)
            {
                if (!walkers[i].activeInHierarchy)
                {
                    return walkers[i];
                }
            }
        }
        if (enemyType == 1)
        {
            for (int i = 0; i < strikers.Count; i++)
            {
                if (!strikers[i].activeInHierarchy)
                {
                    return strikers[i];
                }
            }
        }
        else if (enemyType == 2)
        {
            for (int i = 0; i < berserks.Count; i++)
            {
                if (!berserks[i].activeInHierarchy)
                {
                    return berserks[i];
                }
            }
        }
        else if (enemyType == 3)
        {
            for (int i = 0; i < kamikazes.Count; i++)
            {
                if (!kamikazes[i].activeInHierarchy)
                {
                    return kamikazes[i];
                }
            }
        }
        else if (enemyType == 4)
        {
            for (int i = 0; i < grenadiers.Count; i++)
            {
                if (!grenadiers[i].activeInHierarchy)
                {
                    return grenadiers[i];
                }
            }
        }
        else if (enemyType == 5)
        {
            for (int i = 0; i < cutters.Count; i++)
            {
                if (!cutters[i].activeInHierarchy)
                {
                    return cutters[i];
                }
            }
        }

        return null;
    }

    private void InstantiateEnemies()
    {
        for (int i = 0; i < walkersToPool; i++)
        {
            GameObject obj = Instantiate(walker);
            obj.SetActive(false);
            walkers.Add(obj);
        }
        for (int i = 0; i < strikersToPool; i++)
        {
            GameObject obj = Instantiate(striker);
            obj.SetActive(false);
            strikers.Add(obj);
        }
        for (int i = 0; i < berserksToPool; i++)
        {
            GameObject obj = Instantiate(berserk);
            obj.SetActive(false);
            berserks.Add(obj);
        }
        for (int i = 0; i < kamikazesToPool; i++)
        {
            GameObject obj = Instantiate(kamikaze);
            obj.SetActive(false);
            kamikazes.Add(obj);
        }
        for (int i = 0; i < cuttersToPool; i++)
        {
            GameObject obj = Instantiate(cutter);
            obj.SetActive(false);
            cutters.Add(obj);
        }
        for (int i = 0; i < grenadiersToPool; i++)
        {
            GameObject obj = Instantiate(grenadier);
            obj.SetActive(false);
            grenadiers.Add(obj);
        }
    }

    //projectiles
    public static GameObject GetPooledPlayerProjectile(int projectleType)
    {
        if (projectleType == 0)
        {
            for (int i = 0; i < playerGunProjectiles.Count; i++)
            {
                if (!playerGunProjectiles[i].activeInHierarchy)
                {
                    return playerGunProjectiles[i];
                }
            }
        }
        if (projectleType == 1)
        {
            for (int i = 0; i < explosivePlayerGunProjectiles.Count; i++)
            {
                if (!explosivePlayerGunProjectiles[i].activeInHierarchy)
                {
                    return explosivePlayerGunProjectiles[i];
                }
            }
        }
        if (projectleType == 2)
        {
            for (int i = 0; i < grenadeProjectiles.Count; i++)
            {
                if (!grenadeProjectiles[i].activeInHierarchy)
                {
                    return grenadeProjectiles[i];
                }
            }
        }
        return null;
    }

    public static GameObject GetPooledEnemyProjectile(int projectleType)
    {
        if (projectleType == 0)
        {
            for (int i = 0; i < strikerProjectiles.Count; i++)
            {
                if (!strikerProjectiles[i].activeInHierarchy)
                {
                    return strikerProjectiles[i];
                }
            }
        }
        else if (projectleType == 1)
        {
            for (int i = 0; i < grenadeProjectiles.Count; i++)
            {
                if (!grenadeProjectiles[i].activeInHierarchy)
                {
                    return grenadeProjectiles[i];
                }
            }
        }
        return null;
    }

    private void InstantiateProjectiles()
    {
        for (int i = 0; i < playerGunProjectilesToPool; i++)
        {
            GameObject obj = Instantiate(playerGunProjectile);
            obj.SetActive(false);
            playerGunProjectiles.Add(obj);
        }
        for (int i = 0; i < explosivePlayerGunProjectilesToPool; i++)
        {
            GameObject obj = Instantiate(explosivePlayerGunProjectile);
            obj.SetActive(false);
            explosivePlayerGunProjectiles.Add(obj);
        }
        for (int i = 0; i < strikerProjectilesToPool; i++)
        {
            GameObject obj = Instantiate(strikerProjectile);
            obj.SetActive(false);
            strikerProjectiles.Add(obj);
        }
        for (int i = 0; i < grenadeProjectilesToPool; i++)
        {
            GameObject obj = Instantiate(grenadeProjectile);
            obj.SetActive(false);
            grenadeProjectiles.Add(obj);
        }
    }

    /*
    //drop
    private void InstantiateDrops()
    {
        for (int i = 0; i < ammoDropsToPool; i++)
        {
            GameObject obj = Instantiate(ammoDrop);
            obj.SetActive(false);
            ammoDrops.Add(obj);
        }
        for (int i = 0; i < immortalityDropsToPool; i++)
        {
            GameObject obj = Instantiate(immortalityDrop);
            obj.SetActive(false);
            immortalityDrops.Add(obj);
        }
        for (int i = 0; i < healthDropsToPool; i++)
        {
            GameObject obj = Instantiate(healthDrop);
            obj.SetActive(false);
            healthDrops.Add(obj);
        }
        for (int i = 0; i < soulsToPool; i++)
        {
            GameObject obj = Instantiate(soul);
            obj.SetActive(false);
            souls.Add(obj);
        }
    }

    public static GameObject GetPooledDrop(int dropType)
    {
        if (dropType == 0)
        {
            for (int i = 0; i < ammoDrops.Count; i++)
            {
                if (!ammoDrops[i].activeInHierarchy)
                {
                    return ammoDrops[i];
                }
            }
        }
        if (dropType == 1)
        {
            for (int i = 0; i < immortalityDrops.Count; i++)
            {
                if (!immortalityDrops[i].activeInHierarchy)
                {
                    return immortalityDrops[i];
                }
            }
        }
        if (dropType == 2)
        {
            for (int i = 0; i < healthDrops.Count; i++)
            {
                if (!healthDrops[i].activeInHierarchy)
                {
                    return healthDrops[i];
                }
            }
        }
        return null;
    }
    */
    //floor
    private void InstantiateFloor()
    {
        for (int i = 0; i < floorToPool; i++)
        {
            GameObject obj = Instantiate(floor);
            obj.SetActive(false);
            floors.Add(obj);
        }
    }

    public static GameObject GetPooledFloor()
    {

            for (int i = 0; i < floors.Count; i++)
            {
                if (!floors[i].activeInHierarchy)
                {
                    return floors[i];
                }
            }
        

        return null;
    }

    //soul
    private void InstantiateSouls()
    {
        for (int i = 0; i < soulsToPool; i++)
        {
            GameObject obj = Instantiate(soul);
            obj.SetActive(false);
            souls.Add(obj);
        }
    }

    public static GameObject GetPooledSoul()
    {

        for (int i = 0; i < souls.Count; i++)
        {
            if (!souls[i].activeInHierarchy)
            {
                return souls[i];
            }
        }


        return null;
    }
}
