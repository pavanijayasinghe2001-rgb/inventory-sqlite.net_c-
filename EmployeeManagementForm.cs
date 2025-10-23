
using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Data;

public class EmployeeManagementForm : Form
{
    private DataGridView dataGridView;
    private Button addButton, updateButton, deleteButton;

    public EmployeeManagementForm()
    {
        Text = "Employee Management";
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

        LoadEmployees();
    }

    private void LoadEmployees()
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=inventory.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Employees";
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
            MessageBox.Show($"Failed to load employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        using (var form = new EmployeeForm("Add Employee"))
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO Employees (Name, Position, Salary) VALUES ($name, $position, $salary)";
                        command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                        command.Parameters.AddWithValue("$position", form.positionTextBox.Text);
                        command.Parameters.AddWithValue("$salary", decimal.Parse(form.salaryTextBox.Text));
                        command.ExecuteNonQuery();
                    }
                    LoadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void UpdateButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = dataGridView.SelectedRows[0];
            int employeeId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

            using (var form = new EmployeeForm("Edit Employee"))
            {
                form.nameTextBox.Text = Convert.ToString(selectedRow.Cells["Name"].Value);
                form.positionTextBox.Text = Convert.ToString(selectedRow.Cells["Position"].Value);
                form.salaryTextBox.Text = Convert.ToString(selectedRow.Cells["Salary"].Value);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var connection = new SqliteConnection("Data Source=inventory.db"))
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText = "UPDATE Employees SET Name = $name, Position = $position, Salary = $salary WHERE Id = $id";
                            command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                            command.Parameters.AddWithValue("$position", form.positionTextBox.Text);
                            command.Parameters.AddWithValue("$salary", decimal.Parse(form.salaryTextBox.Text));
                            command.Parameters.AddWithValue("$id", employeeId);
                            command.ExecuteNonQuery();
                        }
                        LoadEmployees();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to update employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show("Please select an employee to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            if (MessageBox.Show("Are you sure you want to delete this employee?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int employeeId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "DELETE FROM Employees WHERE Id = $id";
                        command.Parameters.AddWithValue("$id", employeeId);
                        command.ExecuteNonQuery();
                    }
                    LoadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please select an employee to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
