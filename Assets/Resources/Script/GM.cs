using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GM : MonoBehaviour{

	[Header ("Goal Setup")]
	public Transform[] goalObj;
	public float yRange;

	[Header ("Time Setup")]
	public float maxTime;
	public float timeReducePerScore, timeReduceAmount;
	public Text timeTxt;
	public Image timeImg;

	[Header("Score Tambahan")]
	public Text scoreTxt, tambahScoretxt;
	public Animator canvasAnim;
	public int ronde;

	[Header ("items")]
	public ParticleSystem[] ballParticles;
	public GameObject[] bg;

	int score;
	int curGoal;
	float time, tempTimeReducePerScore;
	public static GM gm;

    public GameObject pauseMenu;

	[HideInInspector] public admobSc _admobScript;

    public void pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void unPause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void goToMenu()
    {
        Time.timeScale = 1;
        Application.LoadLevel(0);
    }

	void Awake(){
		gm = this;
	}

	void Start (){
		int indexBola = PlayerPrefs.GetInt ("indexBall");
		ballParticles [indexBola].transform.parent = null;
		ballParticles [indexBola].gameObject.SetActive(true);
		bg [PlayerPrefs.GetInt ("indexBg")].SetActive (true);

		score = 0;
		PlayerPrefs.SetInt ("score", 0);
		tempTimeReducePerScore = timeReducePerScore;
		time = maxTime;
		timeTxt.text = ""+time;
		timeImg.fillAmount = 1f;
		StartCoroutine (InitializeGoalObj (0f));
	}

	void Update (){
		if (time > 0f) {
			time -= Time.deltaTime;
			if (time == 0f) Handheld.Vibrate ();
			canvasAnim.SetBool ("timeAlert", time < (maxTime / 2f));
			timeTxt.text = "" + Mathf.Round (time);
			timeImg.fillAmount = (time / maxTime);
		} else {
			canvasAnim.SetBool ("timeAlert", false);
		}
	}

	public void UpScore(int amount, float delay){
		int tempScore = PlayerPrefs.GetInt ("score");
		if (score != tempScore) score = tempScore;
		score += amount;
		tambahScoretxt.text = "+" + amount;
		PlayerPrefs.SetInt ("score", score);
		StartCoroutine (InitializeGoalObj (delay));
	}

	IEnumerator InitializeGoalObj(float delay){
		if (score > 0) canvasAnim.SetTrigger ("TambahScore");
		if (score >= timeReducePerScore) {
			timeReducePerScore += tempTimeReducePerScore;
			maxTime -= timeReduceAmount;
		}
		yield return new WaitForSeconds (delay);
		time = maxTime;
		timeTxt.text = ""+time;
		timeImg.fillAmount = 1f;
		scoreTxt.text = "" + score;
		goalObj [curGoal].gameObject.SetActive (false);
		curGoal = (curGoal+1) % 2;
		goalObj[curGoal].position = new Vector3 (goalObj[curGoal].position.x, Random.Range (-yRange, yRange), -10f);
		goalObj [curGoal].gameObject.SetActive (true);
	}

	public float curTime(){
		return time;
	}

	public int Score(){
		return score;
	}

	public void ShowLeaderboard(){
		GSL.gsl.ShowLeaderboard ();
	}

	public void LoadScene(int index){
		if (index == 0) {
			if (_admobScript != null) _admobScript._destroyBanner();
		}
		GSL.gsl.LoadScene (index);
	}
}