using UnityEngine;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class Particle : MonoBehaviour {

    public Vector3 targetLocation = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
        gameObject.transform.DOMove(targetLocation, 1f).SetEase(Ease.InOutQuad).OnComplete(KillYourself);
    }
	

    void KillYourself()
    {
        Destroy(gameObject);
    }
}
