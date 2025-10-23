
using System;
using System.Windows.Forms;

public class MainForm : Form
{
    private Button inventoryButton;
    private Button customerButton;
    private Button employeeButton;

    public MainForm()
    {
        Text = "Main Menu";
        Size = new System.Drawing.Size(400, 300);
        StartPosition = FormStartPosition.CenterScreen;

        inventoryButton = new Button { Text = "Manage Inventory", Location = new System.Drawing.Point(50, 50), Width = 300, Height = 50 };
        customerButton = new Button { Text = "Manage Customers", Location = new System.Drawing.Point(50, 110), Width = 300, Height = 50 };
        employeeButton = new Button { Text = "Manage Employees", Location = new System.Drawing.Point(50, 170), Width = 300, Height = 50 };

        inventoryButton.Click += (s, e) => new InventoryManagementForm().ShowDialog();
        customerButton.Click += (s, e) => new CustomerManagementForm().ShowDialog();
        employeeButton.Click += (s, e) => new EmployeeManagementForm().ShowDialog();

        Controls.Add(inventoryButton);
        Controls.Add(customerButton);
        Controls.Add(employeeButton);
    }
}
