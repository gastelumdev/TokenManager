using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TokenManagerUI.Forms
{
    public partial class FormDashboard : Form
    {
        int eventId = 0;
        public FormDashboard()
        {
            InitializeComponent();

            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "Select * from event";
            MySqlCommand cmd = new MySqlCommand(query, databaseConnection.connect());
            MySqlDataReader reader = cmd.ExecuteReader();

            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";

            while (reader.Read())
            {
                comboBox1.Items.Add(new ComboboxValue(Convert.ToInt32(reader["id"].ToString()), reader["name"].ToString()));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.resetErrors();
            ComboboxValue tmpComboboxValue = (ComboboxValue)comboBox1.SelectedItem;
            if (tmpComboboxValue != null)
            {
                this.eventId = tmpComboboxValue.Id;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form eventsForm = new FormCreateEvent();
            eventsForm.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.resetErrors();
            if (this.eventId != 0)
            {
                SessionInfo.id = this.eventId;

                // Display current event
                DatabaseConnection databaseConnection = new DatabaseConnection();

                string query = "Select * from event where id = " + SessionInfo.id.ToString();
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection.connect());
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                Form form = new FormMainMenuUI(reader["name"].ToString());
                form.Show();
                this.Hide();
            }
            else
            {
                labelError.Text = "Selection Required";
            }
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FormCreateEvent form = new FormCreateEvent();
            form.Show();
            this.Hide();
        }

        private void resetErrors()
        {
            labelError.Text = "";
        }
    }
}
