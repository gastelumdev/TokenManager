using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using MySql.Data.MySqlClient;
using TokenManagerUI.Forms;

namespace TokenManagerUI
{
    public partial class FormMainMenuUI : Form
    {
        // The current button selected
        private IconButton currentBtn;
        // Left border for the current button selected
        private Panel leftBorderBtn;
        // The current form selected that will display in the body panel
        private Form currentChildForm;
        private string label1Text = "";
        public FormMainMenuUI(string labelText = null)
        {
            InitializeComponent();
            // Set label text to the label text being passed in
            this.label1Text = labelText;

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);

            // Activate the currently selected button
            ActivateButton(dashboardIconButton, Colors.background);

            // Remove default title bar
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            // Display the dashboard form in body panel by default
            OpenChildForm(new FormAddItems());
        }

        //Structs
        private struct RGBColors
        {
            // Background Color
            public static Color background = Color.FromArgb(248, 249, 250);
        }

        // Activates button by changing the look
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                // Disable the current button
                DisableButton();
                // Create Current button
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Colors.panelBg;
                currentBtn.ForeColor = Colors.heading;
                currentBtn.IconColor = Colors.heading;

                //Left border button
                leftBorderBtn.BackColor = Colors.heading;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();

                //Current Child Form Icon
                iconCurrentChildForm.IconChar = currentBtn.IconChar;
            }
        }
        // Disable the current button by changing the look
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Colors.background;
                currentBtn.ForeColor = Colors.heading;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Colors.heading;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }
        // Open the form to be displayed in the body panel when it is selected from the navigation
        private void OpenChildForm(Form childForm)
        {
            //open only form
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitleChildForm.Text = this.label1Text + " - " + childForm.Text;
        }
        // When an icon button is clicked in the nav bar, activate the button and open the form
        private void dashboardIconButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Colors.background);
            OpenChildForm(new FormAddItems());
        }

        private void itemIconButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Colors.background);
            OpenChildForm(new FormItems());
        }

        private void cardsIconButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Colors.background);
            OpenChildForm(new FormCards());
        }

        private void cardScannerIconButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Colors.background);
            OpenChildForm(new FormCardScanner());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void titleBarPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //Remove transparent border in maximized state
        private void FormMainMenu_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();

            FormDashboard form = new FormDashboard();
            form.Show();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }

    
}
