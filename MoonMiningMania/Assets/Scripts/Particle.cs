using UnityEngine;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class Particle : MonoBehaviour {
    
    public Sprite[] sprite;
    public Vector3 targetLocation = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
    }
	

    void KillYourself()
    {
        Destroy(gameObject);
    }

    public void InitializeParticle(int player)
    {
        switch(player)
        {
            case 1:
                GetComponent<SpriteRenderer>().sprite = sprite[0];
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = sprite[1];
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = sprite[2];
                break;
            case 4:
                GetComponent<SpriteRenderer>().sprite = sprite[3];
                break;
            default:
                break;
        }
        gameObject.transform.DOMove(targetLocation, 1f).SetEase(Ease.InOutQuad).OnComplete(KillYourself);
    }
}
