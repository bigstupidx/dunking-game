using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Advertisements;

public class Bola : MonoBehaviour {

	public float power, posBatas;
	public bool kanan, isPerfect;

	public GameObject perfectObj, gameOverObj, continueBtn;
	public ParticleSystem ps;
	public Sprite[] sprite;
	public Text perfectTxt, highScoreTxt, goalPerfectTxt;

	[HideInInspector] public admobSc _admobScript;

	int perfectIndex;
	float curStartSizePS;
	Rigidbody2D rb;
	CircleCollider2D circleCol;
	SpriteRenderer sr;
		
	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		circleCol = GetComponent<CircleCollider2D> ();
		sr = GetComponent<SpriteRenderer> ();
	}

	void Start () {
		ps = GM.gm.ballParticles [PlayerPrefs.GetInt ("indexBall")];
		//Advertisement.Initialize ("1603963");
		curStartSizePS = ps.startSize;
		isPerfect = true;
	}

	void Update () {
		if (GM.gm.curTime () > 0f) {
			if (Input.GetMouseButtonDown (0)) {
				rb.velocity = Vector2.zero;
				rb.AddForce (new Vector2 (kanan ? power * 1.2f : -power * 1.2f, power * 8));
				rb.velocity += new Vector2 (0, -0.2f);
				ps.startSize = curStartSizePS * ((perfectIndex > 2 ? 2 : perfectIndex) + 1) * 1.2f;
				ps.Play ();
			}
		} else {
			if (rb.velocity.y == 0f && !gameOverObj.activeInHierarchy) {
				gameOverObj.SetActive (true);
				//_admobScript.showInterstitialAdmob ();
				//if (!Advertisement.IsReady ()) continueBtn.SetActive (false);
				int score = GM.gm.Score (); 
				//var tempScore = PlayerPrefs.GetInt ("score");
				var tempScore = score;
				if (PlayerPrefs.GetInt ("highScore") < tempScore) {
					PlayerPrefs.SetInt ("highScore", tempScore);
					highScoreTxt.text = "HIGH SCORE: " + tempScore;
				} else highScoreTxt.text = "HIGH SCORE: " + PlayerPrefs.GetInt ("highScore");
				PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt("Coins") + tempScore);
				//GSL.gsl.UploadScoreToLeaderboard (PlayerPrefs.GetInt ("highScore"));
			}
		}
		if (transform.position.x < -posBatas && !kanan) {
			circleCol.enabled = false;
			Vector2 pos = transform.position;
			pos.x = posBatas;
			transform.position = pos;
			StartCoroutine (fixJaringBug ());
		} else if (transform.position.x > posBatas && kanan) {
			circleCol.enabled = false;
			Vector2 pos = transform.position;
			pos.x = -posBatas;
			transform.position = pos;
			StartCoroutine (fixJaringBug ());
		}
	}

	IEnumerator fixJaringBug(){
		yield return new WaitForFixedUpdate();
		circleCol.enabled = true;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Ring" && rb.velocity.y < 0f) {
			if (perfectIndex > 2) other.GetComponentInParent<Animator> ().SetTrigger ("Terbakar");
			if (isPerfect) perfectIndex++;
			GM.gm.UpScore (isPerfect ? perfectIndex * 2 : 1, 0.5f);
			if (isPerfect) {
				perfectObj.transform.position = transform.position;
				perfectObj.GetComponent<Animator> ().SetTrigger ("Perfect");
				goalPerfectTxt.text = cheerGen() + "\n+" + perfectIndex * 2;
				GM.gm.canvasAnim.SetTrigger ("PerfectStart");
				perfectTxt.text = "PERFECT x" + perfectIndex;
				Handheld.Vibrate ();
			} else {
				perfectIndex = 0;
				perfectTxt.text = "";
			}
			kanan = !kanan;
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.collider.tag == "Ring")
			StartCoroutine (NotPerfectCD ());
	}

	public void Continue(){
		//ShowAd ();
	}

	IEnumerator NotPerfectCD(){
		isPerfect = false;
		yield return new WaitForSeconds (0.75f);
		isPerfect = true;
	}

	string cheerGen(){
		int wordIndex = Random.Range (0, 10);
		return (wordIndex == 0 ? "GOOD!" : wordIndex == 1 ? "NICE!" : wordIndex == 2 ? "GREAT!" :  
			wordIndex == 3 ? "AWESOME!" : wordIndex == 4 ? "MASTER!" : wordIndex == 5 ? "PERFECT!" : wordIndex == 6 ? "GOD LIKE!" : 
			wordIndex == 7 ? "AMAZING!" : wordIndex == 8 ? "UNBEATABLE!" : wordIndex == 9 ? "LEGENDARY!" : "BULL'S EYE");
	}


	/*void ShowAd (){
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		Advertisement.Show("rewardedVideo", options);
	}

	void HandleShowResult (ShowResult result)
	{
		if(result == ShowResult.Finished) {
			GM.gm.UpScore (0, 0f);
			transform.position = Vector2.zero;
			rb.velocity = Vector2.up * 1f;
			kanan = !kanan;
			gameOverObj.SetActive (false);
		} else if(result == ShowResult.Skipped) {
			Debug.LogWarning("Video was skipped - Do NOT reward the player");
		} else if(result == ShowResult.Failed) {
			Debug.LogError("Video failed to show");
		}
	}*/
}