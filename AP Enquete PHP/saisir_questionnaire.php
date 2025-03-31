<!DOCTYPE html>
<html lang="fr">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Elaboration d'un QUESTIONNAIRE</title>
  <link rel="stylesheet" href="questionnaires.css" type="text/css">
</head>
<body>
  <form method="POST" action="traiter_questionnaire.php">
    <!-- "traiter_questionnaire.php" : A ECRIRE -->

    <!-- Zone de saisie du QUESTIONNAIRE -->
    <!-- Nom (name), Libellé à afficher (displayName), Description (description) -->
    <fieldset>
      <legend>Questionnaire</legend>
      <label for="name">Nom :</label>
      <input type="text" id="name" name="name" size="40" maxlength="40">
      
      <label for="displayName">Libellé à afficher :</label>
      <input type="text" id="displayName" name="displayName" size="40" maxlength="60">
      
      <label for="description">Descriptif :</label>
      <textarea id="description" name="description" cols="30" rows="4"></textarea>
    </fieldset>
    
    <!-- Boutons -->
    <p>
      <input type="submit" value="Envoyer">
      <input type="reset" value="Annuler">
    </p>
  </form>
</body>
</html>