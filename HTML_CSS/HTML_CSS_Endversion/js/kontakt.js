// globale Vorgaben
// -----------------------------------------------------------------------------
// Erstellt von Lukas Ebbers
// -----------------------------------------------------------------------------
// Muss-Felder je Bereich
var arr_kontaktformular = [
      'i_name'
    , 'i_mail'
	, 'i_nachricht'
  ];


// Zentrale Feldprüfung
// -----------------------------------------------------------------------------
function fields_check(){
  var err_id =  0;
  
// 0. Knoten zu Knoten-ID's in Array einlesen

	var arr_knoten_kf = nodes_get(arr_kontaktformular);

	
// --------------------------  
// 1. alle Muss-Felder prüfen
// 1.1 alle Felder zum Kontaktformular
	err_id = summary_must_check(arr_knoten_kf); 

// 2. Fehler-Status Auswertung
	if (err_id == 1)
	{
	return false;
	}
	else{
	return true;
	}	
}



// Muss-Felder je Summary prüfen
// -----------------------------------------------------------------------------
function summary_must_check(arr_node){
	for	(var i = 0; i<arr_node.length; i++) {  
	if(arr_node[i].value == "")
		{err_id = 1;
		arr_node[i].classList.add('err_io');
		alert("Fehler");
		}
	else
		{
			arr_node[i].classList.remove('err_io');
		}
	}
	return err_id;
}


// Helfer
// -----------------------------------------------------------------------------
function nodes_get(node_ids){
  var arr_nodes = new Array;
  for(var i=0; i<node_ids.length; i++){
    arr_nodes[i] = document.getElementById(node_ids[i]);
  }
  return arr_nodes;
}
