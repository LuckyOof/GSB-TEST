using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppliQCM
{
    public partial class FrmAccueil : Form
    {
        //---------------------------------------------
        // Propriétés : fenêtres pouvant être affichées
        //---------------------------------------------
        FrmQuestionnaire fenQuestionnaire;

        public FrmAccueil()
        {
            InitializeComponent();
        }

        private void mnuOuvrir_Click(object sender, EventArgs e)
        {
            try
            {
                // Paramétrage des propriétés de la boîte de dialogue
                openFileDialog1.FileName = "";
                openFileDialog1.InitialDirectory = "d:\\";
                openFileDialog1.Filter = "xml files (*.xml)|*.xml";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                // Ouverture et test du bouton cliqué. Si oui, récupérer le nom du fichier
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string documentXML = openFileDialog1.FileName;
                    // Création et affichage d'un objet de classe "questionnaire"
                    fenQuestionnaire = new FrmQuestionnaire(documentXML, this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Questionnaire", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuFermer_Click(object sender, EventArgs e)
        {
            // On recupère la fenêtre fille active
            Form fenFille = this.ActiveMdiChild;
            if (fenFille != null)
                fenFille.Close();
        }

        private void mnuQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
