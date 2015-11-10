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
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email","user_friends"},AuthCallback);
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
                            "Test Description",
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
}
