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
    private static String username, password;


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
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/register", GetPostDatafromString(parameter)));

        //wenn die Registrierung erfolgreich war wird ein True zurück gegeben anderenfalls 
        //ein false
        if (response["success"].AsBool)
            return new Response(response["success"].AsBool, "");
        else
            return new Response(response["success"].AsBool, response["error"].Value);
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
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/login", GetPostDatafromString(parameter)));
        if (response["success"].AsBool)
        {
            //Wenn sich ein Benutzer anmeldet bekommt er ein sogenanntes session_secret. Das ist
            //ein random string. Der counter wird auf 0 gesetzt und dass password und der username 
            //werden ebenfalls in der Klassse gepseichert.
            secret = response["session_secret"].Value;
            counter = 0;
            username = _username;
            password = _password;
            return new Response(response["success"].AsBool, "");
        }
        else
            return new Response(response["success"].AsBool, response["error"].Value);
    }


    //Die Funktion UpdatePosition aktuallisiert den Standpunkt des Benutzers. Der Funktion wird
    //dafür Die Position in Laengen- und Breitengrad übergeben.
    public Response UpdatePosition(float longitude, float latitude)
    {
        //Die Parameter werden mit dem dazu gehörigen namen erstellt und in einer Liste gespeichert
        List<Parameter> parameter = new List<Parameter>();
        parameter.Add(new Parameter("username", username));
        parameter.Add(new Parameter("longitude", Convert.ToString(longitude)));
        parameter.Add(new Parameter("latitude", Convert.ToString(latitude)));

        //bei bestimmten funktionen muss ein token mit geschickt werden um sicher zu stellen das 
        //nur der angemeldete user bestimmte daten verändern kann
        parameter.Add(new Parameter("token", GenrateToken()));

        //Die Parameter werden mit der Url an die Funktion Communication gesendet und die Antwort
        //des Servers wird dann mit der Hilfe von SimpleJSON in die Variable response geschrieben
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/updatePosition", GetPostDatafromString(parameter)));

        //Wenn die aktualisierung des Standortes erfolgreich war, wird ein true zurück gegeben, anderen falls ein false
        if (response["success"].AsBool)
        {
            return new Response(response["success"].AsBool, "");
        }
        else
        {
            if (response["message"].Value == "Invalid Authentication Token.")
            {
                LoginUser(username, password);
            }
            return new Response(response["success"].AsBool, response["error"].Value);
        }
    }

    //Die Funktion GetPositionMap gibt eine Liste von allen aktiven Benutzern zurück, welche die Positionen und 
    //und Namen der Benutzer enthällt
    public List<Position> GetPositionMap()
    {
        //Die Funktion gibt eine Liste an Personen zurück, die zuerst initialisiert wird
        List<Position> positionen = new List<Position>();
        //Da diese Funktion keine Parameter braucht, wird einfach ein Emptyarry an Bytes übergeben
        byte[] emptyarray = new byte[0];
        //Abrufen der Positionen
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/getPositionsMap", emptyarray));
        /*JSON Bespiel
        {
            "success": true,
            "user_map": {
                "olli": [8.735941, 51.73075]
            }
        }*/

        if (response["success"].AsBool)
        {
            //Jede empfangene Position wird ausgewerted und in die Liste eingefügt
            for (int i = 0; i < response["uder_map"].Count; i++)
            {
                positionen.Add(new Position((response["User_map"][i][0].AsFloat), response["User_map"][i][1].AsFloat, response["User_map"][i].Value));
            }
        }
        //Die Komplette Liste wird zurück gegeben
        return positionen;
    }

    //Dies Funktion gibt für jedes Minigame die top 10 Scores zurück
    public GameScoreListResponse GetTopTenScoresForAllMinigames()
    {
        //Da diese Funktino keine Parameter braucht ,wird einfach ein Emptyarray an Bytes übergeben
        byte[] emptyarray = new byte[0];
        //Abrufen der Scores
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/getTopTenScoresForAllMinigames", emptyarray));
        /*JSON Beispiel
        {
            "game": {
                "Zukunftsmeile": [{
                    "username": "to_delete75cc737c-7173-4620-ab",
                    "date": 1409212652,
                    "points": 984870
                }, {
                    "username": "to_delete96fe2a29-ef23-4890-89",
                    "date": 1409640521,
                    "points": 966482
                }],
                "HNF": [{
                    "username": "to_delete75cc737c-7173-4620-ab",
                    "date": 1409212653,
                    "points": 996279
                }, {
                    "username": "to_delete96fe2a29-ef23-4890-89",
                    "date": 1409640520,
                    "points": 984009
                }]
            },
            "success": true
        }*/

        if (response["success"].AsBool)
        {
            //Zuerst wird ein GameScoreListResponse erstellt. dieser enthält die verschieden Scores
            //wie auch das success-Feld aus dem Response
            GameScoreListResponse gslr = new GameScoreListResponse(response["success"].AsBool);
            for (int i = 0; i < response["game"].Count; i++)
            {
                //Der GameScoreListResponse ennhält eine Liste an Games, dieses Ogjekt enthält eine Liste an Scores.
                Game newgame = new Game(response["game"][i].Value);
                for (int x = 0; x < response["game"][i].Count; x++)
                {
                    //Die Liste an Scores wird mit dem Username, den Punkten die erzieltwurden und der zeit an dem der Score gamcht wurde
                    newgame.AddScore(response["game"][i][x]["username"].Value, (int)response["game"][i][x]["points"].AsFloat, (int)response["game"][i][x]["date"].AsFloat);
                }
                //Wenn ein Spiel alle Scores hat wird es in die Gameliste im GameScoreListResponse angefügt
                gslr.AddGame(newgame);
            }
            //wenn alle Spiele angefügt sind wird der GameScoreListResponse zurück gegeben
            return gslr;
        }
        else
        {
            GameScoreListResponse gslr = new GameScoreListResponse(response["success"].AsBool);
            return gslr;
        }
    }

    //Mit dieser Funktion wird ein Gästebuch Eintrag gemacht
    //Parameter ist die Nachricht die in das Gästebuch geschrieben werdenn soll
    public Response MakeGuestbookEntry(string message)
    {
        //Zuerst wird eine Liste an Parametern erstellt
        List<Parameter> parameter = new List<Parameter>();
        //Die Parameter werden angefügt
        parameter.Add(new Parameter("message", message));
        //Abrufen des Response und anschließende Rückgabe
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/makeGuestbookEntry", GetPostDatafromString(parameter)));
        if (response["success"].AsBool)
        {
            return new Response(response["success"].AsBool, "");
        }
        else
        {
            if (response["message"].Value == "Invalid Authentication Token.")
            {
                LoginUser(username, password);
            }
            return new Response(response["success"].AsBool, response["error"].Value);
        }
    }

    //Mit der Funktion SubmitGameScore wird ein Gamescore für ein Bstimmtes Spiel eingetragen
    //Parameter:
    //          -score : Die erreichten punkte
    //          -gameID: Die gameID zu identifizierung des Spiels
    public Response SubmitGameScore(int score, string gameID)
    {
        //Da für die Parameter einen String brauchen und es sich bei score um einen int haldelt wird
        //der wert erst in einen String geschrieben
        string strgameid = Convert.ToString(gameID);
        //Danach wird eine Liste an Parametern angelegt
        List<Parameter> parameter = new List<Parameter>();
        parameter.Add(new Parameter("username", username));
        parameter.Add(new Parameter("points", score.ToString()));
        parameter.Add(new Parameter("cache", strgameid));
        //Neben dem Username,dem Score und der GameID wird auch ein Token generiert da sonst jeder beliebig scores erstellen könnte
        parameter.Add(new Parameter("token", GenrateToken()));
        //Abrufen des Response und anschließende Rückgabe
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/submitGameScore", GetPostDatafromString(parameter)));
        if (response["success"].AsBool)
        {
            return new Response(response["success"].AsBool, "");
        }
        else
        {
            if (response["error"].Value == "Invalid Authentication Token.")
            {
                LoginUser(username, password);
            }
            return new Response(response["success"].AsBool, response["error"].Value);
        }
    }

    //Die Funktion markPuzzleSolved wird aufgerufen wenn ein Pozzle gelöst wurde
    public Response markPuzzelSolved()
    {
        //Zuerst wird eine Liste an Parametern erstellt
        List<Parameter> parameter = new List<Parameter>();
        //Danach werden die Parameter angefügt
        parameter.Add(new Parameter("username", username));
        parameter.Add(new Parameter("token", GenrateToken()));
        //Abrufen des Response und anschließende Rückgabe
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/markPuzzleSolved", GetPostDatafromString(parameter)));
        if (response["success"].AsBool)
        {
            return new Response(response["success"].AsBool, "");
        }
        else
        {
            if (response["error"].Value == "Invalid Authentication Token.")
            {
                LoginUser(username, password);
            }
            return new Response(response["success"].AsBool, response["error"].Value);
        }
    }
    //Die Funktion getAllLogBookEntrys Holt sich alle Logbucheinträge des Users
    public List<Logbookentry> getAllLogBookEntrys()
    {
        //Zuerst wird eine Liste an Logbucheinträgen erstellt
        List<Logbookentry> logbookentries = new List<Logbookentry>();
        //Es wird eine Liste an Parametern erstellt
        List<Parameter> parameter = new List<Parameter>();
        //Danach wird der Username als Parameter angefügt
        parameter.Add(new Parameter("username", username));

        //Abrufen der Logbucheinträge
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/getAllLogbookEntriesByUser", GetPostDatafromString(parameter)));
        //Die Logbucheintrage werdn dann aus den JSON gelesen und in die Liste eingfügt
        if (response["success"].AsBool && response["entries"].Count > 0)
        {
            for (int i = 0; i < response["entries"].Count; i++)
            {
                logbookentries.Add(new Logbookentry(response["entries"][i]["cache"].Value, response["entries"][i]["message"].Value, response["entries"][i]["puzzle_solved"].AsBool, response["entries"][i]["found_date"].AsInt));
            }
        }
        //Anschließend werden die Logbucheinträge zurück gegeben
        return logbookentries;
    }

    //Die Funktion checkCacheSecret überprüft die Richtigkeit des Eingegebenen Cache Secrets
    //Parameter ist der eingegebene Chache
    public Response checkCacheSecret(string userCachInput)
    {
        //Zuerst wird eine List an Parametern erstellt
        List<Parameter> parameter = new List<Parameter>();
        parameter.Add(new Parameter("cache_secret", userCachInput));
        parameter.Add(new Parameter("username", username));

        //Abruf des Response und anschließende Rückgabe
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/secretValidForNextCache", GetPostDatafromString(parameter)));
        if (response["success"].AsBool)
        {
            return new Response(response["success"].AsBool, "");
        }
        else
        {
            return new Response(response["success"].AsBool, response["error"].Value);
        }
    }

    //Die Funktion MakteLogBookEntry funktioniert ähnlich wie der Guestbookentry nur das er einen Logbuch eintrag macht
    //Parameter :
    //          -message: die eingegebene Nachricht
    //          -secret : das Secret zu dem der Logbuch eintrag gemacht werden soll
    public Response MakeLogBookEntry(string message, string secret)
    {
        //Zuerst wird eine Liste an Parametern erstellt
        List<Parameter> parameter = new List<Parameter>();
        //Anschließend werden alle Parameter angefügt
        parameter.Add(new Parameter("message_str", message));
        parameter.Add(new Parameter("token", GenrateToken()));
        parameter.Add(new Parameter("username", username));
        parameter.Add(new Parameter("secret", secret));
        //Abruf des Response und anschließende Rückgabe
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/makeLogbookEntry", GetPostDatafromString(parameter)));
        if (response["success"].AsBool)
        {
            return new Response(true, "");
        }
        else
        {
            if (response["error"].Value == "Invalid Authentication Token.")
            {
                LoginUser(username, password);
            }
            return new Response(false, response["error"].Value);
        }
    }

    //Die Funktion GetTopScorebyUser gibt dehn Besten Score für ein bestimmtes Spiel zurück
    public Score getTopScoreByUser(string gameID)
    {
        //Zuerst wird eine Liste an Parametern erstellt
        List<Parameter> parameter = new List<Parameter>();
        //Anschließend werden die Parameter angefügt
        parameter.Add(new Parameter("username", username));
        //Abruf des Response und anschließende Rückgabe
        var response = JSON.Parse(Communication("http://btcwash.de:8080/api/getTopTenScoresForAllMinigames", GetPostDatafromString(parameter)));
        if (response["success"].AsBool && response["game"][gameID].Count > 0)
        {
            return new Score(username, response["game"][gameID][0]["points"].AsInt, response["game"][0][gameID]["date"].AsInt);
        }
        //wenn der Abruf fehlerhaft war wird ein leerer Score zurück gegeben
        return new Score(username, 0, 0);
    }

    //Für Niclas ;P
    public Response TestServerConnection()
    {
        byte[] emptyarray = new byte[0];
        var response = Communication("http://btcwash.de:8080", emptyarray);
        if (response == "You found the index. There is nothing here. Go, use the API.")
        {
            return new Response(true, "All fine");
        }
        else
        {
            return new Response(false, "meh no connectionz");
        }

    }

    //public int[] GetGuestbookIndex()
    //{
    //    List<int> indexlist = new List<int>();
    //    byte[] emptyarray = new byte[0];
    //    var response = JSON.Parse(Communication("http://btcwash.de:8080/api/makeGuestbookEntry", emptyarray));
    //    if (response["success"].AsBool)
    //    {
    //        foreach (int index in response["index"])
    //        {
    //            indexlist.Add(index);
    //        }
    //    }
    //    return indexlist.ToArry();
    //}

    //public List<GuestbookEntry> GetlastGuestbookentries()
    //{
    //List<GuestbookEntry> name = new List<GuestbookEntry>();
    //byte[] emptyarray = new byte[0];
    //var response = JSON.Parse(Communication("http://btcwash.de:8080/api/", emptyarray));
    //if (response["success"].AsBool)
    //{
    //}

    //return null;
    //}

    //private String[] GetUsers()
    //{
    //    List<string> alluser = new List<string>();
    //    byte[] emptyarray = new byte[0];
    //    var response = JSON.Parse(Communication("http://btcwash.de:8080/api/getUsers", emptyarray));

    //    if (response["success"].AsBool)
    //    {
    //        foreach (var person in response["users"])
    //        {
    //            alluser.Add(person);
    //        }
    //    }
    //    return alluser.ToArry();
    //}


    //--------------------------------------------------------------------Help Funtions-----------------------------------------------------------------------------------------------------------




    //Die Funktion Communication stellt die Verbindung zum Server her, sie übermittelt dabei
    //die gewünschten Parameter und gibt die Antwort des Servers dann als String zurück
    public string Communication(String url, byte[] postdata)
    {
        //Zu beginn wird eine POST-Request erstellt und die Typ des für die Parameter bestimmt
        WebRequest request = WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        //Anschließend wird ein Datenstream geöffnet dehn wir über den Request öffnen
        Stream datastream = request.GetRequestStream();
        //Wenn wir den Datastream geöffnet haben schreiben wir die Parameter hinein und schließen ihn wieder
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
    public byte[] GetPostDatafromString(List<Parameter> parameter)
    {
        //Zuerst werden die Parameter aus der Liste Passen in einen String gesetzt
        String postdata = "";
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
        string token = "";
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

    //Eigenschaftem
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    public string Content
    {
        get
        {
            return content;
        }
        set
        {
            content = value;
        }
    }
    //Konstruktor
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

    //Eigenschaften
    public float Latitude
    {
        get
        {
            return latitude;
        }
    }

    public float Longitude
    {
        get
        {
            return longitude;
        }
    }

    public string Userid
    {
        get
        {
            return userid;
        }
    }
    //Konstruktor
    public Position(float longitude, float latitude, string userid)
    {
        this.latitude = latitude;
        this.longitude = longitude;
        this.userid = userid;
    }
}
//Die Klasse Response wir bei Jeder funktion zurück gegeben bei der kein Spezeller Datentyp benutzt wird
public class Response
{
    bool success;
    string message;
    //Eigenschaften
    public bool Success
    {
        get
        {
            return success;
        }
    }

    public string Message
    {
        get
        {
            return message;
        }
    }
    //Konstruktor
    public Response(bool _success, string _message)
    {
        this.success = _success;
        this.message = _message;
    }
}
//Bei GameScoreListResponse handelt es sich um einen Speziellen Resopnse der von der Funktion GetTopTenScoresForAllMinigames
//benutzt wird
public class GameScoreListResponse
{
    bool success;
    List<Game> gameslist;

    //Eigenschaften
    public bool Success
    {
        get
        {
            return success;
        }
    }

    public List<Game> GamesList
    {
        get
        {
            return gameslist;
        }
    }
    //Konstruktor
    public GameScoreListResponse(bool _success)
    {
        this.success = _success;
        this.gameslist = new List<Game>();
    }
    //Die Funktion AddGame fügt ein neues Spiel in die Liste ein
    public void AddGame(Game _game)
    {
        gameslist.Add(_game);
    }
}
//Die Klasse Game wird von der Klasse GameScoreListbenutzt
public class Game
{
    string name;
    List<Score> toptenScore;
    //Eigenschaften
    public string Name
    {
        get
        {
            return name;
        }
    }

    public List<Score> ToptenScore
    {
        get
        {
            return toptenScore;
        }
    }
    //Konstruktor
    public Game(string _name)
    {
        this.name = _name;
        toptenScore = new List<Score>();
    }
    //Die Funktion AddScore fügt einen neuen Score zum Spiel hinzu
    public void AddScore(string _user, int _points, int _time)
    {
        toptenScore.Add(new Score(_user, _points, _time));
    }
}
//Die Klasse Score wird von der Klasse Game und der Funktion getTopScoreByUser benutzt
public class Score
{
    string user;
    int points;
    int time;
    //Eigenschaften
    public string User
    {
        get
        {
            return user;
        }
    }
    public int Points
    {
        get
        {
            return points;
        }
    }
    public int Time
    {
        get
        {
            return time;
        }
    }
    //Konstruktor
    public Score(string _user, int _points, int _time)
    {
        this.user = _user;
        this.points = _points;
        this.time = _time;
    }
}
//Die Klasse Guestbookentry ist eine Fachklasse
public class GuestbookEntry
{
    int id;
    string author;
    string message;
    int date;
    //Eigenschaften
    public int Id
    {
        get
        {
            return id;
        }
    }
    public string Author
    {
        get
        {
            return author;
        }
    }
    public string Message
    {
        get
        {
            return message;
        }
    }
    public int Date
    {
        get
        {
            return date;
        }
    }
    //konstruktor
    public GuestbookEntry(int _id, string _author, string _message, int _date)
    {
        this.id = _id;
        this.author = _author;
        this.message = _message;
        this.date = _date;
    }
}
//Die Klasse Logbookentry ist eine Fachklasse
public class Logbookentry
{
    string cache;
    string message;
    bool puzzlesolved;
    int founddate;
    //Eigenschaften
    public string Cache
    {
        get
        {
            return cache;
        }
    }
    public string Message
    {
        get
        {
            return message;
        }
    }
    public bool Puzzlesolved
    {
        get
        {
            return puzzlesolved;
        }
    }
    //Konstruktor
    public Logbookentry(string _cache, string _message, bool _puzzlesolved, int _date)
    {
        this.cache = _cache;
        this.message = _message;
        this.puzzlesolved = _puzzlesolved;
        this.founddate = _date;
    }
}