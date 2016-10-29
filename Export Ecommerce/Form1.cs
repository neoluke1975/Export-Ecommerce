using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace Export_Ecommerce
{
    public partial class FormAvvio : Form
    {
        string disponibilita;
        private MySqlDataAdapter adatta_query;
        double ricarico = Export_Ecommerce.Properties.Settings.Default.ricarico;

        int contatore_ricarico = 0;
        int contatore_bancadati = 0;


        public FormAvvio()
        {
            InitializeComponent();
        }

        private void btnRichiediDati_Click(object sender, EventArgs e)
        {
            MySqlConnection connesione_mysql = new MySqlConnection("Server=localhost;Database=bancadati;Uid=root;Pwd=phs2012;");
            MySqlCommand query_mysql_command = new MySqlCommand("select bfmagazzino.Descrizione," +
                                                              "anagraficabancadati.kean," +
                                                              "anagraficabancadati.tabiva," +
                                                              "anagraficabancadati.MOTIVOINVENDIBILITA," +
                                                              "anagraficabancadati.TABDEGRASSI," +
                                                              "tabelladegrassi.Descrizione," +
                                                              "anagraficagmp.Descrizione," +
                                                              "anagraficabancadati.PrezzoEuro1," +
                                                              "bfMagazzino.PrezzoDiscrezionale," +
                                                              "bf2.Giacenza," +
                                                              "bfMagazzino.PrezzoDiscrezionalePrevalente," +
                                                              "anagraficaditte.ragionesociale," +
                                                              "bfmagazzino.CodiceProdotto," +
                                                              "listini.PzoEuro," +
                                                              "bfmagazzino.CostoMedio, " +
                                                              "'disponibilita'='0' " +
                                                              "FROM bf2000.bfmagazzino " +
                                                              "INNER JOIN bancadati.anagraficabancadati " +
                                                              "ON(anagraficabancadati.km10 = bfmagazzino.CodiceProdotto) " +
                                                              "INNER JOIN bancadati.tabelladegrassi " +
                                                              "ON(anagraficabancadati.TABDEGRASSI = tabelladegrassi.Codice) " +
                                                              "INNER JOIN bancadati.anagraficaditte " +
                                                              "ON(anagraficabancadati.Kdit1 = anagraficaditte.codice) " +
                                                              "INNER JOIN bancadati.anagraficagmp " +
                                                              "ON(anagraficagmp.codice = anagraficabancadati.Kgm) " +
                                                              "INNER JOIN(select CodiceProdotto, SUM(GiacenzaAttuale) as giacenza from bf2000.bfmagazzino2 GROUP BY CodiceProdotto) bf2 " +
                                                              "ON bfmagazzino.CodiceProdotto = bf2.CodiceProdotto " +
                                                              "INNER JOIN(select CodiceProdotto, MAX(prezzoEuro) pzoEuro from bf2000.tabellalistini GROUP BY CodiceProdotto) listini " +
                                                              "ON bfmagazzino.CodiceProdotto = listini.CodiceProdotto " +
                                                              "WHERE bancadati.anagraficabancadati.TABDEGRASSI <> '1151' order by bfmagazzino.descrizione limit 10", connesione_mysql);
            adatta_query = new MySqlDataAdapter(query_mysql_command);
            try
            {
                connesione_mysql.Open();
            }
            catch (Exception)
            {

                MessageBox.Show("problemi di connesione");
            }
            try
            {
                adatta_query.Fill(query_mysql, "tabella_query_mysql");

                GridViewQuery.DataSource = query_mysql;
                GridViewQuery.DataMember = "tabella_query_mysql";
                GridViewQuery.AutoResizeColumns();
            }
            catch (Exception)
            {

                MessageBox.Show("problema query");
            }

            connesione_mysql.Close();


        }



        private void btnEsci_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFarmaclick_Click(object sender, EventArgs e)
        {
            //DataColumn disponibilita = new DataColumn();
            //disponibilita.DataType = System.Type.GetType("System.Int16");
            //disponibilita.AllowDBNull = false;
            //disponibilita.Caption = "disponibilita";
            //disponibilita.ColumnName = "disponibilita";
            //disponibilita.DefaultValue = 0;
            //query_mysql.Tables[0].Columns.Add(disponibilita);

            try
            {


                GridViewQuery.DataSource = query_mysql;
                GridViewQuery.DataMember = "tabella_query_mysql";
                GridViewQuery.AutoResizeColumns();
            }
            catch (Exception)
            {

                MessageBox.Show("problema query");
            }
            foreach (DataRow row in query_mysql.Tables[0].Rows)
            {

                object item = row[12];

                fck2010info.InfoComInputBean inbean = new fck2010info.InfoComInputBean();
                fck2010info.ArticoloInputBean[] articoli = new fck2010info.ArticoloInputBean[1];


                inbean.IDSessione = Export_Ecommerce.Properties.Settings.Default.IdSessione;
                inbean.codiceFornitore = Export_Ecommerce.Properties.Settings.Default.fornitore;
                inbean.descrizioneArticoli = false;
                inbean.descrizioneMotivazioneMancanza = false;
                inbean.riferimentoOrdineFarmacia = "12345678";

                articoli[0] = new fck2010info.ArticoloInputBean();
                articoli[0].codiceProdotto = item.ToString();
                articoli[0].quantitaRichiesta = 10;

                inbean.arrayArticoliInput = articoli;

                fck2010info.farmaclick2010001Service ws = new fck2010info.farmaclick2010001Service();
                ws.Url = Export_Ecommerce.Properties.Settings.Default.servizi;
                fck2010info.InfoComOutputBean output = ws.FCKInfoCom(inbean);

                row[15] = (output.arrayArticoli[0].quantitaConsegnata.ToString());
                //MessageBox.Show(output.esitoServizio.ToString());

            }
            GridViewQuery.BindingContext[query_mysql].EndCurrentEdit();
            query_mysql.AcceptChanges();
            adatta_query.Update(query_mysql, "tabella_query_mysql");
            GridViewQuery.DataSource = null;
            try
            {
                GridViewQuery.DataSource = query_mysql;
                GridViewQuery.DataMember = "tabella_query_mysql";
                GridViewQuery.AutoResizeColumns(); 

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnParametriFck_Click(object sender, EventArgs e)
        {
            Parametri apriform = new Parametri();
            apriform.Show();
        }

        private void btnGeneraFile_Click(object sender, EventArgs e)
        {
         
            using (StreamWriter scriviFile = new StreamWriter("c:/file_dpsonline/estratto.csv"))
            {

                string testa_descrizione = "PRODOTTO";
                string testa_ean = "EAN";
                string testa_minsan = "MINSAN";
                string testa_prezzo_Bancadati = "PREZZO BDF";
                string testa_prezzo = "PREZZO WEB";
                string testa_costo = "COSTO";
                string testa_giac = "GIAC";
                string testa_codice_degrassi = "CODICE DEGRASSI";
                string testa_degrassi = "DEGRASSI";
                string testa_iva = "IVA";
                string testa_revoca = "REVOCA";
                string testa_gmp = "GMP";
                string testa_ditta = "DITTA";
                string testa_disp = "DISPONIBILITA";



                scriviFile.WriteLine("{0,-41};{1,-13};{2,-13};{3,-10};{4,-10};{5,-8};{6,-4};{7,-15};{8,-41};{9,-3};{10,-6};{11,-70};{12,-50};{13,30};", testa_descrizione, testa_ean, testa_minsan, testa_prezzo_Bancadati, testa_prezzo, testa_costo,testa_giac, testa_codice_degrassi, testa_degrassi, testa_iva, testa_revoca, testa_gmp, testa_ditta,testa_disp);
                foreach (DataRow r in query_mysql.Tables[0].Rows)
                {
                    {

                        string descrizione = r[0].ToString();
                        string ean = r[1].ToString();
                        string minsan = r[12].ToString();
                        string iva = r[2].ToString();
                        if (iva == "20")
                        {
                            iva = "22";
                        }
                        if (iva == "21")
                        {
                            iva = "22";
                        }
                        var costo = double.Parse(r[13].ToString());
                        var costo_medio = double.Parse(r[14].ToString());
                        /*if (costo_medio > 0)
                        {
                            costo = costo_medio;
                        }*/
                        var prezzo_calcolato = (costo * ((ricarico / 100) + 1.00)) * (((double.Parse(iva)) / 100) + 1.00);
                        var prezzo = double.Parse(r[7].ToString());
                        if (prezzo_calcolato < prezzo)
                        {
                            contatore_ricarico += 1;
                        }
                        else if (prezzo == 0.00)
                        {
                            contatore_ricarico += 1;
                        }
                        else if (prezzo_calcolato > prezzo)
                        {
                            prezzo_calcolato = prezzo;
                            contatore_bancadati += 1;
                        }
                        else if (prezzo_calcolato == prezzo)
                        {
                            prezzo_calcolato = prezzo;
                            contatore_bancadati += 1;
                        }
                        string gmp = r[6].ToString();
                        var prezzoDiscrezionale = double.Parse(r[8].ToString());
                        string giacenza = r[9].ToString();
                        string prezzodiscrezionaleprevalente = r[10].ToString();
                        string ditta = r[11].ToString();
                        int disp = int.Parse(r[15].ToString());
                      
                        string codiceDegrassi = r[4].ToString();
                        string descrizioneDegrassi = r[5].ToString();
                        string revoca = r[3].ToString();
                        if (costo == 0.00)
                            if (prezzo_calcolato == 0.00)
                            {
                                continue;
                            }
                        if (disp==0)
                        {
                            disponibilita = "non disponibile";
                        }
                        if (disp>0)
                            if (disp<5)
                           
                        {
                            disponibilita = "scarsa disponibilità";
                        }
                        if (disp>=5)
                        {
                            disponibilita = "disponibile";
                        }

                        scriviFile.WriteLine("{0,-41};{1,13};{2,13};{3,10};{4,10};{5,8};{6,4};{7,15};{8,-41};{9,3};{10,6};{11,70};{12,50};{13,30};", descrizione, ean, minsan, prezzo.ToString("0.00"), prezzo_calcolato.ToString("0.00"), costo.ToString("0.00"),giacenza, codiceDegrassi, descrizioneDegrassi, iva, revoca, gmp, ditta, disponibilita);
                    }
                    



                }
                MessageBox.Show("prezzi ricalcolati: "+contatore_ricarico+" Prezzi Bancad dati: "+contatore_bancadati);
                scriviFile.Close();
            }


        }

       
    }
}

