DROP DATABASE IF EXISTS qcm_enquete;
CREATE DATABASE qcm_enquete;
USE qcm_enquete;
CREATE TABLE questionnaire (
    cle VARCHAR(12) PRIMARY KEY,
    name VARCHAR(40),
    displayName VARCHAR(60),
    description VARCHAR(124)
);
CREATE TABLE reponses (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cle_questionnaire VARCHAR(12),
    rang INT,
    date_creation DATETIME,
    reponse VARCHAR(250),
    FOREIGN KEY (cle_questionnaire) REFERENCES questionnaire(cle) ON DELETE CASCADE
);

INSERT INTO questionnaire (cle, name, displayName, description) VALUES
('25036043154', 'Cereales', 'Enquête consommation Céréales Chocolat', 'Etudier les goûts en matière de céréales au chocolat'),
('25036035702', 'Recrutement', 'Test 1 pour recrutement salariés', 'Test pour recrutement salariés'),
('25036043641', 'Voiture', 'Test pour choix voiture', 'Choisir sa voiture');
