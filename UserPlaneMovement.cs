using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UserPlaneMovement : MonoBehaviour
{
    bool accelerate_L, accelerate_R;

    private InputDevice l_Hand;

    private InputDevice r_Hand;

    [SerializeField]
    private GameObject userView;

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
    }

    // Update is called once per frame
    void Update()
    {
        l_Hand.TryGetFeatureValue(CommonUsages.gripButton, out accelerate_L);

        r_Hand.TryGetFeatureValue(CommonUsages.gripButton, out accelerate_R);

        if(accelerate_L || accelerate_R) {
            Vector3 tempPos = userView.transform.position;
            tempPos.z += .05f;
            userView.transform.position = tempPos;
        }

        /*
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
