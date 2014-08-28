<?PHP
/*
Erstellt von André Münstermann

Kurze Beschreibung der Klasse:
Diese Klasse wird auf der Homepage eingebunden und bietet die komplette Kommunikation
von der Homepage mit der Datenbank
*/
class apiWrapper
{
	/*Variablen initzialiesirung*/
	public $baseurl = "http://btcwash.de:8080/api/";//BILDET DEN STAMM DER URL
	public $token = ""; //wird aus $session_secrect gebildet 
	public $commandCounter = 0; // zählt jeden Sicherheitskritischen befehl 
	public $session_secret =""; // kommt vom Server
	public $username = "";
	public $password = "";
	
	function apiWrapper()
	{
		//Standartkonstruktor
	}
	
	/*klassenFunktionen*/
	
	/*
	
	*/
	function postData($data,$functionurl)
	{
		/*
		bekommt ein array mit zu postenden daten
		returnt ein objekt mit daten vom Server*/
		
		$options = array(
			'http' => array(
				'header'  => "Content-type: application/x-www-form-urlencoded\r\n",
				'method'  => 'POST',
				'content' => http_build_query($data),
			),
		);
		$context  = stream_context_create($options);
		$json = file_get_contents($functionurl, false, $context);
		//Man erhält vom Server ein JSON Objekt
		
		
		$obj = json_decode($json);
		//Dieses wird nun zu einem PHP Objekt decodiert
		
		return $obj;
		
		
	}
	
	/*
		Diese Funktion generiert aus dem Session_Secret und dem CommandCounter ein Token
		Dieses wird auf dem Server ebenfalls gemacht und abgeglichen
	*/
	function tokenGen()
	{
		
		$newToken = md5($this->session_secret.$this->commandCounter);
		$this->commandCounter++;
		return $newToken;
		
	}
	
	/*ServerFunktionen*/
	
	
	/*
	Diese Funktion registriert einen neuen Benutzer
	Wenn es klappt 
	Antwort = true
	Wenn nicht steht der Fehler in 'success'
	*/
	function register($username, $password)
	{
		
		$functionurl ="";
		$functionName = 'register';
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;
		
		$this->username = $username;
		$this->password = $password;
		
		$data = array('username' => $username, 'password' => $password);
		$obj = $this->postData($data,$functionurl);
		
		if(isset($obj->{'success'}))//verbindung steht
		{
			if($obj->{'success'}==true)//hat geklappt
			{
				return $obj->{'success'};
			}
			else//hat nicht geklappt
			{
				return $obj->{'error'};//fehlermeldung
			}
			
		}

	}
	
	
	/*
	Diese Funktion loggt einen Benutzer ein
	Wenn es klappt 
	Antwort = true
	Wenn nicht steht der Fehler in 'success'
	*/
	function login($username,$password)
	{
		/*einloggen
		im TRUE Fall bekommt man sessionsecret
		im false fall hat es nicht geklappt und kein sessionsecret
		*/
		$functionurl ="";
		$functionName = 'login';
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;
		
		$this->username = $username;
		$this->password = $password;
		
		$data = array('username' => $username, 'password' => $password);
		$obj = $this->postData($data,$functionurl);
		
		if(isset($obj->{'success'}))
		{
			if($obj->{'success'}==true)
			{
				$this->session_secret = $obj->{'session_secret'};
				$this->commandCounter = 0;
				echo $obj->{'success'};
			}
			else
			{
				return $obj->{'error'};
			}
		}
	}
	
	
	/*
	Diese Funktion bietet den username und die zugehörigen GPS daten
	von den Usern die in den letzten 5 Minuten online wahren
	*/
	function getPositionsMap()
	{
		$functionurl ="";
		$functionName = 'getPositionsMap';
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;
		
		$json = file_get_contents($functionurl, false);
		//Man erhält vom Server ein JSON Objekt
		
		
		$obj = json_decode($json);
		
		
		
		
		if(isset($obj->{'success'}))//verbindung steht
		{
			if($obj->{'success'}==true)//hat geklappt
			{
				return $obj->{'user_map'};
			}
			else//hat nicht geklappt
			{
				return $obj->{'error'};//fehlermeldung
			}
			
		}
	}
	

	
	/*
	Liefert ein zweidimensionales Array mit allen Spielen und der zugehörigen TopTen ohne User bezug
	Username,Score, array[spiel,zeile,werte]
	
	foo = [(spiel, username, score), (username, score)]
	foo[0][2
	*/
	function getTopTenScoresForAllMinigames()// und ohne $username
	{
		$functionurl ="";
		$functionName = 'getTopTenScoresForAllMinigames';
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;
		
		$json = file_get_contents($functionurl, false);
		//Man erhält vom Server ein JSON Objekt
		
		
		$obj = json_decode($json);
		
		
		
		
		if(isset($obj->{'success'}))//verbindung steht
		{
			if($obj->{'success'}==true)//hat geklappt
			{
				return $obj->{'game'};
			}
			else//hat nicht geklappt
			{
				return $obj->{'error'};//fehlermeldung
			}
			
		}
		
		
	}


	function getTopTenScoresForAllMinigamesByUsername($username)// und ohne $username
	{
		$functionurl ="";
		$functionName = 'getTopTenScoresForAllMinigames';
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;



		$data = array('username' => $username);
		$obj = $this->postData($data,$functionurl);

		if(isset($obj->{'success'}))
		{
			if($obj->{'success'}==true)
			{

				return $obj->{'game'};
			}
			else
			{
				return $obj->{'error'};
			}
		}
		
		
	}
	
	/*
	Diese Funktion liefert eine Liste mit allen registrierten Usern.
	username
	*/
	function getUserList()
	{
		//getUsers
		
		$functionurl ="";
		$functionName = 'getUsers';
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;
		
		$json = file_get_contents($functionurl, false);
		//Man erhält vom Server ein JSON Objekt
		
		
		$obj = json_decode($json);
		
		
		
		
		if(isset($obj->{'success'}))//verbindung steht
		{
			if($obj->{'success'}==true)//hat geklappt
			{
				return $obj->{'users'};
			}
			else//hat nicht geklappt
			{
				return $obj->{'error'};//fehlermeldung
			}
			
		}
		
	}
	
	/*
	Liefert ein Objekt mit allen Gästebucheinträgen
	Autor,Datum,Nachricht
	*/
	function getGuestbookEntryById($id)
	{
		$functionurl ="";
		$functionName = 'getGuestbookEntryById';
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;
		
		
		$data = array('id' => $id);
		$obj = $this->postData($data,$functionurl);
		
		if(isset($obj->{'success'}))
		{
			if($obj->{'success'}==true)
			{

				return $obj;
			}
			else
			{
				return $obj->{'error'};
			}
		}
	}


	//lIEFERT EINE Liste mit Indexen von GB Einträgen
	function getGuestbookIndex()//kriegt nichts und liefert index --> Liste zahlen
	{
		$functionurl ="";
		$functionName = 'getGuestbookIndex';
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;
		
		$json = file_get_contents($functionurl, false);
		//Man erhält vom Server ein JSON Objekt
		
		
		$obj = json_decode($json);
		
		
		
		
		if(isset($obj->{'success'}))//verbindung steht
		{
			if($obj->{'success'}==true)//hat geklappt
			{
				return $obj->{'index'};
			}
			else//hat nicht geklappt
			{
				return $obj->{'error'};//fehlermeldung
			}
			
		}

	}
	
	
	/*
	Diese Funktion bekommt einen Autor und eine Nachrricht
	die in der Datenbank gespeichert wird. 
	Die Zeit wird vom Server hinzugefügt
	*/
	function setNewGaestebuchEintrag($autor,$nachricht)
	{
		$functionurl ="";
		$functionName = 'makeGuestbookEntry';//TODO
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;
		
		
		$data = array('author' => $autor, 'message_str' => $nachricht);
		$obj = $this->postData($data,$functionurl);
		
		if(isset($obj->{'success'}))
		{
			if($obj->{'success'}==true)
			{

				return $obj->{'success'};
			}
			else
			{
				return $obj->{'error'};
			}
		}
	}
	



	
	/*Ab hier sollen Testmethoden entstehen*/
	
	function generalTest()
	{
		//TODO
	}
	
	
	/*
	Stellt eine Testmethode für die authentifizierung da
	*/
	function nop($username)
	{
		$token = $this->tokenGen();
		
		
		
		$functionurl ="";
		$functionName = 'nop';
		
		$functionurl .= $this->baseurl;
		$functionurl .= $functionName;
		
		$this->username = $username;
		
		$data = array('username' => $username, 'token' => $token);
		$obj = $this->postData($data,$functionurl);
		
		if(isset($obj->{'success'}))//verbindung steht
		{
			if($obj->{'success'}==true)//hat geklappt
			{
				echo $obj->{'success'};
			}
			else//hat nicht geklappt
			{
				echo $obj->{'error'};//fehlermeldung
			}
			
		}
	}
	


}

?>