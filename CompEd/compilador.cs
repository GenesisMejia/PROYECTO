using ManejoDeErrores;
using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;//using necesario , llama ala referencia de la libreria de expresiones regulares 
using System.Windows.Forms;
using Tsimbolos;
using Excel = Microsoft.Office.Interop.Excel;



namespace CompEd
{
    public partial class Ide : Form
    {
        int cantLineas = 0;
        string nomarchivox;
        TS tabla_simbolos = new TS();
        TE tabla_errorres = new TE();



        public Ide()
        {
            InitializeComponent();


        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //AnlzdrSntctc();

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Ide_Load(object sender, EventArgs e)//---------------
        {
            tabla_errorres.inicialestaE();
            tabla_simbolos.inicialista();
            tabControl1.Visible = false;
            PagCodigo.Select();
            PagCodigo.DetectUrls = true;
            #region  area de notificacion
            notifyIcon1.Text = " CompEd# 2013";
            notifyIcon1.BalloonTipTitle = " <# Hello World";
            notifyIcon1.BalloonTipText = "Bienvenido a CompEd# 2013";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            this.Click += new EventHandler(notifyIcon1_Click);

            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(3000);
            #endregion
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Maximized;

            this.Activate();
        }

        private void notifyIcon1_BalloonTipShown(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void Ide_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogo = MessageBox.Show("¿ Desea cerrar CompE-dark ?", "Cerrar Compilador", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogo == DialogResult.OK)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void acercaDeCompEdToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirarchivo();
        }


        //-----------------------------METODOS DE ARCHIVOS -----------------------------------------

        public void exportaraexcel(DataGridView tabla)
        {

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            excel.Application.Workbooks.Add(true);

            int ColumnIndex = 0;

            foreach (DataGridViewColumn col in tabla.Columns)
            {

                ColumnIndex++;

                excel.Cells[1, ColumnIndex] = col.Name;

            }

            int rowIndex = 0;

            foreach (DataGridViewRow row in tabla.Rows)
            {

                rowIndex++;

                ColumnIndex = 0;

                foreach (DataGridViewColumn col in tabla.Columns)
                {

                    ColumnIndex++;

                    excel.Cells[rowIndex + 1, ColumnIndex] = row.Cells[col.Name].Value;

                }

            }

            excel.Visible = true;

            Worksheet worksheet = (Worksheet)excel.ActiveSheet;

            worksheet.Activate();



        }

        public void abrirarchivo()
        {

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Title = "CompEd#                                                                     Abrir Archivo                                                                       ";
                ofd.ShowDialog();
                // ofd.Filter = "Archivos ed#(*.ed)|*.ed";
                if (File.Exists(ofd.FileName))
                {
                    using (Stream stream = ofd.OpenFile())
                    {
                        //MessageBox.Show("archivo encontrado:  "+ofd.FileName);
                        leerarchivo(ofd.FileName);
                        nomarchivox = ofd.FileName;

                        txt_direccion.Text = ofd.FileName;
                        tabControl1.Visible = true;
                    }

                }
            }
            catch (Exception)
            {

                MessageBox.Show("El archivo no se abrio correctamente");

                tabla_errorres.addliste(2);
            }

        }

        public void leerarchivo(string nomarchivo)
        {
            StreamReader reader = new StreamReader(nomarchivo, System.Text.Encoding.Default);
            //string read = reader.ReadLine();
            string texto;
            // while (read != null)
            //{
            texto = reader.ReadToEnd();
            // read = read + "\n";

            reader.Close();

            PagCodigo.Text = texto;
            // read =reader.ReadLine();

            //}


        }

        public bool revisasiarchivoexiste(string nomarchivo)
        {

            bool existe;

            if (File.Exists(nomarchivo))
            {
                // el archivo existe
                existe = true;
            }
            else
            {
                // el archivo no extiste
                existe = false;
            }
            return existe;
        }

        public void guardaArchivo()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Archivos ed|*.ed";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {

                if (File.Exists(saveFile.FileName))
                {
                    // el archivo existe
                    //------------------- para un log que agrega eventos ..................
                    //StreamWriter writer = File.AppendText(nomarchivo);
                    //writer.WriteLine("\n <</ ---Actualizacion del " + DateTime.Now.ToString() + " />>"); 
                    //writer.Write(PagCodigo.Text); 
                    //writer.Close(); 

                    //------------------ para sobrescribir el texto ...................
                    StreamWriter codigonuevo = File.CreateText(saveFile.FileName);
                    codigonuevo.Write(PagCodigo.Text);
                    codigonuevo.Flush();
                    codigonuevo.Close();
                    nomarchivox = saveFile.FileName;
                    txt_direccion.Text = saveFile.FileName;

                }
                else
                {
                    // el archivo no extiste

                    StreamWriter codigonuevo = File.CreateText(saveFile.FileName);
                    codigonuevo.Write(PagCodigo.Text);
                    codigonuevo.Write("\n \n <</ Archivo creado el: " + DateTime.Now.ToString() + " />> \n ");
                    codigonuevo.Flush();
                    codigonuevo.Close();
                    nomarchivox = saveFile.FileName;
                    txt_direccion.Text = saveFile.FileName;
                }


            }

        }

        public void guardaArchivo2(string nomarchivo)
        {
            try
            {
                if (nomarchivo == null)
                {
                    guardaArchivo();

                }
                else
                {
                    // el archivo nuevo
                    StreamWriter codigonuevo = File.CreateText(nomarchivo);
                    codigonuevo.Write(PagCodigo.Text);
                    codigonuevo.Flush();
                    codigonuevo.Close();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("error al guardar");

            }


        }

        public void leer_archivo_al(string nomarchivo)
        {

            int contador_Ambitoi = 0;
            int contador_Ambitf = 0;
            int ambito = 0;
            try
            {
                StreamReader reader = new StreamReader(nomarchivo);
                string[] Palabras_Separadas;
                string read;
                int numero_de_lineas = 0;
                PagCodigo.Select(0, PagCodigo.SelectionStart);

                while (reader != null)
                {
                    numero_de_lineas = numero_de_lineas + 1;
                    read = reader.ReadLine();


                    if (reader.EndOfStream)
                    {
                        //MessageBox.Show("ultima linea");

                        break;
                    }
                    else
                    {


                        Palabras_Separadas = read.Split(' ');
                        foreach (var palabra in Palabras_Separadas)
                        {
                            #region Medicion del ambito

                            if (palabra == "{")
                            {
                                contador_Ambitoi = contador_Ambitoi + 1;
                            }
                            if (palabra == "}")
                            {
                                contador_Ambitf = contador_Ambitf + 1;
                            }
                            ambito = contador_Ambitoi;


                            #endregion


                            //-----------------------------------------------------------------------


                            if (tabla_simbolos.compararAL(palabra.ToString()) && palabra != null)// se manda a comparar la palabra con la tabla de simbolos
                            {
                                //                                                    simb  ,val, nunlin           ,tam,ambit,                 id_,           tipo,       descrip
                                //uneSentencias();

                                tabla_de_simbolos objnuevo = new tabla_de_simbolos(palabra, "", numero_de_lineas, -0, ambito, tabla_simbolos.compararALRef(palabra.ToString()), "palabra nueva", "palabra que coincide con la Tabla de simbolos", "");
                                tabla_simbolos.añadir_obj(objnuevo);

                                PagCodigo.SelectionStart = PagCodigo.Find(palabra);
                                PagCodigo.SelectionColor = Color.DodgerBlue;

                            }
                            else//de no estar en la tabla de simbolos se agrega a un campo nuevo
                            {
                                if (Regex.IsMatch(palabra, @"[a-zA-Z]") && palabra != null)//sentencia que revisa los dos texbox 
                                {
                                    // System.Windows.Forms.MessageBox.Show("esto es una palabra");
                                    tabla_de_simbolos objnuevo = new tabla_de_simbolos(palabra, "", numero_de_lineas, -0, ambito, tabla_simbolos.contlineas() + 1, "palabra nueva", "palabra que no coincide con la Tabla de simbolos,pero no se considera error", "");
                                    tabla_simbolos.añadir_obj(objnuevo);
                                }
                                else if (Regex.IsMatch(palabra, @"\d{1}|\d{2}|\d{3}|\d{4}|\d{5}") && palabra != null)
                                {
                                    //System.Windows.Forms.MessageBox.Show("esto es un numero");
                                    tabla_de_simbolos objnuevo = new tabla_de_simbolos(palabra, palabra, numero_de_lineas, -0, ambito, tabla_simbolos.contlineas() + 1, "numero nuevo", "numero", "");
                                    tabla_simbolos.añadir_obj(objnuevo);

                                    PagCodigo.SelectionStart = PagCodigo.Find(palabra);
                                    PagCodigo.SelectionColor = Color.Aquamarine;

                                }
                                else
                                {
                                    // System.Windows.Forms.MessageBox.Show("Error en la expresion \n no cumple con un formato correcto ");

                                }
                            }
                        }//fin del analisis lexico

                    }
                    Palabras_Separadas = null;
                    cantLineas = numero_de_lineas;
                }

                if (contador_Ambitf != contador_Ambitoi)
                {
                    //MessageBox.Show("error de ambito");
                    tabla_errorres.addliste(8);


                }


                reader.Close();
            }
            catch (ArgumentNullException)
            {

                MessageBox.Show("El archivo no se abrio correctamente");

                tabla_errorres.addliste(2);
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            }


        }

        public string[] uneSentencias()
        {
            string sentencia = null;
            string[] sentencias = new string[cantLineas];
            int bandera = 0;
            string tipov = "";

            for (int i = 1; i < cantLineas; i++) //une los token de cada linea
            {
                foreach (var token in tabla_simbolos.llamatabla())
                {
                    if (token.NumLinea == i && token != null)
                    {
                        if (bandera == 0 && Regex.IsMatch(token.Simbolo, @"(<#int|<#integer|<#double|<#bool|<#string|<#real|<#boolean)$"))
                        {
                            token.TipoVar = token.Simbolo;
                            tipov = token.Simbolo;
                        }


                        if (bandera != 0)
                        {
                            sentencia = sentencia + " " + token.simbolo.ToString();
                            token.TipoVar = tipov;
                        }
                        else
                        {
                            sentencia = sentencia + token.simbolo.ToString();
                            bandera = 1;
                        }

                    }
                }
                sentencias[i] = sentencia;
                sentencia = null;
                bandera = 0;
                tipov = "";
            }

            return sentencias;
        }
        #region analizador sintactico
        public void AnlzdrSntctc(string[] sentencias)
        {

            for (int i = 1; i < sentencias.Length; i++)
            {
                //MessageBox.Show(sentencias[i]);
                #region  Expresiones regulares
                if (sentencias[i] != null)
                {
                    if (Regex.IsMatch(sentencias[i], @"^<#int|<#integer\s+[a-z](1,15)(\s+:\s+\d(0,32000))*;$"))
                    {

                        System.Windows.Forms.MessageBox.Show("esto es una sentencia int");
                        #region parte semantica
                        string[] separanum;
                        separanum = sentencias[i].Split(' ');
                        try
                        {
                            int num;
                            num = int.Parse(separanum[3]);

                            MessageBox.Show("si es un numero entero");

                        }
                        catch (FormatException e)
                        {

                            MessageBox.Show("no es un numero entero");
                            tabla_errorres.addliste(0, i);

                        }
                        catch (IndexOutOfRangeException e)
                        {

                            tabla_errorres.addliste(10, i);
                            MessageBox.Show("error de escritura");

                        }
                        #endregion


                    }
                    else if (Regex.IsMatch(sentencias[i], @"^<#double|<#real\s+[a-z](1,15)(\s+:\s+\d(0,32000))*;$"))
                    {
                        System.Windows.Forms.MessageBox.Show("esto es una sentencia double");
                        #region parte semantica
                        string[] separanum;
                        separanum = sentencias[i].Split(' ');
                        try
                        {
                            double num;
                            num = double.Parse(separanum[3]);

                            MessageBox.Show("si es un numero double");

                        }
                        catch (FormatException e)
                        {
                            tabla_errorres.addliste(0, i);
                            MessageBox.Show("no es un numero double");

                        }
                        catch (IndexOutOfRangeException e)
                        {

                            tabla_errorres.addliste(10, i);
                            MessageBox.Show("error de escritura");

                        }


                        #endregion

                    }
                    else if (Regex.IsMatch(sentencias[i], @"^<#string|<#texto\s+[a-z](1,15)(\s+:\s+[a-z](1,15)')*;$"))
                    {
                        System.Windows.Forms.MessageBox.Show("esto es una sentencia string");

                    }
                    else if (Regex.IsMatch(sentencias[i], @"^<#bool|<#boolean\s+[a-z](1,15)(\s+:\s+(true|false))*;$"))
                    {
                        System.Windows.Forms.MessageBox.Show("esto es una sentencia bool");

                        #region parte semantica
                        string[] separavar;
                        separavar = sentencias[i].Split(' ');
                        try
                        {
                            bool var;
                            var = bool.Parse(separavar[3]);

                            MessageBox.Show("si es una variable bool");

                        }
                        catch (FormatException e)
                        {

                            MessageBox.Show("no es una variable bool");
                            tabla_errorres.addliste(0, i);
                        }
                        #endregion

                    }
                    else if (Regex.IsMatch(sentencias[i], @"<<*.*>>$"))
                    {
                        MessageBox.Show("Esto es un comentario");
                    }
                    else if (Regex.IsMatch(sentencias[i], @"[a-z]\s+:\s[a-z]|(\w)*\s\+\s(\w)*|\d(0,32000)*\s;$"))
                    {
                        MessageBox.Show("esto es una sentecia de asignacion");

                        #region parte semantica
                        string tpv1 = "";
                        string tpv2 = "";
                        string tpv3 = "";

                        string[] separavar;
                        separavar = sentencias[i].Split(' ');

                        if (Regex.IsMatch(sentencias[i], @"[a-z]\s+:\s(\w)*\s\+\s(\w)*\s;$"))//-- asignacion del tipo monto : num1 + num2 ;
                        {

                            foreach (var token in tabla_simbolos.llamatabla())
                            {
                                if (token.Simbolo == separavar[0])
                                {
                                    tpv1 = token.TipoVar;
                                }
                                if (token.Simbolo == separavar[2])
                                {
                                    tpv2 = token.TipoVar;
                                }
                                if (token.Simbolo == separavar[4])
                                {
                                    tpv3 = token.TipoVar;
                                }

                            }//-- fin del foreach
                            if (tpv1 == tpv2 && tpv2 == tpv3 && tpv1 != "")
                            {
                                MessageBox.Show("el tipo de las variables son el mismo");
                            }
                        }
                        if (Regex.IsMatch(sentencias[i], @"[a-z]\s+:\s(\w)*\s\-\s(\w)*\s;$"))//-- asignacion del tipo monto : num1 - num2 ;
                        {

                            foreach (var token in tabla_simbolos.llamatabla())
                            {
                                if (token.Simbolo == separavar[0])
                                {
                                    tpv1 = token.TipoVar;
                                }
                                if (token.Simbolo == separavar[2])
                                {
                                    tpv2 = token.TipoVar;
                                }
                                if (token.Simbolo == separavar[4])
                                {
                                    tpv3 = token.TipoVar;
                                }

                            }//-- fin del foreach
                            if (tpv1 == tpv2 && tpv2 == tpv3 && tpv1 != "")
                            {
                                MessageBox.Show("el tipo de las variables son el mismo");
                            }
                        }
                        if (Regex.IsMatch(sentencias[i], @"[a-z]\s+:\s(\w)*\s\/\s(\w)*\s;$"))//-- asignacion del tipo monto : num1 / num2 ;
                        {

                            foreach (var token in tabla_simbolos.llamatabla())
                            {
                                if (token.Simbolo == separavar[0])
                                {
                                    tpv1 = token.TipoVar;
                                }
                                if (token.Simbolo == separavar[2])
                                {
                                    tpv2 = token.TipoVar;
                                }
                                if (token.Simbolo == separavar[4])
                                {
                                    tpv3 = token.TipoVar;
                                }

                            }//-- fin del foreach
                            if (tpv1 == tpv2 && tpv2 == tpv3 && tpv1 != "")
                            {
                                MessageBox.Show("el tipo de las variables son el mismo");
                            }
                        }
                        if (Regex.IsMatch(sentencias[i], @"[a-z]\s+:\s(\w)*\s\*\s(\w)*\s;$"))//-- asignacion del tipo monto : num1 * num2 ;
                        {

                            foreach (var token in tabla_simbolos.llamatabla())
                            {
                                if (token.Simbolo == separavar[0])
                                {
                                    tpv1 = token.TipoVar;
                                }
                                if (token.Simbolo == separavar[2])
                                {
                                    tpv2 = token.TipoVar;
                                }
                                if (token.Simbolo == separavar[4])
                                {
                                    tpv3 = token.TipoVar;
                                }

                            }//-- fin del foreach
                            if (tpv1 == tpv2 && tpv2 == tpv3 && tpv1 != "")
                            {
                                MessageBox.Show("el tipo de las variables son el mismo");
                            }
                        }


                        #endregion


                    }
                    else if (Regex.IsMatch(sentencias[i], @"^{$"))
                    {
                        MessageBox.Show("inicio de ambito");
                    }
                    else if (Regex.IsMatch(sentencias[i], @"^}$"))
                    {
                        MessageBox.Show("fin de ambito");
                    }
                    else if (Regex.IsMatch(sentencias[i], @"<<si_\s\(\s+\w+\s(<|>|<:|>:|::|!:)\s\w+\s\)\s\{$"))//--
                    {
                        MessageBox.Show("comienzo de if");
                    }
                    else if (Regex.IsMatch(sentencias[i], @"<<ysi_\s\(\s+\w+\s(<|>|<:|>:|::|!:)\s\w+\s\)\s\{$"))//--
                    {
                        MessageBox.Show("comienzo de else if");
                    }
                    else if (Regex.IsMatch(sentencias[i], @"<<sino\s*\{$"))//--
                    {
                        MessageBox.Show("comienzo de else");
                    }
                    else if (Regex.IsMatch(sentencias[i], @"#ncasd\s\(\s\w+\s(<|>|<:|>:|::|!:)\s\w+\s\)\s\{$"))//--
                    {
                        MessageBox.Show("comienzo del switch");
                    }
                    else if (Regex.IsMatch(sentencias[i], @"casd\s\(\s(\w+|\d+)\s\)\s{$"))//--
                    {
                        MessageBox.Show("comienzo de case");
                    }
                    else if (Regex.IsMatch(sentencias[i], @"fcasd\s;$"))//--
                    {
                        MessageBox.Show("break del case");
                    }
                    else if (Regex.IsMatch(sentencias[i], @"#mintrs\s\(\s\w+\s(<|>|<:|>:|::|!:)\s\w+\s\)\s{$"))
                    {
                        MessageBox.Show("inicio de un while");
                    }

                    else if (Regex.IsMatch(sentencias[i], @"#mostrar\s\(\s(\w*)|'\w*'\)\s;$"))
                    {
                        MessageBox.Show("mostrar por pantalla \n" + sentencias[i]);

                    }
                    //else if (Regex.IsMatch(sentencias[i],@""))
                    //{
                    //MessageBox.Show("");
                    //}
                    //else if (Regex.IsMatch(sentencias[i],@""))
                    //{
                    //MessageBox.Show("");
                    //}
                    //else if (Regex.IsMatch(sentencias[i],@""))
                    //{
                    //MessageBox.Show("");
                    //}
                    //else if (Regex.IsMatch(sentencias[i],@""))
                    //{
                    //MessageBox.Show("");
                    //}
                    else
                    {
                        if (sentencias[i] != null)
                        {
                            tabla_errorres.addliste(9, i);
                            //MessageBox.Show("Expresion invalida");
                        }
                    }
                }

                #endregion

                //System.Windows.Forms.MessageBox.Show("" + sentencias[i]);
            }





        }
        #endregion





        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guardaArchivo();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            guardaArchivo2(nomarchivox);


        }

        private void guardarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            guardaArchivo2(nomarchivox);

        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.Visible = true;

        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = tabla_simbolos.llamatabla();
            dataGridView2.DataSource = tabla_errorres.llamatablaE();

        }

        private void analizadorLexicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            leer_archivo_al(nomarchivox);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            tabControl1.Visible = true;

        }

        private void cerrarProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.Visible = false;

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            abrirarchivo();
        }

        private void maximizarVentanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void minimizarVentanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void minimizarVentanaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void opcionesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void colorDeLaFuenteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var cl = colorDialog1.ShowDialog();
            if (cl == System.Windows.Forms.DialogResult.OK)
            {
                //PagCodigo.SelectionColor = colorDialog1.Color; <..... esto para una parte del texto
                PagCodigo.ForeColor = colorDialog1.Color;
            }



        }

        private void colorDeConsolaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cl = colorDialog1.ShowDialog();
            if (cl == System.Windows.Forms.DialogResult.OK)
            {
                //PagCodigo.SelectionColor = colorDialog1.Color; <..... esto para una parte del texto
                PagCodigo.BackColor = colorDialog1.Color;
            }

        }

        private void formatoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var fm = fontDialog1.ShowDialog();
            if (fm == DialogResult.OK)
            {
                //PagCodigo.SelectionColor = colorDialog1.Color; <..... esto para una parte del texto
                PagCodigo.Font = fontDialog1.Font;
            }


        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Cb_resultados.Text == "ver log de errores")
            {
                exportaraexcel(dataGridView2);
            }
            else if (Cb_resultados.Text == "ver log de simbolos")
            {
                exportaraexcel(dataGridView1);
            }

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            guardaArchivo2(nomarchivox);
            tabla_simbolos.reinicialista();
            tabla_errorres.reinicialista();
            tabla_errorres.inicialestaE();
            tabla_simbolos.inicialista();
            leer_archivo_al(nomarchivox);
            string[] sent = uneSentencias();
            tabla_simbolos.compararALsemantic();

            if (tabla_simbolos.revisar_duplicados())
            {
                tabla_errorres.addliste(11);
            }

            AnlzdrSntctc(sent);
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = tabla_simbolos.llamatabla();
            dataGridView2.DataSource = tabla_errorres.llamatablaE();
            System.Media.SystemSounds.Asterisk.Play();




        }

        private void PagCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            // Crear una instancia de Excel
            Excel.Application excel = new Excel.Application();
            Excel.Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Excel.Worksheet worksheet = null;

            try
            {
                worksheet = workbook.ActiveSheet;

                int columnIndex = 1;
                int rowIndex = 1;

                // Recorrer las columnas del DataGridView y escribir los encabezados en Excel
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    worksheet.Cells[rowIndex, columnIndex] = column.HeaderText;
                    columnIndex++;
                }

                rowIndex++;

                // Recorrer las filas del DataGridView y escribir los datos en Excel
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    columnIndex = 1;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        worksheet.Cells[rowIndex, columnIndex] = cell.Value;
                        columnIndex++;
                    }

                    rowIndex++;
                }

                // Guardar el archivo de Excel
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Archivos de Excel (*.xlsx)|*.xlsx";
                saveDialog.FileName = "ExportedData.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Datos exportados correctamente a Excel.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar los datos a Excel: " + ex.Message);
            }
            finally
            {
                // Cerrar y liberar recursos
                workbook.Close();
                excel.Quit();

                releaseObject(worksheet);
                releaseObject(workbook);
                releaseObject(excel);
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Error al liberar el objeto: " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Crear una instancia de Excel
            Excel.Application excel = new Excel.Application();
            Excel.Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Excel.Worksheet worksheet = null;

            try
            {
                worksheet = workbook.ActiveSheet;

                int columnIndex = 1;
                int rowIndex = 1;

                // Recorrer las columnas del DataGridView y escribir los encabezados en Excel
                foreach (DataGridViewColumn column in dataGridView2.Columns)
                {
                    worksheet.Cells[rowIndex, columnIndex] = column.HeaderText;
                    columnIndex++;
                }

                rowIndex++;

                // Recorrer las filas del DataGridView y escribir los datos en Excel
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    columnIndex = 1;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        worksheet.Cells[rowIndex, columnIndex] = cell.Value;
                        columnIndex++;
                    }

                    rowIndex++;
                }

                // Guardar el archivo de Excel
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Archivos de Excel (*.xlsx)|*.xlsx";
                saveDialog.FileName = "ExportedData.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Datos exportados correctamente a Excel.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar los datos a Excel: " + ex.Message);
            }
            finally
            {
                // Cerrar y liberar recursos
                workbook.Close();
                excel.Quit();

                releaseObject2(worksheet);
                releaseObject2(workbook);
                releaseObject2(excel);
            }
        }

        private void releaseObject2(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Error al liberar el objeto: " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (!Directory.Exists(@"C:\Users\juan_\Downloads\CompEd"))
            {
                Directory.CreateDirectory(@"C:\Users\juan_\Downloads\CompEd");
            }

            TextWriter sw = new StreamWriter(@"C:\Users\juan_\Downloads\CompEd\Archivo.txt");
            int rowcount = dataGridView1.Rows.Count;
            for (int i = 0; i < rowcount - 1; i++)
            {
                sw.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString() + "\t"
                             + dataGridView1.Rows[i].Cells[1].Value.ToString() + "\t"
                              + dataGridView1.Rows[i].Cells[2].Value.ToString() + "\t"
                               + dataGridView1.Rows[i].Cells[3].Value.ToString() + "\t");
            }
            sw.Close();
            MessageBox.Show("Datos Exportados correctamente");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"C:\Users\juan_\Downloads\CompEd"))
            {
                Directory.CreateDirectory(@"C:\Users\juan_\Downloads\CompEd");
            }

            TextWriter sw = new StreamWriter(@"C:\Users\juan_\Downloads\CompEd\Archivo.txt");
            int rowcount = dataGridView2.Rows.Count;
            for (int i = 0; i < rowcount - 1; i++)
            {
                sw.WriteLine(dataGridView2.Rows[i].Cells[0].Value.ToString() + "\t"
                             + dataGridView2.Rows[i].Cells[1].Value.ToString() + "\t"
                              + dataGridView2.Rows[i].Cells[2].Value.ToString() + "\t"
                               + dataGridView2.Rows[i].Cells[3].Value.ToString() + "\t");
            }
            sw.Close();
            MessageBox.Show("Datos Exportados correctamente");
        }
    }
}
