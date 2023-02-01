using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UserPlaneMovement : MonoBehaviour
{
    bool accelerate_L, accelerate_R;

    [SerializeField]
    private Transform leftHand;
    private InputDevice l_Hand;
    
    [SerializeField]
    private Transform rightHand;
    private InputDevice r_Hand;
    
    [SerializeField]
    private GameObject userView;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private int spin = 2;

    private List<InputDevice> devices = new List<InputDevice>();

    // Start is called before the first frame update
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach(var device in inputDevices) {
            if(device.role== InputDeviceRole.LeftHanded) {
                l_Hand = device;
            }
            if(device.role == InputDeviceRole.RightHanded) {
                r_Hand = device;
            }
        }

        //Get Rb component

    }

    // Update is called once per frame
    void Update()
    {
        l_Hand.TryGetFeatureValue(CommonUsages.gripButton, out accelerate_L);

        r_Hand.TryGetFeatureValue(CommonUsages.gripButton, out accelerate_R);


        float tilt = leftHand.position.y - rightHand.position.y;
        if(Mathf.Abs(tilt) > .1) {
           userView.transform.Rotate(0.0f, tilt*spin, 0.0f, Space.Self);
        }


        

        if(accelerate_L || accelerate_R) {
            //Vector3 tempPos = userView.transform.position;
            //tempPos.z += .05f;

            //add force in this direction, not velocity change
           
            userView.transform.position += userView.transform.rotation * Vector3.forward * speed * Time.deltaTime;
        
            //Note: Camera view might not be the best option for direction
            // We can use the camera offset, where how you hold your hands controls to orientation of the offset <- or XRRig
            // Hands will rotate XR rig, 
        }

        /* //Maybe certain acceleration is needed to loop-de-loop?
        if(device.TryGetFeatureValue(gripUsage, out gripValue) && gripValue > 0 && !gripIsPressed) {
            gripIsPressed = true;
            DilmerGamesLogger.Instance.LogInfo($"Grip value {gripValue} activated on {xRNode}");
        } else if(gripValue == 0 && gripIsPressed) {
            gripIsPressed = false;
            DilmerGamesLogger.Instance.LogInfo($"Grip value {gripValue} deactivated on {xRNode}");
        }
        */
    }
}
