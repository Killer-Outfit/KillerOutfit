using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }else if(mainCam.GetComponent<CameraScript>().locked = true)
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
        else
        {

        }
    }

    private void SpawnWave(GameObject[] arr)
    {
        if (arr[0] != null) { Instantiate(arr[0], new Vector3(pT.position.x - 12, 0, 0), arr[0].transform.rotation); }
        if (arr[1] != null) { Instantiate(arr[1], new Vector3(pT.position.x - 12, -2, -4), arr[1].transform.rotation); }
        if (arr[2] != null) { Instantiate(arr[2], new Vector3(pT.position.x - 12, -4, -6), arr[2].transform.rotation); }
        if (arr[3] != null) { Instantiate(arr[3], new Vector3(pT.position.x + 12, 0, 0), arr[3].transform.rotation); }
        if (arr[4] != null) { Instantiate(arr[4], new Vector3(pT.position.x + 12, -2, -4), arr[4].transform.rotation); }
        if (arr[5] != null) { Instantiate(arr[5], new Vector3(pT.position.x + 12, -4, -6), arr[5].transform.rotation); }
    }

    public void Reset()
    {
        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < currentEnemies.Length; i++)
        {
            Destroy(currentEnemies[i]);
        }

        GameObject[] currentProjectiles = GameObject.FindGameObjectsWithTag("Projectile");
        for (int i = 0; i < currentProjectiles.Length; i++)
        {
            Destroy(currentProjectiles[i]);
        }
    }
}
