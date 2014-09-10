<!doctype html>     <!-- Dokument erstellt von Lukas Ebbers-->
<html>
  <head>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <style type="text/css">
      #map_canvas { height: 500px; }
    </style>
    <script type="text/javascript"
      src="http://maps.googleapis.com/maps/api/js?key=AIzaSyAqr9LPH1ryoaN2gky0bBIq-QnT3Pm1HFs&sensor=true">//verbindung zu google mit api key
    </script>
    <script type="text/javascript">

    /*Javascript und php erstellt von Andre Münstermann
    Dieser Javascript Code greift auf die Datenbank zu und lässt die User auf der Map erscheinen
    */

        var markersArray = [];//array mit gps daten der user
        var map;
            
        function addMarker(location,title) //funktion die die User zur LiveMap hinzufügt
        {
          marker = new google.maps.Marker({
          position: location,
          title: title
          });

          markersArray.push(marker);
        }


        function initialize() //wird beim laden der Seite ausgeführt
        {
            var bib = new google.maps.LatLng(51.7307805133534, 8.7374138832092294);//Koordinaten des bibs
            var mapOptions = {
            center: bib,//karte soll anfangs überm bib zentriert werden
            zoom: 18,
            mapTypeId: google.maps.MapTypeId.HYBRID
            };

            var map = new google.maps.Map(document.getElementById("map_canvas"),mapOptions);

        	<?PHP
              function stringToColorCode($str) 
              {
                $code = dechex(crc32($str));
                $code = substr($code, 0, 6);
                return $code;
              }

              function drawLine($path,$username)
              {
                  $count = 0;
                  echo "var koorArr = [\n";
                  foreach ($path as $key) //array erzeugen
                  {
                    if($count != 0)
                    {
                      echo ",\n";
                    }
                    echo "new google.maps.LatLng(".$key[1].", ".$key[0].")";
                    $count++;
                  }
                  echo "];\n";

                  $color = stringToColorCode($username);

                  ?>var flightPath = new google.maps.Polyline({
                  path: koorArr,
                  strokeColor: <?PHP echo "'#".$color."',"?>// USER BEKOMMT eigene color
                  strokeOpacity: 1.0,
                  strokeWeight: 2
                });

                flightPath.setMap(map);<?PHP


              }



        	    include('api.php');
        	   $klasse = new apiWrapper;//neues objekt zur Datenbankverbindung
        		$posArr = $klasse -> getPositionsMap();//hohlt array mit usernamen und gps daten
        		$count = 0;

        	    foreach ($posArr as $key => $value) //schreibt javascript code
        	    {
        	        echo "var pos".$count." = new google.maps.LatLng(".$value[1].",".$value[0].");\n";
        	        echo "addMarker(pos".$count.",'".$key."');\n";
        	        $count++;

                  $path = $klasse -> getUserPath($key);

                  drawLine($path,$key);
                  
        	    }
                                  
        	?>


            for (i in markersArray) 
            {
                markersArray[i].setMap(map);
            }

        }

</script>


<title>Vorprojekt</title>
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Meta-->
<meta charset="UTF-8">                                                                      <!-- Charset für umlaute -->
<meta name="viewport" content="width=device-width, initial-scale=1" />              <!--Für das Responsive Webdesign -->
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
<body onload="initialize()">
        
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->    
<!--Header-->
        <header id="mainHeader">                                                          <!-- Beinhaltet den Banner -->
            <h1 class="wrapper">Quantums Quests</h1>                                    <!-- Titel der Internetseite -->


<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Navigation  Erstellt von Martin Dirkmorfeld-->          
        <div id="naviBackground">                                                  <!-- Navigation der Internetseite -->
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
                        
                    
                        <?php     //Tabellen und Spielerauswahl Erstellt von Martin Dirkmorfeld und Andre Muenstermann          
                        //connection    
                        $arr = $klasse -> getTopTenScoresForAllMinigamesByUsername($Username);                      
                        $arr = $klasse -> getTopTenScoresForAllMinigames();
                        //var_dump($arr);
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
                            Nr
                            </th>
                            <th>
                            Username
                            </th>
                            <th>
                            Pts.
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
                              echo date("Y-m-d H:i:s", $array[$jdx]->{'date'});
                             
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

                    </content>
                </article>


                <article>
                    <header class="articleHeader">
                        <h2>Livemap</h2>             
                    </header>
                    <footer>
                    </footer>
                    <content>
                        <div id="livemap" style="height:500px;"><!-- Live Map erstellt von Andre Münstermann-->
							<div id="map_canvas" style="width:100%; height:100%"></div>

						</div>
                    </content>
                </article>
            </div>
        </div>  
<!--Main closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->

<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->
<!--Footer-->
        <footer id="mainFooter" class="wrapper">            <!-- Footer der extra Informationen zur Seite beinhaltet -->
            blablbla<a href="http://www.bib.de">b.i.b.</a>blablabla
        </footer>
<!--Footer closed-->
<!-- ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| -->        
</body>
</html>
