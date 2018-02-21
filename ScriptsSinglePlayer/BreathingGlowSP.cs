
using UnityEngine;
using System.Collections;

public class BreathingGlowSP : MonoBehaviour
{
    public Material glowingMaterial;
    private Color colourChange;
    public bool isGrowing = true;
    public bool isShrinking = false;


    // Use this for initialization
    void Start()
    {
        Color colourChange = new Color(0.0f, 0.0f, 0.0f);
        glowingMaterial.color = colourChange;
    }

    // Update is called once per frame
    void Update()
    {
        glowingMaterial.color = colourChange;
        StartCoroutine("UpdateColour");
    }

    private IEnumerator UpdateColour()
    {
       
        if (colourChange.r > 5.0f)
        {
            isShrinking = true;
            isGrowing = false;
        }
        if (colourChange.r < 0.001f)
        {
            isGrowing = true;
            isShrinking = false;
        }

        if (isGrowing == true)
        {
            colourChange.r += 0.035f;
            colourChange.g += 0.035f;
            colourChange.b += 0.02f;
        }
        else
        {
            colourChange.r -= 0.035f;
            colourChange.g -= 0.035f;
            colourChange.b -= 0.02f;
        }

        yield return new WaitForSeconds(1.01f);

    }
}
