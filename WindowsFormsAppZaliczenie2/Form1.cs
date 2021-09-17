using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsAppZaliczenie2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void updateDataGrid(OracleConnection conn)
        {
            string combotext = comboBox2.Text.ToString();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM " + combotext;
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet3.FORMAZATR' table. You can move, or remove it, as needed.
         //   this.fORMAZATRTableAdapter.Fill(this.dataSet3.FORMAZATR);
         //   Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(46, 51, 73);
            comboBox2.SelectedIndex = 0;
            string connectionString = "User Id=admin;Password=admin;Data Source=localhost:1521/xe";
            string queryString = "SELECT * FROM pracownik";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand command = new OracleCommand(queryString, connection);
                command.BindByName = true;
                OracleParameter op = new OracleParameter();
                connection.Open();
                OracleDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetInt32(0) + ", " + reader.GetString(1));
                        comboBox1.Items.Add(reader.GetString(1));
                    }
                }
                finally
                {
                    // always call Close when done reading.
                    reader.Close();
                }

                updateDataGrid(connection);
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           /* string[] slowa = comboBox1.Text.ToString().Split(' ');
            Console.WriteLine(comboBox1.Text.ToString());
            textBox1.Text = slowa[0];
            textBox2.Text = slowa[1];
            textBox3.Text = slowa[2];*/
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = "User Id=admin;Password=admin;Data Source=localhost:1521/xe";           
            string combotext = comboBox2.Text.ToString();
            Console.WriteLine(combotext);
            string queryString = "SELECT * FROM " + combotext;

            if (comboBox2.Text.ToString() == "zatrudnienie")
            {
                queryString = "select zatrudnienie.nr_umowy, zatrudnienie.od, zatrudnienie.do, zatrudnienie.wynagrodzenie, pracownik.imie, pracownik.drugie_imie, pracownik.nazwisko, pracownik.pesel, dzial.nazwa AS DZIAL, stanowisko.nazwa AS STANOWISKO,miejscepracy.ulica, formazatr.rodzaj_zatrudnienia from zatrudnienie JOIN pracownik on zatrudnienie.pracownik_id = pracownik.id JOIN dzial on zatrudnienie.dzial_id = dzial.id JOIN stanowisko on zatrudnienie.stanowisko_id = stanowisko.id JOIN miejscepracy on zatrudnienie.miejscepracy_id = miejscepracy.id JOIN formazatr on zatrudnienie.forma_zatr_id = formazatr.id ORDER BY pracownik.imie";
            }

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                
                comboBox1.Items.Clear();
                OracleCommand command = new OracleCommand(queryString, connection);
                command.BindByName = true;
                OracleParameter op = new OracleParameter();
                connection.Open();
                OracleDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        updateDataGrid(connection);
                        if (comboBox2.Text.ToString() == "pracownik")
                        {
                            Console.WriteLine(reader.GetInt32(0) + ", " + reader.GetString(1));
                            string dane = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4) + " " + reader.GetString(5);
                            comboBox1.Items.Add(dane);

                          /*  label3.Visible = true;
                            label4.Visible = true;
                            label5.Visible = true;

                            textBox1.Visible = true;
                            textBox2.Visible = true;
                            textBox3.Visible = true;

                            label3.Text = "Imie";
                            label4.Text = "Drugie imie";
                            label5.Text = "Nazwisko";
                          */
                            Console.WriteLine(comboBox1.Text.ToString());
                            

                        }

                        else if (comboBox2.Text.ToString() == "tramwaje")
                        {
                            Console.WriteLine(reader.GetInt32(0) + ", " + reader.GetString(1));
                            string dane = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4);
                            comboBox1.Items.Add(dane);
                        }

                        else if (comboBox2.Text.ToString() == "zatrudnienie")
                        {
                            string dane = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4) + " " + reader.GetString(5) + " " + reader.GetString(6) + " " + reader.GetString(7) + " " + reader.GetString(8) + " " + reader.GetString(9);
                            comboBox1.Items.Add(dane);
                        }




                        else
                            comboBox1.Items.Add(reader.GetString(1));
                    }
                }
                finally
                {
                    // always call Close when done reading.
                    reader.Close();
                }
            }
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            Dzial dzial = new Dzial();
            dzial.Show();
        }

        private void dzialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dzial dzial = new Dzial();
           // dzial.MdiParent = this;
         
            dzial.Show();
            


        }

        private void pracownikToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pracownik pracownik = new Pracownik();
            pracownik.Show();
        }

        private void zatrudnienieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormaZatrudnienia zatrudnienie = new FormaZatrudnienia();
            zatrudnienie.Show();
        }

        private void zatrudnienieToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Zatrudnienie zatrudnienie = new Zatrudnienie();
            zatrudnienie.Show();
        }

        private void linieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Linie linie = new Linie();
            linie.Show();
        }

        private void miejscePracyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MiejscePracy miejscePracy = new MiejscePracy();
            miejscePracy.Show();
        }

        private void stanowiskoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stanowisko stanowisko = new Stanowisko();
            stanowisko.Show();
        }

        private void tramwajeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tramwaje tramwaje = new Tramwaje();
            tramwaje.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Zatrudnienie zatrudnienie = new Zatrudnienie();
            zatrudnienie.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Tramwaje tramwaje = new Tramwaje();
            tramwaje.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Stanowisko stanowisko = new Stanowisko();
            stanowisko.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MiejscePracy miejscePracy = new MiejscePracy();
            miejscePracy.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Pracownik pracownik = new Pracownik();
            pracownik.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormaZatrudnienia formaZatrudnienia = new FormaZatrudnienia();
            formaZatrudnienia.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Linie linie = new Linie();
            linie.Show();
        }
    }
}
