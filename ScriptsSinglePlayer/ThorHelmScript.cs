using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorHelmScript : MonoBehaviour {

    private ArrayList childObjects = new ArrayList();
    public Material metal;
    public Material lightning;
    public Material gold;

    // Use this for initialization
    void Start () {
        foreach (Transform child in this.transform)
        {
            childObjects.Add(child);
        }

        foreach (Transform helmPiece in childObjects)
        {
            Renderer rend = helmPiece.gameObject.GetComponent<Renderer>();
            if (rend != null)
            {
               if (helmPiece.gameObject.name.Contains("Thor H0039") ||//front pieces
                    helmPiece.gameObject.name.Contains("Thor H0042") ||
                    helmPiece.gameObject.name.Contains("Thor H0052") ||
                    helmPiece.gameObject.name.Contains("Thor H0057") ||
                    helmPiece.gameObject.name.Contains("Thor H0062") ||
                    helmPiece.gameObject.name.Contains("Thor H0071") ||
                    helmPiece.gameObject.name.Contains("Thor H0081") ||
                    helmPiece.gameObject.name.Contains("Thor H0086") ||
                    helmPiece.gameObject.name.Contains("Thor H0091") || //back pieces
                    helmPiece.gameObject.name.Contains("Thor H0046") ||
                    helmPiece.gameObject.name.Contains("Thor H0056") ||
                    helmPiece.gameObject.name.Contains("Thor H0061") ||
                    helmPiece.gameObject.name.Contains("Thor H0066") ||
                    helmPiece.gameObject.name.Contains("Thor H0067") ||
                    helmPiece.gameObject.name.Contains("Thor H0077") ||
                    helmPiece.gameObject.name.Contains("Thor H0082") ||
                    helmPiece.gameObject.name.Contains("Thor H0087")) 
                {
                    rend.material = gold;
                }
                else if (helmPiece.gameObject.name.Contains("Thor H0031"))
                {
                    rend.material = lightning;
                }
                else
                {
                    rend.material = metal;
                }
                
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
