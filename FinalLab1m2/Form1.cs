using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace FinalLab1m2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //VARIABLES Y ARREGLOS GLOBALES
        string[,] salon = new string[20, 10];
        string[,] arriba = new string[10, 10];
        string[,] living = new string[3, 2];
        string[,] patio = new string[5, 5];

        string[,] matAux; //matriz auxiliar, defino el tipo de dato pero no la dimension

        int cap = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            DefaultForm();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            string dni = txtDNI.Text;
            int index = cmbSectores.SelectedIndex;
            matSelection(index);

            //verifico que no exista el dni en ningun otro sector
            if(controlDNI(salon,dni) && controlDNI(arriba, dni) && controlDNI(living, dni) && controlDNI(patio, dni))
            {
                dgvGrilla.Rows.Clear();
                add(matAux, dni);
                printGrid(matAux,true, cmbSectores.Text);
                if (cap >= matAux.Length)
                {
                    MessageBox.Show("Capacidad superada", "Error");
                }
            }
            else
            {
                MessageBox.Show("El DNI ingresado ya se encuentra en el sistema", "Error");
            }
        }

        private void cmbSectores_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbSectores.SelectedIndex;
            matSelection(index);
            dgvGrilla.Rows.Clear();
            printGrid(matAux,true, cmbSectores.Text);
        }

        private void btnVerTodos_Click(object sender, EventArgs e)
        {
            dgvGrilla.Rows.Clear();
            printGrid(salon, false, "Salon");
            printGrid(arriba, false, "Arriba");
            printGrid(living, false, "Living");
            printGrid(patio, false, "Patio");
        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            DefaultForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = cmbSectores.SelectedIndex;
            string dni = txtDelete.Text;
            bool control = true;
            int f = 0;
            int c = 0;
            matSelection(index);

            while(f < matAux.GetLength(0))
            {
                c = 0;
                while(c < matAux.GetLength(1))
                {
                    if (matAux[f,c] == dni)
                    {
                        matAux[f,c] = null;
                        control = false;
                    }
                    c++;
                }
                f++;
            }

            if (control)
            {
                MessageBox.Show("DNI no encontrado", "Error");
            }
            else
            {
                MessageBox.Show("Mesa liberada", "Confirmación");
            }
        
            txtDelete.Text = string.Empty;
            printGrid(matAux, true, cmbSectEliminar.Text);
        }

        public void printGrid(string[,] mat, bool clear, string sector)
        {
            cap = 0;
         
            if (clear)
            {
                dgvGrilla.Rows.Clear();
            }
            for (int f = 0; f < mat.GetLength(0); f++)
            {
                for(int c = 0; c < mat.GetLength(1); c++)
                {
                    if (mat[f,c] != null)
                    {
                        dgvGrilla.Rows.Add(mat[f, c], f+1, c+1, sector);
                        cap++;
                    }
                }
            }
            lblCap.Text = cap + "/" + matAux.Length.ToString();
        }

        public void matSelection(int index)
        {
            //Le doy el valor de la matriz segun el sector a la matriz auxiliar
            switch (index)
            {
                case 0:
                    matAux = salon;
                    break;
                case 1:
                    matAux = arriba;
                    break;
                case 2:
                    matAux = living;
                    break;
                case 3:
                    matAux = patio;
                    break;
            }
        }

        public void add(string[,] mat, string dni)
        {
            cap = 0;
            bool control = true;
            int c = 0;
            int f = 0;
            while ( f < mat.GetLength(0) && control)
            {
                c = 0;
                while (c < mat.GetLength(1) && control)
                {
                    if (mat[f,c] == null)
                    {
                        if(cap < mat.Length && mat[f,c] == null)
                        {
                            mat[f, c] = dni;
                            control = false;
                            cap++;
                        }
                    }
                    else
                    {
                        cap++;
                    }
                    c++;
                }
                f++;
            }
        }

        static bool controlDNI(string[,] mat, string dni)
        {
            bool control = true;
            int f = 0;
            int c = 0;

            while (f < mat.GetLength(0) && control)
            {
                c = 0;
                while(c < mat.GetLength(1) && control)
                {
                    if (mat[f, 0] == dni)
                    {
                        control = false;
                    }
                    c++;
                }
                f++;
            }
            return control;
        }
        public void cleanMat(string[,] mat )
        {
            for(int f = 0; f < mat.GetLength(0); f++)
            {
                for(int c = 0; c < mat.GetLength(1); c++)
                {
                    mat[f,c] = null;
                }
            }
        }

        public void DefaultForm()
        {
            string[] sectores = { "Salon", "Arriba", "Living", "Patio" };
            cmbSectores.Items.Clear();

            foreach (string s in sectores)
            {
                cmbSectores.Items.Add(s);
                cmbSectEliminar.Items.Add(s);
            }

            txtDNI.Text = string.Empty;
            lblCap.Text = string.Empty;

            //Reinicio el valor de las varibales y arreglos globales
            cap = 0;
            cleanMat(salon);
            cleanMat(arriba);
            cleanMat(living);
            cleanMat(patio);


            dgvGrilla.Rows.Clear();
            dgvGrilla.Columns.Clear();

            dgvGrilla.Columns.Add("col0", "DNI");
            dgvGrilla.Columns.Add("col1", "FILA");
            dgvGrilla.Columns.Add("col1", "MESA");
            dgvGrilla.Columns.Add("col1", "SECTOR");
        }
    }
}
