using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 PlayerPos;
    private GameObject Player;
    public List<float> checkpointLocations;
    private float playerHealth;
    private int score;
    private int scraps;
    private int energy;
    private int currentCheckpoint;
    private Camera mainCam;
    private int nextCombatNumber;



    // Start is called before the first frame update
    void Start()
    {
        nextCombatNumber = 0;
        PlayerPos = new Vector3(-26.71169f, 171f, -306f);
        Player = GameObject.Find("PlayerBody");
        playerHealth = 100f;
        energy = 300;
        score = 0;
        scraps = 0;
        currentCheckpoint = 0;
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        Debug.Log(Player.transform.position.x);
        Debug.Log(checkpointLocations[currentCheckpoint]);
        if (mainCam.transform.position.x >= checkpointLocations[currentCheckpoint])
        {
            Debug.Log("Updated Checkpoints");
            PlayerPos = new Vector3(Player.transform.position.x, 171f, -306f);
            currentCheckpoint++;
            energy = Player.GetComponent<playerNew>().energy;
            playerHealth = Player.GetComponent<playerNew>().currentHealth;
            score = Player.GetComponent<playerNew>().score;
            scraps = Player.GetComponent<playerNew>().scraps;
            nextCombatNumber = this.gameObject.GetComponent<Map>().currentCombatNum;
        }
    }

    public void restartAtCheckpoint()
    {
        Player.GetComponent<playerNew>().energy = energy;
        Player.GetComponent<playerNew>().currentHealth = playerHealth;
        Player.GetComponent<playerNew>().score = score;
        Player.GetComponent<playerNew>().scraps = scraps;
        Player.transform.position = PlayerPos;
        this.gameObject.GetComponent<Map>().currentCombatNum = nextCombatNumber;
        this.gameObject.GetComponent<Map>().mainCam.GetComponent<CameraScript>().locked = false;
        Player.GetComponent<playerNew>().revive();
        this.gameObject.GetComponent<Map>().mainCam.GetComponent<CameraScript>().revive();
    }
   

}
