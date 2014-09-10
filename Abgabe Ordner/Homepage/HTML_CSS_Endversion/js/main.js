//Erstellt von Martin Dirkmorfeld
window.onload = function()
{
	var moreLinks = GetNodesByName('more');
	for(var idx=0; idx<moreLinks.length;idx++)
	{
		moreLinks[idx].onclick = function()
		{
			//Funktion
		};
	}
};


// Hilfsfunktionen
// -----------------------------------------------------------------------------
function GetNodesByName(string_name) 
{
// Knoten-Objekte zu den ID's aus dem HTML einlesen
  var nodes = new Array(); 
   nodes = document.getElementsByName(string_name);
  return la_range_nodes;
}

function GetNodesByIDs(array_ids) 
{
// Knoten-Objekte zu den ID's aus dem HTML einlesen
  var nodes = new Array(); 
  for(i=0; i<array_ids.length;i++)
  {
    nodes[i] = document.getElementById(ia_ranges[i]);
  }
  return nodes;
}

function GetNodeByID(string_ID) 
{
// Knoten-Objekte zu den ID's aus dem HTML einlesen
  var node; 
  node= document.getElementById(string_ID);
  return nodes;
}
