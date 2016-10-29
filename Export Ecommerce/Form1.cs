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
using System.Net;
using System.Threading;

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
        {   //stringa di connesione amysql
            MySqlConnection connesione_mysql = new MySqlConnection("Server=localhost;Database=bancadati;Uid=root;Pwd=phs2012;");
            //query per la richiesta dati
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
            //adapter per passaggio dati al dataset
            adatta_query = new MySqlDataAdapter(query_mysql_command);
          //prova la connesione
            try
            {
                connesione_mysql.Open();
            }
            catch (Exception)
            {

                MessageBox.Show("problemi di connesione");
            }
            //provo a mettere nel dataset i dati della query
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
            adatta_query.Dispose();
            connesione_mysql.Close();


        }



        private void btnEsci_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFarmaclick_Click(object sender, EventArgs e)
        {
            //ricarico i dati
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
            //per ogni riga nel dataset consulto il grossista per i dati
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
            //alla fine metto a posto i dati e li salvo nel dataset
            GridViewQuery.BindingContext[query_mysql].EndCurrentEdit();
            query_mysql.AcceptChanges();
            adatta_query.Update(query_mysql, "tabella_query_mysql");
            GridViewQuery.DataSource = null;
            //ricarico il dataset nella gridview
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
                //setto le intestazioni campi
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
                //ciclo il dataset per scrivere i dati nel file
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
                MessageBox.Show("Prezzi con Ricarico: "+contatore_ricarico+"\n"+ "Prezzi Banca Dati: "+contatore_bancadati);
                scriviFile.Close();
            }


        }

        private void btnFtp_Click(object sender, EventArgs e)
        {
            //spedisc tramite ftp
            string filename = "c:/file_dpsonline/estratto.csv";
            string ftpServerIP = "ftp.dpsonline.it";
            string ftpUserName = "farmaciapiaggio_gestionale";
            string ftpPassword = "P14gg10";

            FileInfo objFile = new FileInfo(filename);
            FtpWebRequest objFTPRequest;

            // Create FtpWebRequest object 
            objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + objFile.Name));

            // Set Credintials
            objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

            // By default KeepAlive is true, where the control connection is 
            // not closed after a command is executed.
            objFTPRequest.KeepAlive = false;

            // Set the data transfer type.
            objFTPRequest.UseBinary = true;

            // Set content length
            objFTPRequest.ContentLength = objFile.Length;

            // Set request method
            objFTPRequest.Method = WebRequestMethods.Ftp.UploadFile;

            // Set buffer size
            int intBufferLength = 16 * 1024;
            byte[] objBuffer = new byte[intBufferLength];

            // Opens a file to read
            FileStream objFileStream = objFile.OpenRead();

            try
            {
                // Get Stream of the file
                Stream objStream = objFTPRequest.GetRequestStream();

                int len = 0;

                while ((len = objFileStream.Read(objBuffer, 0, intBufferLength)) != 0)
                {
                    // Write file Content 
                    objStream.Write(objBuffer, 0, len);

                }

                objStream.Close();
                objFileStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("problemi con Invio File")
            }
        }
    }
}

