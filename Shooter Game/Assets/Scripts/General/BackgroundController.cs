using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private const int floorTypesAmount = 13;
    public static bool isFloorUpdated = false;

    private void Start()
    {
        isFloorUpdated = false;
    }

    private void Update()
    {
        if (!isFloorUpdated && !GameManager.gameOver)
        {
            for (int i = -12; i < 13; i++)
            {
                GameObject currentFloor = null;

                for (int j = -12; j < 13; j++)
                {
                    int floorType = 0;
                    int floorTypeChance = Random.Range(1, 101);
                    currentFloor = ObjectPooler.GetPooledFloor();
                    currentFloor.transform.position = new Vector3(i * 2, j * 2);
                    currentFloor.transform.rotation = Quaternion.Euler(0, 0, 90 * Random.Range(0, 4));
                    currentFloor.SetActive(true);
                    if (floorTypeChance > 10)
                    {
                        floorType = Random.Range(1, 3);

                        if (currentFloor.GetComponent<Animator>().enabled)
                        {
                            currentFloor.GetComponent<Animator>().enabled = false;
                        }
                        currentFloor.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Background/FloorType" + floorType);
                       
                    }
                    else if (floorTypeChance <= 10)
                    {
                        floorType = Random.Range(1, 7);

                        if (!currentFloor.GetComponent<Animator>().enabled)
                        {
                            currentFloor.GetComponent<Animator>().enabled = true;
                        }
                        if (floorType <= 5)
                        {
                            currentFloor.GetComponent<Animator>().Play("AnimatedFloorType" + floorType + "Idle"); //= Resources.Load<Sprite>("Sprites /Background/FloorType" + floorType);
                            currentFloor.GetComponent<FloorController>().StartAnimationDelay("AnimatedFloorType" + floorType);
                        }
                        else
                        {
                            currentFloor.GetComponent<Animator>().Play("AnimatedFloorType" + floorType);
                        }

                    }

                }
            }
            isFloorUpdated = true;
        }
    }
}
