using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace InventoryManagementSystem;

public partial class LoginForm : Form
{
    private TextBox usernameTextBox;
    private TextBox passwordTextBox;
    private Button loginButton;
    private Button registerButton;

    public LoginForm()
    {
        Text = "Login";
        Size = new System.Drawing.Size(300, 250);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;

        usernameTextBox = new TextBox { Location = new System.Drawing.Point(50, 30), Width = 200 };
        passwordTextBox = new TextBox { Location = new System.Drawing.Point(50, 70), Width = 200, PasswordChar = '*' };
        loginButton = new Button { Text = "Login", Location = new System.Drawing.Point(50, 110), Width = 90 };
        registerButton = new Button { Text = "Register", Location = new System.Drawing.Point(160, 110), Width = 90 };

        loginButton.Click += LoginButton_Click;
        registerButton.Click += RegisterButton_Click;

        Controls.Add(new Label { Text = "Username:", Location = new System.Drawing.Point(50, 10) });
        Controls.Add(usernameTextBox);
        Controls.Add(new Label { Text = "Password:", Location = new System.Drawing.Point(50, 50) });
        Controls.Add(passwordTextBox);
        Controls.Add(loginButton);
        Controls.Add(registerButton);
    }

    private void LoginButton_Click(object? sender, EventArgs e)
    {
        string username = usernameTextBox.Text;
        string password = passwordTextBox.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Username and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            using (var connection = new SqliteConnection("Data Source=inventory.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM Users WHERE Username = $username AND Password = $password";
                command.Parameters.AddWithValue("$username", username);
                command.Parameters.AddWithValue("$password", password); // In a real app, hash the password!

                var result = command.ExecuteScalar();

                if (result != null && (long)result == 1)
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Open the main application window here
                    this.Hide();
                    var mainForm = new MainForm();
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Login failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RegisterButton_Click(object? sender, EventArgs e)
    {
        var registrationForm = new RegistrationForm();
        registrationForm.ShowDialog();
    }
}
