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
using System.Configuration;

namespace WindowsFormsAppZaliczenie2
{
    public partial class Pracownik : Form
    {
        OracleConnection con = null;
        List<int> IdS = new List<int>();
        public Pracownik()
        {
            this.setConnection();
            InitializeComponent();
        }

        private void setConnection()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["WindowsFormsAppZaliczenie2.Properties.Settings.ConnectionString"].ConnectionString;
            con = new OracleConnection(connectionString);

            try
            {
                con.Open();
            }
            catch (Exception exp)
            {

            }
        }

        private void FillComboBox()
        {
            string queryString = "SELECT * FROM PRACOWNIK";
            OracleCommand command = new OracleCommand(queryString, con);
            OracleDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetInt32(0) + ", " + reader.GetString(1));
                    string dane = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4) + " " + reader.GetString(5);
                    comboBox2.Items.Add(dane);
                    
                    IdS.Add(reader.GetInt32(0));
                }
            }
            finally
            {
                // always call Close when done reading.
                reader.Close();
            }
        }

        private void updateDataGrid()
        {
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM PRACOWNIK";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
        }

        private void Pracownik_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet4.PRACOWNIK' table. You can move, or remove it, as needed.
            this.pRACOWNIKTableAdapter.Fill(this.dataSet4.PRACOWNIK);
            this.FillComboBox();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            String sql = "INSERT INTO PRACOWNIK(ID, IMIE, DRUGIE_IMIE, NAZWISKO, PESEL, PLEC) VALUES (:ID, :IMIE, :DRUGIE_IMIE, :NAZWISKO, :PESEL, :PLEC)";
            this.AUD(sql, 0);
            resetAll();
        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
            String sql = "DELETE FROM PRACOWNIK WHERE ID = :ID";
            this.AUD(sql, 2);
            this.resetAll();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            String sql = "UPDATE PRACOWNIK SET IMIE = :IMIE, DRUGIE_IMIE = :DRUGIE_IMIE, NAZWISKO = :NAZWISKO, PESEL = :PESEL, PLEC = :PLEC WHERE ID = :ID";
            this.AUD(sql, 1);
            resetAll();
        }

        private void resetAll()
        {
            IDTextBox.Text = "";
            NameTextBox.Text = "";
            Name2TextBox.Text = "";
            LastNameTextBox.Text = "";
            PeselTextBox.Text = "";
            comboBox1.SelectedIndex = -1;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.resetAll();
        }


        private void AUD(String sql_stm, int state)
        {
            String msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stm;
            cmd.CommandType = CommandType.Text;

            switch (state)
            {
                case 0:
                    msg = "Dane zostaly wstawione do tabeli!";
                    cmd.Parameters.Add("ID", OracleDbType.Int32, 6).Value = Int32.Parse(IDTextBox.Text);
                    cmd.Parameters.Add("IMIE", OracleDbType.Varchar2, 25).Value = NameTextBox.Text;
                    cmd.Parameters.Add("DRUGIE_IMIE", OracleDbType.Varchar2, 25).Value = Name2TextBox.Text;
                    cmd.Parameters.Add("NAZWISKO", OracleDbType.Varchar2, 25).Value = LastNameTextBox.Text;
                    cmd.Parameters.Add("PESEL", OracleDbType.Varchar2, 25).Value = PeselTextBox.Text;
                    cmd.Parameters.Add("PLEC", OracleDbType.Varchar2, 25).Value = comboBox1.SelectedItem.ToString();
                    break;
                case 1:
                    msg = "Dane zostaly zaktualizowane!";                   
                    cmd.Parameters.Add("IMIE", OracleDbType.Varchar2, 25).Value = NameTextBox.Text;
                    cmd.Parameters.Add("DRUGIE_IMIE", OracleDbType.Varchar2, 25).Value = Name2TextBox.Text;
                    cmd.Parameters.Add("NAZWISKO", OracleDbType.Varchar2, 25).Value = LastNameTextBox.Text;
                    cmd.Parameters.Add("PESEL", OracleDbType.Varchar2, 25).Value = PeselTextBox.Text;
                    cmd.Parameters.Add("PLEC", OracleDbType.Varchar2, 25).Value = comboBox1.SelectedItem.ToString();
                    cmd.Parameters.Add("ID", OracleDbType.Int32, 6).Value = Int32.Parse(IDTextBox.Text);
                    break;
                case 2:
                    msg = "Dane zostaly usuniete!";
                    cmd.Parameters.Add("ID", OracleDbType.Int32, 6).Value = Int32.Parse(IDTextBox.Text);
                    break;
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);

                }
            }
            catch (Exception expe) { }
            comboBox2.Items.Clear();
            this.FillComboBox();
            updateDataGrid();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                IDTextBox.Text = row.Cells[0].Value.ToString();
                NameTextBox.Text = row.Cells[1].Value.ToString();
                Name2TextBox.Text = row.Cells[2].Value.ToString();
                LastNameTextBox.Text = row.Cells[3].Value.ToString();
                PeselTextBox.Text = row.Cells[4].Value.ToString();

                if(row.Cells[5].Value.ToString() == "M")
                {
                    comboBox1.SelectedIndex = 0;
                }

                else if (row.Cells[5].Value.ToString() == "K")
                {
                    comboBox1.SelectedIndex = 1;
                }

                else if (row.Cells[5].Value.ToString() == "I")
                {
                    comboBox1.SelectedIndex = 2;
                }
                else
                {
                    comboBox1.SelectedIndex = -1;
                }

              
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            String queryString = "SELECT * FROM PRACOWNIK WHERE ID = " + "\'" + IdS[comboBox2.SelectedIndex] + "\'";
            Console.WriteLine(queryString);
           
            OracleCommand command = new OracleCommand(queryString, con);
            command.BindByName = true;
            OracleParameter op = new OracleParameter();
            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                IDTextBox.Text = reader.GetInt32(0).ToString();
                NameTextBox.Text = reader.GetString(1);
                Name2TextBox.Text = reader.GetString(2);
                LastNameTextBox.Text = reader.GetString(3);
                PeselTextBox.Text = reader.GetString(4);

                if(reader.GetString(5) == "M")
                {
                    comboBox1.SelectedIndex = 0;
                }
                else if (reader.GetString(5) == "K")
                {
                    comboBox1.SelectedIndex = 1;
                }

                else if (reader.GetString(5) == "I")
                {
                    comboBox1.SelectedIndex = 2;
                }
                else
                {
                    comboBox1.SelectedIndex = -1;
                }
            }

        }
    }
    
}
