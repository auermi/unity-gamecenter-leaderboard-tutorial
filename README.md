# unity-gamecenter-leaderboard-tutorial
A short tutorial for getting your application working with Unity's leaderboard

#### Introduction

Unity has a handy API for iOS Game Center Integration called [Social](http://docs.unity3d.com/Manual/net-SocialAPI.html) which you can access via an import:

```c#
using UnityEngine.SocialPlatforms;
```
With their library you can integrate Game Center Leaderboards, Achievements, Friends Lists, etc. into your Unity application. I am going to show you how to implement Game Center Leaderboards.

#### Set Up iTunes Connect

In order to use a leaderboard in Game Center with your iOS application you must register your app with iTunes Connect. I am going to walk you step by step through this process.

- Login to your iTunes Connect account.

- Navigate [here](https://developer.apple.com/account/ios/identifiers/bundle/bundleCreate.action) to create our BundleID 

- Follow the instructions, filling out the both the App ID Description and the Bundle ID fields.

- Navigate back to the iTunes connect dashboard.

- Click on the My Apps Icon.

- Click on the plus sign in the top right corner and click the “New iOS App dialogue".

- You will be presented with a form. Fill out all the fields. You should see your BundleID in the dropdown menu here. If not you incorrectly filled out the BundleID form.

- Now you should be in your app’s dashboard. Great job! Next we will navigate to the game center tab.

- You will be presented with a screen asking you tenable game center for a single game or for a group of games. We want to choose single game.

- Next we want to click on the “Add Leaderboard” button under the Leaderboards section.

- Click on Single Leaderboard.

- Fill out the form on this page. Give the Leaderboard Reference Name a recognizable name. It will be the inward facing name of the keyboard from iTunes Connect. The Leaderboard ID is what we will use to reference the Leaderboard in our code (since we are starting with one leaderboard we can give it an ID of “1”).

- Let’s set our Score Format type to Integer.

- Leave Score Submission type at “Best Score”.

- Sort order is “High to Low”.

- Leave the “Score Range” field blank.

- Next click on the add language button.

- Fill out the form here.
 
- Set the language (You can add multiple localizations).

- The name field is the name of the leaderboard as seen by your users.

- Set score format based on your preference. It doesn’t matter too much because the sample application I give you will have a max score of 100.

- Save the language form and then save the leaderboard form.

- Congratulations! You have added your scoreboard to iTunes Connect! Now lets hop into Unity and get it working.

#### Preparing our application in Unity

Open the sample application I have provided you. It’s just a canvas with some buttons and event listeners already set up already for you. We are going to do the Unity social integration ourselves.

*If you wish to see the final results enter the following commands. (Make sure you have git installed!)*

```shell
cd <your project directory>
```
*and*
```shell
git checkout final-project
```

*To get back to our tutorial branch, in your directory, type:*
```shell
git checkout master
```
*If at any time you get stuck and wish to reset the tutorial type:*
```shell
git reset --hard 202868c
```
So the project we are working from has one scene already prepped for us so lets open it up and dive right in to integrating our leaderboard.

Before we start coding the leaderboard into our project lets get a few things set up. 

Navigate to File > Build Settings and in the modal hit player settings. In the inspector window you should see your build settings. Under the *Other Settings* tab fill in the Bundle Identifier with the one we just registered on iTunes Connect. It MUST match this one otherwise we won't be able to access our leaderboard. Also while your at it click on the *Resolution and Presentation* tab and change *Default Orientation* to *Landscape Left*.

Something else you must do is to enable Playground for Game Center on your iOS device so that we can test our app without publishing it. To do this make sure you are plugged in and have Xcode up in running. If you restart your device the option should show up inside the GameCenter settings inside the iOS settings.

### Scripting our Leaderboard integration

Now we are ready to implement our leaderboard. So in the project pane double click *GameCenterLeaderboard.cs* to open the code file we will be working from. I already have thrown in a bunch of code here to make it easier for us to get up and running with simple score reporting. In the file we have made three imports:

```c#
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
```

One for our standard Unity libs, our UI libs for our UI elements, and the Social API. Next going to be using five public Transforms for the GameObjects that we are already referencing in our code. They will be listening for interaction that we will be implementing. 

I have also already coded functionality to update the UI text for our app when the slider value is changed.

So here is what we have to code. We want to add Leaderboard integration in our app so we need to implement two things. We need to authenticate our Game Center user, and then report our scores to our leaderboard. We will also be integrating a neat UI View that the Unity Social API gives us by default for the Leaderboard.

#### Game Center Authentication
Let's get started with Unity's Game Center `Authenticate()` method. We are going to wrap this in our Start method.

```c#
void Start () {
    // Authenticate
    Social.localUser.Authenticate (ProcessAuthentication);
}
``` 

We are passing in a callback method `ProcessAuthentication()` which provides error handling for our authentication. Let's include this function below our start method.

```c#
void ProcessAuthentication (bool success) {
    if (success) {
        Debug.Log ("Authenticated, checking achievements");
    } else {
	Debug.Log ("Failed to authenticate");
    }     
}
```

#### Report Score to Game Center

Great! So now we can successfully authenticate our app. let's move on to reporting our score with Unity's baked in method `Social.ReportScore()`. We are going to wrap this method call in a function that provides error handling. Below our `ProcessAuthentication()` method type the following.

```c#
void ReportScore (long score, string leaderboardID) {
    Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
    Social.ReportScore (score, leaderboardID, success => {
	Debug.Log(success ? "Reported score successfully" : "Failed to report score");
    });
}
```

#### Link our methods to UI Buttons

Ok so now that we have defined our report score method let's call it. We are going to insert it into an event handler for our *sendScoreButton* which has conveniently been set up for you already. 

```c#
sendScoreButton.GetComponent<Button>().onClick.AddListener(() => { ReportScore(sliderVal, "1"); });
```

The last thing we need to add is a way to view our scores from within the app. Thankfully, Unity has done all the dirty work to give us a baked in UI View for the leaderboards. Awesome right? And it's only one line to implement! We will be calling the method, `Social.ShowLeaderboardUI()` from our checkScoreBoardButton event listener which has also been set up for you.

```c#
checkScoreboardButton.GetComponent<Button>().onClick.AddListener(() => { Social.ShowLeaderboardUI(); });
```

Great! And that is all we are doing with our script here. Simple right? If you understand everything we just did, lets move on to preparing the app to run on our devices.

### Wrapping up

One last thing we must do is enable Game Center from inside Xcode. To do this first build the application to Xcode from Unity. Next navigate to the project settings and enable Game Center. One thing that is very important to note is that every time you rebuild your project from Unity to Xcode it breaks one of the Game Center libs. To fix this you would just want to hit the *Fix* dialogue button that pops up if there is an issue every time before you build to your device.

Now run the app! If you followed my instructions carefully you will have a functioning scoreboard in your application! Neat right? I hope this tutorial was helpful. Thanks for reading!
