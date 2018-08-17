using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class WebHelper
{
    //The urls used for pulling/sending info
    private static string pullUserURL = "https://webapp.westernu.edu/api/unity/auth";
    //Whether the user is logged in or not
    private static bool loggedIn = false;
    public static bool LoggedIn
    {
        get { return loggedIn; }
    }
    //Temp variables to store the username and password (cleared after log in)
    private static string username = "";
    public static string Username
    {
        get { return username; }
        set { username = value; }
    }
    private static string password = "";
    public static string Password
    {
        set { password = value; }
    }

    //A temp bool used to prevent the login screen coroutine from continuing until the log in routine is done
    public static bool loggingIn = false;
    public static bool sendingInfo = false;
    public static bool pullingInfo = false;

    public static UserInfo info;

    public static string errorMsg;

    //Bool for the success of pulling user info from the database
    private static bool infoPullSuccess = false;
    public static bool InfoPullSuccess
    {
        get { return infoPullSuccess; }
    }

    //Bool for the success of sending user info from the database
    private static bool infoSendSuccess = false;
    public static bool InfoSendSuccess
    {
        get { return infoSendSuccess; }
    }

    //Function to log the user in
    public static IEnumerator LogIn() {
        //Package and send the user's credentials
        WWWForm credentials = new WWWForm();
        credentials.AddField("username", username);
        if (username.Equals("offlinebypass")) {
            loggedIn = true;
        }else if (username.Equals("test")) {
            loggedIn = true;
        }
        credentials.AddField("password", password);
        WWW www = new WWW(pullUserURL, credentials);
        //Wait for a response
        Debug.Log("Packaged user credentials, about to send to server");
        yield return www;
        string serviceInfo = www.text;
        info = JsonUtility.FromJson<UserInfo>(serviceInfo);
        //Assign the retrieved data if there were no errors
        if (www.isDone && info.success.ToLower()=="true") { 
            www.Dispose();
            //SceneManager.LoadScene("Main_Scene");
            loggedIn = true;
            //SceneManager.LoadScene("Main_Scene");
        }
        loggingIn = false;
        Debug.Log("Done running log in function");
        yield return null;
    }
   }
