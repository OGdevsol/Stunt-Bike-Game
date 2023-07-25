using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class ShareGameLink : MonoBehaviour
{
	private string subject, ShareMessage;

	void Awake ()
	{
		subject = "https://play.google.com/store/apps/details?id=" + Application.identifier;
		ShareMessage = "https://play.google.com/store/apps/details?id=" + Application.identifier;
	}

	

	public void shareText()
	{
		//execute the below lines if being run on a Android device
#if UNITY_ANDROID
		//Refernece of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		//Refernece of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		//add data to be passed to the other activity i.e., the data to be sent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), ShareMessage);
		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		//start the activity by sending the intent data
		currentActivity.Call ("startActivity", intentObject);
#endif

	}


	public void RateUs ()
	{


		Application.OpenURL ("https://play.google.com/store/apps/details?id=" + Application.identifier);
	}


	

	public void CustomLink (string link)
	{


		Application.OpenURL ("https://play.google.com/store/apps/details?id=" + link);

	}


	public void Quit ()
	{


		Application.Quit ();
	}


}