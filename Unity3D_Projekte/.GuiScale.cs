using UnityEngine;
using System.Collections;

public class Gui_Control : MonoBehaviour
{
    private enum GameStates
    {
        Login,
        Register,
        Game,
    }
    private GameStates gameState = GameStates.Login;

    void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    void Update()
    {

    }

    void OnGUI()
    {
        switch (gameState)
        {
            case (GameStates.Login):
                GUI.Box(GetRelativeRect(new Rect(25, 25, 50, 50)), "Test");
                break;
            case (GameStates.Register):
                break;
        }
    }

    Rect GetRelativeRect(Rect oldRect)
    {
        Rect newRect = new Rect();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        newRect.width = (oldRect.width * screenWidth) / 100;
        newRect.height = (oldRect.height * screenHeight) / 100;

        newRect.x = (oldRect.x * screenWidth) / 100;
        newRect.y = (oldRect.y * screenHeight) / 100;

        return newRect;
    }
}