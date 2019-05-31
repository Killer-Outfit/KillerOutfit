using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    CanvasGroup canvas;
    Button startButton;
    Button quitButton;
    GameObject title;
    bool fadeDone;
    bool nextScene;
    GameObject[] cutscenes = new GameObject[6];

    void Awake()
    {
        Time.timeScale = 1.0f;
        canvas = GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>();
        startButton = GameObject.Find("Start Game (Button)").GetComponent<Button>();
        quitButton = GameObject.Find("Exit (Button)").GetComponent<Button>();
        title = GameObject.Find("Title image - Orange_green");
        cutscenes[0] = GameObject.Find("sketch1");
        cutscenes[1] = GameObject.Find("sketch2");
        cutscenes[2] = GameObject.Find("sketch3");
        cutscenes[3] = GameObject.Find("sketch4");
        cutscenes[4] = GameObject.Find("sketch5");
        cutscenes[5] = GameObject.Find("sketch6");
        foreach (GameObject i in cutscenes)
            i.SetActive(false);
        fadeDone = true;
        nextScene = false;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            nextScene = true;
        }
    }

    public void startFirstLevel()
    {
        startButton.interactable = false;
        quitButton.interactable = false;
        title.SetActive(false);

        //StartCoroutine(FadeOut());
        StartCoroutine(PlayCutscene());
    }

    public void ReturntoMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeOut()
    {
        startButton.interactable = false;
        quitButton.interactable = false;
        while (canvas.alpha > 0)
        {
            Debug.Log("alpha is " + canvas.alpha);
            canvas.alpha -= Time.deltaTime / 2;
            Debug.Log("1/2 delta time is " + Time.deltaTime / 2);
            if(canvas.alpha <= 0)
                fadeDone = true;
            yield return null;
        }
    }

    private IEnumerator PlayCutscene()
    {
        Debug.Log("cutscene started");
        for (int i = 0; i < 6; i++)
        {
            Debug.Log("cutscene " + i + " active");
            cutscenes[i].SetActive(true);
            nextScene = false;
            yield return NextCutscene();
            if (i == 5)
            {
                Debug.Log("reached end of cutscenes");
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                cutscenes[i].SetActive(false);
            }
        }
    }

    private IEnumerator NextCutscene()
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            Debug.Log("is key pressed?: " + nextScene);
            //yield return new WaitForSeconds(3);
            if (nextScene)
            {
                nextScene = false;
                done = true; // breaks the loop
            }
            yield return null;
        }
    }
}
