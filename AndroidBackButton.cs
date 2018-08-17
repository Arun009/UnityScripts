using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackButton : MonoBehaviour {

    // 200x300 px window will apear in the center of the screen.
    private Rect windowRect = new Rect((Screen.width - 800) / 2, (Screen.height - 300) / 2, 800, 300);
    // Only show it if needed.
    private bool show = false;
    private GUIStyle myLabel;
    private GUIStyle myButton;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                Open();
            }
        }
	}

    void OnGUI()
    {
        if (show)
        {
            myLabel = new GUIStyle(GUI.skin.label);
            myButton = new GUIStyle(GUI.skin.button);
            myLabel.fontSize = 40;
            myLabel.fontStyle = FontStyle.Bold;
            myButton.fontSize = 35;
            myButton.fontStyle = FontStyle.Normal;

            windowRect = GUI.Window(0, windowRect, DialogWindow, "");
            GUI.backgroundColor = Color.gray;
        }
    }

    // This is the actual window.
    void DialogWindow(int windowID)
    {
        GUI.Label(new Rect(100, 50, windowRect.width, 100), "Are you sure you want to exit?", myLabel);

        if (GUI.Button(new Rect(100, 150, 200, 100), "Yes", myButton))
        {
            Application.Quit();
            show = false;
            return;
        }

        if (GUI.Button(new Rect(500, 150, 200, 100), "No", myButton))
        {
            show = false;
        }
    }

    // To open the dialogue from outside of the script.
    public void Open()
    {
        show = true;
    }
}
