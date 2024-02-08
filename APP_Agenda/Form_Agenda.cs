using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APP_Agenda
{
    public partial class Form_Agenda : Form
    {
        // Variaveis Publicas para o MySQL
        string servidor;
        MySqlConnection conexao;
        MySqlCommand comando;

        // Variaveis Publicas de Sistema
        string idREGISTRO;

        public Form_Agenda()
        {
            InitializeComponent();

            // Utilização das variaveis publicas para o MySQL
            servidor = "Server=localhost;Database=bd_agenda;Uid=root;Pwd=";
            conexao = new MySqlConnection(servidor);
            comando = conexao.CreateCommand();

            // Preenche o DataGridViewAGENDA
            ATUALIZA_Agenda();
        }

        private void LIMPAR_Formulario()
        {
            // Limpar Campos...
            textBoxNOME.Clear();
            textBoxSOBRENOME.Clear();
            textBoxTELEFONE.Clear();
            textBoxCELULAR.Clear();
            textBoxEMAIL.Clear();
            textBoxENDERECO.Clear();
            textBoxREDESOCIAL.Clear();
        }

        private void ATUALIZA_Agenda()
        {
            try
            {
                conexao.Open();

                comando.CommandText = "SELECT * FROM tbl_contatos;";
                MySqlDataAdapter adaptadorAGENDA = new MySqlDataAdapter(comando);
                DataTable tabelaAGENDA = new DataTable();
                adaptadorAGENDA.Fill(tabelaAGENDA);

                dataGridViewAGENDA.DataSource = tabelaAGENDA;
                dataGridViewAGENDA.Columns["id"].HeaderText = "Código";
            }
            catch (Exception erro_mysql)
            {
                MessageBox.Show(erro_mysql.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void buttonSALVAR_Click(object sender, EventArgs e)
        {
            labelNOME.ForeColor = Color.Black;
            labelCELULAR.ForeColor = Color.Black;

            try
            {
                if (textBoxNOME.Text != "" && textBoxCELULAR.Text != "")
                {
                    conexao.Open();
                    comando.CommandText = "INSERT INTO tbl_contatos(nome, sobrenome, telefone, celular, email, endereco, rede_social) VALUES ('" + textBoxNOME.Text + "', '" + textBoxSOBRENOME.Text + "', '" + textBoxTELEFONE.Text + "', '" + textBoxCELULAR.Text + "', '" + textBoxEMAIL.Text + "', '" + textBoxENDERECO.Text + "', '" + textBoxREDESOCIAL.Text + "');";
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Contato adicionado com sucesso!");

                    LIMPAR_Formulario();
                }
                else
                {
                    MessageBox.Show("Nome e/ou Celular estão em BRANCO! Por favor preencha!");

                    if (textBoxNOME.Text == "")
                    {
                        textBoxNOME.Focus();
                        labelNOME.ForeColor = Color.Red;                        
                    }
                    else
                    {
                        textBoxCELULAR.Focus();
                        labelCELULAR.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception erro_mysql)
            {
                // Mensagem de erro - MySQL
                // MessageBox.Show(erro_mysql.Message);

                // Mensagem de erro - USUÁRIO
                MessageBox.Show("Erro de Sistema. Solicite ajuda!");
            }
            finally
            {
                conexao.Close();
            }
            ATUALIZA_Agenda();

        }

        private void buttonAPAGAR_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente EXCLUIR este registro?", "Atenção!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    conexao.Open();

                    comando.CommandText = "DELETE FROM tbl_contatos WHERE id = " + idREGISTRO + ";";
                    int resultado = comando.ExecuteNonQuery();
                    if (resultado > 0)
                    {
                        MessageBox.Show("Contato(s) removido(s) com sucesso! - " + resultado + " registros removidos...");
                    }
                    else
                    {
                        MessageBox.Show("Contato não encontrado!");
                    }
                }
                catch (Exception erro_mysql)
                {
                    // Mensagem de erro - MySQL
                    // MessageBox.Show(erro_mysql.Message);

                    // Mensagem de erro - USUÁRIO
                    MessageBox.Show("Erro de Sistema. Solicite ajuda!");
                }
                finally
                {
                    conexao.Close();
                }
                ATUALIZA_Agenda();
            }
            else
            {
                // MessageBox.Show("NÃO");
            }
            LIMPAR_Formulario();
        }

        private void buttonALTERAR_Click(object sender, EventArgs e)
        {
            try
            {
                conexao.Open();

                comando.CommandText = "UPDATE tbl_contatos SET nome = '" + textBoxNOME.Text + "', sobrenome = '" + textBoxSOBRENOME.Text + "', telefone = '" + textBoxTELEFONE.Text + "', celular = '" + textBoxCELULAR.Text + "', email = '" + textBoxEMAIL.Text + "', rede_social = '" + textBoxREDESOCIAL.Text + "', endereco = '" + textBoxENDERECO.Text + "' WHERE id = " + idREGISTRO + ";";
                int resultado = comando.ExecuteNonQuery();
                if (resultado > 0)
                {
                    MessageBox.Show("Contato(s) atualizado(s) com sucesso! - " + resultado + " registros atualizados...");
                }
                else
                {
                    MessageBox.Show("Contato não encontrado!");
                }
            }
            catch (Exception erro_mysql)
            {
                // Mensagem de erro - MySQL
                // MessageBox.Show(erro_mysql.Message);

                // Mensagem de erro - USUÁRIO
                MessageBox.Show("Erro de Sistema. Solicite ajuda!");
            }
            finally
            {
                conexao.Close();
            }
            ATUALIZA_Agenda();
            LIMPAR_Formulario();
        }

        private void dataGridViewAGENDA_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            idREGISTRO = dataGridViewAGENDA.CurrentRow.Cells[0].Value.ToString();

            textBoxNOME.Text = dataGridViewAGENDA.CurrentRow.Cells[1].Value.ToString();
            textBoxSOBRENOME.Text = dataGridViewAGENDA.CurrentRow.Cells[2].Value.ToString();
            textBoxTELEFONE.Text = dataGridViewAGENDA.CurrentRow.Cells[3].Value.ToString();
            textBoxCELULAR.Text = dataGridViewAGENDA.CurrentRow.Cells[4].Value.ToString();
            textBoxEMAIL.Text = dataGridViewAGENDA.CurrentRow.Cells[5].Value.ToString();
            textBoxREDESOCIAL.Text = dataGridViewAGENDA.CurrentRow.Cells[7].Value.ToString();
            textBoxENDERECO.Text = dataGridViewAGENDA.CurrentRow.Cells[6].Value.ToString();
        }

        private void buttonBUSCA_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxPESQUISA.Text == "")
                {
                    ATUALIZA_Agenda();
                }
                else
                {
                    conexao.Open();

                    comando.CommandText = "SELECT * FROM tbl_contatos WHERE nome = '" + textBoxPESQUISA.Text + "';";
                    MySqlDataAdapter adaptadorAGENDA = new MySqlDataAdapter(comando);
                    DataTable tabelaAGENDA = new DataTable();
                    adaptadorAGENDA.Fill(tabelaAGENDA);

                    dataGridViewAGENDA.DataSource = tabelaAGENDA;
                    dataGridViewAGENDA.Columns["id"].HeaderText = "Código";
                }
            }
            catch (Exception erro_mysql)
            {
                MessageBox.Show(erro_mysql.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void dataGridViewPESQUISA_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}