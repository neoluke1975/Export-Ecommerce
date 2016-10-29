using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Export_Ecommerce
{
    public partial class Parametri : Form
    {
        public Parametri()
        {
            InitializeComponent();
        }

        private void btnEsc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSalva_Click(object sender, EventArgs e)
        {

            Export_Ecommerce.Properties.Settings.Default.farmaclickUrl = tbxFckLogin.Text;
            Export_Ecommerce.Properties.Settings.Default.UtenteFck = tbxUtenteFck.Text;
            Export_Ecommerce.Properties.Settings.Default.PwFck = tbxPwFck.Text;
            Export_Ecommerce.Properties.Settings.Default.IdSessione = lblIdSessione.Text;
            Export_Ecommerce.Properties.Settings.Default.fornitore = cbxFornitori.Text;
            Export_Ecommerce.Properties.Settings.Default.servizi = cbxServizi.Text;
            Export_Ecommerce.Properties.Settings.Default.ricarico = double.Parse(tbxRicarico.Text);
            Export_Ecommerce.Properties.Settings.Default.Save();
            if (tbxFckLogin.Text == "")
                if (tbxUtenteFck.Text == "")
                    if (tbxPwFck.Text == "")
                        if (cbxFornitori.SelectedItem.ToString() == "")
                            if (cbxServizi.SelectedItem.ToString() == "")
                                if (tbxRicarico.Text == "")
                                {
                                    MessageBox.Show("seleziona tutti i campi");
                                }           
        }

        private void Parametri_Load(object sender, EventArgs e)
        {
            tbxFckLogin.Text = Export_Ecommerce.Properties.Settings.Default.farmaclickUrl;
            tbxUtenteFck.Text = Export_Ecommerce.Properties.Settings.Default.UtenteFck;
            tbxPwFck.Text = Export_Ecommerce.Properties.Settings.Default.PwFck;
            lblIdSessione.Text = Export_Ecommerce.Properties.Settings.Default.IdSessione;
            cbxFornitori.Text=(Export_Ecommerce.Properties.Settings.Default.fornitore);
            cbxServizi.Text=(Export_Ecommerce.Properties.Settings.Default.servizi);
            tbxRicarico.Text = (Export_Ecommerce.Properties.Settings.Default.ricarico).ToString();
            
        }

        private void btnTestFck_Click(object sender, EventArgs e)
        {
         //eseguo un test su farmaclick per avere i dati corretti
            string descrizioneBreve;
            string codiceSitoLogistico;
            string codice;
            string esitoServizio;
            
            fck2010login.LoginInputBean login = new fck2010login.LoginInputBean();
            login.userName = tbxUtenteFck.Text;
            login.password = tbxPwFck.Text;
            login.nomeTerminale = "SERVER";

            fck2010login.LoginOutputBean output = new fck2010login.LoginOutputBean();

            fck2010login.farmaclick2010001Service webservice = new fck2010login.farmaclick2010001Service();
            webservice.Url = (tbxFckLogin.Text);

            output = webservice.FCKLogin(login);

            lblIdSessione.Text = output.arrayFornitori[1].IDSessione;
            descrizioneBreve = output.arrayFornitori[1].descrizioneBreve;
            codiceSitoLogistico = output.arrayFornitori[1].codiceSitoLogistico;
            for (int i = 0; i < output.arrayFornitori.Length; i++)
            {
                cbxFornitori.Items.Add(output.arrayFornitori[i].codice);
            }
            
            esitoServizio = output.esitoServizio.ToString();
            var servizi = output.arrayFornitori[1].arrayServizi;
            for (int i = 0; i < servizi.Length - 1; i++)
            {
                cbxServizi.Items.Add(servizi[i].url1);
                //url = servizi[19].url1;
            }
            if (esitoServizio=="0")
            {
                MessageBox.Show("Login Fck eseguita correttamente");
            }
            else
            {
                MessageBox.Show("comunicare all'assistenza il numero di errore =" + esitoServizio);
            };
        }
    }
}
