using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class GameCenterTest : MonoBehaviour {
	
	public Transform scoreText;
	public Transform checkScoreboardButton;
	public Transform sendScoreButton;
	public Transform slider;
	
	private long sliderVal;
	
	void Awake () {
		// Get our slider value 
		sliderVal = (long) slider.GetComponent<Slider>().value;
		// Update Score Text
		updateScoreText();
		// Event listeners
		slider.GetComponent<Slider>().onValueChanged.AddListener (delegate { updateScoreText(); });
		checkScoreboardButton.GetComponent<Button>().onClick.AddListener(() => { Social.ShowLeaderboardUI(); });
		sendScoreButton.GetComponent<Button>().onClick.AddListener(() => { ReportScore(sliderVal, "1"); });
	}
	
	void Start () {
		// Authenticate
		Social.localUser.Authenticate (ProcessAuthentication);
	}
	
	// Game Center Auth Error Handling
	void ProcessAuthentication (bool success) {
       	   	if (success) {
            		Debug.Log ("Authenticated, checking achievements");
        	} else {
			Debug.Log ("Failed to authenticate");
		}     
    	}
	
	// Pushes the score to iOS GameCenter
	void ReportScore (long score, string leaderboardID) {
		Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore (score, leaderboardID, success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
	}
	
	// We run this when the slider value is changed
	void updateScoreText() {
		sliderVal = (long) slider.GetComponent<Slider>().value;
		scoreText.GetComponent<Text>().text = sliderVal.ToString();
	} 
}
