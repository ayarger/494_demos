using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterScreenEffect : MonoBehaviour
{
    public Material water_screen_effect_material;

    /* This function allows us to "intercept" the camera's shot before it gets renderer to the screen.
     * The camera's image is represented by "src" parameter. The screen's image is represented by the "dest" parameter.
     * The "Blit" function allows us to copy one image to another while APPLYING A MATERIAL (and therefore a shader).*/
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, water_screen_effect_material);
    }
}
