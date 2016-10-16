using UnityEngine;
using System.Collections;

public class SpringAnchorAdjuster : MonoBehaviour {

    public GameObject anchorHere;

    SpringJoint2D joint;

	// Use this for initialization
	void Start () {
        joint = GetComponent<SpringJoint2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
