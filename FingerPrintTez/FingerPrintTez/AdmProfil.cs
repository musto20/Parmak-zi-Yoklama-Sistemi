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
using System.IO;

namespace FingerPrintTez
{
    public partial class AdmProfil : Form
    {
        public string id = Login.id;
        SqlConnection sc = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public AdmProfil()
        {
            InitializeComponent();
        }

        private void AdmProfil_Load(object sender, EventArgs e)
        {
            if (id != null)
            {
                sc.Open();
                string getirme = "select (Name+' '+Surname) As Isim from EgitmenTBL where Egitmenid = '" + id.ToString() + "'";

                SqlCommand komut = new SqlCommand(getirme, sc);

                SqlDataReader dr = komut.ExecuteReader();

                while (dr.Read())
                {
                    label1.Text = dr["Isim"].ToString();
                }

                dr.Close();
                SqlCommand com2 = new SqlCommand("SELECT Resim FROM EgitmenTBL where egitmenid='" + id + "'", sc);
                SqlDataReader dr2 = com2.ExecuteReader();
                dr2.Read();

                if (dr2.HasRows)
                {
                    byte[] image = dr2[0] as byte[] ?? null;
                    if (image == null)
                    {
                        pictureBox1.Image = null;
                        MessageBox.Show("Resim Yok!!");
                    }
                    else
                    {
                        MemoryStream mstream = new MemoryStream(image);
                        pictureBox1.Image = Image.FromStream(mstream);
                    }
                }
                else
                {
                    MessageBox.Show("Resim Yok");
                }
                sc.Close();
            }
        }
        private void bolumEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bolum form2sec = new Bolum();
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

       
    }
}
