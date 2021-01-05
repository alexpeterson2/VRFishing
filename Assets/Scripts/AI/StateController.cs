using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State currentState;

    // Game Manager
    public GameManager manager;

    // The part of the fishing rod for fish to chase and attach to
    public GameObject fishHook;

    // Distance between fish and fishHook
    public float remainingDistance;
    public Transform destination;

    // Unique instance of AIMove script on fish
    public AIMove ai;

    // Instance of AISpawner attached to specific zone
    public AISpawner _AIManager;
    public GameObject[] hooks;

    // How far away fish can sense fishHook
    public float detectionRange = 10;

    // How close fish has to be before attaching to fishHook
    public float biteRange = 0.2f;

    // The player charactera
    public GameObject player;

    // How close a fish must be reeled in to be caught
    public float catchRange = 1f;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        ai = GetComponent<AIMove>();
        _AIManager = GetComponent<AISpawner>();
        // Initiates new fish in with SwimState active
        SetState(new SwimState(this));
    }

    // Update is called once per frame
    void Update()
    {
        currentState.CheckTransition();
        currentState.Act();
    }

    // Checks if the fish is close enough to see the fish hook and chase it
    public bool CheckIfInRange(string tag)
    {
        hooks = GameObject.FindGameObjectsWithTag(tag);

        // Will return false if a fish hook isn't in the scene
        if (hooks != null)
        {
            foreach (GameObject h in hooks)
            {
                // Will return true if close enough and will set fishHook to the valid object
                if (Vector3.Distance(h.transform.position, transform.position) < detectionRange)
                {
                    fishHook = h;
                    return true;
                }
            }
        }
        return false;
    }

    // Checks if the fish has reached the hook and if it should bite it
    public bool CheckIfBit(string tag)
    {
        hooks = GameObject.FindGameObjectsWithTag(tag);

        if(hooks != null)
        {
            foreach (GameObject h in hooks)
            {
                if (Vector3.Distance(h.transform.position, transform.position) < biteRange)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Checks if the fish has been brought close enough to be caught
    public bool CheckIfCaught()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < catchRange)
            {
                return true;
            } 
        }
        return false;
    }

    // Changes states
    public void SetState(State state)
    {
        // Exits the current state
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        // Sets new state
        currentState = state;

        // Initiates current state
        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
