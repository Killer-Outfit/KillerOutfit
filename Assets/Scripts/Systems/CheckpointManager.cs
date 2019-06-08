using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private Vector3 camCheckpointPos;
    private GameObject checkpointText;
    private Color textColor;


    // Start is called before the first frame update
    void Start()
    {
        checkpointText = GameObject.Find("Checkpoint Text");
        textColor = checkpointText.GetComponent<Text>().color;
        camCheckpointPos = new Vector3(0f, 0f, 0f);
        nextCombatNumber = 0;
        PlayerPos = new Vector3(-26.71169f, 170f, -304f);
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
        if (mainCam.transform.position.x >= checkpointLocations[currentCheckpoint])
        {
            Debug.Log("Updated Checkpoints");
            StartCoroutine("textAppear");
            camCheckpointPos = mainCam.transform.position;
            PlayerPos = Player.transform.position;
            currentCheckpoint++;
            energy = Player.GetComponent<playerNew>().energy;
            playerHealth = Player.GetComponent<playerNew>().currentHealth;
            score = Player.GetComponent<playerNew>().score;
            scraps = Player.GetComponent<playerNew>().scraps;
            nextCombatNumber = this.gameObject.GetComponent<Map>().currentCombatNum;
        }
    }
    IEnumerator textAppear()
    {
        float t = 0f;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            textColor.a += .1f;
            checkpointText.GetComponent<Text>().color = textColor;
            Debug.Log(textColor.a);
            yield return new WaitForSeconds(.01f);
        }
        while (t < 3f)
        {
            t += Time.deltaTime;
            textColor.a -= .1f;
            checkpointText.GetComponent<Text>().color = textColor;
            yield return new WaitForSeconds(.01f);
        }
        textColor.a = 0f;
        checkpointText.GetComponent<Text>().color = textColor;
    }
    public void restartAtCheckpoint()
    {
        Player.GetComponent<playerNew>().energy = energy;
        Player.GetComponent<playerNew>().currentHealth = playerHealth;
        Player.GetComponent<playerNew>().score = score;
        Player.GetComponent<playerNew>().scraps = scraps;
        Player.transform.position = PlayerPos;
        this.gameObject.GetComponent<Map>().currentCombatNum = nextCombatNumber;
        mainCam.GetComponent<CameraScript>().locked = false;
        Player.GetComponent<playerNew>().revive();
        mainCam.GetComponent<CameraScript>().revive(camCheckpointPos);
        this.gameObject.GetComponent<Map>().reset();
    }

    public bool continueGame(){
        if (Player.GetComponent<playerNew>().spendScore(20000))
        {
            restartAtCheckpoint();
            return true;
        }
        else
        {
            return false;
        }
    }
}
