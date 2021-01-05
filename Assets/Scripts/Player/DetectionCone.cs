using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCone : MonoBehaviour
{
    public ConeGrab hand;
    public GameObject detectedObject;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Cone has detected: " + other.gameObject.name);
        if (other.gameObject.layer == hand.grabLayer)
        {
            detectedObject = other.gameObject;
            //Debug.Log(detectedObject.name + ": has been set to detectedObject");
            hand.RenderLine();
        }
    }
}
