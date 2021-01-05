using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager { get; private set; } = null;

    // Fish is chasing hook
    public bool isChasing = false;

    // Fish has bitten the hook
    public bool onHook = false;

    // Number of fish caught
    public int numberCaught { get; private set; } = 0;

    void Awake()
    {
        // Only a single GameManager is allowed. Extras are destroyed.
        if (manager == null)
        {
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }

        // Persist when loading between scenes
        DontDestroyOnLoad(this);
    }

    public void caughtFish()
    {
        numberCaught++;
    }
}
