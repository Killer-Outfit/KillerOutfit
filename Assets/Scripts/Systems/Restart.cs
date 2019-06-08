using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        if (OverMind.GetComponent<CheckpointManager>().continueGame()) { }
        else
        {
            Debug.Log("not enough score");
            StartCoroutine("changeText");
        }
    }

    IEnumerator changeText()
    {
        GameObject.Find("ContText").GetComponent<Text>().text = "Not Enough Score";
        yield return new WaitForSeconds(.5f);
        GameObject.Find("ContText").GetComponent<Text>().text = "Continue ? -5,000 Score";
    }
}
