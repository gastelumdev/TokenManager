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
using USB_Barcode_Scanner;

namespace TokenManagerUI.Forms
{
    public partial class FormCards : Form
    {
        private string barcode;
        private string cardId;
        private string cardNumber;
        private int tokenAmount = 0;
        public FormCards()
        {
            InitializeComponent();
            // Create an barcode scannner and pass in the desired textbox target
            BarcodeScanner barcodeScanner = new BarcodeScanner(textBox1);
            barcodeScanner.BarcodeScanned += BarcodeScanner_BarcodeScanned;
            
        }
        // Method called when the barcode scanner is scans a barcode
        private void BarcodeScanner_BarcodeScanned(object sender, BarcodeScannerEventArgs e)
        {
            // Set the barcode number to the textbox and set the class variable
            textBox1.Text = e.Barcode;
            this.barcode = e.Barcode;
            // Find the card based on the card number scanned
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "Select * from card where ticket_number = " + e.Barcode + " AND event_id = " + SessionInfo.id;
            MySqlDataReader reader = databaseConnection.Query(query);
            // If nothing found set the instance card number to the barcode number scanned
            if (!reader.Read())
            {
                label4.Text = "0";
                this.cardId = null;
                this.cardNumber = this.barcode;
                this.tokenAmount = 0;
            }
            else
            {
                // Set the card to the card from the database
                label4.Text = reader["token_quantity"].ToString();
                this.cardId = reader["id"].ToString();
                this.cardNumber = reader["ticket_number"].ToString();
                this.tokenAmount = Convert.ToInt32(reader["token_quantity"].ToString());
            }
        }

        private string getTokenAmount(string cardId)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string query = "Select * from card where id = " + cardId;
            MySqlDataReader reader = databaseConnection.Query(query);

            reader.Read();
            return reader["token_quantity"].ToString();
        }

        private void FormCards_Activated(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Set validation errors
            bool validated = true;
            string error = "Required";
            this.resetErrors();
            int tokenValue = 0;
            // If any of the fields are empty set to not validated
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
            else
            {
                // also check if the value is 1 or more
                try
                {
                    tokenValue = Convert.ToInt32(textBox2.Text);
                    if (tokenValue < 1)
                    {
                        validated = false;
                        labelError2.Text = "Token amount should be 1 or more";
                    }
                // Display error
                } catch (Exception err)
                {
                    labelError2.Text = err.Message;
                    textBox2.Text = "0";
                }
                
            }
            // If everything checks out update of insert into database 
            if (validated)
            {
                this.tokenAmount += Convert.ToInt32(textBox2.Text);

                DatabaseConnection databaseConnection = new DatabaseConnection();

                if (this.cardId == null)
                {
                    string query = "INSERT INTO card (id, event_id, ticket_number, token_quantity) VALUES (NULL, " + SessionInfo.id + ", " + this.cardNumber + ", " + this.tokenAmount + ")";
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnection.connect());
                    MySqlDataReader reader = cmd.ExecuteReader();
                }
                else
                {
                    string query = "UPDATE card SET event_id = " + SessionInfo.id + ", ticket_number = " + this.cardNumber + ", token_quantity = " + this.tokenAmount + " WHERE id = " + this.cardId;
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnection.connect());
                    MySqlDataReader reader = cmd.ExecuteReader();
                }

                label4.Text = this.tokenAmount.ToString();
            }
        }

        private void resetErrors()
        {
            labelError.Text = "";
            labelError2.Text = "";
        }
    }

    
}
