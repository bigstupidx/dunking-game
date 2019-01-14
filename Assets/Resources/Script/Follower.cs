using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour{

	public Transform target;
	public float maxMag;
    public bool isBallShadow;

	void LateUpdate (){
		if (isBallShadow) {
			Vector2 targetPos = target.position;
			targetPos.y = transform.position.y;
			transform.position = targetPos;

			transform.localScale = Vector3.one * (1 - ((target.position.y - transform.position.y) / maxMag));
		} else {
			transform.position = target.position;
		}
	}
}