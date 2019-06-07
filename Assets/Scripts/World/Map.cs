using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Camera mainCam;
    private Transform pT;
    public int currentCombatNum;
    private GameObject enemyManager;

    public float[] combatPositions;
    public GameObject[] Encounter1 = new GameObject[6];
    public GameObject[] Encounter2 = new GameObject[6];
    public GameObject[] Encounter3 = new GameObject[6];
    public GameObject[] Encounter4 = new GameObject[6];
    public GameObject[] Encounter5 = new GameObject[6];
    public GameObject[] Encounter6 = new GameObject[6];
    public GameObject[] Encounter7 = new GameObject[6];
    public GameObject[] Encounter8 = new GameObject[6];
    public GameObject[] Encounter9 = new GameObject[6];
    public GameObject[] Encounter10 = new GameObject[6];
    public GameObject[] Encounter11 = new GameObject[6];
    public GameObject[] Encounter12 = new GameObject[6];
    public GameObject[] Encounter13 = new GameObject[6];
    public GameObject[] Encounter14 = new GameObject[6];
    public GameObject[] Encounter15 = new GameObject[6];
    public GameObject[] Encounter16 = new GameObject[6];
    public GameObject[] Encounter17 = new GameObject[6];
    public GameObject[] Encounter18 = new GameObject[6];
    public GameObject[] Encounter19 = new GameObject[6];
    public GameObject[] Encounter20 = new GameObject[6];

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        pT = GameObject.Find("PlayerBody").transform;
        enemyManager = GameObject.Find("Overmind");
        currentCombatNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (combatPositions.Length > 0 && !mainCam.GetComponent<CameraScript>().locked)
        {
            if (mainCam.transform.position.x >= combatPositions[currentCombatNum]) 
            {
                mainCam.GetComponent<CameraScript>().locked = true;
                combatEvent(currentCombatNum);
                currentCombatNum += 1;
            }
        }else if(mainCam.GetComponent<CameraScript>().locked == true)
        {
            if(enemyManager.GetComponent<Overmind>().areThereEnemies())
            {
                mainCam.GetComponent<CameraScript>().locked = false;
            }
        }
    }

    public void combatEvent(int num)
    {
        if (num == 0)
        {
            SpawnWave(Encounter1);
        }
        else if (num == 1)
        {
            SpawnWave(Encounter2);
        }
        else if (num == 2)
        {
            SpawnWave(Encounter3);
        }
        else if (num == 3)
        {
            SpawnWave(Encounter4);
        }
        else if (num == 4)
        {
            SpawnWave(Encounter5);
        }
        else if (num == 5)
        {
            SpawnWave(Encounter6);
        }
        else if (num == 6)
        {
            SpawnWave(Encounter7);
        }
        else if (num == 7)
        {
            SpawnWave(Encounter8);
        }
        else if (num == 8)
        {
            SpawnWave(Encounter9);
        }
        else if (num == 9)
        {
            SpawnWave(Encounter10);
        }
        if (num == 10)
        {
            SpawnWave(Encounter11);
        }
        else if (num == 11)
        {
            SpawnWave(Encounter12);
        }
        else if (num == 12)
        {
            SpawnWave(Encounter13);
        }
        else if (num == 13)
        {
            SpawnWave(Encounter14);
        }
        else if (num == 14)
        {
            SpawnWave(Encounter15);
        }
        else if (num == 15)
        {
            SpawnWave(Encounter16);
        }
        else if (num == 16)
        {
            SpawnWave(Encounter17);
        }
        else if (num == 17)
        {
            SpawnWave(Encounter18);
        }
        else if (num == 18)
        {
            SpawnWave(Encounter19);
        }
        else if (num == 19)
        {
            SpawnWave(Encounter20);
        }
    }

    private void SpawnWave(GameObject[] arr)
    {
        if (arr[0] != null) { Instantiate(arr[0], new Vector3(pT.position.x - 11, 0, 0), arr[0].transform.rotation); }
        if (arr[1] != null) { Instantiate(arr[1], new Vector3(pT.position.x - 11, -2, -4), arr[1].transform.rotation); }
        if (arr[2] != null) { Instantiate(arr[2], new Vector3(pT.position.x - 11, -4, -6), arr[2].transform.rotation); }
        if (arr[3] != null) { Instantiate(arr[3], new Vector3(pT.position.x + 10, 0, 0), arr[3].transform.rotation); }
        if (arr[4] != null) { Instantiate(arr[4], new Vector3(pT.position.x + 10, -2, -4), arr[4].transform.rotation); }
        if (arr[5] != null) { Instantiate(arr[5], new Vector3(pT.position.x + 10, -4, -6), arr[5].transform.rotation); }
    }

    public void reset()
    {
        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < currentEnemies.Length; i++)
        {
            Destroy(currentEnemies[i]);
        }

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        for (int i = 0; i < projectiles.Length; i++)
        {
            Destroy(projectiles[i]);
        }

        enemyManager.GetComponent<Overmind>().ClearLists();
    }
}
