using UnityEngine;
using System.Collections;

public class MoonScript : MonoBehaviour {

    //EXTERNAL LINKS
    public GameObject earth;

    //EXTERNAL VARIABLES
    public float SECONDS_FOR_REVOLUTION = 10f;

    private Vector2 baseAnchorPosition;
    private float timer;


    void Start () {
        baseAnchorPosition = transform.position - earth.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0) timer = SECONDS_FOR_REVOLUTION;

        float rotationPercent = timer / SECONDS_FOR_REVOLUTION;
        Vector2 currentAnchorPosition = rotateVectorByDegrees(baseAnchorPosition, rotationPercent * 360);
        transform.position = new Vector3(currentAnchorPosition.x, currentAnchorPosition.y, transform.position.z);

    }

    Vector2 rotateVectorByDegrees(Vector2 startVec, float degrees)
    {
        float theta = degrees * Mathf.Deg2Rad;

        float cs = Mathf.Cos(theta);
        float sn = Mathf.Sin(theta);

        return new Vector2(startVec.x * cs - startVec.y * sn, startVec.x * sn + startVec.y * cs);
    }
}
