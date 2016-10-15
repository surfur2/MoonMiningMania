using UnityEngine;
using System.Collections;

public class WrappingObject : MonoBehaviour {

    public float screenOffSet;
    public float objectBufferOffSet;
    public string side;
    public WrappingObject minX;
    public WrappingObject maxX;
    public WrappingObject minY;
    public WrappingObject maxY;

    private BoxCollider2D myBoxCollider;
    void Start()
    {
        myBoxCollider = gameObject.GetComponent<BoxCollider2D>();

        float sizeOfCollider = 0.0f;

        if (side.ToLower() == "minx")
        {
            Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 upperLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
            sizeOfCollider = Mathf.Abs(upperLeftCorner.y - lowerLeftCorner.y);

            gameObject.transform.position = new Vector3(lowerLeftCorner.x - screenOffSet, (sizeOfCollider/2.0f) + lowerLeftCorner.y , 0);
            myBoxCollider.size = new Vector2(myBoxCollider.size.x, sizeOfCollider);
        }
        else if (side.ToLower() == "maxx")
        {
            Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
            Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            sizeOfCollider = Mathf.Abs(upperRightCorner.y - lowerRightCorner.y);

            gameObject.transform.position = new Vector3(lowerRightCorner.x + screenOffSet, (sizeOfCollider / 2.0f) + lowerRightCorner.y, 0);
            myBoxCollider.size = new Vector2(myBoxCollider.size.x, sizeOfCollider);
        }
        else if (side.ToLower() == "miny")
        {
            Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
            sizeOfCollider = Mathf.Abs(lowerRightCorner.x - lowerLeftCorner.x);

            gameObject.transform.position = new Vector3((sizeOfCollider / 2.0f) + lowerLeftCorner.x, lowerRightCorner.y - screenOffSet, 0);
            myBoxCollider.size = new Vector2(sizeOfCollider, myBoxCollider.size.y);
        }
        else if (side.ToLower() == "maxy")
        {
            Vector3 upperLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
            Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            sizeOfCollider = Mathf.Abs(upperRightCorner.x - upperLeftCorner.x);

            gameObject.transform.position = new Vector3((sizeOfCollider / 2.0f) + upperLeftCorner.x, upperRightCorner.y + screenOffSet, 0);
            myBoxCollider.size = new Vector2(sizeOfCollider, myBoxCollider.size.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (side.ToLower() == "minx")
            other.gameObject.transform.position = new Vector3(maxX.transform.position.x - objectBufferOffSet, other.gameObject.transform.position.y, 0);
        else if (side.ToLower() == "maxx")
            other.gameObject.transform.position = new Vector3(minX.transform.position.x + objectBufferOffSet, other.gameObject.transform.position.y, 0);
        else if (side.ToLower() == "miny")
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, maxY.transform.position.y - objectBufferOffSet, 0);
        else if (side.ToLower() == "maxy")
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, minY.transform.position.y + objectBufferOffSet, 0);
    }
}
