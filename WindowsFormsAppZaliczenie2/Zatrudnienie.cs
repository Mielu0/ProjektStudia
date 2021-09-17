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
    public partial class Zatrudnienie : Form
    {
        OracleConnection con = null;

        public Zatrudnienie()
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
            String queryString = "select zatrudnienie.id, zatrudnienie.nr_umowy, zatrudnienie.od, zatrudnienie.do, zatrudnienie.wynagrodzenie, pracownik.imie, pracownik.drugie_imie, pracownik.nazwisko, pracownik.pesel, dzial.nazwa AS DZIAL, stanowisko.nazwa AS STANOWISKO,miejscepracy.ulica, formazatr.rodzaj_zatrudnienia from zatrudnienie JOIN pracownik on zatrudnienie.pracownik_id = pracownik.id JOIN dzial on zatrudnienie.dzial_id = dzial.id JOIN stanowisko on zatrudnienie.stanowisko_id = stanowisko.id JOIN miejscepracy on zatrudnienie.miejscepracy_id = miejscepracy.id JOIN formazatr on zatrudnienie.forma_zatr_id = formazatr.id ORDER BY pracownik.imie";
            cmd.CommandText = queryString;
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();

        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void Zatrudnienie_Load(object sender, EventArgs e)
        {
            this.updateDataGrid();
        }

       
    }
}
