using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Vdopia;
using System.Net;

public class Done_GameController : MonoBehaviour
{
	public GameObject[] hazards;
	public GameObject[] showAds;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Text scoreText;
	public Text gameOverText;
	public Text cordinates;

	public GameObject restartButton;
	public GameObject quitButton;

	private bool gameOver;
	private int score;

	public static Done_GameController Instance;

	public InternetConnectivity internetConnectivity;
	public Ads vdopiaAds;

	void Awake ()   
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start ()
	{
		score = PlayerPrefs.GetInt("score",0);

		gameOver = false;
		gameOverText.text = "";

		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}

	void Update(){
		
		if (gameOver) {
			restartButton.SetActive (true);
			quitButton.SetActive (false);
		} else {
			restartButton.SetActive (false);
			quitButton.SetActive (true);	
		}

		Debug.Log ("debug log => This is game over ......." + gameOver);
		cordinates.text="Lat:" + LatlongCoordinates.Instance.latitude.ToString() + "  Long:" + LatlongCoordinates.Instance.longitude.ToString();

	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

			if (gameOver)
			{
				restartButton.SetActive (true);
				break;
			}
		}
	}

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
		PlayerPrefs.SetInt("score",score);
	}

	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
    //  Vdopia Ad request is call only when the game is over
		AdRequest();
	}

	public void RestartGame () {

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// Vodpia plugin ad functions are call from here

	public void AdRequest(){

		if (internetConnectivity.connectivity && gameOver) {
			Debug.Log ("debug log => Loading AdsLoad function....... " + Time.deltaTime);
			vdopiaAds.LoadInterstitial ();
			Debug.Log ("debug log => Ad request done........." + Time.deltaTime);
			Invoke ("AdRender", 18);
		} else {
			Debug.Log ("debug log => Internet Connectivity " + internetConnectivity.connectivity);

		}

	}

	// Ad render function 
	public void AdRender(){
		Debug.Log ("debug log => Calling ShowAd function........ " + Time.deltaTime);
		if (vdopiaAds.isInterstitialLoaded) {
			vdopiaAds.ShowInterstitial();
			Debug.Log ("debug log => INTERSTITIAL_AD_LOADED......." + Time.deltaTime);
		} else{
			Debug.Log ("debug log => INTERSTITIAL_AD_LOADED is not true......." + Time.deltaTime);
		}
	}

	public void QuitGame(){
		// flushing the session score
		PlayerPrefs.SetInt("score",0);
		// exiting the app
		Application.Quit();
	}
}