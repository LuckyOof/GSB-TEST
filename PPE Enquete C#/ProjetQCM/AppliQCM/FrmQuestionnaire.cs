using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AppliQCM
{
    public partial class FrmQuestionnaire : Form
    {
        //**********
        // ATTRIBUTS
        //**********

        // Constantes
        private const int LARGEUR_CONTROLES = 300;
        private const int CARACTERES_PAR_LIGNE = 30;
        private const int HAUTEUR_PAR_LIGNE = 19;

        // Va permettre de définir l'emplacement :
        // 		a) Des contrôles créés dans la feuille
        // 		b) D'une nouvelle feuille en fonction du nombre et 
        // 		de la taille des contrôles qui seront créés dynamiquement
        //
        // Remarque : la structure "Point" représente une paire 
        // ordonnée de coordonnées x et y entières qui définit 
        // un point dans un plan à deux dimensions.
        private Point emplacement = new Point(10, 10);

        // Document XML associé
        private XmlDocument xr;

        // Titre de la feuille
        private string titre;


        //*************
        // CONSTRUCTEUR
        //*************
        public FrmQuestionnaire(string docXML, Form fenMere)
        {
            InitializeComponent();
            // Associer cette feuille fille à la fenêtre mère
            this.MdiParent = fenMere;

            // Remplir le questionnaire à partir du document XML
            CreerAPartirXML(docXML);
        }

        //***********
        // ACCESSEURS
        //***********

        // Retourne ou modifie la propriété "Height" de la feuille
        private int LaHauteur
        {
            get { return this.Height; }
            set { this.Height = value; }
        }

        // Retourne ou modifie la propriété "Width" de la feuille
        private int Largeur
        {
            get { return this.Width; }
            set { this.Width = value; }
        }

        // Retourne une COLLECTION des CONTROLES graphiques figurant sur la feuille
        private Control.ControlCollection TousLesControles
        {
            get { return this.Controls; }
        }

        // Retourne ou modifie la propriété privée "Titre", et dans ce dernier cas, 
        // la propriété "Text" de la feuille est renseignée.
        private string LeTitre
        {
            get { return titre; }

            set
            {
                titre = value;
                this.Text = titre;
            }
        }

        //**********
        //  METHODES
        //**********

        //---------------------------------------------------------
        // Création dynamique des contrôles sur la feuille à partir
        //  du contenu d'un document XML représentant un QCM
        //---------------------------------------------------------
        private void CreerAPartirXML(string doc)
        {
            // Accesseur
            this.LeTitre = "Questionnaire";

            // Appel de l'accesseur 'TousLesControles' pour récupérer la collection de  
            // contrôles sur la feuille
            Control.ControlCollection lesControles = this.TousLesControles;

            // Initialisation de l'emplacement
            emplacement = new Point(10, 10);

            // Creation d'un document XML qui servira à remplir la nouvelle feuille
            xr = new XmlDocument();
            xr.Load(doc);

            // Sélectionne le premier noeud (ici : <questionnaire>) et récupère la valeur
            // de son attribut "name" 
            string premierNoeud = xr.SelectSingleNode("questionnaire").Attributes["name"].Value;

            // Initialise la propriété "Titre" de la nouvelle feuille à partir de la valeur
            // de l'attribut "displayName" 
            this.LeTitre = xr.SelectSingleNode("questionnaire").Attributes["displayName"].Value;

            // Création d'une collection ordonnée de noeuds <question>
            XmlNodeList lesNoeuds;
            lesNoeuds = xr.GetElementsByTagName("question");

            // Parcours de l'ensemble des noeuds <question> présents dans la collection
            foreach (XmlNode unNoeud in lesNoeuds)
            {
                if (unNoeud.Attributes != null)
                {
                    // Détermine le type du contrôle à créer.
                    // Le type est spécifié dans l'attribut "type" : <question type= ... >
                    // Suivant le type de contrôle, une procédure "Add..." est
                    // appelée. Les paramètres sont les suivants :
                    // 		a) L'objet noeud <question> en cours
                    // 		b) La collection de contrôles de la feuille
                    // 		c) L'emplacement (coordonnées X et Y)
                    //		d) L'objet premier noeud du document XML (<questionnaire>)
                    switch (unNoeud.Attributes["type"].Value)
                    {
                        case "text":
                            emplacement = AddTextBox(unNoeud, lesControles, emplacement, premierNoeud);
                            break;
                        case "combo":
                            emplacement = AddComboBox(unNoeud, lesControles, emplacement, premierNoeud);
                            break;
                        case "liste":
                            emplacement = AddListBox(unNoeud, lesControles, emplacement, premierNoeud);
                            break;
                    }
                }
            }

            // On spécifie la largeur et la hauteur de la feuille créée dynamiquement.
            // En effet, sa dimension dépend du nombre de contrôles à placer, et par
            // conséquent du contenu du document XML.
            // Un ajustement (de 40) s'avère cependant nécessaire...
            this.Largeur = emplacement.X + LARGEUR_CONTROLES + 40;
            this.LaHauteur = emplacement.Y + 40;

            // Affichage du questionnaire
            this.Show();
        }


        //-----------------------------------------------------------------------------------------
        // Ensemble des méthodes qui, suivant le cas vont ajouter une ComboBox, une ListBox ou 
        // une TextBox à la collection passée en paramètre. 
        //
        // Retournent des coordonnées (X,Y) permettant de définir la dimension de la feuille 
        // qui va contenir ces contrôles...
        //
        // Ces méthodes sont appelées par la méthode "creerAPartirXML" qui crée d'abord
        // dynamiquement une feuille, puis l'ensemble de ses contrôles, et ceci à partir des 
        // données d'un document XML (un contrôle par noeud <question>)
        //
        // Les paramètres sont les suivants :
        // 	a) L'objet noeud <question> en cours
        // 	b) La collection de contrôles de la feuille
        //	c) L'emplacement (coordonnées X et Y) en cours (permet de placer les nouveaux contrôles)
        //	d) L'objet premier noeud du document XML (<questionnaire>)
        //------------------------------------------------------------------------------------------

        private Point AddTextBox(XmlNode unNoeud, Control.ControlCollection desControles, Point unEmplacement, string tag)
        {
            TextBox maTextBox = new TextBox();

            if (unNoeud.SelectSingleNode("defaultreponse") != null)
                maTextBox.Text = unNoeud.SelectSingleNode("defaultreponse").InnerText;

            if (unNoeud.Attributes["name"] != null)
                maTextBox.Name = unNoeud.Attributes["name"].Value;

            maTextBox.Tag = tag;
            maTextBox.Width = LARGEUR_CONTROLES;

            if (unNoeud.SelectSingleNode("maxCharacters") != null)
                maTextBox.MaxLength = int.Parse(unNoeud.SelectSingleNode("maxCharacters").InnerText);

            if (maTextBox.MaxLength > 0)
            {
                int numLines = (maTextBox.MaxLength / CARACTERES_PAR_LIGNE) + 1;
                if (numLines == 1)
                    maTextBox.Multiline = false;
                else
                {
                    maTextBox.Multiline = true;
                    maTextBox.Height = Math.Min(numLines, 4) * HAUTEUR_PAR_LIGNE;
                    maTextBox.ScrollBars = numLines >= 4 ? ScrollBars.Vertical : ScrollBars.None;
                }
            }

            Label monLabel = new Label();
            monLabel.Name = maTextBox.Name + "Label";
            if (unNoeud.SelectSingleNode("text") != null)
                monLabel.Text = unNoeud.SelectSingleNode("text").InnerText;
            monLabel.Width = LARGEUR_CONTROLES;

            monLabel.Location = unEmplacement;
            desControles.Add(monLabel);
            unEmplacement.Y += monLabel.Height;

            maTextBox.Location = unEmplacement;
            desControles.Add(maTextBox);
            unEmplacement.Y += maTextBox.Height + 10;

            return unEmplacement;
        }

        private Point AddComboBox(XmlNode unNoeud, Control.ControlCollection desControles, Point unEmplacement, string tag)
        {
            ComboBox maComboBox = new ComboBox();
            maComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            maComboBox.Width = LARGEUR_CONTROLES;

            if (unNoeud.Attributes["name"] != null)
                maComboBox.Name = unNoeud.Attributes["name"].Value;
            maComboBox.Tag = tag;

            foreach (XmlNode reponse in unNoeud.SelectNodes("reponses/reponse"))
            {
                maComboBox.Items.Add(reponse.InnerText);
                if (reponse.Attributes["default"]?.Value == "true")
                    maComboBox.SelectedItem = reponse.InnerText;
            }

            Label monLabel = new Label();
            monLabel.Name = maComboBox.Name + "Label";
            if (unNoeud.SelectSingleNode("text") != null)
                monLabel.Text = unNoeud.SelectSingleNode("text").InnerText;
            monLabel.Width = LARGEUR_CONTROLES;

            monLabel.Location = unEmplacement;
            desControles.Add(monLabel);
            unEmplacement.Y += monLabel.Height;

            maComboBox.Location = unEmplacement;
            desControles.Add(maComboBox);
            unEmplacement.Y += maComboBox.Height + 10;

            return unEmplacement;
        }

        private Point AddListBox(XmlNode unNoeud, Control.ControlCollection desControles, Point unEmplacement, string tag)
        {
            ListBox maListBox = new ListBox();
            maListBox.SelectionMode = SelectionMode.MultiSimple;
            maListBox.Width = LARGEUR_CONTROLES;

            if (unNoeud.Attributes["name"] != null)
                maListBox.Name = unNoeud.Attributes["name"].Value;
            maListBox.Tag = tag;

            int index = 0;
            foreach (XmlNode reponse in unNoeud.SelectNodes("reponses/reponse"))
            {
                maListBox.Items.Add(reponse.InnerText);
                if (reponse.Attributes["default"]?.Value == "true")
                    maListBox.SetSelected(index, true);
                index++;
            }

            Label monLabel = new Label();
            monLabel.Name = maListBox.Name + "Label";
            if (unNoeud.SelectSingleNode("text") != null)
                monLabel.Text = unNoeud.SelectSingleNode("text").InnerText;
            monLabel.Width = LARGEUR_CONTROLES;

            monLabel.Location = unEmplacement;
            desControles.Add(monLabel);
            unEmplacement.Y += monLabel.Height;

            maListBox.Location = unEmplacement;
            int nbItems = maListBox.Items.Count;
            int visibleItems = Math.Min(nbItems, 5);
            maListBox.Height = visibleItems * HAUTEUR_PAR_LIGNE;

            desControles.Add(maListBox);
            unEmplacement.Y += maListBox.Height + 10;

            return unEmplacement;
        }

    }

}
