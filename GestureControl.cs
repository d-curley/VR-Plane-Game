using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GestureControl : MonoBehaviour
{
    [SerializeField]
    private GameObject leftHand;
    private InputDevice l_Hand;
    private Vector3 l_Velocity;
    private bool l_grip;

    [SerializeField]
    private GameObject rightHand;
    private InputDevice r_Hand;
    private Vector3 r_Velocity;
    private Vector3 r_starting_pos;
    private bool r_grip;
    private bool r_primary;

    private bool gestureStarted = false;
    private bool gesture1 = false;
    private bool gesture2 = false;
    private bool gesture3 = false;

    [SerializeField]
    private TreeGeneration trees;
    public List<GameObject>[] treeSections;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private GameObject[] fogSections= new GameObject[6];

    private bool[] sectionColored= new bool[6];


    // Start is called before the first frame update
    void Start()
    {
        treeSections = trees.TreeSections;
    }

    // Update is called once per frame
    void Update()
    {
        if(!l_Hand.isValid || !r_Hand.isValid) {
            refreshDevices();
        }


        r_Hand.TryGetFeatureValue(CommonUsages.triggerButton, out r_grip);
        if(r_grip) {
            int section = FogSection(cameraTransform.rotation.eulerAngles.y);

            if(!sectionColored[section]) {
                fogSections[section].GetComponent<ParticleSystem>().startColor = Color.red;
                Debug.Log(section);
                foreach(GameObject tree in treeSections[section]) {
                    tree.GetComponent<Renderer>().material.color = Color.red;
                }
                sectionColored[section] = true;
            } 
            
        }


        /*
        //l_Hand.TryGetFeatureValue(CommonUsages.deviceVelocity, out l_Velocity);
        //
        Debug.Log(rightHand.transform.localPosition);// local position just bakes in a comparison to the position of the parent.  

        if(r_grip){// && !gestureStarted) {
            gestureStarted = true;
            r_starting_pos = rightHand.transform.position;
            Debug.Log("pos: " + r_starting_pos);
            
        }

        

        
        float yaw = leftHand.position.y - rightHand.position.y;
        l_Hand.TryGetFeatureValue(CommonUsages.primaryButton, out pitch_L);
        r_Hand.TryGetFeatureValue(CommonUsages.primaryButton, out pitch_R);
        float lPitch = leftHand.rotation.eulerAngles.z;
        */
    }
    public int FogSection(double viewAngle) {
        int section = 0;

        if(-30 < viewAngle && viewAngle < 30) { // 4
            section = 1;
        } else if(30 < viewAngle && viewAngle < 90) { //5
            section = 0;
        } else if(90 < viewAngle && viewAngle < 150) { //0
            section = 5;
        } else if(150 < viewAngle && viewAngle < 210) { //1
            section = 4;
        } else if(210 < viewAngle && viewAngle < 270) { //2
            section = 3;
        } else if(270 < viewAngle && viewAngle < 330) { //3
            section = 2;
        }
        return section;
    }
    void refreshDevices() {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        foreach(var device in inputDevices) {
            if(device.characteristics == InputDeviceCharacteristics.Left) {
                l_Hand = device;
            }
            if(device.role == InputDeviceRole.RightHanded) {
                r_Hand = device;
            }
        }
    }


    void velocityGesture1() {
        //Look for gesture 1
        if(!gesture1) {
            r_Hand.TryGetFeatureValue(CommonUsages.gripButton, out r_grip);
            if(r_grip) {
                r_Hand.TryGetFeatureValue(CommonUsages.deviceVelocity, out r_Velocity);//be wary of local vs world x
                if(r_Velocity.x > 2) {
                    //consider a visual effect here to show they did it
                    gesture1 = true;
                    Debug.Log("gesture 1");
                }
            }
        }
    }

    void velocityGesture2() {
        //Look for gesture 2
        if(gesture1 && !gesture2) {
            r_Hand.TryGetFeatureValue(CommonUsages.triggerButton, out r_primary);
            if(r_primary) {
                r_Hand.TryGetFeatureValue(CommonUsages.deviceVelocity, out r_Velocity);//be wary of local vs world x
                if(r_Velocity.y > 1) {
                    gesture2 = true;
                    Debug.Log("Gesture 2");
                }
            }
        }
    }

    void velocityGesture3() {
        if(gesture1 && gesture2) {
            r_Hand.TryGetFeatureValue(CommonUsages.triggerButton, out r_primary);
            r_Hand.TryGetFeatureValue(CommonUsages.gripButton, out r_grip);
            if(r_grip && r_primary) {
                r_Hand.TryGetFeatureValue(CommonUsages.deviceVelocity, out r_Velocity);//be wary of local vs world x
                float xVel = r_Velocity.x;
                float yVel = r_Velocity.y;
                if(-1f < xVel && xVel < -.5f && -1 < yVel && yVel < -.5f) {
                    Debug.Log("gesture 3");
                    gesture1 = false;
                    gesture2 = false;
                }
            }
        }
    }
}
