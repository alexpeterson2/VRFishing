using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    // Declare variable for AISpawner manager script
    private AISpawner _AIManager;

    private Rigidbody _rBody;

    // Declare variables for moving and turning
    private bool _hasTarget = false;
    private bool _isTurning;

    // Variable for the current waypoint
    private Vector3 _wayPoint;

    // Variable for previous waypoint
    private Vector3 _lastWaypoint = new Vector3(0f, 0f, 0f);

    // Going to use this to set the animation speed
    private Animator _animator;
    private float _speed;

    void Start()
    {
        // Get the AISpawner from its parent
        _AIManager = transform.parent.GetComponentInParent<AISpawner>();
        _rBody = transform.GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        SetUpNPC();
    }

    void SetUpNPC()
    {
        // Randomly scale each NPC
        float m_scale = Random.Range(0f, 1.5f);
        transform.localScale += new Vector3(m_scale * 1.5f, m_scale, m_scale);
    }

    void Update()
    {
        //Debug.Log("Has a Target: " + m_hasTarget);
        //Debug.Log("Can Find Target: " + CanFindTarget());
        //Debug.Log("Waypoint: " + m_wayPoint);
    }

    // Swim to a random waypoint
    public void Swim()
    {
        // If we have not found a way point to move to, find one
        // If a waypoint is found, move to it
        if (!_hasTarget)
        {
            _hasTarget = CanFindTarget();
        }
        else
        {
            // Rotate the NPC to face its waypoint
            RotateNPC(_wayPoint, _speed);

            // Move the NPC in a straight line toward the waypoint
            transform.position = Vector3.MoveTowards(transform.position, _wayPoint, _speed * Time.deltaTime);
            //Debug.Log("Move Towards Waypoint!");
        }
    }

    // Swim to a specific GameObject
    public void SwimTo(Transform destination)
    {
        _wayPoint = destination.position;
        Swim();
    }


    // Stop swimming
    public void Hooked(Transform hook)
    {
        // Set parent to hook
        //transform.parent = hook;

        // Turn gravity on
        _rBody.useGravity = true;
    }

    private bool CanFindTarget(float start = 0.5f, float end = 1f)
    {
        _wayPoint = _AIManager.RandomWaypoint();
        // Make sure we don't set the same waypoint twice
        if (_lastWaypoint == _wayPoint)
        {
            // Get a new waypoint
            _wayPoint = _AIManager.RandomWaypoint();
            return false;
        }
        else
        {
            // Set the new waypoints as the last waypoint
            _lastWaypoint = _wayPoint;
            // Get random speed for movement and animation
            _speed = Random.Range(start, end);
            _animator.speed = _speed;
            // Set bool to true to say we found a WP
            //Debug.Log("Speed: " + m_speed);
            return true;
        }
    }

    // If fish reaches waypoint reset target
    public bool WasTargetReached()
    {
        if (transform.position == _wayPoint)
        {
            _hasTarget = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Rotate the NPC to face new waypoint
    private void RotateNPC(Vector3 waypoint, float currentSpeed)
    {
        // Get random speed up for the turn
        float TurnSpeed = currentSpeed * Random.Range(1f, 3f);

        // Get new direction to look at for target
        Vector3 LookAt = waypoint - this.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookAt), TurnSpeed * Time.deltaTime);
    }
}


