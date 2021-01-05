using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatInterface : MonoBehaviour
{
    // Is player in the boat
    [SerializeField]
    private bool _inBoat = false;

    // Is player in range of boat
    [SerializeField]
    private bool _nearBoat = false;

    // Boat transform
    private Transform _boatTrans;

    // Boat transform before being entered
    private Vector3 _boatStartPos;

    // Player transform after entering boat
    private Vector3 _playerStartPos;

    private CharacterController _characterController;

    private OVRPlayerController _playerController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerController = GetComponent<OVRPlayerController>();
    }

    void Update()
    {
        EnterBoat();
        ExitBoat();
    }

    private void EnterBoat()
    {
        // Runs if player is standing near boat
        if (OVRInput.GetDown(OVRInput.RawButton.A) && _inBoat == false && _nearBoat == true)
        {
            // Removes gravity
            _playerController.GravityModifier = 0;

            _playerStartPos = transform.position;
            _boatStartPos = _boatTrans.position;

            /* Disable Character Controller while setting position to prevent conflict.
               Not doing this results in the character controller moving the player to a 
               random position after transform.position is manually set. */
            _characterController.enabled = false;
            transform.position = _boatStartPos;
            _characterController.enabled = true;

            _boatTrans.SetParent(GameObject.Find("TrackingSpace").transform);
            _boatTrans.position = new Vector3(_boatTrans.position.x, -1.5f, _boatTrans.position.z);

            _inBoat = true;

            Debug.Log("Entered boat!");
        }
    }

    private void ExitBoat()
    {
        // Runs if player is already in boat
        if (OVRInput.GetDown(OVRInput.RawButton.B) && _inBoat == true)
        {
            _inBoat = false;

            // Unparent boat from player and reset both to start positions
            _boatTrans.parent = null;
            _boatTrans.position = _boatStartPos;
            _characterController.enabled = false;
            transform.position = _playerStartPos;
            _characterController.enabled = true;
            _playerController.GravityModifier = 1;

            Debug.Log("Can find Player: " + (GameObject.FindGameObjectWithTag("Player") == null));
            Debug.Log("Exit boat!");
        }
        else
        {
            Debug.Log("Exit Failed!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "boat")
        {
            _boatTrans = other.gameObject.transform;
            _nearBoat = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "boat")
        {
            _nearBoat = false;
        }
    }

}
