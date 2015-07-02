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
		checkScoreboardButton.GetComponent<Button>().onClick.AddListener(() => {  });
		sendScoreButton.GetComponent<Button>().onClick.AddListener(() => {  });
	}
	
	// We run this when the slider value is changed
	void updateScoreText() {
		sliderVal = (long) slider.GetComponent<Slider>().value;
		scoreText.GetComponent<Text>().text = sliderVal.ToString();
	} 
	
	
}