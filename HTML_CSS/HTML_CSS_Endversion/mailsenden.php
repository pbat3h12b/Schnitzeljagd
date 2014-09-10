<?PHP //Erstellt von Lukas Ebbers

$text = "Kontaktaufname von: ".$_POST[name]."\n Seine Nachricht lautet:".$_POST[nachricht]."\n\n Antworten können Sie hier:".$_POST[email];

mail("dozkeh@pb.bib.de","Email per Formular",$text);

header('Location: http://btcwash.de/vorprojekt/contact.php'); exit; 
?>
