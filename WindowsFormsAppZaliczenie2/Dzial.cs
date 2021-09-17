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
    public partial class Dzial : Form
    {
        OracleConnection con = null;

        public Dzial()
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
            catch(Exception exp)
            {

            }
        }

        private void updateDataGrid()
        {            
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM DZIAL";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
        }

        private void Dzial_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.DZIAL' table. You can move, or remove it, as needed.
            this.dZIALTableAdapter.Fill(this.dataSet1.DZIAL);
            this.FillComboBox();

        }

        private void FillComboBox()
        {
            string queryString = "SELECT * FROM DZIAL";
            OracleCommand command = new OracleCommand(queryString, con);
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
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            String sql = "INSERT INTO DZIAL(ID, NAZWA) VALUES (:ID, :NAZWA)";
            this.AUD(sql, 0);
            resetAll();
        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
            String sql = "DELETE FROM DZIAL WHERE ID = :ID";
            this.AUD(sql, 2);
            this.resetAll();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            String sql = "UPDATE DZIAL SET NAZWA = :NAZWA WHERE ID = :ID";
            this.AUD(sql, 1);
            resetAll();
        }

        private void resetAll()
        {
            IDTextBox.Text = "";
            NameTextBox.Text = "";
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

            switch(state)
            {
                case 0:
                    msg = "Dane zostaly wstawione do tabeli!";
                    cmd.Parameters.Add("ID", OracleDbType.Int32, 6).Value = Int32.Parse(IDTextBox.Text);
                    cmd.Parameters.Add("NAZWA", OracleDbType.Varchar2, 25).Value = NameTextBox.Text;
                    break;
                case 1:
                    msg = "Dane zostaly zaktualizowane!";                  
                    cmd.Parameters.Add("NAZWA", OracleDbType.Varchar2, 25).Value = NameTextBox.Text;
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
                if(n>0)
                {
                    MessageBox.Show(msg);
                    
                }
            }
            catch(Exception expe) { }
            updateDataGrid();
            comboBox1.Items.Clear();
            this.FillComboBox();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
            
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                IDTextBox.Text = row.Cells[0].Value.ToString();
                NameTextBox.Text = row.Cells[1].Value.ToString();
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            NameTextBox.Text = comboBox1.SelectedItem.ToString();

            String queryString = "SELECT ID FROM DZIAL WHERE NAZWA = " + "\'"  + comboBox1.SelectedItem.ToString() + "\'";
            Console.WriteLine(queryString);
            OracleCommand command = new OracleCommand(queryString, con);
            command.BindByName = true;
            OracleParameter op = new OracleParameter();            
            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                IDTextBox.Text = reader.GetInt32(0).ToString();
            }
        }
    }
}
