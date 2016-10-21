using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HookShootScript : MonoBehaviour {

    public GameObject hook;

    public float GRAPPLE_COOLDOWN = 1f;
    public float EXTENSION_LENGTH = 1f;
    public float EXTENSION_DURATION = 0.25f;
    public float HOOK_GRAB_WIDTH = 0.5f;

    private float cooldown;
    private float fireTimer;
    public int hookState;
    public Player myPlayer;
    private Vector3 originalHookLocalPosition;
    private SpringJoint2D tether;

    public GameObject hookTarget = null;

    private const int HOOK_IN_CANNON = 0;
    private const int HOOK_FIRING = 1;
    private const int HOOK_GRABBED = 2;
    private const int HOOK_RESETTING = 3;

    //LINE AND HOOK GRAPHICAL MANAGEMENT
    public GameObject hookStart;
    public GameObject hookLine;
    public GameObject hookEnd;

    private LineRenderer line;

    private AudioSource sound;
    public AudioClip hookShootSound;
    public AudioClip hookGrabSound;
    public AudioClip hookReleaseSound;

    void Start () {
        cooldown = 0;
        myPlayer = GetComponent<Player>();
        hookState = HOOK_IN_CANNON;
        originalHookLocalPosition = hook.transform.localPosition;
        tether = GetComponent<SpringJoint2D>();
        tether.enabled = false;
        tether.connectedBody = null;
        sound = GetComponent<AudioSource>();

        line = hookLine.GetComponent<LineRenderer>();
	}
	
	void Update () {
        cooldown -= Time.deltaTime;

        switch (hookState)
        {
            case (HOOK_IN_CANNON):
                if (cooldown <= 0 && Input.GetAxisRaw("Fire1_P" + myPlayer.player) == -1 || Input.GetButtonDown("Fire1_P" + myPlayer.player))
                {
                    cooldown = GRAPPLE_COOLDOWN;
                    hookState = HOOK_FIRING;
                    fireTimer = EXTENSION_DURATION;

                    sound.PlayOneShot(hookShootSound);
                }
                break;
            case (HOOK_FIRING):
                //Line Graphic
                line.SetPosition(0, hookStart.transform.position);
                line.SetPosition(1, hookEnd.transform.position);

                fireTimer -= Time.deltaTime;
                hook.gameObject.transform.localPosition = new Vector3(hook.gameObject.transform.localPosition.x, -EXTENSION_LENGTH * (1 - fireTimer/EXTENSION_DURATION), hook.gameObject.transform.localPosition.z);
                //Collider2D hit = Physics2D.OverlapPoint(new Vector2(hook.gameObject.transform.position.x, hook.gameObject.transform.position.y), (1 << LayerMask.NameToLayer("asteroid_unscoreable") | 1 << LayerMask.NameToLayer("asteroid_scoreable")) );
                Collider2D hit = Physics2D.OverlapBox(new Vector2(hook.gameObject.transform.position.x, hook.gameObject.transform.position.y), new Vector2(HOOK_GRAB_WIDTH, HOOK_GRAB_WIDTH), 0, (1 << LayerMask.NameToLayer("asteroid_unscoreable") | 1 << LayerMask.NameToLayer("asteroid_scoreable")));

                if (hit)
                {
                    hookState = HOOK_GRABBED;

                    tether.enabled = true;
                    tether.connectedBody = hit.gameObject.GetComponent<Rigidbody2D>();
                    tether.distance = -hook.gameObject.transform.localPosition.y;

                    hookTarget = hit.gameObject;
                    hookTarget.GetComponent<Asteroid>().HookedAsteroid(myPlayer);

                    sound.PlayOneShot(hookGrabSound);
                }
                else if (fireTimer <= 0)
                {
                    resetHook();
                }    

                break;
            case (HOOK_GRABBED):
                //Line Graphic
                line.SetPosition(0, hookStart.transform.position);
                line.SetPosition(1, hookEnd.transform.position);

                //Listen for disconnect or asteroid destruction
                if (cooldown <= 0 && (Input.GetAxisRaw("Fire1_P" + myPlayer.player) == -1 || Input.GetButtonDown("Fire1_P" + myPlayer.player) || hookTarget == null))
                {
                    resetHook();
                }
                else
                {
                    hook.transform.position = hookTarget.transform.position;
                }
                break;
            case (HOOK_RESETTING):
                line.SetPosition(0, hookStart.transform.position);
                line.SetPosition(1, hookEnd.transform.position);
                break;
        }
        if (cooldown > 0) cooldown -= Time.deltaTime;
    
	}

    public void resetHook()
    {
        tether.enabled = false;
        tether.connectedBody = null;
        hookState = HOOK_RESETTING;
        hook.transform.DOLocalMove(originalHookLocalPosition, EXTENSION_DURATION / 2).OnComplete(setHookToInCannon);
        if (hookTarget != null)
        {
            hookTarget.GetComponent<Asteroid>().ReleasedAsteroid();
            hookTarget = null;
        }

        sound.PlayOneShot(hookReleaseSound);
    }

    void setHookToInCannon()
    {
        line.SetPosition(1, hookEnd.transform.position);
        hookState = HOOK_IN_CANNON;
    }
}
