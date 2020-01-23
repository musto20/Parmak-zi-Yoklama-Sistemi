using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;
using System.Data.SqlClient;
using System.Threading;

namespace FingerPrintTez
{
    public partial class Login : Form
    {
        public event System.IO.Ports.SerialErrorReceivedEventHandler ErrorReceived;
        public static string yetki, id, gg,bolum;
        public int sayac = 0;
        private string data;
        
        SqlConnection sc = new SqlConnection("Data Source=MZEYCAN\\SQLEXPRESS;Initial Catalog=Fingerprint;Integrated Security=True");
        public Login()
        {
            InitializeComponent();
        }
        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort2.IsOpen) serialPort2.Close();    //Seri port açıksa kapat
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
            try
            {
                sc.Open();
                string sql = "Select * From EgitmenTBL Where Nickname= @nick AND Password= @pass";
                string sql_2 = "Select Egitmenid,Yetki,Bolum From EgitmenTBL Where Nickname ='" + textBox1.Text + "'";

                SqlParameter prm1 = new SqlParameter("nick", textBox1.Text);
                SqlParameter prm2 = new SqlParameter("pass", textBox2.Text);

                SqlCommand komut = new SqlCommand(sql, sc);
                SqlCommand komut_2 = new SqlCommand(sql_2, sc);

                komut.Parameters.Add(prm1);
                komut.Parameters.Add(prm2);
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(komut);
                ad.Fill(dt);

                SqlDataReader dr = komut_2.ExecuteReader();
                while (dr.Read())
                {
                    id =dr["Egitmenid"].ToString();
                    yetki = dr["Yetki"].ToString();
                    bolum= dr["Bolum"].ToString();

                }
                if (dt.Rows.Count > 0)
                {
                    
                    if (yetki == "Egitmen")
                    {
                        serialPort2.Close();
                        EgtProfil frm = new EgtProfil();
                        frm.id = id;
                        frm.Show();
                        this.Hide();

                    }
                    else if (yetki == "Admin")
                    {
                        serialPort2.Close();
                        AdmProfil frm1 = new AdmProfil();
                        frm1.Show();
                        this.Hide();

                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı adını ve şifrenizi kontrol ediniz.");
                }
                //Değişkeni gönderecek olan formda public değişkene gerek yoktur.
                //Normal bir değişken gönderebiliriz.
                sc.Close();
            }

            catch (Exception)
            {
                MessageBox.Show("Hatalı Giriş");
                throw;
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort2.Close();
            button3.Enabled = false;     //Durdurma butonunu pasif hale getir
            button2.Enabled = true;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            timer1.Interval = 5000;
            string[] ports = SerialPort.GetPortNames();  //Seri portları diziye ekleme
            foreach (string port in ports)
                comboBox1.Items.Add(port);

            serialPort2.DataReceived += new SerialDataReceivedEventHandler(SerialPort2_DataReceived);
        }
        private void SerialPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
                data = serialPort2.ReadLine();                      //Veriyi al
                this.Invoke(new EventHandler(displayData_event));
           

        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort2.IsOpen) serialPort2.Close();    //Seri port açıksa kapat
        }
        private void displayData_event(object sender, EventArgs e)
        {
            button3.Enabled = true;     //Durdurma butonunu pasif hale getir
            button2.Enabled = false;
            char ayrac = '\r'; //char türünde her hangi bir sembole göre ayrılabilir
            string[] deger = data.Split(ayrac);
            if (deger[0]== "Parmak Izi Bulunamadi")
            {
              
            }
            else
            {
                try
                {

                    sc.Open();
                    string sql = "Select * From EgitmenTBL Where PId=@Pid";
                    string sql_2 = "Select Egitmenid,Yetki From EgitmenTBL Where PId ='" + deger[0] + "'";

                    SqlParameter prm1 = new SqlParameter("Pid", deger[0]);

                    SqlCommand komut = new SqlCommand(sql, sc);
                    SqlCommand komut_2 = new SqlCommand(sql_2, sc);

                    komut.Parameters.Add(prm1);
                    DataTable dt = new DataTable();
                    SqlDataAdapter ad = new SqlDataAdapter(komut);
                    ad.Fill(dt);

                    SqlDataReader dr = komut_2.ExecuteReader();
                    while (dr.Read())
                    {
                        id = dr["Egitmenid"].ToString();
                        yetki = dr["Yetki"].ToString();

                    }
                    if (sayac==0)
                    {
                        if (dt.Rows.Count > 0)
                        {

                            if (yetki == "Admin")
                            {
                                try
                                {
                                    AdmProfil frm = new AdmProfil();
                                    frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                                    Task.Run(() =>
                                    {
                                        serialPort2.Close();
                                    }
                                    );
                                    frm.Show();

                                    Hide();

                                    sayac++;
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                               

                            }
                            else if (yetki == "Egitmen")
                            {

                                EgtProfil frm1 = new EgtProfil();
                                frm1.FormClosed += new FormClosedEventHandler(frm1_FormClosed);
                                Task.Run(() =>
                                {
                                    serialPort2.Close();
                                }
                                );
                                frm1.Show();

                                Hide();

                                sayac++;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Kullanici Bulunamadi");
                        }
                        //Değişkeni gönderecek olan formda public değişkene gerek yoktur.
                        //Normal bir değişken gönderebiliriz.
                        sc.Close();
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Hatalı Giriş");
                }
            }
        }
        void frm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
        void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
        private void CloseSerialOnExit()

        {

            try

            {

                serialPort2.Close(); //close the serial port

            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.Message); //catch any serial port closing error messages

            }

            this.Invoke(new EventHandler(NowClose)); //now close back in the main thread

        }

        private void NowClose(object sender, EventArgs e)

        {

            this.Close(); //now close the form

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort2.PortName = comboBox1.Text;  
                serialPort2.BaudRate = 9600;            
                serialPort2.Open();                     
                button3.Enabled = true;                  
                button2.Enabled = false;
                serialPort2.Write("2");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");    
            }
        }
    }
}
