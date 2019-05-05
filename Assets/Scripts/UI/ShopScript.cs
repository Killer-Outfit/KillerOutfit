using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour, IPointerEnterHandler
{

    Button selectedItem;
    Text itemName;
    Text itemDesc;
    Image itemVid;
    int[,] itemArr;

    // Start is called before the first frame update
    void Start()
    {
        itemArr = new int[3,3];
        itemName = GetComponent<Text>();
        itemDesc = GetComponent<Text>();
        itemVid = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
}
