using UnityEngine;
using System.Collections;
using Facebook.Unity;
using Facebook.MiniJSON;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class FBHolder : MonoBehaviour {

    public GameObject UIFBAvatar;
    public GameObject UIFBUserName;
    public GameObject UserPhoto;
    public Text scoresDebug;
    private List<object> scoreList = null;
    public GameObject ScoreEntryPanel;
    public GameObject ScoreScrolList;
    public GameObject gameScore;
    public GameObject sharetext;

    private Dictionary<string, string> profile = null;

    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    private void SetInit()
    {
        Debug.Log("FB Init done");
        if (FB.IsLoggedIn)
        {
            DealWithFBMenus(true);
            Debug.Log("FB Logged In");
        }
        else
        {
            DealWithFBMenus(false);
            FBlogin();
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FBlogin()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email","user_friends", "publish_actions"},AuthCallback);
    }

    void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("FB login worked");
            DealWithFBMenus(true);
        }
        else
        {
            Debug.Log("Fb login Fail");
            DealWithFBMenus(false);
        }
    }

    void DealWithFBMenus(bool isLoggenIn)
    {
        if (isLoggenIn)
        {
            //get profile picture code
            FB.API("/me/picture", HttpMethod.GET, DealWithProfilePicture);

            FB.API("/me?fields=id,first_name", HttpMethod.GET, DealWithUserName);

            QueryScores();
        }
        else
        {

        }
    }

    void DealWithProfilePicture(IGraphResult result) 
    {
        if (result.Error != null)
        {
            Debug.Log("problem with getting profile picture");
            FB.API("/me/picture", HttpMethod.GET, DealWithProfilePicture);
            return;
        }

        Image UserAvatar = UIFBAvatar.GetComponent<Image>();
        UserAvatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, result.Texture.width, result.Texture.height), new Vector2(0.5f, 0.5f));
        UserPhoto.GetComponent<Image>().sprite = Sprite.Create(result.Texture, new Rect(0, 0, result.Texture.width, result.Texture.height), new Vector2(0.5f, 0.5f));
    }

    void DealWithUserName(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("problem with getting profile picture");
            FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,picture)", HttpMethod.GET, DealWithUserName);
            return;
        }

        profile = Util.DeserializeJSONProfile(result.RawResult);
        Text Usertext = UIFBUserName.GetComponent<Text>();
        Usertext.text = " " + profile["first_name"];
    }

    public void ShareWithFriends()
    {
        /*FB.FeedShare(
            linkCaption: "I'm playing this game.",
            picture: "http://www.procabello.com/game//friendsmash_social_persistence_payments/images/logo_large.png",
            linkName: "Check out this game",
            link: "https://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? "me" : "guest"),
            callback:null
            );*/

        FB.FeedShare(
                            string.Empty,
                            new Uri("https://developers.facebook.com/"),
                            "Test Title",
                            "Test caption",
                            sharetext.GetComponent<Text>().text,
                            new Uri("http://i.imgur.com/zkYlB.jpg"),
                            string.Empty,
                            this.HandleResult);
    }

    public void InviteFriends()
    {
        FB.AppRequest(
            message: "This game is awesome, join me, now.",
            title:"Invite your friends to join you"
            );
    }

    protected void HandleResult(IResult result)
    {
        if (result == null)
        {
            return;
        }
        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
        }
        else if (result.Cancelled)
        {

        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
        }
        else
        {

        }
    }

    // All scores API related thinks
    public void QueryScores()
    {
        FB.API("/app/scores?fields=score,user.limit(30)", HttpMethod.GET, ScoresCallback);
    }

    private void ScoresCallback(IGraphResult result)
    {
        Debug.Log("Scores callback: " + result.RawResult);
        

        scoreList = Util.DeserializeScores(result.RawResult);

        foreach(Transform child in ScoreScrolList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (object score in scoreList)
        {
            var entry = (Dictionary<string, object>) score;
            var user = (Dictionary<string, object>) entry["user"];
            //scoresDebug.text = scoresDebug.text + "UN: " + user["name"] + " - " + entry["score"] + ", ";

            GameObject ScorePanel;
            ScorePanel = Instantiate(ScoreEntryPanel) as GameObject;
            ScorePanel.transform.parent = ScoreScrolList.transform;
            Transform ThisScoreName = ScorePanel.transform.Find("FriendName");
            Transform ThisScoreScore = ScorePanel.transform.Find("FriendScore");
            Text ScoreName = ThisScoreName.GetComponent<Text>();
            Text ScoreScore = ThisScoreScore.GetComponent<Text>();

            ScoreName.text = user["name"].ToString();
            ScoreScore.text = entry["score"].ToString();

            Transform TheUserAvatar = ScorePanel.transform.Find("FriendAvatar");
            Image UserAvatar = TheUserAvatar.GetComponent<Image>();

            FB.API(user["id"].ToString() + "/picture", HttpMethod.GET, delegate(IGraphResult pictureresult) {
                if(pictureresult.Error != null)
                {
                    Debug.Log(pictureresult.Error);
                }
                else
                {
                    UserAvatar.sprite = Sprite.Create(pictureresult.Texture, new Rect(0, 0, pictureresult.Texture.width, pictureresult.Texture.height), new Vector2(0.5f, 0.5f));
                }
            });
        }
    }
    public void SetScore()
    {
        var scoredata = new Dictionary<string, string>();
        scoredata["score"] = gameScore.GetComponent<Text>().text.ToString();
        FB.API("/me/scores", HttpMethod.POST, delegate (IGraphResult result)
        {
            Debug.Log("Score submit result: " + result.RawResult);
        }, scoredata);
    }

}
