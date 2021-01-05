using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

// Must be placed on the object meant to be the fish hook
public class TetherHook : MonoBehaviour
{
    private GameManager _manager;

    // Part of the rod the fish will attach to
    private ObiCollider _hook;

    // Used as a template for new rope
    private ObiRopeBlueprint _blueprint;

    // Allows the renderer to grow and shrink with particles
    private ObiRopeExtrudedRenderer _ropeRender;
    private RaycastHit _hookAttachment;

    // Rope that will attach hook to fish
    private ObiRope _rope;

    // Used to change rope length
    private ObiRopeCursor _cursor;

    // Solver or the fishing pole
    public ObiSolver solver;
    
    public Material material;
    public ObiRopeSection ropeSection;

    void Awake()
    {
        // Create new rope and renderer
        _rope = gameObject.AddComponent<ObiRope>();
        _ropeRender = gameObject.AddComponent<ObiRopeExtrudedRenderer>();
        _ropeRender.section = ropeSection;
        _ropeRender.uvScale = new Vector2(1, 5);
        _ropeRender.normalizeV = false;
        _ropeRender.uvAnchor = 1;
        _rope.GetComponent<MeshRenderer>().material = material;



        // Setup blueprint for the rope
        _blueprint = ScriptableObject.CreateInstance<ObiRopeBlueprint>();
        _blueprint.resolution = 0.3f;

        // Tweak rope parameters
        _rope.maxBending = 0.2f;

        // Add cursor to change rope length
        _cursor = _rope.gameObject.AddComponent<ObiRopeCursor>();
        _cursor.cursorMu = 0;
        //_cursor.direction = true;
    }

    void Start()
    {
        _manager = FindObjectOfType<GameManager>();
        _hook = GetComponent<ObiCollider>();
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
            StartCoroutine(AttachHook());
        }
    }

    // Tether the hook to a fish using a newly instantiated ObiRope
    private IEnumerator AttachHook()
    {
        yield return 0;

        Vector3 localHit = _rope.transform.InverseTransformPoint(_hookAttachment.point);

        // Generate a straight line path for the rope
        _blueprint.path.Clear();
        _blueprint.path.AddControlPoint(Vector3.zero, -localHit.normalized, localHit.normalized, Vector3.up, 0.1f, 0.1f, 1, 1, Color.white, "Hook Start");
        _blueprint.path.AddControlPoint(localHit, -localHit.normalized, localHit.normalized, Vector3.up, 0.1f, 0.1f, 1, 1, Color.white, "Hook End");
        _blueprint.path.FlushEvents();

        // Generate the particle representation of the rope (Wait until it's finished)
        yield return _blueprint.Generate();

        // Pin both ends of the rope (This enables two-way interaction between hook and rope)
        var pinConstraints = _blueprint.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
        var batch = pinConstraints.batches[0];
        batch.AddConstraint(0, _hook, transform.localPosition, Quaternion.identity);
        batch.AddConstraint(_blueprint.activeParticleCount - 1, _hookAttachment.collider.GetComponent<ObiColliderBase>(), 
            _hookAttachment.collider.transform.InverseTransformPoint(_hookAttachment.point), Quaternion.identity);
        batch.activeConstraintCount = 2;

        // Set the blueprint (This adds particles/constraints to the solver and starts simulating them)
        _rope.ropeBlueprint = _blueprint;
        _rope.GetComponent<MeshRenderer>().enabled = true;

        _cursor.ChangeLength(0.1f);
    }

    // Removes the ObiRope tethering the hook to a fish
    public void DetachFish()
    {
        Destroy(_blueprint);
    }
}
