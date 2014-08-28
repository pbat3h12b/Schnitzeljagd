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
	                        <li id="active"><a href="#">G&auml;stebuch</a></li>
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
		<div id="mainBackground">
			<main class="wrapper">
			<br>
			<br>	
	<h1>Mein G&auml;stebuch</h1>
		<legend>Neuer Eintrag</legend>
		<a href="guestbook.html">Neuen Eintrag schreiben &raquo;</a>
	<fieldset>
		<legend>G&auml;stebuch</legend>
		<?php
			$verbindung = mysql_connect("")
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
	</fieldset>
		<legend>Weitere Seiten</legend>
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
