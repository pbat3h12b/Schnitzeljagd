<?php

/*erstellt von Andre Münstermann*/
	include('api.php');
	$klasse = new apiWrapper;
	$name =$_GET['name'];
	$nachricht =$_GET['nachricht'];
	$errMsg = $klasse -> setNewGaestebuchEintrag($name,$nachricht);


	header('Location: http://btcwash.de/vorprojekt/contact.php');
	exit();  
?>
