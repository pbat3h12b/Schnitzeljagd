using UnityEngine;
using System.Collections;

public class CacheScript : MonoBehaviour
{
    private string _description;

    private float _longitude;
    public float Longitude
    {
        get { return _longitude; }
        set { _longitude = value; }
    }

    private float _latitude;
    public float Latitude
    {
        get { return _latitude; }
        set { _latitude = value; }
    }

    private bool _founded;
    public bool Founded
    {
        get { return _founded; }
        set { _founded = value; }
    }

    public CacheScript(string description, float longitude, float latitude)
    {
        _description = description;
        _longitude = longitude;
        _latitude = latitude;
        _founded = false;
    }

    public override string ToString()
    {
        return _description;
    }
}