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
    public partial class Yoklama : Form
    {
        private string data,id,value;
        SqlConnection con = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public string kullaniciid = Login.id;
        public string bolumid = Login.bolum;
        class ComboboxItem
        {

            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
        class ComboboxItem2
        {

            public string Text { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
        void AddValue()
        {
            for (int i = 1; i < 16; i++)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text =i+". Hafta";
                item.Value = i;
                comboBox2.Items.Add(item);
            }
        }
        public Yoklama()
        {
            InitializeComponent();
            AddValue();
        }

        private void Yoklama_Load(object sender, EventArgs e)
        {
            con.Open();
            comboBox1.Items.Add("");
            SqlDataAdapter com = new SqlDataAdapter("SELECT DersId,DersAdi  FROM DersTBL where EgtId ='" + kullaniciid + "'", con);
            DataTable tbl = new DataTable();
            com.Fill(tbl);
            con.Close();
            comboBox1.DisplayMember = "DersAdi";
            comboBox1.ValueMember = "DersId";
            comboBox1.DataSource = tbl;
            comboBox1.SelectedIndex = -1;
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived);
        }
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data = serialPort1.ReadLine();                      //Veriyi al
            this.Invoke(new EventHandler(displayData_event));
        }
        private void displayData_event(object sender, EventArgs e)
        {
            char ayrac = '\r'; //char türünde her hangi bir sembole göre ayrılabilir
            string[] deger = data.Split(ayrac);
            try
            {
                
                con.Open();
                string sql_2 = "Select Ogrid From OgrTBL Where PId ='" + deger[0] + "'";
                SqlCommand komut_2 = new SqlCommand(sql_2, con);
                SqlDataReader dr = komut_2.ExecuteReader();
                while (dr.Read())
                {
                    id = dr["Ogrid"].ToString();
                }
                con.Close();
                con.Open();
                string sql = "Select * From KatalogTBL Where Ogrnci_id= @id AND Ders_id= @ders";
                SqlParameter prm1 = new SqlParameter("id", id);
                SqlParameter prm2 = new SqlParameter("ders",comboBox1.SelectedValue);

                SqlCommand komut = new SqlCommand(sql, con);
                komut.Parameters.Add(prm1);
                komut.Parameters.Add(prm2);
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(komut);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    con.Close();
                    con.Open();
                    string sql3 = "Select * From YoklamaTBL Where Ogr_Id= @idx AND DersId= @dersy And Hafta=@hafta";
                    SqlParameter prm3 = new SqlParameter("idx", id);
                    SqlParameter prm4 = new SqlParameter("dersy", comboBox1.SelectedValue);
                    SqlParameter prm5 = new SqlParameter("hafta", value);
                    SqlCommand komut3 = new SqlCommand(sql3, con);
                    komut3.Parameters.Add(prm3);
                    komut3.Parameters.Add(prm4);
                    komut3.Parameters.Add(prm5);
                    DataTable dt2 = new DataTable();
                    SqlDataAdapter ad2 = new SqlDataAdapter(komut3);
                    ad2.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                    {
                        MessageBox.Show("Yoklama Alınmıs");
                    }
                    else
                    {
                        string kayit = "INSERT INTO YoklamaTBL (DersId,Ogr_Id,Hafta) VALUES (@derz,@idy,@haftax)";
                        SqlCommand komut4 = new SqlCommand(kayit, con);
                        komut4.Parameters.AddWithValue("@idy",id );
                        komut4.Parameters.AddWithValue("@derz",comboBox1.SelectedValue );
                        komut4.Parameters.AddWithValue("@haftax",value );
                        komut4.ExecuteNonQuery();
                        //seriPort.Write(textBox1.Text);
                        con.Close();
                        con.Open();
                        string com = "SELECT  Ogrid,(OgrName+' '+OgrSurname)As Isim from OgrTBL where Ogrid='" + id+"'";
                        SqlCommand komut5 = new SqlCommand(com, con);
                        SqlDataReader read = komut5.ExecuteReader();
                        //SqlDataAdapter da3 = new SqlDataAdapter(komut5);
                        DataTable dt3 = new DataTable();

                            read.Read();
                            listBox1.Items.Add(read["Isim"]);
                        con.Close();

                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı adını ve şifrenizi kontrol ediniz.");
                }
                con.Close();

            }

            catch (Exception)
            {
                MessageBox.Show("Hatalı Giriş");
                throw;
            }


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

                serialPort1.PortName = "COM6";  //ComboBox1'de seçili nesneyi port ismine ata
                serialPort1.BaudRate = 9600;            //BaudRate 9600 olarak ayarla
                serialPort1.Open();                     //Seri portu aç
                button2.Enabled = true;                  //Durdurma butonunu aktif hale getir
                button1.Enabled = false;                 //Başlatma butonunu pasif hale getir
                serialPort1.Write("3");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");    //Hata mesajı göster
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            serialPort1.Close();        //Seri Portu kapa
            button2.Enabled = false;     //Durdurma butonunu pasif hale getir
            button1.Enabled = true;      //Başlatma butonunu aktif hale getir

        }

        private void profilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EgtProfil frm = new EgtProfil();
            frm.Show();
            this.Hide();
        }

        private void yoklamaAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Devamsızlık frm = new Devamsızlık();
            frm.Show();
            this.Hide();
        }

        private void yoklamalarıGörToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Devamsızlık frm = new Devamsızlık();
            frm.Show();
            this.Hide();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            int selectedIndex = cmb.SelectedIndex;
            ComboboxItem selectedCar = (ComboboxItem)comboBox2.SelectedItem;
            int selecteVal = Convert.ToInt32(selectedCar.Value);
            value =Convert.ToString (selecteVal);    
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();    //Seri port açıksa kapat
        }
    }
}
