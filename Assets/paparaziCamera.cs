using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paparaziCamera : MonoBehaviour
{
    public void snap(Color snapColor, float spotAngle)
    {
        GetComponent<Light>().color = snapColor;
        GetComponent<Light>().spotAngle = spotAngle;
        StartCoroutine("flash");
    }
    private IEnumerator flash()
    {
        Debug.Log(GetComponent<Light>().enabled);
        GetComponent<Light>().enabled = true;
        Debug.Log(GetComponent<Light>().enabled);
        yield return new WaitForSeconds(.2f);
        GetComponent<Light>().enabled = false;
        Debug.Log(GetComponent<Light>().enabled);
    }
}
