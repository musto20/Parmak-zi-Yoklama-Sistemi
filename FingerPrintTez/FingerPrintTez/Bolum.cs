using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace FingerPrintTez
{
    public partial class Bolum : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public Bolum()
        {
            InitializeComponent();
            con.Open();
            SqlCommand com = new SqlCommand("SELECT BolumAdı FROM BolumTBL", con);
            SqlDataReader dr;
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["BolumAdı"]);
            }
            con.Close();

        }

        private void profilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdmProfil form2sec = new AdmProfil();
            form2sec.Show();
            this.Hide();
        }
        private void öğrenciEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OgrKayit form2sec = new OgrKayit();
            form2sec.Show();
            this.Hide();
        }

        private void öğretmenEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EgitmenEkle form2sec = new EgitmenEkle();
            form2sec.Show();
            this.Hide();
        }

        private void parmakIziEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFP form2sec = new AddFP();
            form2sec.Show();
            this.Hide();
        }

        private void dersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DersEkle form2sec = new DersEkle();
            form2sec.Show();
            this.Hide();
        }

        private void kAPATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text!="")
                {
                    con.Open();
                    string kayit = "INSERT INTO BolumTBL (BolumAdı) VALUES (@bolum)";
                    SqlCommand komut = new SqlCommand(kayit, con);
                    komut.Parameters.AddWithValue("@bolum", textBox1.Text);
                    komut.ExecuteNonQuery();
                    comboBox1.Items.Clear();
                    textBox1.Clear();
                    SqlCommand com = new SqlCommand("SELECT BolumAdı FROM BolumTBL", con);
                    SqlDataReader dr;
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        comboBox1.Items.Add(dr["BolumAdı"]);
                    }
                    con.Close();
                    MessageBox.Show("Ekleme Başarılı");
                }
                else
                {
                    MessageBox.Show("Bölüm İsmi Giriniz!!");
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Ekleme Hatalı!!");
                throw;
            }
            

        }
    }
}
