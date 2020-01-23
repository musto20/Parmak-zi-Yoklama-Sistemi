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
using System.IO.Ports;

namespace FingerPrintTez
{
    public partial class AddFP : Form
    {
        public int OgrDurum, OgrtDurum;
        private string data,max;
        SqlConnection con = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public AddFP()
        {
            InitializeComponent();
            con.Open();
            comboBox1.Items.Add("");
            SqlDataAdapter com = new SqlDataAdapter("SELECT * FROM BolumTBL", con);
            DataTable tbl = new DataTable();
            com.Fill(tbl);
            con.Close();
            this.comboBox2.SelectedIndexChanged -= new EventHandler(comboBox2_SelectedIndexChanged);
            comboBox1.DisplayMember = "BolumAdı";
            comboBox1.ValueMember = "BolumId";
            comboBox1.DataSource = tbl;
            comboBox1.SelectedIndex = -1;
            this.comboBox2.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged);
        }

        private void Ogr_Btt_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                try
                {
                    OgrtDurum = 0;
                    OgrDurum = 1;
                    if (comboBox2.Visible ==false)
                    {
                        comboBox2.Visible = true;
                        label2.Visible = true;
                    }
                    this.comboBox2.SelectedIndexChanged -= new EventHandler(comboBox2_SelectedIndexChanged);
                    comboBox2.DataSource = null;
                    con.Open();

                    SqlDataAdapter com = new SqlDataAdapter("SELECT Ogrid,(OgrName +' '+OgrSurname) AS Isim FROM OgrTBL where BolumId='" + comboBox1.SelectedValue + "'and PId IS Null", con);
                    DataTable tbl = new DataTable();
                    com.Fill(tbl);
                    con.Close();
                    comboBox2.DisplayMember = "Isim";
                    comboBox2.ValueMember = "Ogrid";

                    comboBox2.DataSource = tbl;
                    comboBox2.SelectedIndex = -1;

                    this.comboBox2.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged);
                    
                    con.Close();
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            else
            {
                MessageBox.Show("Bölüm Seçiniz !!");
            }
        }

        private void Egt_Bttn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                this.comboBox2.SelectedIndexChanged -= new EventHandler(comboBox2_SelectedIndexChanged);
                comboBox2.DataSource=null;
                comboBox2.ResetText();
                OgrtDurum = 1;
                OgrDurum = 0;
                if (comboBox2.Visible == false)
                {
                    comboBox2.Visible = true;
                    label2.Visible = true;
                }
                
                comboBox2.DataSource = null;
                con.Open();
                SqlDataAdapter com = new SqlDataAdapter("SELECT Egitmenid,(Name +' '+ Surname) AS Isim FROM EgitmenTBL where Bolum='" + comboBox1.SelectedValue + "'and PId IS Null", con);
                DataTable tbl = new DataTable();
                com.Fill(tbl);
                con.Close();
                
                comboBox2.DisplayMember = "Isim";
                comboBox2.ValueMember = "Egitmenid";
                comboBox2.DataSource = tbl;
                comboBox2.SelectedIndex = -1;
                this.comboBox2.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged);
            }
            else
            {
                MessageBox.Show("Bölüm Seçiniz!!!");
            }
                
            
           
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OgrDurum==1)
            {
                SqlCommand com2 = new SqlCommand("SELECT ResimId FROM OgrTBL where Ogrid='" + comboBox2.SelectedValue + "'", con);
                con.Open();
                SqlDataReader drc = com2.ExecuteReader();
                drc.Read();

                if (drc.HasRows)
                {
                    byte[] image = drc[0] as byte[] ?? null;
                    if (image == null)
                    {
                        pictureBox1.Image = null;
                        MessageBox.Show("Resim Yok");

                    }
                    else
                    {
                        MemoryStream mstream = new MemoryStream(image);
                        pictureBox1.Image = Image.FromStream(mstream);
                    }
                    con.Close();
                }     
            }
            else
            {
                SqlCommand com2 = new SqlCommand("SELECT Resim FROM EgitmenTBL where Egitmenid='" + comboBox2.SelectedValue + "'", con);
                con.Open();
                SqlDataReader drc = com2.ExecuteReader();
                drc.Read();
                
                if (drc.HasRows)
                {
                    byte[] image = drc[0] as byte[] ?? null;
                    if (image == null)
                    {
                        pictureBox1.Image = null;
                        MessageBox.Show("Resim Yok");
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
                con.Close();
            }
                
         
        }

        private void dersEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DersEkle form2sec = new DersEkle();
            form2sec.Show();
            this.Hide();
        }

        private void bolumEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bolum form2sec = new Bolum();
            form2sec.Show();
            this.Hide();
        }

        private void öğretmenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EgitmenEkle form2sec = new EgitmenEkle();
            form2sec.Show();
            this.Hide();
        }

        private void öğrenciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OgrKayit form2sec = new OgrKayit();
            form2sec.Show();
            this.Hide();
        }

        private void profilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdmProfil form2sec = new AdmProfil();
            form2sec.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox3.Text;  //ComboBox1'de seçili nesneyi port ismine ata
                serialPort1.BaudRate = 9600;
                serialPort1.Close();//BaudRate 9600 olarak ayarla
                serialPort1.Open();
                button2.Visible = true;
                button2.Enabled = true;                  //Durdurma butonunu aktif hale getir
                button1.Enabled = false;
                button1.Visible = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");    //Hata mesajı göster
            }
            

        }

        private void AddFP_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();  //Seri portları diziye ekleme
            foreach (string port in ports)
                comboBox3.Items.Add(port);

            serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived);
        }
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data = serialPort1.ReadLine();                      //Veriyi al
            this.Invoke(new EventHandler(displayData_event));
        }
        private void displayData_event(object sender, EventArgs e)
        {

            if (data== "Parmaginizi Kaldirin\r")
            {
                MessageBox.Show("Parmağınız Kaldırın");
            }
            else if (data== "Parmak Izi Id\r")
            {
                serialPort1.Write(max);
            }
            else if ("Parmak izi eslesmedi\r"==data)
            {
                MessageBox.Show("Parmak izi eslesmedi");
            }
            else if(data== "Depolandi!\r")
            {
                if (OgrDurum==1)
                {
                    con.Open();
                    string kayit = "update OgrTBL set PId=@id where Ogrid='"+comboBox2.SelectedValue+"'";
                    SqlCommand komut = new SqlCommand(kayit, con);
                    komut.Parameters.AddWithValue("@id", max);
                    komut.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Parmak izi Eklendi");
                }
                else if (OgrtDurum==1)
                {

                    con.Open();
                    string kayit = "update EgitmenTBL set PId=@id where Egitmenid='" + comboBox2.SelectedValue + "'";
                    SqlCommand komut = new SqlCommand(kayit, con);
                    komut.Parameters.AddWithValue("@id", max);
                    komut.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Parmak izi Eklendi");
                }
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            con.Open();
            string sql_2 = "Select (Max(PId)+1) As maxx From (Select Ogrid, PId from OgrTBL Union All select Egitmenid,PId from EgitmenTBL)foo";
            SqlCommand komut_2 = new SqlCommand(sql_2, con);
            SqlDataReader dr = komut_2.ExecuteReader();
            while (dr.Read())
            {
                max = dr["maxx"].ToString();

            }
            con.Close();
            serialPort1.WriteLine("1");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            button1.Visible = true;
            button1.Enabled = true;                  //Durdurma butonunu aktif hale getir
            button2.Enabled = false;
            button2.Visible = false;
        }

        private void kAPATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
}
