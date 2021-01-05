using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class JointHook : MonoBehaviour
{
    private GameManager _manager;

    private Rigidbody _rBody;
    private SphereCollider _hookCollider;
    private Rigidbody _fishBody;

    private bool fishHit = false;

    private RaycastHit _hookAttachment;

    void Awake()
    {
        _rBody = gameObject.GetComponent<Rigidbody>();
        _hookCollider = gameObject.GetComponent<SphereCollider>();
    }

    void Start()
    {
        _manager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Detach fish

    }

    // Used by HookedState to call on the AttachHook coroutine
    public void HookFish(Transform fishMouth)
    {
        // Get Ray from the hook to the fish
        Ray ray = new Ray(transform.position, fishMouth.position - transform.position);

        // Raycast to see what was hit
        if (Physics.Raycast(ray, out _hookAttachment))
        {
            // If a fish is hit, attach the hook
            AttachHook();
        }
    }

    // Tether the hook to a fish using a newly instantiated ObiRope
    private void AttachHook()
    {
        // Generate a straight line path for the rope
        gameObject.AddComponent<FixedJoint>();
        gameObject.GetComponent<FixedJoint>().connectedBody = _hookAttachment.rigidbody;
    }

    // Removes the ObiRope tethering the hook to a fish
    public void DetachFish()
    {
        Destroy(gameObject.GetComponent<FixedJoint>());
    }
}
