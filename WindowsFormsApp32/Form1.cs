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
    public partial class Form1 : Form
    {
        //static string cs = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection con = new SqlConnection(Properties.Settings.Default.a3ConnectionString);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'a3DataSet.PROJEKAT' table. You can move, or remove it, as needed.
            this.pROJEKATTableAdapter.Fill(this.a3DataSet.PROJEKAT);
            // TODO: This line of code loads data into the 'a3DataSet.KVALIFIKACIJA' table. You can move, or remove it, as needed.
            kopiraj(this.a3DataSet.PROJEKAT, listView1);

        }

        private void kopiraj(DataTable tb, ListView LV)
        {
            LV.View = View.Details;
            LV.Columns.Clear();
            foreach (DataColumn col in tb.Columns)
            {
                LV.Columns.Add(col.ColumnName);
            }
            LV.Items.Clear();
            LV.FullRowSelect = true;
            foreach (DataRow row in tb.Rows)
            {
                ListViewItem item = new ListViewItem(row[0].ToString());
                for (int i = 1; i < tb.Columns.Count; i++)
                    item.SubItems.Add(row[i].ToString());
                LV.Items.Add(item);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string projekatID = listView1.SelectedItems[0].Text;
                textBox1.Text = projekatID;
                SqlCommand cm = new SqlCommand("select * from PROJEKAT where ProjekatID=@pro", con);
                cm.Parameters.AddWithValue("pro", projekatID);
                SqlDataReader r = cm.ExecuteReader();
                while (r.Read())
                {
                    textBox2.Text = r[1].ToString();
                    textBox3.Text = r[2].ToString();
                    textBox4.Text = r[3].ToString();
                    checkBox1.Checked = Convert.ToBoolean(r[4].ToString());
                    richTextBox1.Text = r[5].ToString();
                }
            }
            catch
            {


            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("delete from PROJEKAT where ProjekatID=@pro", con);
                cm.Parameters.AddWithValue("pro", textBox1.Text);
                cm.ExecuteNonQuery();
                //  *** pocetak rada sa LOG datotekom
                string dodatak = textBox1.Text + " " + textBox2.Text + "\n";
                DateTime t = DateTime.Now;
                System.IO.File.AppendAllText("log_" + t.Day + "_" + t.Month + "_" + t.Year + ".txt", dodatak);
                //  *** kraj rada sa LOG datotekom
                this.pROJEKATTableAdapter.Fill(this.a3DataSet.PROJEKAT);
                kopiraj(this.a3DataSet.PROJEKAT, listView1);
                MessageBox.Show("Uspesno brisanje");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Brisanje nije uspelo" + ex.ToString());

            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }
    }
    

    
}
