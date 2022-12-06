using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    public partial class FormAddItems : Form
    {
        private int itemId = 0;
        private int listItemId = 0;
        public FormAddItems()
        {
            InitializeComponent();

            this.setCombobox();
            this.getItemIds();
        }

        private void setCombobox()
        {
            // Establish a database connection
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "Select * from item";
            MySqlDataReader reader = databaseConnection.Query(query);
            // Set what the display name and value member will be for the combo box
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
            // Go through every row and create an combo box item for each
            while (reader.Read())
            {
                comboBox1.Items.Add(new ComboboxValue(Convert.ToInt32(reader["id"].ToString()), reader["name"].ToString()));
            }
            // Close the database connection
            databaseConnection.Close();
        }

        private void getItemIds()
        {
            // Clear all items and and get current item from the database
            listBox1.Items.Clear();
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "Select * from event_item where event_id = " + SessionInfo.id;
            MySqlDataReader reader = databaseConnection.Query(query);
            // Pass the items to the set list view method
            this.setListView(reader);
        }

        private void setListView(MySqlDataReader r)
        {
            // Set the display member and value member of the listbox
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "id";
            // Go though event items rows passed in get all the items based on the ids
            while (r.Read())
            {
                DatabaseConnection databaseConnection = new DatabaseConnection();
                string query = "Select * from item where id = " + r["item_id"].ToString();
                MySqlDataReader reader = databaseConnection.Query(query);
                // Add the items to the listbox
                reader.Read();
                listBox1.Items.Add(new ComboboxValue(Convert.ToInt32(reader["id"].ToString()), reader["name"].ToString()));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Establish an empty error string
            labelError.Text = "";
            // If there is an itemId established
            if (this.itemId != 0)
            {
                // Create a relationship between the item selected and the current event
                DatabaseConnection databaseConnection = new DatabaseConnection();
                string query = "INSERT INTO event_item (id, event_id, item_id) VALUES (NULL, " + SessionInfo.id + ", " + this.itemId + ")"; ;
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection.connect());
                MySqlDataReader reader = cmd.ExecuteReader();
                // Get an updated list of items
                this.getItemIds();
            }
            else
            {
                // Display error
                labelError.Text = "Select the item you would like to add.";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set an item when it is selected
            ComboboxValue tmpComboboxValue = (ComboboxValue)comboBox1.SelectedItem;
            if (tmpComboboxValue != null)
            {
                this.itemId = tmpComboboxValue.Id;

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set an item when it is selected
            ComboboxValue tmpComboboxValue = (ComboboxValue)listBox1.SelectedItem;
            if (tmpComboboxValue != null)
            {
                this.listItemId = tmpComboboxValue.Id;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Establish an empty error string
            labelError.Text = "";
            // If there is an itemId established
            if (this.listItemId != 0)
            {
                // Delete item
                DatabaseConnection databaseConnection = new DatabaseConnection();
                string query = "DELETE FROM event_item WHERE item_id = " + this.listItemId + " AND event_id = " + SessionInfo.id;
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection.connect());
                MySqlDataReader reader = cmd.ExecuteReader();
                // Get an updated list of items
                this.getItemIds();
                this.listItemId = 0;
            }
            else
            {
                // Display error
                labelError.Text = "Select the event you want to delete.";
            }
            
        }
    }
}
