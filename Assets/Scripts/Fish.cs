using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Rendering;

public class Fish : MonoBehaviour
{
    public int Weight;

    public float ApproachDistance = 1;

    public float Power = 1;

    public AudioSource chime;

    private Rigidbody2D rb;

    private Transform hookTrans;

    private Vector2 OffsetToHook => hookTrans.position - transform.position;

    private Hook hook;

    public Vector2 direction = Vector2.right;

    public int size = 1;

    public float speed = 1f;

    Vector3 rEdge;

    Vector3 lEdge;

    bool reeling = false;

    // Vector3 tEdge;

    // Vector3 bEdge;

    // Start is called before the first frame update
    void Start()
    {
        chime = GetComponent<AudioSource>();
        chime.playOnAwake = false;
        lEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        // tEdge = Camera.main.ViewportToWorldPoint(FindObjectOfType<boat>().transform);
        rb = GetComponent<Rigidbody2D>();
        hook = FindObjectOfType<Hook>();
        hookTrans = hook.transform;
        reeling = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 offsetToHook = OffsetToHook;
        float distanceToHook = offsetToHook.magnitude;
        var controlSign = distanceToHook <= ApproachDistance ? -1 : 1;
        rb.AddForce(controlSign * (Power / distanceToHook) * offsetToHook);
    }

    /// <summary>
    /// Position of a GameObject in screen coordinates
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    Vector2 ScreenPosition(GameObject o)
    {
        // Project through the main (only) camera to get screen coordinates
        return Camera.main.WorldToScreenPoint(rb.gameObject.transform.position);
    }

    /// <summary>
    /// True if gameobject is off the screen
    /// </summary>
    bool IsOffScreen(GameObject o)
    {
        var pos = ScreenPosition(o);
        return pos.x < 0 || pos.y < 0 || pos.x > Screen.width || pos.y > Screen.height;
    }
    private void Update()
    {
        

        if (reeling)
        {
            // transform.SetParent(hookTrans, false);
            // transform.position.Set(hook.transform.position.x, hook.transform.position.y, hook.transform.position.z);
            // print("reeling");
            transform.Translate(hook.transform.position);
        } else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Hook") && Input.GetButton("Reel"))
        {
            reeling = true;
        }   
    }

    private void OnBecameInvisible()
    {
        if (reeling)
        {
            ScoreKeeper.AddToScore(Weight);
            chime.PlayOneShot(chime.clip);
            reeling = false;
        }
    }
}
