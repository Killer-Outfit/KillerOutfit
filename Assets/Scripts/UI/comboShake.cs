using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class comboShake : MonoBehaviour
{
    private float red;
    private float green;
    private float blue;
    private float curTime;
    private Text currentText;
    private float shakePower;
    private Vector3 startPos;
    private int combo;

    // Start is called before the first frame update
    void Start()
    {
        shakePower = 0;
        resetColor();
        curTime = 0;
        currentText = this.gameObject.GetComponent<Text>();
        startPos = transform.position;
        StartCoroutine("shake");
        combo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime >= 10)
        {
            pop();
        }
        if (combo > 0)
        {
            currentText.text = "x" + combo.ToString();
        }
        else
        {
            pop();
        }

        this.gameObject.GetComponent<Outline>().effectColor = new Color(red, green, blue);
    }

    private void pop()
    {
        currentText.text = "";
        resetColor();
    }

    public void changeCombo(int com)
    {
        combo = com;
        if (red < 255)
        {
            red += combo * 5;
        }
        else
        {
            green -= combo * 3;
        }
        if (combo % 5 == 0)
        {
            shakePower = combo;
        }
    }

    IEnumerable shake()
    {
        for (int i = 1; i < 11; i++)
        {
            if (i % 2 == 0)
            {
                transform.Translate(-1 * startPos);
            }
            else
            {
                startPos = new Vector3(Random.Range(-1, 2) * shakePower, Random.Range(-1, 2) * shakePower, 0);
                transform.Translate(startPos);
            }
            yield return new WaitForSeconds(0.001f);
        }
        restartShake();
        yield return null;
    }

    private void restartShake()
    {
        StartCoroutine("shake");
    }

    public void resetTime()
    {
        curTime = 0;
    }

    private void resetColor()
    {
        red = 0f;
        green = 255f;
        blue = 0f;
    }
}
