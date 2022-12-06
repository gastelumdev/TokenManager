using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using USB_Barcode_Scanner;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TokenManagerUI.Forms
{
    public partial class FormCardScanner : Form
    {
        private int itemId = 0;
        private int itemTokensRequired = 0;
        private string currentItem;
        public FormCardScanner()
        {
            InitializeComponent();
            // Update the items in the drop down
            this.getItemIds();
            // Create an barcode scannner and pass in the desired textbox target
            BarcodeScanner barcodeScanner = new BarcodeScanner(comboBox1);
            barcodeScanner.BarcodeScanned += BarcodeScanner_BarcodeScanned;
        }
        // Method called when the barcode scanner is scans a barcode
        private void BarcodeScanner_BarcodeScanned(object sender, BarcodeScannerEventArgs e)
        {
            this.resetErrors();
            label3.ForeColor = Colors.heading;
            // If an item is selected
            if (this.itemId != 0)
            {
                // Change the text of the combobox to the currentItem
                comboBox1.Text = this.currentItem;
                // Get the card
                DatabaseConnection databaseConnection = new DatabaseConnection();
                string query = "Select * from card where ticket_number = " + e.Barcode;
                MySqlDataReader reader = databaseConnection.Query(query);
                // If the card does not exist, display error
                if (!reader.Read())
                {
                    label3.Text = "Card " + e.Barcode + " does not exist";
                    label3.ForeColor = Color.FromArgb(192, 0, 0);
                }
                // Otherwise, check if the amount of tokens is greater or equal to the amount of tokens required
                // adn update the card value by subtracting the amount of tokens required from the amount
                // Also display the amount of tokens left in the card
                else
                {
                    int tokens = (int)reader["token_quantity"];
                    if (tokens >= this.itemTokensRequired)
                    {
                        int updatedCardValue = tokens - this.itemTokensRequired;
                        this.updateCard(e.Barcode, updatedCardValue);
                        label3.Text = "Tokens Available: " + (tokens - this.itemTokensRequired).ToString();
                        label3.ForeColor = Color.Green;
                    }
                    // Display that tokens are insufficient
                    else
                    {
                        label3.Text = "Tokens are not sufficient: " + tokens.ToString() + " tokens";
                        label3.ForeColor = Color.FromArgb(192, 0, 0);
                    }
                }
            }
            // Need to select an item
            else
            {
                labelError.Text = "Please select an item";
            }
        }
        // Update a card's token quantity based on it's card number
        private void updateCard(string cardNumber, int updatedValue)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "UPDATE card SET token_quantity = " + updatedValue.ToString() + " WHERE ticket_number = " + cardNumber;
            MySqlDataReader reader = databaseConnection.Query(query);
        }

        private void getItemIds()
        {
            // Clear all items and and get current item from the database
            comboBox1.Items.Clear();

            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "Select * from event_item where event_id = " + SessionInfo.id;
            MySqlDataReader reader = databaseConnection.Query(query);
            // Pass the items to the set list view method
            this.setListView(reader);
        }

        private void setListView(MySqlDataReader r)
        {
            // Set the display member and value member to the combobox
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
            // Go through each item that was passed in
            while (r.Read())
            {
                DatabaseConnection databaseConnection = new DatabaseConnection();
                string query = "Select * from item where id = " + r["item_id"].ToString();
                MySqlDataReader reader = databaseConnection.Query(query);

                reader.Read();
                // Add item to combobox
                comboBox1.Items.Add(new ComboboxValue(Convert.ToInt32(reader["id"].ToString()), reader["name"].ToString()));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When an item is selected
            ComboboxValue tmpComboboxValue = (ComboboxValue)comboBox1.SelectedItem;
            this.itemId = tmpComboboxValue.Id;
            // Get it from the database and set the tokens required
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "Select * from item where id = " + this.itemId;
            MySqlDataReader reader = databaseConnection.Query(query);

            reader.Read();

            label2.Text = "Tokens Required: " + reader["price"];
            this.itemTokensRequired = (int)reader["price"];
            this.currentItem = reader["name"].ToString();
        }

        private void resetErrors()
        {
            labelError.Text = "";
        }
    }

    
}
