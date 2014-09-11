using UnityEngine;
using System.Collections;

/// <summary>
/// Erstellt am 27.08.2014 von Oliver Noll
/// Zuletzt bearbeitet am 05.09.2014 von Oliver Noll
/// </summary>

public class CacheScript : MonoBehaviour
{
    // Beschreibung des Caches (Name)
    private string _description;

    // Längengrad
    private float _longitude;
    // get/set Längengrad
    public float Longitude
    {
        get { return _longitude; }
        set { _longitude = value; }
    }

    // Breitengrad
    private float _latitude;
    // get/set Breitengrad
    public float Latitude
    {
        get { return _latitude; }
        set { _latitude = value; }
    }

    // bool ob gefunden
    private bool _founded;
    // get/set founded
    public bool Founded
    {
        get { return _founded; }
        set { _founded = value; }
    }

    // Neuen Cache initialisieren
    public CacheScript(string description, float longitude, float latitude)
    {
        // Beschriftung setzen
        _description = description;
        // Längengrad setzen
        _longitude = longitude;
        // Breitengrad setzen
        _latitude = latitude;
        // ob gefunden setzen (Standard: falsch)
        _founded = false;
    }
}