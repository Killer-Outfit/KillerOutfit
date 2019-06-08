using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuCharacter : MonoBehaviour
{
    public Material face;
    private float tiltAroundy;

    // Start is called before the first frame update
    void Start()
    {
        tiltAroundy = -35f;
    }

    // Update is called once per frame
    void Update()
    {
        // Smoothly tilts a transform towards a target rotation
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("RStick X") > 0)
        {
            tiltAroundy -= 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("RStick X") < 0)
        {
            tiltAroundy += 1;
        }


        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(0, tiltAroundy, 0);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5f);
    }

    // Change outfit function takes in the new outfit 
    public void changeOutfit(outfits newOutfit)
    {
        newOutfit.outfitMenuSkinRenderer.sharedMesh = newOutfit.outfitMesh;
        if (newOutfit.outfitType == "Misc")
        {
            Material[] mats = new Material[2];
            //mats[0] = face;
            mats[0] = newOutfit.outfitMaterial;
            mats[1] = newOutfit.outfitMaterial;
            newOutfit.outfitMenuSkinRenderer.materials = mats;
        }
        else
        {
            newOutfit.outfitMenuSkinRenderer.material = newOutfit.outfitMaterial;
        }
    }
}
