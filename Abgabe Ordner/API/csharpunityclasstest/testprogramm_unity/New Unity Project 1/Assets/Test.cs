using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using SimpleJSON;
using System.IO;


//Author Benedikt Ahle

public class Test : MonoBehaviour {


	// Use this for initialization
	void Start () {
		Test1 ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	String[] Generate_new_User(int testnumber){
		string[] user = {"testuser"+testnumber,"testpassword"};
		return user;
	}

	void Test1()
	{
		RESTCommunication restcontroller = new RESTCommunication();
		List<Testergebnis> testergebnisse = new List<Testergebnis> ();
		string pathnumber = "testnumber";
		string pathprotocol = "testprotocol";
		string gameID = "Zukunftsmeile";
		int score = 12345;
		string secret = "d1741e41";
		string testnumber = File.ReadAllText (pathnumber);
		string[]user = Generate_new_User(Convert.ToInt32(testnumber)+1);


		////Register Tests
		testergebnisse.Add (Test_Register_New_User_success (restcontroller, user [0], user [1]));
		testergebnisse.Add (Test_Register_New_user_username_already_taken (restcontroller, user [0], user [1]));
		testergebnisse.Add (Test_Register_new_user_username_too_long (restcontroller, user [1]));

		//Login Tests
		testergebnisse.Add (Test_Login_success (restcontroller, user [0], user [1]));
		testergebnisse.Add (Test_Login_username_does_not_exist (restcontroller, user [0], user [1]));
		testergebnisse.Add (Test_Login_password_is_wrong (restcontroller, user [0], user [1]));

		//UpdatePosition Tests
		testergebnisse.Add (Test_Update_Position_successfull (restcontroller));

		//Score Tests
		testergebnisse.Add (Test_Submit_Score (restcontroller, score, gameID));
		testergebnisse.Add (Test_Score_for_user (restcontroller, gameID, score));
		testergebnisse.Add (Test_All_top_ten_scores (restcontroller));

		//Guestbook Tests
		//testergebnisse.Add (Test_Make_Guestbook_Entry (restcontroller, "testmessage"));

		//CheckSecret
		//testergebnisse.Add (Test_Check_Cache_secret (restcontroller, secret));

		//Logbook Tests
		//testergebnisse.Add (Test_Make_Logbook_entry (restcontroller, "testmessage", secret));

		//markpuzzle test
		//testergebnisse.Add (Test_mark_puzzle_solved (restcontroller));





		List<String> protocol = new List<string> ();
		protocol.Add ("This is the testprotocol from test nr."+testnumber);
		protocol.Add ("");
		int counter = 0;
		foreach (Testergebnis test in testergebnisse) {
			if(test.Success == false){
				protocol.Add(String.Format("{0,-45}\t{1}",test.Functionname,test.Message));
			}
			else {
				counter++;
				protocol.Add(String.Format("{0,-45}\tsuccessfull",test.Functionname));
			}
		}
		protocol.Add ("");
		protocol.Add (counter+" from "+testergebnisse.Count+" Tests where successfull!");

		File.WriteAllLines (pathprotocol, protocol.ToArray());
		//set test one further
		File.WriteAllText (pathnumber,Convert.ToString( Convert.ToInt32 (testnumber) + 1));
		Debug.Log ("Done");

	}

	//Successfull register
	Testergebnis Test_Register_New_User_success(RESTCommunication restcontroller,string username,string password){
		Testergebnis testergebnis;
		Response response = restcontroller.RegisterNewUser(username,password);
		if (response.Success) {
			testergebnis = new Testergebnis ("Test_RegisterNewUser_success", response.Success, "Test ran successfully");
		}
		else {
			testergebnis = new Testergebnis ("Test_RegisterNewUser_success", response.Success, response.Message );
		}
		return testergebnis;
	}
	//Username already Exists
	Testergebnis Test_Register_New_user_username_already_taken(RESTCommunication restcontroller,string username,string password){
		Testergebnis testergebnis;
		Response response = restcontroller.RegisterNewUser(username,password);
		if (response.Success) {
			testergebnis = new Testergebnis ("Test_RegisterNewUser_username_already_taken", false, "Username should have been taken");
		}
		else {
			if(response.Message == "Username already taken.")
			{
				testergebnis = new Testergebnis ("Test_RegisterNewUser_username_already_taken", true,"" );
			}
			else{
				testergebnis = new Testergebnis ("Test_RegisterNewUser_username_already_taken", false,response.Message);
			}
		}
		return testergebnis;
	}
	//USername to Long
	Testergebnis Test_Register_new_user_username_too_long(RESTCommunication restcontroller,string password){
		Testergebnis testergebnis;
		Response response = restcontroller.RegisterNewUser ("123456789101112131415161718192021222324", password);
		if (response.Success) {
			testergebnis = new Testergebnis("Test_RegisterNewUser_username_too_long",false,"Username shoud have been to Long,");
		}
		else{
			if(response.Message =="Username too long."){
				testergebnis = new Testergebnis("Test_RegisterNewUser_username_too_long",true,"");
			}
			else{
				testergebnis = new Testergebnis("Test_RegisterNewUser_username_too_long",false,response.Message);
			}
		}
		return testergebnis;
	}

	//Successfull Login
	Testergebnis Test_Login_success (RESTCommunication restcontroller, string username, string password){
		Testergebnis testergebnis;
		Response response = restcontroller.LoginUser (username, password);
		if (response.Success) {
			testergebnis= new Testergebnis("Test_Login_success",true,"");		
		}
		else{
			testergebnis = new Testergebnis("Test_Login_success",false,response.Message);
		}
		return testergebnis;
	}

	//Username does not exist
	Testergebnis Test_Login_username_does_not_exist(RESTCommunication restcontroller,string username,string password){
		Testergebnis testergebnis;
		Response response = restcontroller.LoginUser (username + "u", password);
		if (response.Success) {
			testergebnis = new Testergebnis("Test_Login_username_does_not_exist",false,"Username should not exist");		
		}
		else{
			if(response.Message =="Username doesn't exist."){
				testergebnis = new Testergebnis("Test_Login_username_does_not_exist",true,"");
			}
			else{
				testergebnis = new Testergebnis("Test_Login_username_does_not_exist",false,response.Message);
			}
		}
		return testergebnis;
	}
	//Password is wrong
	Testergebnis Test_Login_password_is_wrong (RESTCommunication restcontroller, string username, string password) {
		Testergebnis testergebnis;
		Response response = restcontroller.LoginUser (username, password + "u");
		if (response.Success){
			testergebnis = new Testergebnis("Test_Login_password_is_wrong",false,"password shoud have been wrong");
		}
		else{
			if(response.Message == "Wrong Password."){
				testergebnis = new Testergebnis("Test_Login_password_is_wrong",true,"");
			}
			else{
				testergebnis = new Testergebnis("Test_Login_password_is_wrong",false,response.Message);
			}
		}
		return testergebnis;
	}
	//Update Position successfull
	Testergebnis Test_Update_Position_successfull(RESTCommunication restcontroller){
		Testergebnis testergebnis;
		float longitude = 5.456456F;
		float latitude =  6.645645F;
		Response response = restcontroller.UpdatePosition (latitude, longitude);
		if (response.Success) {
			testergebnis = new Testergebnis("Test_Update_Position_successfull",true,"");		
		}
		else{
			testergebnis = new Testergebnis("Test_Update_Position_successfull",false,response.Message);
		}
		return testergebnis;
	}
	//Top 10 score for every mini game
	Testergebnis Test_Top10_Minigame_Scores(RESTCommunication restcontroller){
		Testergebnis testergebnis;
		GameScoreListResponse gslr = restcontroller.GetTopTenScoresForAllMinigames ();
		if (gslr.Success) {
			testergebnis = new Testergebnis("Test_Top10_Minigame_Scores",true,"");
		}
		else{
			testergebnis = new Testergebnis("Test_Top10_Minigame_Scores",false,"Proglem with the Server");
		}
		return testergebnis;
	}
	//Submit Score
	Testergebnis Test_Submit_Score(RESTCommunication restcontroller,int score, string gameID){
		Testergebnis testergebnis;
		Response response = restcontroller.SubmitGameScore (score, gameID);
		if (response.Success) {
			testergebnis = new Testergebnis("Test_Submit_Score",true,"");
		}
		else{
			testergebnis = new Testergebnis("Test_Submit_Score",false,response.Message);
		}
		return testergebnis;
	}
	//Score for user
	Testergebnis Test_Score_for_user(RESTCommunication restcontroller,string gameid,int _score){
		Testergebnis testergebnis;
		Score score = restcontroller.getTopScoreByUser(gameid);
		if (score.Points == _score) {
			testergebnis = new Testergebnis("Test_Score_for_User",true,"");
		}
		else{
			testergebnis = new Testergebnis("Test_Score_for_User",false,"Score is different");
		}
		return testergebnis;
	}
	//All top10 scores
	Testergebnis Test_All_top_ten_scores(RESTCommunication restcontroller){
		Testergebnis testergebnis;
		GameScoreListResponse gslr = restcontroller.GetTopTenScoresForAllMinigames ();
		if (gslr.Success) {
			testergebnis = new Testergebnis("Test_All_top_ten_scores",true,"");
		}
		else{
			testergebnis = new Testergebnis("Test_All_top_ten_scores",false,"Problem with the Server");
		}
		return testergebnis;
	}
	//Make Guestbook Entry
	Testergebnis Test_Make_Guestbook_Entry(RESTCommunication restcontroller,string _message){
		Testergebnis testergenis;
		Response response = restcontroller.MakeGuestbookEntry (_message);
		if (response.Success) {
			testergenis = new Testergebnis ("Test_Make_Guestbook_Entry", true, "");
		}
		else {
			testergenis = new Testergebnis("Test_Make_Guestbook_Entry",false,"Problems makeing a Logbook entry");		
		}
		return testergenis;
	}
	//Make Logbookentry
	Testergebnis Test_Make_Logbook_entry(RESTCommunication restcontroller,string _message,string cache){
		Testergebnis testergebnis;
		Response response = restcontroller.MakeLogBookEntry (_message, cache);
		if (response.Success) {
			testergebnis = new Testergebnis("Test_Make_Logbook_entry",true,"");		
		}
		else{
			testergebnis = new Testergebnis("Test_Make_Logbook_entry",false,response.Message);
		}
		return testergebnis;
	}
	//checkCacheSecret
	Testergebnis Test_Check_Cache_secret(RESTCommunication restcontroller,string secret){
		Testergebnis testergebnis;
		Response response = restcontroller.checkCacheSecret (secret);
		if (response.Success) {
			testergebnis = new Testergebnis("Test_Check_Cache_secret",true,"");
		}
		else{
			testergebnis = new Testergebnis("Test_Check_Cache_secret",false,response.Message);
		}
		return testergebnis;
	}
	//markPuzzelSolved
	Testergebnis Test_mark_puzzle_solved(RESTCommunication restcontroller){
		Testergebnis testergebnis;
		Response response = restcontroller.markPuzzelSolved ();
		if (response.Success) {
			testergebnis = new Testergebnis("Test_mark_puzzle_solved",true,"");
		}
		else{
			testergebnis = new Testergebnis("Test_mark_puzzle_solved",false,response.Message);
		}
		return testergebnis;
	}

}





public class Testergebnis
{
	string functionname;
	bool success;
	string message;

	public string Functionname
	{
		get{return functionname;}
	}
	public bool Success
	{
		get{return success;}
	}
	public string Message
	{
		get{ return message;}
	}

	public Testergebnis(string functionname,bool success, string message) 
	{
		this.functionname = functionname;
		this.success = success;
		this.message = message;
	}
}
