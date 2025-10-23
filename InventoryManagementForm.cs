
using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Data;

public class InventoryManagementForm : Form
{
    private DataGridView dataGridView;
    private Button addButton, updateButton, deleteButton;

    public InventoryManagementForm()
    {
        Text = "Inventory Management";
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

        LoadProducts();
    }

    private void LoadProducts()
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=inventory.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Products";
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
            MessageBox.Show($"Failed to load products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        using (var form = new ProductForm("Add Product"))
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO Products (Name, Quantity, Price) VALUES ($name, $quantity, $price)";
                        command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                        command.Parameters.AddWithValue("$quantity", int.Parse(form.quantityTextBox.Text));
                        command.Parameters.AddWithValue("$price", decimal.Parse(form.priceTextBox.Text));
                        command.ExecuteNonQuery();
                    }
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void UpdateButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = dataGridView.SelectedRows[0];
            int productId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

            using (var form = new ProductForm("Edit Product"))
            {
                form.nameTextBox.Text = Convert.ToString(selectedRow.Cells["Name"].Value);
                form.quantityTextBox.Text = Convert.ToString(selectedRow.Cells["Quantity"].Value);
                form.priceTextBox.Text = Convert.ToString(selectedRow.Cells["Price"].Value);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var connection = new SqliteConnection("Data Source=inventory.db"))
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText = "UPDATE Products SET Name = $name, Quantity = $quantity, Price = $price WHERE Id = $id";
                            command.Parameters.AddWithValue("$name", form.nameTextBox.Text);
                            command.Parameters.AddWithValue("$quantity", int.Parse(form.quantityTextBox.Text));
                            command.Parameters.AddWithValue("$price", decimal.Parse(form.priceTextBox.Text));
                            command.Parameters.AddWithValue("$id", productId);
                            command.ExecuteNonQuery();
                        }
                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to update product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a product to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            if (MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int productId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
                    using (var connection = new SqliteConnection("Data Source=inventory.db"))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "DELETE FROM Products WHERE Id = $id";
                        command.Parameters.AddWithValue("$id", productId);
                        command.ExecuteNonQuery();
                    }
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a product to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
