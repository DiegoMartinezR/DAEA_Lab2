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
using System.Configuration;

namespace Semana2_Martinez
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Diego"].ConnectionString);

        private void label5_Click(object sender, EventArgs e)
        {

        }


        public void ListaAnios()
        {
            using(SqlCommand cmd = new SqlCommand("Usp_ListaAnios", cn))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    using(DataSet df = new DataSet())
                    {
                        //El metodo Fill cargara los datos del procedimiento almacenado
                        da.Fill(df, "ListaAnios");
                        comboBox1.DataSource = df.Tables["ListaAnios"];
                        comboBox1.DisplayMember = "Anios";
                        comboBox1.ValueMember = "Anios";
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListaAnios();
        }





        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using(SqlCommand cmd = new SqlCommand("Usp_Lista_Pedidos_Anios", cn))
            {
                using (SqlDataAdapter Da = new SqlDataAdapter())
                {
                    Da.SelectCommand = cmd;
                    Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    Da.SelectCommand.Parameters.AddWithValue("@anio", comboBox1.SelectedValue);
                    using (DataSet df = new DataSet())
                    {
                        Da.Fill(df, "Pedidos");
                        // Mostrar los datos en el datagridview
                        dataGridView1.DataSource = df.Tables["Pedidos"];
                        textBox1.Text = df.Tables["Pedidos"].Rows.Count.ToString();
                    }
                }
            }
        }

        private void DgPedidos_DoubleClick(object sender, EventArgs e)
        {
            int Codigo;
            Codigo = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            using(SqlCommand cmd = new SqlCommand("Usp_Detalle_Pedido", cn))
            {
                using(SqlDataAdapter Da = new SqlDataAdapter())
                {
                    Da.SelectCommand = cmd;
                    Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    Da.SelectCommand.Parameters.AddWithValue("@idpedido", Codigo);
                    using (DataSet df = new DataSet())
                    {
                        Da.Fill(df, "detallesdepedidos");
                        //mostrar los datos en el Datagridview
                        dataGridView2.DataSource = df.Tables["detallesdepedidos"];
                        textBox2.Text = df.Tables["detallesdepedidos"].Compute("Sum(Monto", "").ToString();
                    }
                }
            }
        }
    }
}
