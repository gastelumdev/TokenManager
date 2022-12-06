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
    public partial class FormItems : Form
    {
        private int itemId = 0;
        public FormItems()
        {
            InitializeComponent();

            getItemIds();
        }

        private void getItemIds()
        {
            listBox1.Items.Clear();
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "Select * from item";
            MySqlDataReader reader = databaseConnection.Query(query);

            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "id";

            while (reader.Read())
            {
                listBox1.Items.Add(new ComboboxValue(Convert.ToInt32(reader["id"].ToString()), reader["name"].ToString()));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxValue tmpComboboxValue = (ComboboxValue)listBox1.SelectedItem;
            this.itemId = tmpComboboxValue.Id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "INSERT INTO item (id, name, price) VALUES (NULL, '" + textBox1.Text + "', '" + textBox2.Text + "')";
            MySqlDataReader reader = databaseConnection.Query(query);

            this.getItemIds();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "DELETE FROM item WHERE id = " + this.itemId;
            MySqlCommand cmd = new MySqlCommand(query, databaseConnection.connect());
            MySqlDataReader reader = cmd.ExecuteReader();

            this.getItemIds();
        }
    }
}
