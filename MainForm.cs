using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace KursDataBase
{
    public partial class MainForm : Form
    {
        private static String connString = "Host=localhost;Port=5432;User Id=postgres;Password=1310;Database=Kurs";
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dt;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connString);

            var generate = new Randomize.Generate();

            for (int i = 0; i < 5000; i++)
            {
                var city = generate.GetRandomCity();
                var country = generate.GetCountry(city);
                try
                {
                    conn.Open();
                    sql = $"select country_id from Countries Where country_id = {country.id}";
                    cmd = new NpgsqlCommand(sql, conn);
                    if (cmd.ExecuteScalar() == null)
                    {
                        sql = $"select * from country_insert(:_id, :_name)";
                        cmd = new NpgsqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("_id", country.id);
                        cmd.Parameters.AddWithValue("_name", country.Name);
                        cmd.ExecuteScalar();
                    }
                    sql = $"select city_id from Cities Where city_id = {city.city_id}";
                    cmd = new NpgsqlCommand(sql, conn);
                    if (cmd.ExecuteScalar() == null)
                    {
                        sql = $"select * from city_insert(:_id, :_name, :coun_id)";
                        cmd = new NpgsqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("_id", city.city_id);
                        cmd.Parameters.AddWithValue("_name", city.name);
                        cmd.Parameters.AddWithValue("coun_id", city.country_id);
                        cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            SelectAll("Cities");
        }

        private void SelectAll(String table)
        {
            try
            {
                conn.Open();
                sql = $"select * from {table}";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                dataGridView1.DataSource = null; //очистка таблицы
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
