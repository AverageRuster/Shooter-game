using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeController : MonoBehaviour
{
    public int enemyType = 0;
    public GameObject[] enemies;

    //health
    private const float strikerHealth = 5;
    private const float kamikazeHealth = 1;
    private const float berserkHealth = 3;

    //speed
    private const float strikerSpeed = 7.5f;
    private const float kamikazeSpeed = 9;
    private const float berserkSpeed = 8;

    //damage
    private const float strikerDamage = 1;
    private const float kamikazeDamage = 10;
    private const float berserkDamage = 2;

    private void Start()
    {
        

    }

    /*
    public void ActivateEnemyType()
    {
        enemyType = Random.Range(0, 3);
        if (enemyType == 0)
        {
            SetStriker();
        }
        else if (enemyType == 1)
        {
            SetKamikaze();
        }
        else if (enemyType == 2)
        {
            SetBerserk();
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].activeSelf)
            {
                enemies[i].SetActive(false);
            }
        }
        if (!enemies[enemyType].activeSelf)
        {
            enemies[enemyType].SetActive(true);
        }
    }
    */


}
