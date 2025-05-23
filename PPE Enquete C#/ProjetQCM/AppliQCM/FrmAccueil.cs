﻿using System;
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
            AfficherRecapitulatif();
            Form fenFille = this.ActiveMdiChild;
            if (fenFille != null)
                fenFille.Close();
        }

        private void mnuQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void mnuHorizontale_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void mnuVerticale_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void AfficherRecapitulatif()
        {
            if (this.ActiveMdiChild is FrmQuestionnaire fenQuestionnaire)
            {
                StringBuilder recap = new StringBuilder();

                foreach (Control ctrl in fenQuestionnaire.Controls)
                {
                    if (ctrl is Label label && label.Name.EndsWith("Label"))
                    {
                        // Retrouver le contrôle associé
                        string baseName = label.Name.Replace("Label", "");
                        Control reponseCtrl = fenQuestionnaire.Controls.Find(baseName, false).FirstOrDefault();

                        recap.AppendLine(label.Text);

                        if (reponseCtrl is TextBox tb)
                        {
                            recap.AppendLine("→ " + tb.Text);
                        }
                        else if (reponseCtrl is ComboBox cb)
                        {
                            recap.AppendLine("→ " + cb.SelectedItem?.ToString());
                        }
                        else if (reponseCtrl is ListBox lb)
                        {
                            foreach (var item in lb.SelectedItems)
                            {
                                recap.AppendLine("→ " + item.ToString());
                            }
                        }

                        recap.AppendLine(); // ligne vide pour séparer les questions
                    }
                }

                MessageBox.Show(recap.ToString(), "Récapitulatif des réponses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Aucun questionnaire actif.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void mnuFichier_Click(object sender, EventArgs e)
        {

        }

        private void validerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is FrmQuestionnaire fenetre)
            {
                fenetre.Valider();
            }
        }

        private void mnuApropos_Click(object sender, EventArgs e)
        {
            string message =
              "Projet GSB - Enquête\n" +
              "Version : 1.0\n" +
              "Développé par : Laghoueg Khaled\n" ;
            MessageBox.Show(message, "À propos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
