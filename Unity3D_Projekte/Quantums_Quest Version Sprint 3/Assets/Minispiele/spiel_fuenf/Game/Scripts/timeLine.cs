using UnityEngine;
using System.Collections;

public class timeLine : MonoBehaviour
{
    #region fields
    //Beschreibt den Zeitpunkt in Sekunden an dem das Spiel beginnt
    public float startTime;

    //Beschreibt den aktuellen Wert der zeit die noch übrig ist
    [HideInInspector]
    public float timeLeft;

    //Beschreibt die Strafe in Sekunden
    public float timePunishment;

    //Beschreibt die Länge des Spiels in Sekunden
    public float runTime;

    //Beschreibt 1 Prozent der Länge des Spiels
    private float percent;

    //GameObject der Textur für die Anzeige der Zeit
    private GameObject timeLineSprite;

    //Beschreibt die Anfangslänge der Textur für die Anzeige der Zeit
    private float timeLineSpriteScaleX;

    //GUISkin für die Anzeige der verbleibenden Sekunden
    public GUISkin skin;

    //Farbe zur Veränderung der Anzeige der verbleibenden Sekunden
    Color timeLeftColor; 
    #endregion



    //Initialization
    void Start()
    {
        startTime = Time.time;
        percent = runTime / 100;
        timeLineSprite = GameObject.Find("TimeLine");
        timeLineSpriteScaleX = timeLineSprite.transform.localScale.x;
        changeGUISkinColor(Color.white, skin);
    }

    // Update is called once per frame
    void Update()
    {
        timer();
        timeLeft = runTime - (Time.time - startTime);

        if (timeLeft < 15)
        {
            timeLeftColor = changeColor(255, 84, 0);
            changeGUISkinColor(timeLeftColor, skin);
        }
        if (timeLeft < 10)
        {
            timeLeftColor = changeColor(255, 0, 0);
            changeGUISkinColor(timeLeftColor, skin);
        }
    }

    //Zeitleiste skalieren
    void timer()
    {
        if (elapsedTimePercent() >= 100.0f)
        {
            Time.timeScale = 0.0f;
            //GameObject.Find("TimeLine").transform.localScale = new Vector3(0, 1, 1);
            transform.localScale = new Vector3(0, 1, 1);
        }

        else
        {
            
            transform.localScale = new Vector3(timeLineSpriteScaleX - ((elapsedTimePercent() * timeLineSpriteScaleX) / 100), 1, 1);
        }

    }

    //Gibt den Prozentwert der Zeit, die schon vergangen ist, an
    public float elapsedTimePercent()
    {
        float elapsedTimePercent = (Time.time - startTime) / percent;
        return elapsedTimePercent;
    }

    //Zeitstrafe
    public void punishment()
    {
        startTime -= percent * timePunishment;
    }

    //Verwaltung der GUI
    void OnGUI()
    {
        GUI.skin = skin;
        float timeleft = timeLeft;

        string timeLeftTxt = Mathf.RoundToInt(timeleft).ToString();
        Rect timeLabel = new Rect(0,0,200,35);
        timeLabel.x = (Screen.width / 2) - timeLabel.width / 2;
        
        GUI.Label(timeLabel, timeLeftTxt);
    }

    //Änderung eines GUISkins
    private void changeGUISkinColor(Color color, GUISkin guiSkin)
    {
        guiSkin.label.normal.textColor = color;
        guiSkin.label.hover.textColor = color;
        guiSkin.label.active.textColor = color;
        guiSkin.label.focused.textColor = color;
        guiSkin.label.onNormal.textColor = color;
        guiSkin.label.onHover.textColor = color;
        guiSkin.label.onActive.textColor = color;
        guiSkin.label.onFocused.textColor = color;
    }


    private Color changeColor(float red, float green, float blue)
    {
        Color skinColor = new Color(red,green,blue);
        return skinColor;
    }

}
