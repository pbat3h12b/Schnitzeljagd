<!-- Dokument erstellt von Andre Münstermann-->
<?PHP
include('api.php');
$klasse = new apiWrapper;


$path = $klasse -> getUserPath('Marty');
          var_dump($path);

?>