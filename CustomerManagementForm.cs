
using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Data;

public class CustomerManagementForm : Form
{
    private DataGridView dataGridView;
    private Button addButton, updateButton, deleteButton;

    public CustomerManagementForm()
    {
        Text = "Customer Management";
        Size = new System.Drawing.Size(600, 400);
        StartPosition = FormStartPosition.CenterScreen;

        dataGridView = new DataGridView { Dock = DockStyle.Top, Height = 300 };
        addButton = new Button { Text = "Add", Location = new System.Drawing.Point(10, 320) };
        updateButton = new Button { Text = "Update", Location = new System.Drawing.Point(100, 320) };
        deleteButton = new Button { Text = "Delete", Location = new System.Drawing.Point(190, 320) };

        addButton.Click += AddButton_Click;
        updateButton.Click += UpdateButton_Click;
        deleteButton.Click += DeleteButton_Click;

        Controls.Add(dataGridView);
        Controls.Add(addButton);
        Controls.Add(updateButton);
        Controls.Add(deleteButton);

        LoadCustomers();
    }

    private void LoadCustomers()
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=inventory.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Customers";
                using (var reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    dataGridView.DataSource = table;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        using (var form = new CustomerForm("Add Customer"))
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO Customers (Name, Email, Phone) VALUES ($name, $email, $phone)";
                        command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                        command.Parameters.AddWithValue("$email", form.emailTextBox.Text);
                        command.Parameters.AddWithValue("$phone", form.phoneTextBox.Text);
                        command.ExecuteNonQuery();
                    }
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void UpdateButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = dataGridView.SelectedRows[0];
            int customerId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

            using (var form = new CustomerForm("Edit Customer"))
            {
                form.nameTextBox.Text = Convert.ToString(selectedRow.Cells["Name"].Value);
                form.emailTextBox.Text = Convert.ToString(selectedRow.Cells["Email"].Value);
                form.phoneTextBox.Text = Convert.ToString(selectedRow.Cells["Phone"].Value);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var connection = new SqliteConnection("Data Source=inventory.db"))
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText = "UPDATE Customers SET Name = $name, Email = $email, Phone = $phone WHERE Id = $id";
                            command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                            command.Parameters.AddWithValue("$email", form.emailTextBox.Text);
                            command.Parameters.AddWithValue("$phone", form.phoneTextBox.Text);
                            command.Parameters.AddWithValue("$id", customerId);
                            command.ExecuteNonQuery();
                        }
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to update customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a customer to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            if (MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int customerId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "DELETE FROM Customers WHERE Id = $id";
                        command.Parameters.AddWithValue("$id", customerId);
                        command.ExecuteNonQuery();
                    }
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a customer to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
