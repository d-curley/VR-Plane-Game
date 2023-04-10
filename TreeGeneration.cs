using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject treePrefab;

    public int fogX = 150;
    public int fogZ = 150;
    public float perlinNoiseThreshold = .85f;
    

    void Start()
    {
        // look up proc placement script
        // something like choosing x and z between -50 and 50
        // could theoretically make it self away and look for proximity to other trees?
        // worth researching, and I don't want to do that right now

        // Then base alpha material value based on distance from 0,0

        // after that, make the prefab itself proc gen, building itself

        float maxD = Vector2.Distance(new Vector2(0, 0), new Vector2(fogX, fogZ));

        for (int i = -fogX; i < fogX; i++) {
            for(int j = -fogZ; j < fogZ; j++) {
                
                if(-8<i && i < 8 && -8 < j && j < 8) {continue; } // don't instantiate trees too close to the user

                float iF = (i + fogX);
                float jF = (j + fogZ);
                
                float iRel = iF / 2f;
                float jRel = jF/ 2f;

                float r = Mathf.PerlinNoise(iRel, jRel);
                if(r > perlinNoiseThreshold) {
                    GameObject tree = Instantiate(treePrefab, this.transform);
                    Vector3 tempPos = new Vector3(i, tree.transform.position.y, j);
                    tree.transform.position = tempPos;
                    
                    Material mat = tree.GetComponent<Renderer>().material;
                    Color tempColor = mat.color;

                    float distanceEffect = Vector2.Distance(new Vector2(i, j), new Vector2(0f, 0f))/ maxD; 
                    tempColor.a = (1-distanceEffect)*1.5f; // amplified slightly, can expose to inspector

                    float whitout = .1f + distanceEffect * .8f;
                    if(whitout > .7f) {
                        whitout = .7f;
                    }
                    tempColor.r = whitout;
                    tempColor.g = whitout;
                    tempColor.b = whitout;


                    mat.color = tempColor;
                }
            }
        }
    }
}
