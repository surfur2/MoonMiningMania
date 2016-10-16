using UnityEngine;
using System.Collections;

public class HookRope : MonoBehaviour {

    public GameObject hookStart;
    public GameObject hookEnd;

    private LineRenderer line;

	void Start () {
        line = GetComponent<LineRenderer>();
	}
	
	void Update () {
        
	}
}
