using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class shopBackground : MonoBehaviour
{
    private Camera shopCamera;
    void Awake()
    {
        shopCamera = GameObject.Find("OutfitCamera").GetComponent<Camera>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = shopCamera.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(shopCamera.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }

        transform.position = Vector2.zero; // Optional
        transform.localScale = scale;
    }
    /* private GameObject backgroundImage;
     private Camera shopCamera;

     private void Start()
     {
         backgroundImage = GameObject.Find("ShopBackground");
         shopCamera = GameObject.Find("OutfitCamera").GetComponent<Camera>();
     }

     private void Update()
     {
         ResizeSpriteToScreen(backgroundImage, shopCamera, 1, 1);
     }
     private void ResizeSpriteToScreen(GameObject theSprite, Camera theCamera, int fitToScreenWidth, int fitToScreenHeight)
     {
         SpriteRenderer sr = theSprite.GetComponent<SpriteRenderer>();

         theSprite.transform.localScale = new Vector3(1, 1, 1);

         float width = sr.sprite.bounds.size.x;
         float height = sr.sprite.bounds.size.y;

         float worldScreenHeight = (float)(theCamera.orthographicSize * 2.0);
         float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);

         if (fitToScreenWidth != 0)
         {
             Vector2 sizeX = new Vector2(worldScreenWidth / width / fitToScreenWidth, theSprite.transform.localScale.y);
             theSprite.transform.localScale = sizeX;
         }

         if (fitToScreenHeight != 0)
         {
             Vector2 sizeY = new Vector2(theSprite.transform.localScale.x, worldScreenHeight / height / fitToScreenHeight);
             theSprite.transform.localScale = sizeY;
         }
     }*/
}
