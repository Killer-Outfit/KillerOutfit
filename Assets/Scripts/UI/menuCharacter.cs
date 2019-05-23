using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuCharacter : MonoBehaviour
{
    public Material face;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Change outfit function takes in the new outfit 
    public void changeOutfit(outfits newOutfit)
    {
        newOutfit.outfitMenuSkinRenderer.sharedMesh = newOutfit.outfitMesh;
        if (newOutfit.outfitType == "Misc")
        {
            Material[] mats = new Material[2];
            mats[0] = face;
            mats[1] = newOutfit.outfitMaterial;
            newOutfit.outfitMenuSkinRenderer.materials = mats;
        }
        else
        {
            newOutfit.outfitMenuSkinRenderer.material = newOutfit.outfitMaterial;
        }
    }
}
