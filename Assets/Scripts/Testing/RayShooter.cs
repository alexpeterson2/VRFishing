using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
    // Raycast Origin
    public GameObject rayCastPosition;

    // Oculus Controller
    public OVRInput.Controller controller;

    // Item being detected
    private GameObject m_itemInFocus;

    // Attached Line Renderer
    public LineRenderer lineRenderer;
    public float lineWidth = 0.01f;

    // Max distance an item can be detected
    public float maxLineDistance = 5f;

    // Detectable Layer
    public int detectLayer = 9;

    private Vector3[] initLinePositions;

    void Start()
    {
        initLinePositions = new Vector3[2] { Vector3.zero, Vector3.zero };

        lineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        DetectItem();
    }

    // Try to detect an object in the appropriate layer
    private void DetectItem()
    {
        // Designates detectLayer as the layer to be detected
        int layerMask = 1 << detectLayer;

        // Information about items hit
        RaycastHit hit;

        // Does the Ray intersect any object from the detection Layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxLineDistance, layerMask))
        {
            Debug.Log("Object Detected!");
            lineRenderer.enabled = true;
            lineRenderer.SetPositions(initLinePositions);
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            RenderLine(rayCastPosition.transform.position, transform.TransformDirection(Vector3.forward), maxLineDistance);

            // Object that triggers the ray is set as m_itemInFocus
            if (m_itemInFocus == null)
            {
                Debug.Log("Item is null!");
                m_itemInFocus = hit.collider.gameObject;
            }
        }
        else
        {
            // If the ray isn't hitting anything from the right layer
            lineRenderer.enabled = false;
            m_itemInFocus = null;
        }
    }


    // Create the line between the pointer and the detected item
    private void RenderLine(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition /*+ (length * direction)*/;

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            // Set endPosition to where the line hits a collider
            endPosition = raycastHit.point;
        }

        // Set line start and end points
        lineRenderer.SetPosition(0, targetPosition);
        lineRenderer.SetPosition(1, endPosition);
    }
}
