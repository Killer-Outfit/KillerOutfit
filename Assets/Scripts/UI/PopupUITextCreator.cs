using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUIText : MonoBehaviour
{
    private Dictionary<string, Color> colors;
    private Dictionary<string, GameObject> Targets;
    private Dictionary<string, string> directions;

    [HideInInspector]
    public Text pop;
    public Text t;
    // Start is called before the first frame update
    void Start()
    {
        colors = new Dictionary<string, Color>();
        colors.Add("Health", new Color(212, 23, 19));
        colors.Add("Energy", new Color(148, 0, 224));
        colors.Add("Scraps", new Color(255, 237, 0));
        colors.Add("Score", new Color(18, 0, 255));

        Targets = new Dictionary<string, GameObject>();
        Targets.Add("Health", GameObject.Find("Player Health"));
        Targets.Add("Energy", GameObject.Find("Player Energy"));
        Targets.Add("Scraps", GameObject.Find("ScrapCounter"));
        Targets.Add("Score", GameObject.Find("Score"));

        directions = new Dictionary<string, string>();
        directions.Add("Health", "L");
        directions.Add("Energy", "L");
        directions.Add("Scraps", "R");
        directions.Add("Score", "R");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnText(string pointType, float value, bool positive)
    {
        pop = Instantiate(t) as Text;
        pop.transform.SetParent(GameObject.Find("Main Canvas").GetComponent<Canvas>().transform, false);
        RectTransform rectTransform;
        rectTransform = pop.GetComponent<RectTransform>();
        rectTransform.localPosition = Targets[pointType].GetComponent<RectTransform>().localPosition;
        //rectTransform.sizeDelta = new Vector2(600, 200);
        if (positive)
        {
            pop.GetComponent<Text>().text = "+" + value.ToString();
        }
        else
        {
            pop.GetComponent<Text>().text = "-" + value.ToString();
        }
        pop.GetComponent<Text>().color = colors[pointType];
    }
}
