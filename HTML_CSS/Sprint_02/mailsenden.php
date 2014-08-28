<?PHP

$text = "Kontaktaufname von: ".$_POST[name]."\n Seine Nachricht lautet:".$_POST[nachricht]."\n\n Antworten können Sie hier:".$_POST[email];

mail("a@gmx.de","Email per Formular",$text);

header('Location: http://unkolunix.de/vorprojekt/contact.php'); exit; 
?>
