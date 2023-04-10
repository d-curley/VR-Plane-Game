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


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!l_Hand.isValid || !r_Hand.isValid) {
            refreshDevices();
        }

        //l_Hand.TryGetFeatureValue(CommonUsages.deviceVelocity, out l_Velocity);
        //r_Hand.TryGetFeatureValue(CommonUsages.triggerButton, out r_grip);
        Debug.Log(rightHand.transform.localPosition);// local position just bakes in a comparison to the position of the parent.  

        if(r_grip){// && !gestureStarted) {
            gestureStarted = true;
            r_starting_pos = rightHand.transform.position;
            Debug.Log("pos: " + r_starting_pos);
            
        }

        if(gestureStarted) {
            //
        }
        

        /*
        float yaw = leftHand.position.y - rightHand.position.y;
        l_Hand.TryGetFeatureValue(CommonUsages.primaryButton, out pitch_L);
        r_Hand.TryGetFeatureValue(CommonUsages.primaryButton, out pitch_R);
        float lPitch = leftHand.rotation.eulerAngles.z;
        */
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