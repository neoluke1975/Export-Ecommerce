namespace Export_Ecommerce
{
    partial class FormAvvio
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.query_mysql = new System.Data.DataSet();
            this.GridViewQuery = new System.Windows.Forms.DataGridView();
            this.btnRichiediDati = new System.Windows.Forms.Button();
            this.btnEsci = new System.Windows.Forms.Button();
            this.btnFarmaclick = new System.Windows.Forms.Button();
            this.btnParametriFck = new System.Windows.Forms.Button();
            this.btnGeneraFile = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.query_mysql)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // query_mysql
            // 
            this.query_mysql.DataSetName = "NewDataSet";
            // 
            // GridViewQuery
            // 
            this.GridViewQuery.AllowUserToAddRows = false;
            this.GridViewQuery.AllowUserToOrderColumns = true;
            this.GridViewQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridViewQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewQuery.Location = new System.Drawing.Point(156, 110);
            this.GridViewQuery.Name = "GridViewQuery";
            this.GridViewQuery.Size = new System.Drawing.Size(696, 480);
            this.GridViewQuery.TabIndex = 0;
            // 
            // btnRichiediDati
            // 
            this.btnRichiediDati.Location = new System.Drawing.Point(21, 110);
            this.btnRichiediDati.Name = "btnRichiediDati";
            this.btnRichiediDati.Size = new System.Drawing.Size(116, 69);
            this.btnRichiediDati.TabIndex = 1;
            this.btnRichiediDati.Text = "Richiedi Dati";
            this.btnRichiediDati.UseVisualStyleBackColor = true;
            this.btnRichiediDati.Click += new System.EventHandler(this.btnRichiediDati_Click);
            // 
            // btnEsci
            // 
            this.btnEsci.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEsci.Location = new System.Drawing.Point(880, 648);
            this.btnEsci.Name = "btnEsci";
            this.btnEsci.Size = new System.Drawing.Size(116, 69);
            this.btnEsci.TabIndex = 2;
            this.btnEsci.Text = "Esci";
            this.btnEsci.UseVisualStyleBackColor = true;
            this.btnEsci.Click += new System.EventHandler(this.btnEsci_Click);
            // 
            // btnFarmaclick
            // 
            this.btnFarmaclick.Location = new System.Drawing.Point(21, 185);
            this.btnFarmaclick.Name = "btnFarmaclick";
            this.btnFarmaclick.Size = new System.Drawing.Size(116, 69);
            this.btnFarmaclick.TabIndex = 3;
            this.btnFarmaclick.Text = "Farmaclick";
            this.btnFarmaclick.UseVisualStyleBackColor = true;
            this.btnFarmaclick.Click += new System.EventHandler(this.btnFarmaclick_Click);
            // 
            // btnParametriFck
            // 
            this.btnParametriFck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParametriFck.Location = new System.Drawing.Point(880, 110);
            this.btnParametriFck.Name = "btnParametriFck";
            this.btnParametriFck.Size = new System.Drawing.Size(116, 69);
            this.btnParametriFck.TabIndex = 4;
            this.btnParametriFck.Text = "Parametri Farmaclick";
            this.btnParametriFck.UseVisualStyleBackColor = true;
            this.btnParametriFck.Click += new System.EventHandler(this.btnParametriFck_Click);
            // 
            // btnGeneraFile
            // 
            this.btnGeneraFile.Location = new System.Drawing.Point(21, 260);
            this.btnGeneraFile.Name = "btnGeneraFile";
            this.btnGeneraFile.Size = new System.Drawing.Size(116, 69);
            this.btnGeneraFile.TabIndex = 5;
            this.btnGeneraFile.Text = "Genera File";
            this.btnGeneraFile.UseVisualStyleBackColor = true;
            this.btnGeneraFile.Click += new System.EventHandler(this.btnGeneraFile_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 335);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 69);
            this.button1.TabIndex = 6;
            this.button1.Text = "Invio File";
            this.button1.UseVisualStyleBackColor = true;
           
            // 
            // FormAvvio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGeneraFile);
            this.Controls.Add(this.btnParametriFck);
            this.Controls.Add(this.btnFarmaclick);
            this.Controls.Add(this.btnEsci);
            this.Controls.Add(this.btnRichiediDati);
            this.Controls.Add(this.GridViewQuery);
            this.Name = "FormAvvio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export Ecommerce";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.query_mysql)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewQuery)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Data.DataSet query_mysql;
        private System.Windows.Forms.DataGridView GridViewQuery;
        private System.Windows.Forms.Button btnRichiediDati;
        private System.Windows.Forms.Button btnEsci;
        private System.Windows.Forms.Button btnFarmaclick;
        private System.Windows.Forms.Button btnParametriFck;
        private System.Windows.Forms.Button btnGeneraFile;
        private System.Windows.Forms.Button button1;
    }
}

