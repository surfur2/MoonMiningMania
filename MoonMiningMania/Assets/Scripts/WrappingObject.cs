using UnityEngine;
using System.Collections;

public class WrappingObject : MonoBehaviour {

    public float screenOffSet;
    public float objectBufferOffSet;
    public float extraSizePercentage;
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
            myBoxCollider.size = new Vector2(myBoxCollider.size.x, sizeOfCollider + (sizeOfCollider * extraSizePercentage));
        }
        else if (side.ToLower() == "maxx")
        {
            Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
            Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            sizeOfCollider = Mathf.Abs(upperRightCorner.y - lowerRightCorner.y);

            gameObject.transform.position = new Vector3(lowerRightCorner.x + screenOffSet, (sizeOfCollider / 2.0f) + lowerRightCorner.y, 0);
            myBoxCollider.size = new Vector2(myBoxCollider.size.x, sizeOfCollider + (sizeOfCollider * extraSizePercentage));
        }
        else if (side.ToLower() == "miny")
        {
            Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
            sizeOfCollider = Mathf.Abs(lowerRightCorner.x - lowerLeftCorner.x);

            gameObject.transform.position = new Vector3((sizeOfCollider / 2.0f) + lowerLeftCorner.x, lowerRightCorner.y - screenOffSet, 0);
            myBoxCollider.size = new Vector2(sizeOfCollider + (sizeOfCollider * extraSizePercentage), myBoxCollider.size.y);
        }
        else if (side.ToLower() == "maxy")
        {
            Vector3 upperLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
            Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            sizeOfCollider = Mathf.Abs(upperRightCorner.x - upperLeftCorner.x);

            gameObject.transform.position = new Vector3((sizeOfCollider / 2.0f) + upperLeftCorner.x, upperRightCorner.y + screenOffSet, 0);
            myBoxCollider.size = new Vector2(sizeOfCollider + (sizeOfCollider * extraSizePercentage), myBoxCollider.size.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "asteroid" && other.gameObject.GetComponent<Asteroid>().isHooked)
        {
            return;
        }

        // This block checks if we need to wrap a asteroid with the player if it is hooked
        if (other.tag == "player" && other.gameObject.GetComponent<HookShootScript>().hookState == 2)
        {
            Vector3 tetheredAsteroidPosition = other.gameObject.GetComponent<HookShootScript>().hookTarget.gameObject.transform.position;
            GameObject tetheredAsteroid = other.gameObject.GetComponent<HookShootScript>().hookTarget.gameObject;
            if (side.ToLower() == "minx")
                WrapObject(tetheredAsteroid, new Vector3(maxX.transform.position.x - objectBufferOffSet, tetheredAsteroidPosition.y, 0));
            else if (side.ToLower() == "maxx")
                WrapObject(tetheredAsteroid, new Vector3(minX.transform.position.x + objectBufferOffSet, tetheredAsteroidPosition.y, 0));
            else if (side.ToLower() == "miny")
                WrapObject(tetheredAsteroid, new Vector3(tetheredAsteroidPosition.x, maxY.transform.position.y - objectBufferOffSet, 0));
            else if (side.ToLower() == "maxy")
                WrapObject(tetheredAsteroid, new Vector3(tetheredAsteroidPosition.x, minY.transform.position.y + objectBufferOffSet, 0));     
        }


        if (side.ToLower() == "minx")
            WrapObject(other.gameObject, new Vector3(maxX.transform.position.x - objectBufferOffSet, other.gameObject.transform.position.y, 0));
        else if (side.ToLower() == "maxx")
            WrapObject(other.gameObject, new Vector3(minX.transform.position.x + objectBufferOffSet, other.gameObject.transform.position.y, 0));
        else if (side.ToLower() == "miny")
            WrapObject(other.gameObject, new Vector3(other.gameObject.transform.position.x, maxY.transform.position.y - objectBufferOffSet, 0));
        else if (side.ToLower() == "maxy")
            WrapObject(other.gameObject, new Vector3(other.gameObject.transform.position.x, minY.transform.position.y + objectBufferOffSet, 0));
    }

    void WrapObject(GameObject wrapMe, Vector3 toHere)
    {
        wrapMe.transform.position = toHere;

        //Prevent Engine trail gfx smear from wrap
        if (wrapMe.tag == "player") wrapMe.GetComponent<Player>().DisableEngineGfx();
    }
}
