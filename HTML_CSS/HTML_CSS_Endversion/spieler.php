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
	                <li>
	                    <a href="index.html">Home</a>
	                </li>
	                <li>
	                    <a href="about.html">About</a>
	                    <ul>						
	                        <li><a href="about.html#theteam">The Team</a></li>
	                        <li><a href="about.html#thegame">The Game</a></li>
	                    </ul>						
	                </li>
	                <li id="active">
	                     <a href="information.php">Information</a>
	                     <ul>
	                         <li><a href="information.php#statistiken">Stats</a></li>
	                         <li><a href="information.php#livemap">LiveMap</a></li>
	                     </ul>
	                </li>
	                <li>
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
		<div id="mainBackground">
			<div class="wrapper main">			
				<article>
					<header class="articleHeader">
						<h2>Statistiken</h2>
					</header>
					<footer>
					</footer>
					<content>
					<p>				
						<?php //Erstellt von Martin Dirkmorfeld und Andre Münstermann	
							include('api.php');
							$klasse = new apiWrapper;

							?>
						                        <?php 
                            date_default_timezone_set('Etc/GMT-4');

                            $spielerArray = $klasse -> getUserList();
                            ?>
                            
                        <form action='spieler.php' name="form" method='GET'>                
                        <select name='spielerListe' size='1'>
                        <?php
                            foreach ($spielerArray  as $option)
                            {?>
                               <option><?PHP echo $option;?></option>
                            <?PHP }
                        ?>
                        </select>
                        <input type='submit'>
                        </form>				
						<?php			
						//connection	
						$arr = $klasse -> getTopTenScoresForAllMinigamesByUsername($_GET['spielerListe']);						
						//var_dump($_GET['spielerListe']);
						//var_dump($arr->{'Zukunftsmeile'});
						
						
						?><div class="tables"><?php
						drawTable($arr->{'Zukunftsmeile'},'Zukunftsmeile');
						drawTable($arr->{'HNF'},'HNF');
						drawTable($arr->{'Fluss'},'Fluss');
						drawTable($arr->{'Serverraum'},'Serverraum');
						drawTable($arr->{'Wohnheim'},'Wohnheim');
						?></div><?php
						
						
						
						function drawTable($array,$title)
						{
 
						
							$counter = 0;
							?>
							<h2><?php echo $title?></h2>
							
							<table border="1">
							
							<thead>
							<th>
							Nummmer
							</th>
							<th>
							Username
							</th>
							<th>
							Punkte
							</th>
							<th>
							Date
							</th>
							</thead>
							<tbody>
							<?php
							for ($jdx=0;$jdx < sizeof($array);$jdx++)
							{					
							?>
							<?php
							  $counter ++;
							  ?>
							  <tr>
							  <td> 
							  <?php
							  echo $counter;
							  	?>					
							  </td>
							  <td>
							  <?php 
							  echo $array[$jdx]->{'username'};
							  ?>
							  </td>
							  <td>
							  <?php 
							  echo $array[$jdx]->{'points'};
							  ?>
							  </td>
							  <td>
							  <?php 
							  echo gmdate("Y-m-d H:i:s", $array[$jdx]->{'date'});
							 
							  ?>
							  </td>
							  </tr>
							  
							 <?php  
							}
							?>
							</tbody>
							</table>
							<br><br>
							<?php 
						}
						?>											
						

					</p>
					<br>
					<br>
					<a href="information.php">zurück</a>
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