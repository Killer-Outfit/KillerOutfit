using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mannequinScript : MonoBehaviour
{
    private GameObject shop;
    private GameObject player;
    private MeshRenderer textObject;

    private void Start()
    {
        textObject = transform.parent.GetChild(1).gameObject.GetComponent<MeshRenderer>(); 
        shop = GameObject.Find("ShopMenu");
        player = GameObject.Find("PlayerBody");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("entered");
            player.GetComponent<playerNew>().nearInteractable = true;
            shop.GetComponent<ShopControl>().openable = true;
            textObject.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("exit");
            player.GetComponent<playerNew>().nearInteractable = false;
            shop.GetComponent<ShopControl>().openable = false;
            textObject.enabled = false;
        }
    }
}
