<?PHP //Erstellt von Lukas Ebbers

$text = "Kontaktaufname von: ".$_POST['i_name']."\n Seine Nachricht lautet:" $_POST['i_nachricht']."\n\n Antworten können Sie hier:".$_POST['i_mail'];

mail("dozkeh@pb.bib.de","Email per Formular",$text);

header('Location: http://btcwash.de/vorprojekt/contact.php'); exit; 
?>
