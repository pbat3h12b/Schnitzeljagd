using UnityEngine;
using System.Collections;

/// <summary>
/// Erstellt am 03.09.2014 von Oliver Noll
/// Zuletzt bearbeitet am 05.09.2014 von Oliver Noll
/// </summary>

public class GUI_Scale : MonoBehaviour {

    // Gibt die relative Position zur Auflösung in Pixeln wieder, 
    // bekommt dafür ein Rechteck mit Prozent-Angaben (Position x, y, Breite, Höhe)
    public Rect GetRelativeRect(Rect oldRect)
    {
        // Neues Rechteck (Position x, y, Breite, Höhe)
        Rect newRect = new Rect();

        // Auflösung X
        float screenWidth = Screen.width;
        // Auflösung Y
        float screenHeight = Screen.height;

        // Berechne Breite
        newRect.width = (oldRect.width * screenWidth) / 100;
        // Berechne Höhe
        newRect.height = (oldRect.height * screenHeight) / 100;

        // Berechne Position x
        newRect.x = (oldRect.x * screenWidth) / 100;
        // Berechne Position y
        newRect.y = (oldRect.y * screenHeight) / 100;

        // return Rechteck
        return newRect;
    }
}
