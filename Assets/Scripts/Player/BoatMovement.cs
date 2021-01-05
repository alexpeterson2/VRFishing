using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    Transform boatTransform;
    Vector3 boatVelocity;
    Rigidbody boatBody;
 
    public float boatSpeed;

    void Start()
    {
        boatBody = GetComponent<Rigidbody>();
        boatSpeed = 10f;
    }

    void FixedUpdate()
    {
        var x = OVRInput.RawAxis2D.LThumbstick.ToString();
        var z = OVRInput.RawAxis2D.RThumbstick;

        Debug.Log(x);
       // boatVelocity.x = x;
        
        // Forward movement
        if (OVRInput.Get(OVRInput.Button.DpadUp))
        {
            
        }
    }

    /* void Update()
     {
         bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
         bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
         bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
         bool moveBack = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

         bool dpad_move = false;

         if (OVRInput.Get(OVRInput.Button.DpadUp))
         {
             moveForward = true;
             dpad_move = true;
         }

         if (OVRInput.Get(OVRInput.Button.DpadDown))
         {
             moveBack = true;
             dpad_move = true;
         }

         MoveScale = 1.0f;

         if ((moveForward && moveLeft) || (moveForward && moveRight) ||
             (moveBack && moveLeft) || (moveBack && moveRight))
             MoveScale = 0.70710678f;

         // No positional movement if we are in the air
         if (!Controller.isGrounded)
             MoveScale = 0.0f;

         MoveScale *= SimulationRate * Time.deltaTime;

         // Compute this for key movement
         float moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;

         Quaternion ort = transform.rotation;
         Vector3 ortEuler = ort.eulerAngles;
         ortEuler.z = ortEuler.x = 0f;
         ort = Quaternion.Euler(ortEuler);

         if (moveForward)
             MoveThrottle += ort * (transform.lossyScale.z * moveInfluence * Vector3.forward);
         if (moveBack)
             MoveThrottle += ort * (transform.lossyScale.z * moveInfluence * BackAndSideDampen * Vector3.back);
         if (moveLeft)
             MoveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.left);
         if (moveRight)
             MoveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.right);

         moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;

         Vector3 euler = transform.rotation.eulerAngles;
         float rotateInfluence = SimulationRate * Time.deltaTime * RotationAmount * RotationScaleMultiplier;

         euler.y += buttonRotation;
         buttonRotation = 0f;
     }*/
}
