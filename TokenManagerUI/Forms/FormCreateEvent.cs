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
    public partial class FormCreateEvent : Form
    {
        private int eventId = 0;
        public FormCreateEvent()
        {
            InitializeComponent();

            this.getItemIds();
            labelError.Text = "";
        }

        private void getItemIds()
        {
            listBox1.Items.Clear();
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "Select * from event";
            MySqlDataReader reader = databaseConnection.Query(query);

            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "id";

            while (reader.Read())
            {
                listBox1.Items.Add(new ComboboxValue(Convert.ToInt32(reader["id"].ToString()), reader["name"].ToString()));
            }
        }

        private void FormCreateEvent_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormDashboard form = new FormDashboard();
            form.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.resetErrors();
            ComboboxValue tmpComboboxValue = (ComboboxValue)listBox1.SelectedItem;
            if (tmpComboboxValue != null)
            {
                this.eventId = tmpComboboxValue.Id;
            }
            else
            {
                this.eventId = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool validated = true;
            string error = "Required";
            this.resetErrors();

            if (textBox1.Text == "")
            {
                validated = false;
                labelError.Text = error;
            }

            if (textBox2.Text == "")
            {
                validated = false;
                labelError2.Text = error;
            }

            if (validated)
            {
                DatabaseConnection databaseConnection = new DatabaseConnection();
                string query = "INSERT INTO event (id, name, token_price) VALUES (NULL, '" + textBox1.Text + "', '" + textBox2.Text + "')";
                MySqlDataReader reader = databaseConnection.Query(query);

                this.getItemIds();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.resetErrors();
            if (this.eventId != 0)
            {
                DatabaseConnection databaseConnection = new DatabaseConnection();
                string query = "DELETE FROM event WHERE id = " + this.eventId;
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection.connect());
                MySqlDataReader reader = cmd.ExecuteReader();

                this.getItemIds();
            }
            else
            {
                labelError3.Text = "Select the event to delete.";
            }
            
        }

        private void labelError2_Click(object sender, EventArgs e)
        {

        }

        private void resetErrors()
        {
            labelError.Text = "";
            labelError2.Text = "";
            labelError3.Text = "";
        }
    }
}
