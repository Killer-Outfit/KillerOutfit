using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void startFirstLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ReturntoMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
