using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public AudioClip clip;
    public AudioClip clip2;
    public GameObject board;

    [SerializeField]
    private GameObject[] cutscenes;

    [System.Serializable]
    public class cutsceneSub
    {
        public int parentScene;
        public GameObject[] subScenes;
    }

    public cutsceneSub sub1;
    public cutsceneSub sub2;
    public string[] cutsceneText;
    public GameObject textBox;

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
    SpriteRenderer ler_p;
    SpriteRenderer subLerp;
    SpriteRenderer[] lerps;
    float tim;
    Image background;

    private Text activeMessage;

    void Awake()
    {
        background = GameObject.Find("Image").GetComponent<Image>();
        music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Main Menu");
        music.start();
        Time.timeScale = 1.0f;
        activeMessage = GameObject.Find("sceneText").GetComponent<Text>();
        activeMessage.enabled = false;
        textBox.SetActive(false);
        canvas = GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>();
        startButton = GameObject.Find("Start Game (Button)").GetComponent<Button>();
        start = GameObject.Find("Start Game (Button)");
        quitButton = GameObject.Find("Exit (Button)").GetComponent<Button>();
        quit = GameObject.Find("Exit (Button)");
        title = GameObject.Find("Title image - Orange_green");
        source = GameObject.Find("MainMenuCanvas").GetComponent<AudioSource>();
        ler_p = GameObject.Find(cutscenes[1].name).GetComponent<SpriteRenderer>();
        foreach (GameObject i in cutscenes)
            i.SetActive(false);
        foreach (GameObject i in sub1.subScenes)
            i.SetActive(false);
        foreach (GameObject i in sub2.subScenes)
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
        board.SetActive(false);
        background.color = Color.black;

        //StartCoroutine(LoadAsync());
        StartCoroutine(PlayCutscene());
    }

    public void ReturntoMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeOut(GameObject scene, int mainScene)
    {
        //startButton.interactable = false;
        //quitButton.interactable = false;
        tim = 0f;
        while (ler_p.color != Color.black)
        {
            if (mainScene == sub1.parentScene - 1)
            {
                for (int i = 0; i < sub1.subScenes.Length; i++)
                {
                    //Debug.Log(i);
                    lerps = new SpriteRenderer[sub1.subScenes.Length];
                    lerps[i] = sub1.subScenes[i].GetComponent<SpriteRenderer>();
                    StartCoroutine(FadeSub(lerps[i], sub1.subScenes[i]));
                    //sub1.subScenes[i].SetActive(false);
                }
            }
            if (mainScene == sub2.parentScene - 1)
            {
                for (int i = 0; i < sub2.subScenes.Length; i++)
                {
                    lerps = new SpriteRenderer[sub2.subScenes.Length];
                    lerps[i] = sub2.subScenes[i].GetComponent<SpriteRenderer>();
                    StartCoroutine(FadeSub(lerps[i], sub2.subScenes[i]));
                    //sub2.subScenes[i].SetActive(false);
                }
            }
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

    private IEnumerator FadeSub(SpriteRenderer piece, GameObject piecePart)
    {
        while (piece.color != Color.black)
        {
            //Debug.Log("alpha is " + canvas.alpha);
            piece.color = Color.Lerp(Color.white, Color.black, tim);
            //Debug.Log("time is " + tim);
            //Debug.Log("1/2 delta time is " + Time.deltaTime / 2);
            if (piece.color == Color.black)
            {
                piecePart.SetActive(false);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    private IEnumerator PlayCutscene()
    {
        activeMessage.enabled = true;
        textBox.SetActive(true);
        //Debug.Log("cutscene started");
        for (int i = 0; i < cutscenes.Length; i++)
        {
            //Debug.Log("cutscene " + i + " active");
            cutscenes[i].SetActive(true);
            //Debug.Log(ler_p);
            activeMessage.text = cutsceneText[i];
            ler_p = GameObject.Find(cutscenes[i].name).GetComponent<SpriteRenderer>();
            //Debug.Log(cutscenes[i].name);
            ler_p.color = Color.black;
            nextScene = false;
            yield return StartCoroutine(NextCutscene(i));
            if (i == cutscenes.Length-1)
            {
                activeMessage.enabled = false;
                textBox.SetActive(false);
                music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                source.PlayOneShot(clip2);
                //Debug.Log("reached end of cutscenes");
                cutsceneDone = true;
                //StartCoroutine(LoadAsync());
            }
            else
            {
                source.PlayOneShot(clip);
            }
            yield return StartCoroutine(FadeOut(cutscenes[i], i));
        }
    }

    private IEnumerator NextCutscene(int mainScene)
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

        if(mainScene == sub1.parentScene - 1)
        {
            yield return StartCoroutine(NextSubScene(sub1));
        }
        if(mainScene == sub2.parentScene - 1)
        {
            yield return StartCoroutine(NextSubScene(sub2));
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

    private IEnumerator NextSubScene(cutsceneSub sub)
    {
        for(int i = 0; i < sub.subScenes.Length; i++)
        {
            sub.subScenes[i].SetActive(true);
            subLerp = sub.subScenes[i].GetComponent<SpriteRenderer>();
            subLerp.color = Color.black;
            tim = 0f;
            while (subLerp.color != Color.white)
            {
                //Debug.Log("alpha is " + canvas.alpha);
                subLerp.color = Color.Lerp(Color.black, Color.white, tim);
                tim += Time.deltaTime;
                //Debug.Log("time is " + tim);
                if (tim > 1)
                    tim = 1;
                //Debug.Log("1/2 delta time is " + Time.deltaTime / 2);
                yield return new WaitForEndOfFrame();
            }
        }
        yield return null;
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
