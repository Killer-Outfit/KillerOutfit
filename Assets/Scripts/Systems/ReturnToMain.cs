using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMain : MonoBehaviour
{
    FMOD.Studio.Bus masterBus;

    void Start()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    public void ReturntoMain()
    {
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene("MainMenu");
    }
}
