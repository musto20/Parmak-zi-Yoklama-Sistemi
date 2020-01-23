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
    public partial class EgitmenEkle : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public EgitmenEkle()
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
            comboBox1.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || comboBox1.SelectedValue == null || pictureBox1.Image == null)
            {
                MessageBox.Show("Boşlukları Doldurun!!");
            }
            else
            {

                try
                {
                    byte[] images = null;
                    FileStream streem = new FileStream(resimPath, FileMode.Open, FileAccess.Read);
                    BinaryReader brs = new BinaryReader(streem);
                    images = brs.ReadBytes((int)streem.Length);
                    con.Open();
                    string kayit = "INSERT INTO EgitmenTBL (Name,Surname,Resim,Bolum,Nickname,Password,Yetki) VALUES (@name,@surname,@resim,@bolum,@nick,@pass,@yetki)";
                    SqlCommand komut = new SqlCommand(kayit, con);
                    komut.Parameters.AddWithValue("@name", textBox1.Text);
                    komut.Parameters.AddWithValue("@surname", textBox2.Text);
                    komut.Parameters.AddWithValue("@yetki", comboBox2.SelectedItem.ToString());
                    komut.Parameters.AddWithValue("@nick", textBox3.Text);
                    komut.Parameters.AddWithValue("@pass", textBox4.Text);
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
                    pictureBox1.Image = null;
                }
                catch (Exception)
                {
                    if (resimPath == null)
                    {
                        MessageBox.Show("Bir Resim Ekleyin !!");
                    }
                    else
                    {
                        MessageBox.Show("Nickname Hatalı Farklı bir Nick Yaziniz !!");
                        con.Close();
                    }

                }
            }
            
        }
        string resimPath;
        private void ResimEkle_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Resim Aç";

            openFileDialog1.Filter = "Jpeg Dosyası (*.jpg)|*.jpg|Gif Dosyası (*.gif)|*.gif|Png Dosyası (*.png)|*.png|Tif Dosyası (*.tif)|*.tif";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = ResizeClass.Resize(Image.FromFile(openFileDialog1.FileName), 200, 200);

                resimPath = openFileDialog1.FileName.ToString();
            }
        }


        private void kAPATToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void profilToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            AdmProfil form2sec = new AdmProfil();
            form2sec.Show();
            this.Hide();
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
    }
}
