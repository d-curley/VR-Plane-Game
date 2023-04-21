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

    public List<GameObject>[] TreeSections = new List<GameObject>[6];
    
    void Start()
    {

        for(int i = 0; i < TreeSections.Length; i++) {
            TreeSections[i] = new List<GameObject>();
        }
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

                    mat.color = tempColor;


                    // Use Radius of maxD
                    // See if we can check if a 2D point is within a specific arc
                    double angle = Mathf.Atan2(i, j) * 180.0 / Mathf.PI;

                    TreeSections[SectionView(angle)].Add(tree);
                }
            }
        }
    }

    public int SectionView(double viewAngle) {
        int section = 0;
        
        if(-30 < viewAngle && viewAngle <= 30) { // 4
            section = 1;
        } else if(30 < viewAngle && viewAngle <= 90) { //5
            section = 0;
        } else if(90 < viewAngle && viewAngle <= 150) { //0
            section = 5;
        } else if(150 < viewAngle && viewAngle <= 210) { //1
            section = 4;
        } else if(210 < viewAngle && viewAngle <= 270) { //2
            section = 3;
        } else if(270 < viewAngle && viewAngle <= 330) { //3
            section = 2;
        }
        return section;
    }
}
