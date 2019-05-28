using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private GameObject OverMind;
    // Start is called before the first frame update
    void Start()
    {
        OverMind = GameObject.Find("Overmind");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restartLevel()
    {
        /*SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FMODUnity.RuntimeManager.MuteAllEvents(false);
        Time.timeScale = 1.0f;*/
        OverMind.GetComponent<CheckpointManager>().restartAtCheckpoint();
    }
}
