<?php
//------------------------------------------------
// Récupérer le libellé et la clé du questionnaire
//------------------------------------------------
session_start();
$questionnaire = isset($_SESSION["displayName"]) ? $_SESSION["displayName"] : "Questionnaire inconnu";
$cle = isset($_SESSION["cle"]) ? $_SESSION["cle"] : null;

?>
<html>
<head>
<title>QUESTIONS liées à un Questionnaire</title>
<link rel="stylesheet" href="questionnaires.css" type="text/css" />
<script type="text/javascript">
    // Limiter le nombre de lignes du TEXTAREA à 5
    function bloquer(event, zoneSaisie) {  
      // Si on a tapé sur un caractère de retour à la ligne
      if (event.keyCode === 13 || event.which === 13) {  
        // Nombre de "\n" présents dans la textarea >= 5
        if (zoneSaisie.value.split("\n").length >= 5) { 
          // Empêcher l'insertion du retour à la ligne
          if (event.preventDefault) {
            event.preventDefault();
          } else {
            event.returnValue = false;
          }
          return false;
        }
      }
      return true;
    }
  </script>
</head>
<body>
<form method="POST" action="traiter_questions.php" >
<h1><?php echo $questionnaire;?></h1>

<!-- Zone de saisie des QUESTIONS -->
<!-- Type d'objet (type), Nom (name), Libellé à afficher (text) -->
<!-- Réponses (reponses), Par défaut (defaut) -->
<fieldset>
<legend>Question</legend>

<label for="type">Type de composant :</label>
   <select name="type">
		<option value="combo">Liste déroulante</option>
		<option value="liste">Liste à choix multiples</option>
		<option value="radio">Boutons radio</option>
		<option value="text">Zone de texte</option>
   </select>

<label for="name">Nom :</label>
<input type="text" name="name" size="20" maxlength="20"/>
	
<label for="text">Libellé à afficher :</label>
<input type="text" name="text" size="40" maxlength="60"/>

<br><br>
<table>
<tr>
<td>
<label for="reponses">Réponse(s) (1 par ligne) :</label>
<textarea name="reponses" cols="30" rows="4" wrap="off" onkeyPress="return( bloquer(event, this) );"></textarea>
</td>
<td>
   <h2>Réponse par défaut ?</h2>
   <p>
   <label class="inline">Réponse 1</label>
   <input type="radio" name="defaut" value="1" checked="checked" /> 
   <br>
   <label class="inline">Réponse 2</label>
   <input type="radio" name="defaut" value="2" /> 
   <br>
   <label class="inline">Réponse 3</label>
   <input type="radio" name="defaut" value="3" /> 
   <br>
   <label class="inline">Réponse 4</label>
   <input type="radio" name="defaut" value="4" /> 
   <br>
   <label class="inline">Réponse 5</label>
   <input type="radio" name="defaut" value="5" /> 
   </p>
</td>
</tr>
</table>
</fieldset>
<!-- Boutons -->
<p>
	<input type="submit" value="Envoyer" />
	<input type="reset" value="Annuler" />
	<input value="Nouveau questionnaire" onclick="self.location.href='saisir_questionnaire.php'" />
</p>
</form>
</body>
</html>
