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
using System.IO.Ports;
using System.IO;

namespace FingerPrintTez
{
    public partial class OgrKayit : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public OgrKayit()
        {
            InitializeComponent();
            con.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM BolumTBL", con);
            DataTable tbl = new DataTable();
            tbl.Load(com.ExecuteReader());
            con.Close();
            comboBox1.DataSource = tbl;
            comboBox1.DisplayMember = "BolumAdı";
            comboBox1.ValueMember = "BolumId";
        }
        string resimPath;
        private void ResimEkle_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Resim Aç";

            openFileDialog1.Filter = "Jpeg Dosyası (*.jpg)|*.jpg|Gif Dosyası (*.gif)|*.gif|Png Dosyası (*.png)|*.png|Tif Dosyası (*.tif)|*.tif";

            if (openFileDialog1.ShowDialog() == DialogResult.OK) { 
                pictureBox1.Image = ResizeClass.Resize(Image.FromFile(openFileDialog1.FileName),200,200);
                
                resimPath = openFileDialog1.FileName.ToString();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text==""|| textBox2.Text==""|| textBox3.Text==""||comboBox1.SelectedText==null||pictureBox1.Image==null)
            {
                MessageBox.Show("Boşlukları Doldurun!!");
            }
            else
            {
                byte[] images = null;
                FileStream streem = new FileStream(resimPath, FileMode.Open, FileAccess.Read);
                BinaryReader brs = new BinaryReader(streem);
                images = brs.ReadBytes((int)streem.Length);

                con.Open();
                string kayit = "INSERT INTO OgrTBL (OgrName,OgrSurname,OgrNum,ResimId,BolumId) VALUES (@name,@surname,@ogrnum,@resim,@bolum)";
                SqlCommand komut = new SqlCommand(kayit, con);
                komut.Parameters.AddWithValue("@name", textBox1.Text);
                komut.Parameters.AddWithValue("@surname", textBox2.Text);
                komut.Parameters.AddWithValue("@ogrnum", textBox3.Text);
                komut.Parameters.AddWithValue("@resim", images);
                komut.Parameters.AddWithValue("@bolum", comboBox1.SelectedValue);
                komut.ExecuteNonQuery();
                //seriPort.Write(textBox1.Text);
                con.Close();
                foreach (Control c in Controls)
                {
                    if (c is TextBox)
                    {
                        c.Text = "";
                    }
                }
            }
            
        }

        private void bolumEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bolum form2sec = new Bolum();
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

        private void kAPATToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void profilToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AdmProfil form2sec = new AdmProfil();
            form2sec.Show();
            this.Hide();
        }

        private void öğrenciEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
