using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour{

	public GameObject[] listObjBola, listObjBg;
	public Text coinsTxt;

	int tempHarga;

	void Start (){
		PlayerPrefs.GetInt ("Coins", PlayerPrefs.GetInt("highScore"));
		Refresh ();
	}

	void Update (){
	
	}

	void Refresh(){
		foreach (GameObject obj in listObjBola) obj.SetActive (false);
		foreach (GameObject obj in listObjBg) obj.SetActive (false);
		listObjBola[PlayerPrefs.GetInt("indexBall",  0)].SetActive(true);
		listObjBg[PlayerPrefs.GetInt("indexBg",  0)].SetActive(true);
		coinsTxt.text = "Total Score: " + PlayerPrefs.GetInt ("Coins", PlayerPrefs.GetInt("highScore")).ToString("N0");
	}

	public void ShowLeaderboard(){
		GSL.gsl.ShowLeaderboard ();
	}

	public void LoadScene(int index){
		GSL.gsl.LoadScene (index);
	}

	public void SetHarga (int harga){
		tempHarga = harga;
	}

	public void BuyBall(int index){
		if (PlayerPrefs.GetInt ("buyedBall" + index, 0) > 0) { //jika sudah dibeli
			PlayerPrefs.SetInt ("indexBall", index);
			Refresh ();
		} else {
			int totalCoins = PlayerPrefs.GetInt ("Coins", PlayerPrefs.GetInt("highScore"));
			if (totalCoins >= tempHarga) {
				PlayerPrefs.SetInt ("Coins", totalCoins - tempHarga);
				PlayerPrefs.SetInt ("indexBall", index);
				PlayerPrefs.SetInt ("buyedBall" + index, 1);
				Refresh ();
			} else {
				print ("uang kurang");
			}
		}
	}

	public void BuyBg(int index){
		if (PlayerPrefs.GetInt ("buyedBg" + index, 0) > 0) { //jika sudah dibeli
			PlayerPrefs.SetInt ("indexBg", index);
			Refresh ();
		} else {
			int totalCoins = PlayerPrefs.GetInt ("Coins", PlayerPrefs.GetInt("highScore"));
			if (totalCoins >= tempHarga) {
				PlayerPrefs.SetInt ("Coins", totalCoins - tempHarga);
				PlayerPrefs.SetInt ("indexBg", index);
				PlayerPrefs.SetInt ("buyedBg" + index, 1);
				Refresh ();
			} else {
				print ("uang kurang");
			}
		}
	}
}