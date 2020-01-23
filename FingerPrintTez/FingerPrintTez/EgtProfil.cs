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
    public partial class EgtProfil : Form
    {
        public string id = Login.id;
        SqlConnection sc = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public EgtProfil()
   {
            InitializeComponent();
            
        }

        private void kAPATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void yoklamalarıGörToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Devamsızlık frm = new Devamsızlık();
            frm.kullaniciid= id;
            frm.Show();
            this.Hide();
        }

        private void EgtProfil_Load(object sender, EventArgs e)
        {
            if (id != null)
            {
                sc.Open();
                string getirme = "select (Name+' '+Surname) As Isim from EgitmenTBL where Egitmenid = '" + id.ToString() + "'";

                SqlCommand komut = new SqlCommand(getirme, sc);

                SqlDataReader dr = komut.ExecuteReader();

                while (dr.Read())
                {
                    label2.Text = dr["Isim"].ToString();
                }

                dr.Close();
                SqlCommand com2 = new SqlCommand("SELECT Resim FROM EgitmenTBL where Egitmenid='" + id + "'", sc);
                SqlDataReader dr2 = com2.ExecuteReader();
                dr2.Read();

                if (dr2.HasRows)
                {
                    byte[] image = dr2[0] as byte[] ?? null;
                    if (image == null)
                    {
                        pictureBox1.Image = null;
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

        private void yoklamaAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yoklama frm = new Yoklama();
            frm.Show();
            this.Hide();
        }
    }
}
