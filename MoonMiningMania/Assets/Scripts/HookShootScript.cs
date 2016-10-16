using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HookShootScript : MonoBehaviour {

    public GameObject hook;

    public float GRAPPLE_COOLDOWN = 1f;
    public float EXTENSION_LENGTH = 1f;
    public float EXTENSION_DURATION = 0.25f;

    private float cooldown;
    private float fireTimer;
    public int hookState;
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

    void Start () {
        cooldown = 0;
        hookState = HOOK_IN_CANNON;
        originalHookLocalPosition = hook.transform.localPosition;
        tether = GetComponent<SpringJoint2D>();
        tether.enabled = false;
        tether.connectedBody = null;

        line = hookLine.GetComponent<LineRenderer>();
	}
	
	void Update () {
        cooldown -= Time.deltaTime;

        switch (hookState)
        {
            case (HOOK_IN_CANNON):
                if (cooldown <= 0 && Input.GetAxisRaw("Vertical_P1") == -1)
                {
                    cooldown = GRAPPLE_COOLDOWN;
                    hookState = HOOK_FIRING;
                    fireTimer = EXTENSION_DURATION;
                }
                break;
            case (HOOK_FIRING):
                //Line Graphic
                line.SetPosition(0, hookStart.transform.position);
                line.SetPosition(1, hookEnd.transform.position);

                fireTimer -= Time.deltaTime;
                hook.gameObject.transform.localPosition = new Vector3(hook.gameObject.transform.localPosition.x, -EXTENSION_LENGTH * (1 - fireTimer/EXTENSION_DURATION), hook.gameObject.transform.localPosition.z);
                Collider2D hit = Physics2D.OverlapPoint(new Vector2(hook.gameObject.transform.position.x, hook.gameObject.transform.position.y), 1 << LayerMask.NameToLayer("asteroid"));
                
                if (hit)
                {
                    hookState = HOOK_GRABBED;

                    tether.enabled = true;
                    tether.connectedBody = hit.gameObject.GetComponent<Rigidbody2D>();
                    tether.distance = -hook.gameObject.transform.localPosition.y;

                    hookTarget = hit.gameObject;
                    hookTarget.gameObject.GetComponent<Asteroid>().isHooked = true;
                    hookTarget.gameObject.GetComponent<Rigidbody2D>().mass = .0001f;
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
                if (cooldown <= 0 && (Input.GetAxisRaw("Vertical") == -1 || hookTarget == null))
                {
                    tether.enabled = false;
                    tether.connectedBody = null;

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

    void resetHook()
    {
        hookState = HOOK_RESETTING;
        hook.transform.DOLocalMove(originalHookLocalPosition, EXTENSION_DURATION / 2).OnComplete(setHookToInCannon);
        if (hookTarget != null)
        {
            hookTarget.GetComponent<Asteroid>().isHooked = false;
            hookTarget = null;
            hookTarget.gameObject.GetComponent<Rigidbody2D>().mass = 1.0f;
        }
    }

    void setHookToInCannon()
    {
        line.SetPosition(1, hookEnd.transform.position);
        hookState = HOOK_IN_CANNON;
    }
}
