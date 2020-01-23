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
    public partial class Devamsızlık : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public string kullaniciid = Login.id;
        public Devamsızlık()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string deger = Login.id;
            con.Open();

            //SqlCommand cmd = new SqlCommand("Select OgrNum,(OgrName+' '+OgrSurname)AS Isim from OgrTBL where Ogrid= (SELECT Ogr_Id FROM YoklamaTBL JOIN OgrTBL on OgrTBL.Ogrid = YoklamaTBL.Ogr_Id join DersTBL on DersTBL.DersId = YoklamaTBL.DersId where YoklamaTBL.DersId = '" + comboBox1.SelectedValue+"' Group by Ogr_Id,YoklamaTBL.DersId having COUNT(*)<10)", con);
            SqlCommand cmd = new SqlCommand("select OgrNum,(OgrName+' '+OgrSurname)AS Isim from OgrTBL where Ogrid in(select Ogr_Id from YoklamaTBL where Ogr_Id in(select Ogrnci_id from KatalogTBL where Ders_id='" + comboBox1.SelectedValue + "')  Group By DersId,Ogr_Id having count(*)<10)", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            //Bir DataTable oluşturarak DataAdapter ile getirilen verileri tablo içerisine dolduruyoruz.
            dataGridView1.DataSource = dt;
            con.Close();



        }

        private void Devamsızlık_Load(object sender, EventArgs e)
        {
            con.Open();
            comboBox1.Items.Add("");
            SqlDataAdapter com = new SqlDataAdapter("SELECT DersId,DersAdi  FROM DersTBL where EgtId ='"+kullaniciid + "'", con);
            DataTable tbl = new DataTable();
            com.Fill(tbl);
            con.Close();
            comboBox1.DisplayMember = "DersAdi";
            comboBox1.ValueMember = "DersId";
            comboBox1.DataSource = tbl;
            comboBox1.SelectedIndex = -1;
        }

        private void kAPATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void profilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EgtProfil frm = new EgtProfil();
            frm.Show();
            this.Hide();
        }

        private void yoklamaAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yoklama frm = new Yoklama();
            frm.Show();
            this.Hide();
        }

        private void yoklamalarıGörToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
