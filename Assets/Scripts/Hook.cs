using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Control the hook on screen
/// </summary>
public class Hook : MonoBehaviour
{
    /// <summary>
    /// Power
    /// </summary>
    public float Power = 1;

    /// <summary>
    /// Turn speed in place
    /// </summary>
    public float RotateSpeed = 1;

    /// <summary>
    /// RigidBody2D component of player
    /// </summary>
    public Rigidbody2D RigidBody;

    /// <summary>
    /// How fast to reel in
    /// </summary>
    public float HookVelocity = 100;

    /// <summary>
    /// Where the reel starts
    /// </summary>
    private Vector3 Origin = new Vector3(-3.66f, 5.488f);

    /// <summary>
    /// If reel is reelable
    /// </summary>
    public bool reelable { get; private set; }

    /// <summary>
    /// Gravity
    /// </summary>
    // Vector3 gravity = Vector3.down * 0.05f;

    /// <summary>
    /// Keeping track of velocity
    /// </summary>
    Vector3 velocity;

    public AudioSource reelSound;

    public AudioSource drop;

    private void FixedUpdate()
    {
        Manoeuvre();
        MaybeReel();
    }

    /// <summary>
    /// Position of a GameObject in screen coordinates
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    Vector2 ScreenPosition(GameObject o)
    {
        // Project through the main (only) camera to get screen coordinates
        return Camera.main.WorldToScreenPoint(RigidBody.gameObject.transform.position);
    }

    /// <summary>
    /// True if gameobject is off the screen
    /// </summary>
    bool IsOffScreen(GameObject o)
    {
        var pos = ScreenPosition(o);
        return pos.x < 0 || pos.y < 0 || pos.x > Screen.width || pos.y > Screen.height;
    }
    /// <summary>
    /// Reel in if player is pushing button,
    /// only able to do so if hook is in water
    /// </summary>
    private void MaybeReel()
    {
        if (!IsOffScreen(RigidBody.gameObject))
        {
            reelable = true;
        }

        if (Input.GetButton("Reel"))
        {
            ReelIn();
        }
    }

    /// <summary>
    /// Reset state
    /// </summary>
    private void Reset()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        drop.PlayOneShot(drop.clip);
        reelable = false;
        this.transform.position = this.Origin;
        this.enabled = true;
        RigidBody.velocity = velocity;
    }
    /// <summary>
    /// Reels in line if there is line to be reeled.
    /// Drops line back into water.
    /// </summary>
    private void ReelIn()
    {
        Vector2 currentPos = RigidBody.position;
        if (reelable)
        {
            reelSound.Play();
            Vector2 startPt = new Vector2(Origin.x, Origin.y);
            Vector2 originDir = startPt - currentPos;
            RigidBody.velocity = HookVelocity * originDir;

            //Reset();

            if (IsOffScreen(RigidBody.gameObject))
            {
                Reset();
                reelSound.Stop();
            }
        }
    }

    /// <summary>
    /// Accelerate and rotate
    /// Apply force in a direction
    /// World coords
    /// </summary>
    void Manoeuvre()
    {
        // moving
        Vector2 axes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        axes = axes * Power;
        RigidBody.AddForce(axes);

        // rotating
        // RigidBody.angularVelocity = Input.GetAxis("Rotate");
    }

    // Start is called before the first frame update
    void Start()
    {
        // lineRenderer = GetComponent<LineRenderer>();
        reelable = false;
        RigidBody = GetComponent<Rigidbody2D>();
        velocity = RigidBody.velocity;
        reelSound = GetComponent<AudioSource>();
        drop = GetComponent<AudioSource>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        // velocity += gravity * Time.deltaTime;
        // transform.position += velocity * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        Reset();
    }
}
