using UnityEngine;
using System.Collections;

public class CacheScript : MonoBehaviour
{
    private string _description;
    private float _longitude;
    private float _latitude;
    private bool _founded;
    public bool Founded
    {
        get { return _founded; }
        set { _founded = value; }
    }
    private Rect _transform;
    public Rect Transform
    {
        get { return _transform; }
        set { _transform = value; }
    }

    public CacheScript(string desciption, float longitude, float latitude, Rect transform)
    {
        _description = desciption;
        _longitude = longitude;
        _latitude = latitude;
        _transform = transform;
        _founded = false;
    }

    public override string ToString()
    {
        return _description;
    }
}