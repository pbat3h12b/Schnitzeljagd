using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using SimpleJSON;

	class RESTCommunication : MonoBehaviour
    {

        //In secret wird das sogenannte session secret gespeichert. Mit ihm wird das Token für bestimmte 
        //Funktionen generiert
        private static String secret;
        
        //Der counter wird ebenfalls für das Erstellen des Tokens benötigt.
        private static int counter;

        //Den Usernamen und dass Password werden statisch in der Klasse gespeichert
        private static String username,password;


        //Die Funktion RegisterNewUser ermöglicht es dem Benutzer einen neuen Benutzer für
        //die Datenbank hinzu zu fügen.
        public bool RegisterNewUser(String username, String password)
        {
            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", username));
            parameter.Add(new Parameter("password", password));

            var response = JSON.Parse(Communication("http://81.169.244.213:8080/api/register", GetPostDatafromString(parameter)));

            if (response["success"].AsBool)
                return true;
            else
                return false;
        }

        //Die Funktion LoginUser laesst den Benutzer im System anmelden
        public bool LoginUser(String _username, String _password)
        {
            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", _username));
            parameter.Add(new Parameter("password", _password));
            var response = JSON.Parse(Communication("http://81.169.244.213:8080/api/login", GetPostDatafromString(parameter)));
            if (response["success"].AsBool)
            {
                secret = response["session_secret"].Value;
                counter = 0;
                username = _username;
                password = _password;
                return true;
            }
            return false;

        }

        //Die Funktion UpdatePosition aktuallisiert den Standpunkt des Benutzers. Der Funktion wird
        //dafür Die Position in Laengen- und Breitengrad übergeben.
        public bool UpdatePosition(float longitude, float latitude) 
        {

            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username",username));
            parameter.Add(new Parameter("token",GenrateToken()));
            parameter.Add(new Parameter("longitude", Convert.ToString(longitude)));
            parameter.Add(new Parameter("latitude", Convert.ToString(latitude)));

            var response = JSON.Parse(Communication("http://81.169.244.213:8080/api/updatePosition", GetPostDatafromString(parameter)));

            if (response["success"].AsBool)
                return true;
            else
                return false;
        }

        //Die Funktion Communication stellt die Verbindung zum Server her, sie übermittelt dabei
        //die gewünschten Parameter und gibt die Antwort des Servers dann als String zurück
        private string Communication(String url, byte[] postdata)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            Stream datastream = request.GetRequestStream();
            datastream.Write(postdata, 0, postdata.Length);
            datastream.Close();

            WebResponse response = request.GetResponse();
            datastream = response.GetResponseStream();
            StreamReader streamreader = new StreamReader(datastream);
            String responsestring = streamreader.ReadToEnd();
                      

            return responsestring;
        }
        
        //Diese Funktion wird erstellt aus den Parametern einen Byte-Array der für die 
        //Übermittlung an den Server benötigt wird
        private byte[] GetPostDatafromString(List<Parameter>parameter) 
        {
            String postdata ="";
            foreach (Parameter p in parameter) 
            {
                postdata += p.Name + "=" + p.Content + "&";
            }
            postdata.Remove(postdata.Length - 1);
            byte[] bytes = Encoding.UTF8.GetBytes(postdata);

            return bytes;
        }

        //Diese Funktion generiert das Token das benötigt wird um eine erfolgreiche Kommunikation zu erreichen
        private string GenrateToken() 
        {
            string token ="";
            String concat_secret_counter = secret + counter;
            
            MD5 md5 = MD5.Create();
            byte[] bytehash = md5.ComputeHash(Encoding.UTF8.GetBytes(concat_secret_counter));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytehash.Length; i++) 
            {
                sb.Append(bytehash[i].ToString("x2"));
            }

            token = sb.ToString();
			Debug.Log (token);
			Debug.Log (counter);
                        
            counter++;

            return token;
        }
    }

    //Die klasse Parameter wird dazu benutzt den passenden Parameter einfacher in der 
    //Klasse zu benutzen.
    public class Parameter
    {
        //content enthaelt die zu übertragenden Daten
        string content;
        //name bekommt den Namen des Parameters da dieser an der Server-Seite identisch sein muss
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        public Parameter(String name, String content)
        {
            this.name = name;
            this.content = content;
        }
    }

    public class Position
    {
        float latitude;
        float longitude;
        string userid;

        public float Latitude        
        {
            get{return latitude;}
        }

        public float Longitude
        {
            get{return longitude}
        }

        public string userid
        {
            get{return userid;}
        }

        public Position(float latitude, float longitude,string userid)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.userid = userid;
        }

    }

