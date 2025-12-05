using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    /// <summary>
    /// Line renderer component
    /// </summary>
    private LineRenderer lineRenderer;

    /// <summary>
    /// Transform from Hook object
    /// Used to find Hook position
    /// </summary>
    private Transform hook;

    /// <summary>
    /// Where the reel starts
    /// </summary>
    private Vector2 Origin = new Vector2(-1.4f, 5.93f);

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        hook = FindObjectOfType<Hook>().transform;
        lineRenderer.SetPosition(0, hook.position);
    }

    // Update is called once per frame
    void Update()
    {
        // lineRenderer.SetPosition(0, Origin);
        lineRenderer.SetPosition(1, hook.position);
    }
}
