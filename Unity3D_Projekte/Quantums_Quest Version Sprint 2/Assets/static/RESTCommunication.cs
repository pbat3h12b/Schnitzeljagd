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

        //Erstellt von Benedikt Ahle

        //In secret wird das sogenannte session secret gespeichert. Mit ihm wird das Token für bestimmte 
        //Funktionen generiert
        private static String secret;
        
        //Der counter wird ebenfalls für das Erstellen des Tokens benötigt.
        private static int counter;

        //Den Usernamen und dass Password werden statisch in der Klasse gespeichert
        private static String username,password;


        //Die Funktion RegisterNewUser ermöglicht es dem Benutzer einen neuen Benutzer für
        //die Datenbank hinzu zu fügen.
        //Parameter:    die Parameter username und password werden dan den Server gesendet
        public Response RegisterNewUser(String username, String password)
        {
            //Die Parameter werden mit dem dazu gehörigen namen erstellt und in einer Liste gespeichert
            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", username));
            parameter.Add(new Parameter("password", password));

            //Die Parameter werden mit der url an die Funktion Communication gesendet und die Antwort
            //des Servers wird dann mit der Hilfe von SimpleJSON in die Variable response geschrieben
            var response = JSON.Parse(Communication("http://81.169.244.213:8080/api/register", GetPostDatafromString(parameter)));

            //wenn die Registrierung erfolgreich war wird ein True zurück gegeben anderenfalls 
            //ein false
            if (response["success"].AsBool)
                return new Response(response["success"].AsBool,"");
            else
                return new Response(response["success"].AsBool,response["message"].Value);
        }

        //Die Funktion LoginUser ermöglicht es dem Benutzer sich am System anzumelden
        //Parameter:    die Parameter username und password werden an den Server gesendet
        public Response LoginUser(String _username, String _password)
        {
            //Die Parameter werden mit dem dazu gehörigen namen erstellt und in einer Liste gespeichert
            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", _username));
            parameter.Add(new Parameter("password", _password));

            //Die Parameter werden mit der Url an die Funktion Communication gesendet und die Antwort
            //des Servers wird dann mit der Hilfe von SimpleJSON in die Variable response geschrieben
            var response = JSON.Parse(Communication("http://81.169.244.213:8080/api/login", GetPostDatafromString(parameter)));
            if (response["success"].AsBool)
            {
                //Wenn sich ein Benutzer anmeldet bekommt er ein sogenanntes session_secret. Das ist
                //ein random string. Der counter wird auf 0 gesetzt und dass password und der username 
                //werden ebenfalls in der Klassse gepseichert.
                secret = response["session_secret"].Value;
                counter = 0;
                username = _username;
                password = _password;
                return new Response(response["success"].AsBool,"");
            }
            else
                return new Response(response["success"].AsBool,response["message"].Value);

        }

        //Die Funktion UpdatePosition aktuallisiert den Standpunkt des Benutzers. Der Funktion wird
        //dafür Die Position in Laengen- und Breitengrad übergeben.
        public Response UpdatePosition(float longitude, float latitude) 
        {
            //Die Parameter werden mit dem dazu gehörigen namen erstellt und in einer Liste gespeichert
            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username",username));
            parameter.Add(new Parameter("longitude", Convert.ToString(longitude)));
            parameter.Add(new Parameter("latitude", Convert.ToString(latitude)));
            
            //bei bestimmten funktionen muss ein token mit geschickt werden um sicher zu stellen das 
            //nur der angemeldete user bestimmte daten verändern kann
            parameter.Add(new Parameter("token",GenrateToken()));

            //Die Parameter werden mit der Url an die Funktion Communication gesendet und die Antwort
            //des Servers wird dann mit der Hilfe von SimpleJSON in die Variable response geschrieben
            var response = JSON.Parse(Communication("http://81.169.244.213:8080/api/updatePosition", GetPostDatafromString(parameter)));

            //Wenn die aktualisierung des Standortes erfolgreich war, wird ein true zurück gegeben, anderen falls ein false
            if (response["success"].AsBool)
                return new Response(response["success"].AsBool,"");
            else
                return new Response(response["success"].AsBool,response["message"].Value);
        }

        //Die Funktion GetPositionMap gibt eine Liste von allen aktiven Benutzern zurück, welche die Positionen und 
        //und Namen der Benutzer enthällt
        /*public List<Position> GetPositionMap()
        {
            List<Position>positionen = new List<Position>();
            byte[] emptyarray = new byte[];
            var response = JSON.Parse(Communication("http://btcwash.de:8080/api/getPositionMap"),emptyarray);
            var responseuser = JSON.Parse(Communication("http://btcwash.de:8080/api/alluser"),emptyarray)

            if (response["success"].AsBool)
            {
                foreach(var person in response["user_map"])
                {
                    positionen.add(new Position(person[0],person[1],person.AsString));
                }
            }
            return positionen;
        }*/

        //Die Funktion Communication stellt die Verbindung zum Server her, sie übermittelt dabei
        //die gewünschten Parameter und gibt die Antwort des Servers dann als String zurück
        private string Communication(String url, byte[] postdata)
        {
            //Zu beginn wird eine POST-Request erstellt und die Typ des für die Parameter bestimmt
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //Anschließend wird ein Datenstream geöffnet dehn wir über den Request öffnen
            Stream datastream = request.GetRequestStream();
            //Wenn wir den Datastream geöffnet haben scheiben wird die Parameter hinein und schließen ihn wieder
            datastream.Write(postdata, 0, postdata.Length);
            datastream.Close();

            //Wir bekommen die Antwort des Servers Über den Request Response, dieser wird dann in einen String gelesen
            //der JSON-daten dann über einen String an die Funktion zurück gibt
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
            //Zuerst werden die Parameter aus der Liste Passen in einen String gesetzt
            String postdata ="";
            foreach (Parameter p in parameter) 
            {
                postdata += p.Name + "=" + p.Content + "&";
            }
            postdata.Remove(postdata.Length - 1);
            //anschließend wird der String in einen Byte-Array umgewandelt und zurück gegeben
            byte[] bytes = Encoding.UTF8.GetBytes(postdata);

            return bytes;
        }

        //Diese Funktion generiert das Token das benötigt wird um eine erfolgreiche Kommunikation zu erreichen
        private string GenrateToken() 
        {
            //Zuerst wird das session secret mit dem Counter verbunden.
            string token ="";
            String concat_secret_counter = secret + counter;
            
            //Anschließend wird ein MD5-Hash generiert den wir anschließend wird er wieder zu einem String verbunden welcher dann zurück gegeben wird
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
            
            //Der Counter wird nach jedem generieren eines Tokens um einen zähler hochgezählt da so nie das gleiche Token generiert wird
            counter++;

            return token;
        }
    }

    //Die Klasse Parameter wird dazu benutzt den passenden Parameter einfacher in der 
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

    //Die Klasse Position wird benötigt um die einfache Rückgabe von Positionen von Personen zu realisieren.
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
            get{return longitude;}
        }

        public string Userid
        {
            get{return userid;}
        }

        public Position(float longitude, float latitude,string userid)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.userid = userid;
        }
    }

    public class Response
    {
        bool success;

        public bool Success
        {
            get { return success; }
        }
        string message;

        public string Message
        {
            get { return message; }
        }

        public Response(bool _success,string _message)
        {
            this.success = _success;
            this.message = _message;
        }
    }
