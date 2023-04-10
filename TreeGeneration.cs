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
                float treeDistance = Vector2.Distance(new Vector2(i, j), new Vector2(0f, 0f));
                if(treeDistance<80) { continue; } // don't instantiate trees too close to the user

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

                    float distanceEffect = treeDistance / maxD; 
                    tempColor.a = (.8f-distanceEffect); // amplified slightly, can expose to inspector

                    float whitout = .3f + distanceEffect * .8f;
                    if(whitout > .7f) {
                        whitout = .7f;
                    }
                    tempColor.r = whitout;
                    tempColor.g = whitout;
                    tempColor.b = whitout;


                    // Use Radius of maxD
                    // See if we can check if a 2D point is within a specific arc
                    double angle = Mathf.Atan2(i, j) * 180.0 / Mathf.PI;

                    if(-30<angle && angle <30) { // 4
                        Debug.Log(Mathf.Atan2(i, j) * 180.0 / Mathf.PI);
                        tempColor.b = 0;
                        tempColor.g = 0;
                    }

                    if(30 < angle && angle < 90) { //5
                        Debug.Log(Mathf.Atan2(i, j) * 180.0 / Mathf.PI);
                        tempColor.b = 1;
                        tempColor.g = 1;
                    }
                    if(90 < angle && angle < 150) { //0
                        Debug.Log(Mathf.Atan2(i, j) * 180.0 / Mathf.PI);
                        tempColor.r = 1;
                        tempColor.g = 1;
                    }
                    mat.color = tempColor;

                }
            }
        }
    }
}
