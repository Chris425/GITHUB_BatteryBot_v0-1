
using UnityEngine;
using System.Collections;

public class BreathingGlow : MonoBehaviour
{
    public Material glowingMaterial;
    private Color colourChange;
    public bool isGrowing = true;
    public bool isShrinking = false;
    // Use this for initialization
    void Start()
    {
        Color colourChange = new Color(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        glowingMaterial.color = colourChange;
        StartCoroutine("UpdateColour1"); 
    }

    private IEnumerator UpdateColour1()
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
            colourChange.r += 0.08f;
            colourChange.g += 0.08f;
            colourChange.b += 0.05f;
        }
        else
        {
            colourChange.r -= 0.08f;
            colourChange.g -= 0.08f;
            colourChange.b -= 0.05f;
        }

        yield return new WaitForSeconds(0.01f);

    }
}
