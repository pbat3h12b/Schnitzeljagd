<!doctype html>		<!-- Dokument erstellt von Lukas Ebbers-->
<html>
<head>

<title>Vorprojekt</title>
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Meta-->
<meta charset="UTF-8">																		<!-- Charset für umlaute -->
<meta name="viewport" content="width=device-width, initial-scale=1" /> 				<!--Für das Responsive Webdesign -->
<script type="text/javascript" src="js/kontakt.js"></script>
<!--Meta closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->

<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Style-->
<link rel="stylesheet" type="text/css" href="CSS/style.css" />								 <!-- Einbindung des CSS -->
<link rel="stylesheet" type="text/css" href="CSS/content.css" />
<link rel="stylesheet" type="text/css" href="CSS/footer.css" />
<link rel="stylesheet" type="text/css" href="CSS/navi.css" />
<link rel="stylesheet" type="text/css" href="CSS/responsive.css" />
<link rel="stylesheet" type="text/css" href="CSS/scrollbars.css" />
<link rel="stylesheet" type="text/css" href="CSS/header.css" />
<link rel="stylesheet" type="text/css" href="CSS/button.css" />
<link rel="stylesheet" type="text/css" href="CSS/table.css" />
<!--Style closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->

</head>
<body>
		
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->	
<!--Header-->
		<header id="mainHeader">														  <!-- Beinhaltet den Banner -->
			<h1 class="wrapper">Quantums Quests</h1>									<!-- Titel der Internetseite -->


<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Navigation  Erstellt von Martin Dirkmorfeld-->			
		<div id="naviBackground">												   <!-- Navigation der Internetseite -->
	        <nav class="wrapper">
	            <ul>
	                <li >
	                    <a href="index.html">Home</a>
	                </li>
	                <li>
	                    <a href="about.html">About</a>
	                    <ul>
	                        <li><a href="about.html#theteam">The Team</a></li>
	                        <li><a href="about.html#thegame">The Game</a></li>
	                    </ul>
	                </li>
	                <li>
	                     <a href="information.php">Information</a>
	                     <ul>
	                         <li><a href="information.php#statistiken">Stats</a></li>
	                         <li><a href="information.php#livemap">LiveMap</a></li>
	                     </ul>
	                </li>
	                <li id="active">
	                    <a href="contact.php">Contact</a>
	                    <ul>
	                        <li><a href="contact.php#mailkontakt">Email</a></li>
	                        <li><a href="contact.php#gaestebuch">Guestbook</a></li>
	                    </ul>
	                </li>
	            </ul>
	        </nav>
        </div>
<!--Navigation closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->


		</header>
<!--Header closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->



<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->	
<!--Main-->
<!-- Erstellt von Lukas Ebbers-->
		<div id="mainBackground">
			<div class="wrapper main">
				<article>
					<header class="articleHeader">
						<h2>Kontaktdaten</h2>
					</header>
					<footer>						
					</footer>
					<content>
						Unsere Kontaktdaten:<br>
						Ansprechpartner: XYZ<br>
						b.i.b. International College Paderborn<br>
						Klasse: PBAT3H12B<br>
					</content>
				</article>
				
				<article>
					<header class="articleHeader"> 
						<a name="mailkontakt"><h2>E-Mail Kontakt</h2></a>
					</header>
					<footer>
						<br>
					</footer>
					<content>
						<form action="mailsenden.php" onsubmit="return fields_check()" method="post">
						Dein Name:<br /><input type="text" id="i_name" name="i_name" /><br /><br />
						Dein Mail:<br /><input type="text" id="i_mail" name="i_mail"/><br /><br />
						Dein Text:<br /><textarea id="area" id="i_nachricht" name="i_nachricht"></textarea>
						<br /><br />
						<input type="submit">
						</form>
					</content>
				</article>
				
				<article>
					<header class="articleHeader">
						<a name="gaestebuch"><h2>Gästebuch</h2></a>
					</header>
					<footer>
						<br>
						<a href="guestbook.html" id="newtag">Neuen Eintrag schreiben &raquo;</a>
					</footer>
					<content class="alignLeft">
					
					<?php /*Erstellt von Martin Dirkmorfeld und André Münstermann*/
							include('api.php');
							$klasse = new apiWrapper;
							$idList= $klasse -> getGuestbookIndex();
							date_default_timezone_set('Etc/GMT-4');
							
							
							foreach($idList as $index)
							{
							$eintragArr = $klasse -> getGuestbookEntryById($index);
							showEintrag($eintragArr);
							
							}
							

							
							function showEintrag($eintragArr)
							{
								/*•	/IS106/ Als Spieler möchte ich Gästebucheinträge auf der Seite hinzufügen und abrufen können*/
								?>
								<h3><?php echo $eintragArr->{'author'}; ?>&nbsp;<small style="color:grey;">schrieb:</small></h3>
								<p class="nachricht">
								<?php echo $eintragArr->{'message'}; ?>
								</p>
								<h5 style="color:grey;"><?php echo date("Y-m-d H:i:s",$eintragArr->{'date'}); ?></h5>  
								<hr />				
								<?php
							}
							
					/*  Erstellt von Lukas Ebbers
						Erster Versuch eines Gästebuches mit einer normalen Datenbankverbindung
					
						$verbindung = mysql_connect("localhost", "user", "pw")
						or die ("Fehler im System");

						mysql_select_db("bvb_fsv")
						or die ("Verbidung zur Datenbank war nicht möglich...");

						$pagesuche = 0;
						$url = $_SERVER["REQUEST_URI"];
						$pagesuche = strpos($url, "?page=");
						
						if($pagesuche == "") {
							$page = 1;
						} else {
							$page = $_GET["page"];
						}
						
						$wo = ($page * 5) - 5;
						$wo++;
						
						$zahl = 1;
						$pos = 1;
					
						$abfrage = "SELECT id FROM gaestebuch ORDER BY id DESC";
						$ergebnis = mysql_query($abfrage);
						while($row = mysql_fetch_object($ergebnis)) 
							{
								if($zahl == $wo) {
									$pos = $row->id;
								}
								$zahl++;
							}
						
						
						$abfrage = "SELECT * FROM gaestebuch WHERE id <= '$pos' ORDER BY id DESC LIMIT 5";
						$ergebnis = mysql_query($abfrage);
						while($row = mysql_fetch_object($ergebnis)) 
							{
							?>
								<h3><?php echo $row->name; ?>&nbsp;<small style="color:grey;">schrieb:</small></h3>
								<p>
								<?php echo $row->nachricht; ?>
								</p>
								<h5 style="color:grey;"><?php echo $row->datum; ?></h5>
								<hr />				
							<?php
							}
					?>				
					Weitere Seiten
					<?php
						if($page > 1) {
						?>
						<a href="index.php?page=<?php echo ($page - 1); ?>">Zur&uuml;ck</a>
						<?php
						}
						
						$anzahlseite = ceil($zahl / 5);
						$weiterfrage = $anzahlseite - $page;
						
						if($weiterfrage > 0) {
						?>
						<a href="index.php?page=<?php echo ($page + 1); ?>">Weiter</a>
						<?php
						}
				*/	?>				
					</content>
				</article>
			</div>
		</div>	
<!--Main closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->

<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Footer-->
		<footer id="mainFooter" class="wrapper">			<!-- Footer der extra Informationen zur Seite beinhaltet -->
			<a href"=http://www.bib.de">b.i.b. International College</a>
		</footer>
<!--Footer closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->		
</body>
</html>
