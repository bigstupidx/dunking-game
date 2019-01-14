using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;*/

public class GSL : MonoBehaviour {

	public string leaderboardID;

	public static GSL gsl;

	void Awake(){
		if (GSL.gsl != null && GSL.gsl != this) Destroy (gameObject);
		gsl = this;
		DontDestroyOnLoad (gameObject);
	}

	/*void Start () {
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.RequestEmail()
			.RequestServerAuthCode(false)
			.RequestIdToken()
			.Build();

		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();

		Social.localUser.Authenticate((bool success) => { });
	}*/

	void Update () {
		
	}

	public void UploadScoreToLeaderboard(int score){
		if (Social.localUser.authenticated) {
			Social.ReportScore (score, leaderboardID, (bool success) => {
				
			});
		}
	}

	public void ShowLeaderboard(){
		if (Social.localUser.authenticated) {
			UploadScoreToLeaderboard (PlayerPrefs.GetInt ("highScore"));
			Social.ShowLeaderboardUI();
		} else {
			Social.localUser.Authenticate ((bool success) => { 
				if (success) ShowLeaderboard();
			});
		}
	}

	public void LoadScene(int index){
		SceneManager.LoadScene (index);
	}
}