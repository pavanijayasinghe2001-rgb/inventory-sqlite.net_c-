
using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

public class RegistrationForm : Form
{
    private TextBox usernameTextBox;
    private TextBox passwordTextBox;
    private Button registerButton;

    public RegistrationForm()
    {
        Text = "Register";
        Size = new System.Drawing.Size(300, 200);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;

        usernameTextBox = new TextBox { Location = new System.Drawing.Point(50, 30), Width = 200 };
        passwordTextBox = new TextBox { Location = new System.Drawing.Point(50, 70), Width = 200, PasswordChar = '*' };
        registerButton = new Button { Text = "Register", Location = new System.Drawing.Point(100, 110), Width = 100 };

        registerButton.Click += RegisterButton_Click;

        Controls.Add(new Label { Text = "Username:", Location = new System.Drawing.Point(50, 10) });
        Controls.Add(usernameTextBox);
        Controls.Add(new Label { Text = "Password:", Location = new System.Drawing.Point(50, 50) });
        Controls.Add(passwordTextBox);
        Controls.Add(registerButton);
    }

    private void RegisterButton_Click(object? sender, EventArgs e)
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
                command.CommandText = "INSERT INTO Users (Username, Password) VALUES ($username, $password)";
                command.Parameters.AddWithValue("$username", username);
                command.Parameters.AddWithValue("$password", password); // In a real app, hash the password!

                command.ExecuteNonQuery();

                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
