using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UserPlaneMovement : MonoBehaviour
{
    private bool accelerate_L, accelerate_R;
    private bool pitch_L, pitch_R;
    private Quaternion rotation_L, rotation_R;

    [SerializeField]
    private Transform leftHand;
    private InputDevice l_Hand;
    
    [SerializeField]
    private Transform rightHand;
    private InputDevice r_Hand;
    
    [SerializeField]
    private GameObject userView;

    [SerializeField]
    private float thrust = 1.4f;

    [SerializeField]
    private float yawSpeed = 10;

    [SerializeField]
    private float pitchSpeed = 10;

    private List<InputDevice> devices = new List<InputDevice>();

    // Start is called before the first frame update
    void Start()
    {


        refreshDevices();

        //Get Rb component

    }

    void refreshDevices() {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        foreach(var device in inputDevices) {
            if(device.role == InputDeviceRole.LeftHanded) {
                l_Hand = device;
            }
            if(device.role == InputDeviceRole.RightHanded) {
                r_Hand = device;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!l_Hand.isValid || !r_Hand.isValid) {
            refreshDevices();
        }

        float yaw = leftHand.position.y - rightHand.position.y;
        if(Mathf.Abs(yaw) > .1) {
            userView.transform.Rotate(0.0f, yaw * yawSpeed* Time.deltaTime, 0.0f, Space.Self);
        }

        l_Hand.TryGetFeatureValue(CommonUsages.primaryButton, out pitch_L);
        r_Hand.TryGetFeatureValue(CommonUsages.primaryButton, out pitch_R);
        if(pitch_L && pitch_R) {

            float lPitch= leftHand.rotation.eulerAngles.z;
            //float rPitch = 360 - rightHand.rotation.eulerAngles.z;
            float pitchAVG = lPitch;// + rPitch); / 2;
            Debug.Log("avg: " + pitchAVG);
            /*
            if(345 < pitchAVG || pitchAVG < 20f) {
                pitchAVG = 20f;
            }
            if(310f < pitchAVG && pitchAVG < 345f) {
                pitchAVG = 310f;
            }*/
            float pitchControl = (pitchAVG * pitchAVG * 0.000065f - pitchAVG * 0.02475f + 1.49f) *1.3f;//a= and b=-0.024746 and c=1.4691
            pitchControl = Mathf.Clamp(pitchControl, -1, 1);
            
            Debug.Log("Final: "+pitchControl);

            userView.transform.Rotate(-(pitchControl * Time.deltaTime * pitchSpeed),0.0f, 0.0f, Space.Self);
            //userView.transform.RotateAround(userView.transform.position, userView.transform.localPosition.x, pitchControl*Time.deltaTime*-10f);
        } 

        l_Hand.TryGetFeatureValue(CommonUsages.gripButton, out accelerate_L);
        r_Hand.TryGetFeatureValue(CommonUsages.gripButton, out accelerate_R);
        if(accelerate_L || accelerate_R) {
            //Vector3 tempPos = userView.transform.position;
            //tempPos.z += .05f;

            //add force in this direction, not velocity change
           
            userView.transform.position += userView.transform.rotation * Vector3.forward * thrust * Time.deltaTime;
        
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
