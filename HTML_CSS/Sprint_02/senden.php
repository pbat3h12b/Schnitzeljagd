<!doctype html>		<!-- Dokument erstellt von Lukas Ebbers-->
<html>
<head>

<title>Vorprojekt</title>
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Meta-->
<meta charset="UTF-8">																		<!-- Charset für umlaute -->
<meta name="viewport" content="width=device-width, initial-scale=1" /> 				<!--Für das Responsive Webdesign -->
<!--Meta closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->

<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Style-->
<link rel="stylesheet" type="text/css" href="style.css" />									 <!-- Einbindung des CSS -->
<!--Style closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->

</head>
<body>
		
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->	
<!--Header-->
		<header id="mainHeader">														  <!-- Beinhaltet den Banner -->
			<h1 class="wrapper">Quantums Quests</h1>									<!-- Titel der Internetseite -->
			<img src="./images/logo.png" alt="logo" width="150px" height="150px">		
		</header>
<!--Header closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->

<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Navigation  Erstellt von Martin Dirkmorfeld-->			
		<div id="naviBackground">												   <!-- Navigation der Internetseite -->
	        <nav class="wrapper">
	            <ul>
	                <li>
	                    <a href="index.html">Home</a>
	                </li>
	                <li>
	                    <a href="#">About</a>
	                    <ul>
	                        <li><a href="#">The Team</a></li>
	                        <li><a href="#">The Game</a></li>
	                    </ul>
	                </li>
	                <li>
	                     <a href="#">Informationen</a>
	                     <ul>
	                         <li><a href="#">Statistiken</a></li>
	                         <li><a href="#">LiveMap</a></li>
	                     </ul>
	                </li>
	                <li>
	                    <a href="#">Kontakt</a>
	                    <ul>
	                        <li><a href="#">Email</a></li>
	                        <li id="active"><a href="#">Gästebuch</a></li>
	                    </ul>
	                </li>
	            </ul>
	        </nav>
        </div>
<!--Navigation closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->

<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->	
<!--Main-->
		<div id="mainBackground">
			<main class="wrapper">
			<br>
			<br>	
	<h1>Mein G&auml;stebuch - Senden</h1>

	
		<?php
		include('api.php');
		$klasse = new apiWrapper;
		$name =$_GET['name'];
		$nachricht =$_GET['nachricht'];
		$errMsg = $klasse -> setNewGaestebuchEintrag($name,$nachricht);
		//TODO weiterleitung zum GB
		

		
		/*

		
		if($name == "" or $mail == "" or $nachricht == "") {
			echo "Du hast die Felder nicht ausgef&uuml;llt...";
		} else {
			$verbindung = mysql_connect("")
			or die ("Fehler im System");

			mysql_select_db("bvb_fsv")
			or die ("Verbidung zur Datenbank war nicht möglich...");
			
			$id = 0;
			$abfrage = "SELECT id FROM gaestebuch ORDER BY id DESC LIMIT 1";
			$ergebnis = mysql_query($abfrage);
			while($row = mysql_fetch_object($ergebnis))
				{
					$id = $row->id;
				}
			$id++;
			

			$timestamp = time();
			$datum = date("Y.m.d h:i:s", $timestamp);
			
			$nachricht = str_replace("ä", "&auml;", $nachricht);
			$nachricht = str_replace("Ä", "&Auml;", $nachricht);
			$nachricht = str_replace("ö", "&ouml;", $nachricht);
			$nachricht = str_replace("Ö", "&Ouml;", $nachricht);
			$nachricht = str_replace("ü", "&uuml;", $nachricht);
			$nachricht = str_replace("Ü", "&uuml;", $nachricht);
			$nachricht = str_replace("ß", "&szlig;", $nachricht);
			$nachricht = str_replace("<", "<&nbsp;", $nachricht);
			$nachricht = str_replace(">", ">&nbsp;", $nachricht);
			$nachricht = str_replace("\r\n", "<br />", $nachricht);
			
			$name = str_replace("ä", "&auml;", $name);
			$name = str_replace("Ä", "&Auml;", $name);
			$name = str_replace("ö", "&ouml;", $name);
			$name = str_replace("Ö", "&Ouml;", $name);
			$name = str_replace("ü", "&uuml;", $name);
			$name = str_replace("Ü", "&uuml;", $name);
			$name = str_replace("ß", "&szlig;", $name);
			$name = str_replace("<", "<&nbsp;", $name);
			$name = str_replace(">", ">&nbsp;", $name);
			
			$eintrag = "INSERT INTO gaestebuch
			(id, name, mail, nachricht, datum)

			VALUES
			('$id', '$name', '$mail', '$nachricht', '$datum')";

			$eintragen = mysql_query($eintrag);

			if($eintragen = true) {
				?>
				<p>Vielen Dank. Dein Eintrag wurde erfolgreicht gespeichert...</p>
				<p><a href="index.php">Zur&uuml;ck</a></p>
				<?php
			} else {
				echo "Fehler im System. Konnte nicht gespeichert werden. Versuchen Sie es erneut.";
			}
			
			mysql_close($verbindung);
		}
			*/
	?>
			</main>
		</div>	
<!--Main closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->

<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Footer-->
		<footer id="mainFooter" class="wrapper">			<!-- Footer der extra Informationen zur Seite beinhaltet -->
			blablbla<a href="http://www.bib.de">b.i.b.</a>blablabla
		</footer>
<!--Footer closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->		
</body>
</html>
