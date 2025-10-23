
using System;
using System.Windows.Forms;

public class ProductForm : Form
{
    public TextBox nameTextBox, quantityTextBox, priceTextBox;
    private Button saveButton, cancelButton;

    public ProductForm(string title = "Add Product")
    {
        Text = title;
        Size = new System.Drawing.Size(300, 250);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;

        nameTextBox = new TextBox { Location = new System.Drawing.Point(50, 30), Width = 200 };
        quantityTextBox = new TextBox { Location = new System.Drawing.Point(50, 70), Width = 200 };
        priceTextBox = new TextBox { Location = new System.Drawing.Point(50, 110), Width = 200 };

        saveButton = new Button { Text = "Save", Location = new System.Drawing.Point(50, 150), DialogResult = DialogResult.OK };
        cancelButton = new Button { Text = "Cancel", Location = new System.Drawing.Point(160, 150), DialogResult = DialogResult.Cancel };

        Controls.Add(new Label { Text = "Name:", Location = new System.Drawing.Point(50, 10) });
        Controls.Add(nameTextBox);
        Controls.Add(new Label { Text = "Quantity:", Location = new System.Drawing.Point(50, 50) });
        Controls.Add(quantityTextBox);
        Controls.Add(new Label { Text = "Price:", Location = new System.Drawing.Point(50, 90) });
        Controls.Add(priceTextBox);
        Controls.Add(saveButton);
        Controls.Add(cancelButton);

        AcceptButton = saveButton;
        CancelButton = cancelButton;
    }
}
