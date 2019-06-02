using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mannequinScript : MonoBehaviour
{
    private GameObject shop;
    private GameObject player;

    private void Start()
    {
        shop = GameObject.Find("ShopMenu");
        player = GameObject.Find("PlayerBody");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("entered");
            player.GetComponent<playerNew>().nearInteractable = true;
            shop.GetComponent<ShopControl>().openable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("exit");
            player.GetComponent<playerNew>().nearInteractable = false;
            shop.GetComponent<ShopControl>().openable = false;
        }
    }
}
