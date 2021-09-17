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
    public partial class Tramwaje : Form
    {
        OracleConnection con = null;
        public Tramwaje()
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

        private void updateDataGrid()
        {
            OracleCommand cmd = con.CreateCommand();
            String queryString = "SELECT tramwaje.id, tramwaje.model, tramwaje.numer, linie.nr_linii, miejscepracy.ulica FROM tramwaje JOIN linie on tramwaje.linie_id = linie.id JOIN miejscepracy on tramwaje.miejscepracy_id = miejscepracy.id";
            cmd.CommandText = queryString;
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();

        }

        private void Tramwaje_Load(object sender, EventArgs e)
        {
            this.updateDataGrid();
        }
    }
}
