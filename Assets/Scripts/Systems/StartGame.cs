using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public AudioClip clip;
    public AudioClip clip2;

    FMOD.Studio.EventInstance music;

    CanvasGroup canvas;
    Button startButton;
    Button quitButton;
    GameObject start;
    GameObject quit;
    GameObject title;
    bool fadeDone;
    bool nextScene;
    bool cutsceneDone;
    AudioSource source;
    GameObject[] cutscenes = new GameObject[6];
    SpriteRenderer ler_p;
    float tim;

    void Awake()
    {
        music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Main Menu");
        music.start();
        Time.timeScale = 1.0f;
        canvas = GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>();
        startButton = GameObject.Find("Start Game (Button)").GetComponent<Button>();
        start = GameObject.Find("Start Game (Button)");
        quitButton = GameObject.Find("Exit (Button)").GetComponent<Button>();
        quit = GameObject.Find("Exit (Button)");
        title = GameObject.Find("Title image - Orange_green");
        cutscenes[0] = GameObject.Find("sketch1");
        cutscenes[1] = GameObject.Find("sketch2");
        cutscenes[2] = GameObject.Find("sketch3");
        cutscenes[3] = GameObject.Find("sketch4");
        cutscenes[4] = GameObject.Find("sketch5");
        cutscenes[5] = GameObject.Find("sketch6");
        source = GameObject.Find("MainMenuCanvas").GetComponent<AudioSource>();
        ler_p = GameObject.Find(cutscenes[1].name).GetComponent<SpriteRenderer>();
        foreach (GameObject i in cutscenes)
            i.SetActive(false);
        fadeDone = false;
        nextScene = false;
        cutsceneDone = false;
        tim = 0f;
    }

    void Update()
    {
        //tim += Time.deltaTime;
        if (Input.anyKeyDown)
        {
            nextScene = true;
        }
    }

    public void startFirstLevel()
    {
        //startButton.interactable = false;
        //quitButton.interactable = false;
        start.SetActive(false);
        quit.SetActive(false);
        title.SetActive(false);

        //StartCoroutine(LoadAsync());
        StartCoroutine(PlayCutscene());
    }

    public void ReturntoMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeOut(GameObject scene)
    {
        //startButton.interactable = false;
        //quitButton.interactable = false;
        tim = 0f;
        while (ler_p.color != Color.black)
        {
            //Debug.Log("alpha is " + canvas.alpha);
            ler_p.color = Color.Lerp(Color.white, Color.black, tim);
            tim += Time.deltaTime;
            if (tim > 1)
                tim = 1;
            //Debug.Log("1/2 delta time is " + Time.deltaTime / 2);
            if(ler_p.color == Color.black)
            {
                scene.SetActive(false);
                fadeDone = true;
            }
            yield return new WaitForEndOfFrame();
        }
        if (cutsceneDone)
            StartCoroutine(LoadAsync());
        yield return null;
    }

    private IEnumerator PlayCutscene()
    {
        Debug.Log("cutscene started");
        for (int i = 0; i < 6; i++)
        {
            Debug.Log("cutscene " + i + " active");
            cutscenes[i].SetActive(true);
            //Debug.Log(ler_p);
            ler_p = GameObject.Find(cutscenes[i].name).GetComponent<SpriteRenderer>();
            Debug.Log(cutscenes[i].name);
            ler_p.color = Color.black;
            nextScene = false;
            yield return StartCoroutine(NextCutscene());
            if (i == cutscenes.Length-1)
            {
                music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                source.PlayOneShot(clip2);
                Debug.Log("reached end of cutscenes");
                cutsceneDone = true;
                //StartCoroutine(LoadAsync());
            }
            else
            {
                source.PlayOneShot(clip);
            }
            yield return StartCoroutine(FadeOut(cutscenes[i]));
        }
    }

    private IEnumerator NextCutscene()
    {
        tim = 0f;
        while (ler_p.color != Color.white)
        {
            //Debug.Log("alpha is " + canvas.alpha);
            ler_p.color = Color.Lerp(Color.black, Color.white, tim);
            tim += Time.deltaTime;
            //Debug.Log("time is " + tim);
            if (tim > 1)
                tim = 1;
            //Debug.Log("1/2 delta time is " + Time.deltaTime / 2);
            yield return new WaitForEndOfFrame();
        }

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

    private IEnumerator LoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
