using UnityEngine;
using System.Collections;
using Facebook.Unity;
using Facebook.MiniJSON;
using System.Collections.Generic;
using UnityEngine.UI;

public class FBHolder : MonoBehaviour {

    public GameObject UIFBAvatar;
    public GameObject UIFBUserName;
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
}
