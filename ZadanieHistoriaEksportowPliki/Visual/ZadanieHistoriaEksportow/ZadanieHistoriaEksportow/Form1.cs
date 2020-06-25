using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZadanieHistoriaEksportow
{
    public partial class Form1 : Form
    {
        private DostepDane dane;
        private ObslugaBlad oB;
        private System.Windows.Forms.Timer t;

        public Form1()
        {
            InitializeComponent();

            dane = new DostepDane();
            oB = new ObslugaBlad();

            inicjalizacjaLokale();
            inicjalizacjaTimer();

            startTimer();

        }

        private void inicjalizacjaTimer()
        {
            t = new System.Windows.Forms.Timer();
            t.Interval = 300000;
            t.Tick += odswiezLokale;
        }

        private void startTimer()
        {
            if (t != null)
                t.Start();
        }

        private void stopTimer()
        {
            if (t != null)
                t.Stop();
        }

        private void odswiezLokale(object sender, EventArgs e)
        {
            lokale.Items.Clear();
            inicjalizacjaLokale();
        }

        private void inicjalizacjaLokale()
        {
            string[] tablicaL = dane.DajLokale();
            lokale.Items.AddRange(tablicaL);

            if (lokale.Items.Count == 0)
                lokale.Items.AddRange(new string[] { "biuro", "dom" });
        }

        private void wyczyscHistorieEksportow()
        {
            historiaEksportow.Rows.Clear();
        }

        private void dodajeListeEksportow(List<Eksport> listaE)
        {
            string data, godzina;
            foreach (Eksport e in listaE)
            {
                Przetwarzanie.ZamianaNaDateGodzine(data: out data, godzina: out godzina, e.DataEksportu);
                historiaEksportow.Rows.Add(e.NazwaEksportu, data, godzina, e.NazwaUzytkownika, e.NazwaLokalu);
            }
        }

        /*private void dodajeListeEksportow(List<Eksport> listaE)
        {
            EksportGrid eG;
            foreach (Eksport e in listaE)
            {
                eG = (EksportGrid)e;
                historiaEksportow.Rows.Add(eG.Nazwa, eG.Data, eG.Godzina, eG.Uzytkownik, eG.Lokal);
            }
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            stopTimer();

            if (lokale.SelectedItem == null)
            {
                oB.Komunikat = "Nie wybrano lokalu";
                blad.Text = oB.Komunikat;
                return;
            }

            oB.Komunikat = blad.Text = "";
            wyczyscHistorieEksportow();
            List<Eksport> listaE = dane.DajEksporty(lokale.SelectedItem.ToString(), czasOd: czasOd.Value.Date.ToString("yyy-MM-dd hh:mm"),
                czasDo: czasDo.Value.Date.ToString("yyy-MM-dd hh:mm"));
            dodajeListeEksportow(listaE);

            startTimer();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (t != null)
                t.Dispose();
        }

        private void lokale_MouseClick(object sender, EventArgs e)
        {
            odswiezLokale(null, null);
        }
    }
}
