using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vdopia;

public class Ads : MonoBehaviour {
	public static readonly string SSP_KEY = "p0XmSW";

	VdopiaPlugin plugin;

	public bool isInterstitialLoaded = false;

	private string latitude;
	private string longitute;


	void Wake(){
		
		latitude = LatlongCoordinates.Instance.latitude.ToString();
		longitute = LatlongCoordinates.Instance.longitude.ToString();
		Debug.Log("debug log ads wake => Lat and Long Start "+ " Lat => "+latitude + " Long => " +longitute);
	}

	void Start()
	{
		Screen.fullScreen = false;      //Disable Fullscreen App

		if (Application.platform == RuntimePlatform.Android) {
			plugin = VdopiaPlugin.GetInstance ();        //Initialize Plugin Instance

			if (plugin != null) {
				//Set Delegate Receiver For Vdopia SDK Ad Event (Not Complsory)
				VdopiaListener.GetInstance ().VdopiaAdDelegateEventHandler += onVdopiaEventReceiver;

				//Set USER parameter used for better Ad targeting and higher yield (Not Complsory)
				//Developer can pass empty string for any Param like ""
				//Param 1 : Age
				//Param 2 : BirthDate (dd/MM/yyyy)
				//Param 3 : Gender (m/f/u)
				//Param 4 : Marital Status (single/married/unknown)
				//Param 5 : Ethinicty (example : Africans/Asian/Russians)
				//Param 6 : DMA Code (in String format)
				//Param 7 : Postal Code (in String format)
				//Param 8 : Current Postal Code (in String format)

				Debug.Log("debug log ads => Lat and Long Start "+ " Lat => "+latitude + " Long => " +longitute);
				plugin.SetAdRequestUserData ("23", "23/11/1990", "m", "single", "Asian", "999", "123123", "321321",latitude,longitute);


				//Set APP parameter used better Ad targeting and higher yield (Not Complsory)
				//Developer can pass empty string for any Param like ""
				//Param 1 : App Name
				//Param 2 : Publisher Name
				//Param 3 : App Domain
				//Param 4 : Publisher Domain
				//Param 5 : PlayStore URL of the App
				//Param 6 : Ad Category
				plugin.SetAdRequestAppData ("UnityDemo", "Vdopia Demo", " ", "vdopia.com", " ", "Business");

				plugin.PrefetchRewardAd(SSP_KEY);


				//Set Test Mode parameter used for Getting Test AD (Not Complsory)
				//Param 1 : boolean : true if test mode enabled else false
				//Param 2 : Hash ID (If you are testing Facebook/Google Partner Test Ad you can get from ADB Logcat)
				//plugin.SetAdRequestTestMode(true, "XXXXXXXXXXXXXXXX");
			} else {
				Debug.Log ("debug log ads => Vdopia Plugin Initialize Error.");
			}
		} else {

			Debug.Log ("debug log ads => Application platform is not matcth with running platform" + Application.platform + "  " + RuntimePlatform.Android);
		}


	}

	void Update()
	{
		if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))    //Quit App When Back Pressed
		{
			Application.Quit();
		}

		latitude = LatlongCoordinates.Instance.latitude.ToString();
		longitute = LatlongCoordinates.Instance.longitude.ToString();

		//Debug.Log("debug log ads => debug log ads Lat and Long => "+ " Lat => "+latitude + " Long => " +longitute);

	}

	void onVdopiaEventReceiver(string adType, string eventName)     //Ad Event Delegate Receiver
	{
		Debug.Log("debug log ads => Vdopia Ad Event To Publisher : " + eventName + " : For Ad Type : " + adType);

		if (adType.Equals(VdopiaPlugin.REWARD_AD_TYPE))
		{
			if (eventName.Equals(VdopiaPlugin.REWARD_AD_LOADED))
			{
				isInterstitialLoaded = true;
			}
			else
			{
				isInterstitialLoaded = false;
			}
		}
	}

	public void LoadInterstitial()     //Interstitial Ad Button Load Clicked
	{
		Debug.Log("debug log ads => Loading Interstitial Ads... key: " + SSP_KEY);
		if (Application.platform == RuntimePlatform.Android && plugin != null) {
			//Param 1 : AdUnit Id (Get it from Vdopia Portal)
			plugin.RequestRewardAd (SSP_KEY);
		}
	}

	public void ShowInterstitial()     //Interstitial Ad Button Show Clicked
	{
		Debug.Log("debug log ads => Showing Interstitial Ads...");
		if (Application.platform == RuntimePlatform.Android && plugin != null)
		{
			isInterstitialLoaded = false;
			plugin.ShowRewardAd("qj5ebyZ0F0vzW6yg", "Chocolate1", "coin", "30");
			plugin.PrefetchRewardAd (SSP_KEY);
		}
	}
}
