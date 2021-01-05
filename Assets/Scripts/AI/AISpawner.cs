using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[System.Serializable]
public class AIObjects
{
    // Declare variables
    public string AIGroupName { get { return m_aiGroupName; } }
    public GameObject objectPrefab { get { return m_prefab; } }
    public int maxAI { get { return m_maxAI; } }
    public int spawnRate { get { return m_spawnRate; } }
    public int spawnAmount { get { return m_maxSpawnAmount; } }
    public bool randomizeStats { get { return m_randomizeStats; } }
    public bool enableSpawner { get { return m_enableSpawner; } }

    // Serialize private variables
    [Header("AI Group Stats")]
    [SerializeField]
    private string m_aiGroupName;
    [SerializeField]
    private GameObject m_prefab;
    [SerializeField]
    [Range(0f, 40f)]
    private int m_maxAI;
    [SerializeField]
    [Range(0f, 20f)]
    private int m_spawnRate;
    [SerializeField]
    [Range(0f, 10f)]
    private int m_maxSpawnAmount;

    [Header("Main Settings")]
    [SerializeField]
    private bool m_enableSpawner;
    [SerializeField]
    private bool m_randomizeStats;

    public AIObjects(string Name, GameObject Prefab, int MaxAI, int SpawnRate, int SpawnAmount, bool RandomizeStats)
    {
        this.m_aiGroupName = Name;
        this.m_prefab = Prefab;
        this.m_maxAI = MaxAI;
        this.m_spawnRate = SpawnRate;
        this.m_maxSpawnAmount = SpawnAmount;
        this.m_randomizeStats = RandomizeStats;
    }

    public void setValues(int MaxAI, int SpawnRate, int SpawnAmount)
    {
        this.m_maxAI = MaxAI;
        this.m_spawnRate = SpawnRate;
        this.m_maxSpawnAmount = SpawnAmount;
    }
}
public class AISpawner : MonoBehaviour
{
    // Declare Variables
    public List<Transform> Waypoints = new List<Transform>();

    public float spawnTimer { get { return m_SpawnTimer; } }
    public Vector3 spawnArea { get { return m_SpawnArea; } }

    // Serialize the private variables
    [Header("Global Stats")]
    [Range(0f, 600f)]
    [SerializeField]
    private float m_SpawnTimer; // How often spawner is run
    [SerializeField]
    private Color m_SpawnColor = new Color(1.000f, 0.000f, 0.000f, 0.300f); // Use the colour for the gizmo
    [SerializeField]
    private Vector3 m_SpawnArea = new Vector3(20f, 10f, 20f);

    // Create array from new class
    [Header("AI Groups Settings")]
    public AIObjects[] AIObject = new AIObjects[5];

    void Start()
    {
        GetWaypoints();
        RandomizeGroups();
        CreateAIGroups();
        InvokeRepeating("SpawnNPC", 0.5f, spawnTimer);
    }

    void SpawnNPC()
    {
        // Loop through all of the AI groups
        for (int i = 0; i < AIObject.Count(); i++)
        {
            // Check to make sure spawner is enabled
            if (AIObject[i].enableSpawner && AIObject[i].objectPrefab != null)
            {
                // Make sure that AI group doesn't have max NPCs
                GameObject tempGroup = GameObject.Find(AIObject[i].AIGroupName);
                if (tempGroup.GetComponentInChildren<Transform>().childCount < AIObject[i].maxAI)
                {
                    // Spawn random number of NPCs from 0 to Max Spawn Amount
                    for (int y = 0; y < Random.Range(0, AIObject[i].spawnAmount); y++)
                    {
                        // Get random rotation
                        Quaternion randomRotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(0, 360), 0);
                        // Create spawned gameobject
                        GameObject tempSpawn;
                        tempSpawn = Instantiate(AIObject[i].objectPrefab, RandomPosition(), randomRotation);
                        // Put spawned NPC as child of group
                        tempSpawn.transform.parent = tempGroup.transform;
                        // Add the AIMove script and class to the new NPC
                        tempSpawn.AddComponent<AIMove>();
                        tempSpawn.AddComponent<StateController>();
                    }
                }
            }
        }
    }

    // Public method for Random Position within the Spawn Area
    public Vector3 RandomPosition()
    {
        // Get a random position within the Spawn Area
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z)
            );
        randomPosition = transform.TransformPoint(randomPosition * .5f);
        return randomPosition;
    }

    // Public method for getting a Random Waypoint
    public Vector3 RandomWaypoint()
    {
        int randomWP = Random.Range(0, (Waypoints.Count - 1));
        Vector3 randomWaypoint = Waypoints[randomWP].transform.position;
        return randomWaypoint;
    }

    // Method for putting random values in the AI Group setting
    void RandomizeGroups()
    {
        // Randomize
        for (int i = 0; i < AIObject.Count(); i++)
        {
            // If array item has randomizeStats ticked true
            if (AIObject[i].randomizeStats)
            {
                AIObject[i].setValues(Random.Range(1, 30), Random.Range(1, 20), Random.Range(1, 10));
            }
        }
    }

    void CreateAIGroups()
    {
        for (int i = 0; i < AIObject.Count(); i++)
        {
            // Empty Game Object to keep the AI in
            GameObject AIGroupSpawn;

            // Check to make sure Group has a name
            if (AIObject[i].AIGroupName != null)
            {
                // Creat a new game object
                AIGroupSpawn = new GameObject(AIObject[i].AIGroupName);
                AIGroupSpawn.transform.parent = this.gameObject.transform;
            }
        }
    }

    void GetWaypoints()
    {
        // List using standard library
        // Look through nested children
        Transform[] wpList = this.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < wpList.Length; i++)
        {
            if (wpList[i].tag == "waypoint")
            {
                // Add to the list
                Waypoints.Add(wpList[i]);
            }
        }
    }

    // Show the gizmos in colour
    void OnDrawGizmosSelected()
    {
        Gizmos.color = m_SpawnColor;
        Gizmos.DrawCube(transform.position, spawnArea);
    }
}
