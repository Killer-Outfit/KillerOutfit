using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    FMOD.Studio.Bus bus;

    private GameObject OverMind;
    // Start is called before the first frame update
    void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        OverMind = GameObject.Find("Overmind");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restartLevel()
    {
        bus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FMODUnity.RuntimeManager.MuteAllEvents(false);
        Time.timeScale = 1.0f;
    }

    public void restartCheckpoint()
    {
        OverMind.GetComponent<CheckpointManager>().restartAtCheckpoint();
    }
}
