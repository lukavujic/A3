using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp32
{
    public partial class Form2 : Form
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.a3ConnectionString);
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Crtaj();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("select year(DatumPocetka) as Godina, count(a.DatumAngazovanja) as BrojRadnika, count (p.ProjekatID) as BrojProjekta from PROJEKAT p, ANGAZMAN a where p.ProjekatID=a.ProjekatID and YEAR(GETDATE()) - year(DatumPocetka) -1 < @gd group by YEAR(DatumPocetka)", con);
                cm.Parameters.AddWithValue("gd", numericUpDown1.Value);
                SqlDataAdapter ad = new SqlDataAdapter(cm);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                Crtaj();


            }
            catch (Exception x)
            {
                MessageBox.Show("ulazi" + x.Message);

            }
            con.Close();
        }

        private void Crtaj()
        {
            try
            {
                chart1.Series[0].Points.Clear();
                int n = dataGridView1.RowCount-1;
                int[] brojevi = new int[n];
                int[] godine = new int[n];

                for (int i = 0; i < n; i++)
                {
                    brojevi[i] = (int)dataGridView1.Rows[i].Cells[2].Value;
                    godine[i] = (int)dataGridView1.Rows[i].Cells[0].Value;

                    chart1.Series[0].Points.Add(brojevi[i]);
                    chart1.Series[0].Points[i].LegendText = godine[i].ToString();
                    chart1.Series[0].Points[i].AxisLabel = brojevi[i].ToString();
                }
            }
            catch
            {


            }
            con.Close();
        }
    }
    }
    

