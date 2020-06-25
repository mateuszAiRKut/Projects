namespace ZadanieHistoriaEksportow
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lokale = new System.Windows.Forms.ComboBox();
            this.czasOd = new System.Windows.Forms.DateTimePicker();
            this.czasDo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.historiaEksportow = new System.Windows.Forms.DataGridView();
            this.Nazwa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Godzina = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uzytkownik = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lokal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.blad = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.historiaEksportow)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lokal";
            // 
            // lokale
            // 
            this.lokale.FormattingEnabled = true;
            this.lokale.Location = new System.Drawing.Point(56, 36);
            this.lokale.Name = "lokale";
            this.lokale.Size = new System.Drawing.Size(121, 21);
            this.lokale.TabIndex = 1;
            this.lokale.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lokale_MouseClick);
            // 
            // czasOd
            // 
            this.czasOd.Location = new System.Drawing.Point(56, 103);
            this.czasOd.Name = "czasOd";
            this.czasOd.Size = new System.Drawing.Size(200, 20);
            this.czasOd.TabIndex = 2;
            // 
            // czasDo
            // 
            this.czasDo.Location = new System.Drawing.Point(56, 147);
            this.czasDo.Name = "czasDo";
            this.czasDo.Size = new System.Drawing.Size(200, 20);
            this.czasDo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Od";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Do";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 353);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Zatwierdź";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // historiaEksportow
            // 
            this.historiaEksportow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.historiaEksportow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nazwa,
            this.Data,
            this.Godzina,
            this.Uzytkownik,
            this.Lokal});
            this.historiaEksportow.Location = new System.Drawing.Point(294, 80);
            this.historiaEksportow.Name = "historiaEksportow";
            this.historiaEksportow.Size = new System.Drawing.Size(542, 296);
            this.historiaEksportow.TabIndex = 7;
            // 
            // Nazwa
            // 
            this.Nazwa.HeaderText = "Nazwa";
            this.Nazwa.Name = "Nazwa";
            // 
            // Data
            // 
            this.Data.HeaderText = "Data";
            this.Data.Name = "Data";
            // 
            // Godzina
            // 
            this.Godzina.HeaderText = "Godzina";
            this.Godzina.Name = "Godzina";
            // 
            // Uzytkownik
            // 
            this.Uzytkownik.HeaderText = "Uzytkownik";
            this.Uzytkownik.Name = "Uzytkownik";
            // 
            // Lokal
            // 
            this.Lokal.HeaderText = "Lokal";
            this.Lokal.Name = "Lokal";
            // 
            // blad
            // 
            this.blad.AutoSize = true;
            this.blad.ForeColor = System.Drawing.Color.Red;
            this.blad.Location = new System.Drawing.Point(53, 71);
            this.blad.Name = "blad";
            this.blad.Size = new System.Drawing.Size(0, 13);
            this.blad.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 415);
            this.Controls.Add(this.blad);
            this.Controls.Add(this.historiaEksportow);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.czasDo);
            this.Controls.Add(this.czasOd);
            this.Controls.Add(this.lokale);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "Form1";
            this.Text = "Raport";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.historiaEksportow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lokale;
        private System.Windows.Forms.DateTimePicker czasOd;
        private System.Windows.Forms.DateTimePicker czasDo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView historiaEksportow;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nazwa;
        private System.Windows.Forms.DataGridViewTextBoxColumn Data;
        private System.Windows.Forms.DataGridViewTextBoxColumn Godzina;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uzytkownik;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lokal;
        private System.Windows.Forms.Label blad;
    }
}

