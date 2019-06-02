using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    private GameObject player;

    private RawImage fadeImage;

    [SerializeField]
    private float delay;

    [SerializeField]
    private Color Alpha0;

    [SerializeField]
    private Color Alpha1;

    void Start()
    {
        player = GameObject.Find("PlayerBody");
        fadeImage = GameObject.Find("BlackStart").GetComponent<RawImage>();
        StartCoroutine(FadeIn());
    }

    //Fades the level out
    private IEnumerator FadeOut()
    {
        while (fadeImage.color.a < 1)
        {
            fadeImage.color = Color.Lerp(fadeImage.color, Alpha1, delay * Time.deltaTime);
            yield return null;
        }
    }

    //Fades the level in
    private IEnumerator FadeIn()
    {
        //Debug.Log(player.GetComponent<playerMove>().active);
        while (fadeImage.color.a > 0.1f)
        {
            fadeImage.color = Color.Lerp(fadeImage.color, Alpha0, delay * Time.deltaTime);
            Debug.Log(fadeImage.color.a);
            yield return null;
        }
        fadeImage.color = Alpha0;
        //Debug.Log(player.GetComponent<playerMove>().active);
        player.GetComponent<playerNew>().input = true;
        player.GetComponent<playerMove>().input = true;
        Debug.Log(player.GetComponent<playerMove>().active);
        yield return null;   
    }
}
