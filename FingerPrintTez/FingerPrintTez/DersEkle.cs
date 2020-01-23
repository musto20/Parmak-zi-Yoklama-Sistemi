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
    public partial class DersEkle : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public DersEkle()
        {
            InitializeComponent();
        }

        private void DersEkle_Load(object sender, EventArgs e)
        {
            con.Open();
            comboBox1.Items.Add("");
            SqlDataAdapter com = new SqlDataAdapter("SELECT * FROM BolumTBL", con);
            DataTable tbl = new DataTable();
            com.Fill(tbl);
            con.Close();
            this.comboBox1.SelectedIndexChanged -= new EventHandler(comboBox1_SelectedIndexChanged);

            comboBox1.DisplayMember = "BolumAdı";
            comboBox1.ValueMember = "BolumId";
            comboBox1.DataSource = tbl;
            comboBox1.SelectedIndex = -1;

            this.comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
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

        private void bolumEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bolum form2sec = new Bolum();
            form2sec.Show();
            this.Hide();
        }

        private void kAPATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != "0")
            {
                comboBox2.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                textBox2.Visible = true;
                con.Open();
                SqlDataAdapter com = new SqlDataAdapter("SELECT Egitmenid,(Name +' '+ Surname) AS Isim FROM EgitmenTBL where Bolum='" + comboBox1.SelectedValue + "'", con);
                DataTable tbl = new DataTable();
                com.Fill(tbl);
                con.Close();
                comboBox2.DisplayMember = "Isim";
                comboBox2.ValueMember = "Egitmenid";
                comboBox2.SelectedIndex = -1;
                comboBox2.DataSource = tbl;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Boşlukları Doldurunuz!!");

                }
                else
                {
                    con.Open();
                    string kayit = "INSERT INTO DersTBL (Dersadi,EgtId) VALUES (@Ders,@Id)";
                    SqlCommand komut = new SqlCommand(kayit, con);
                    komut.Parameters.AddWithValue("@Ders", textBox2.Text);
                    komut.Parameters.AddWithValue("@Id", comboBox2.SelectedValue);
                    komut.ExecuteNonQuery();
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
            catch (Exception)
            {

                MessageBox.Show("Hata");
                throw;
            }
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
