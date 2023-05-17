using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WinAppDemoLoginRoles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conex = new SqlConnection("Server=SERV_REDES\\SQLEXPRESS;DataBase=BD_Sistema;Trusted_Connection=True");


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                conex.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            String usua = txtUsuario.Text;
            String pass = txtPassword.Text;

            try
            {
                SqlCommand cmd = new SqlCommand("Select * From Usuario Where usuario='" + usua + "' and contrasena ='" + pass + "'", conex);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Bienvenido " + dr.GetValue(1) + " " + dr.GetValue(3));
                    CompEd.ini frmShared = new CompEd.ini();

                    frmShared.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Usuario no Valido");
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
