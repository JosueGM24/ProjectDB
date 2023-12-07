using Microsoft.Maui.Controls;
using MySqlConnector;

namespace ProjectDB.Resources.Pages;

public partial class pageAdd : ContentPage
{
    public event EventHandler StudentAdded;
    public List<string> listColors = new List<string>() { "3F56EC", "EC7E3F", "EC3F76", "3F95EC", "ECBB3F" };
    public pageAdd()
	{
		InitializeComponent();
        string connectionString = "Server=MYSQL8003.site4now.net;Database=db_aa282a_mauidb;Uid=aa282a_mauidb;Pwd=winterfell11";
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        string query = "SELECT * FROM GROUPS_S";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader readerGroups = command.ExecuteReader();
        List<string> groups = new List<string>();
        while (readerGroups.Read())
        {
            groups.Add(readerGroups.GetString(1));
        }
        readerGroups.Close();
        connection.Close();
        foreach (string group in groups)
        {
            pickerGroup.Items.Add(group);
        }

    }

    private async void cancelBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void addBtn_Clicked(object sender, EventArgs e)
    {
        string connectionString = "Server=MYSQL8003.site4now.net;Database=db_aa282a_mauidb;Uid=aa282a_mauidb;Pwd=winterfell11";
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        Random random = new();
        int randomIndex = random.Next(0, listColors.Count);
        string query = "INSERT INTO STUDENTS VALUES(null,'" +
            name.Text + "','" + 
            lastname1.Text + "','" + lastname2.Text + "'," + Convert.ToInt32(age.Text) + ",'" + listColors[randomIndex] + "', " + 
            (pickerGroup.SelectedIndex + 1) + ")";
        MySqlCommand command = new MySqlCommand(query, connection);
        int rowsAffected = await command.ExecuteNonQueryAsync();
        await DisplayAlert("Success", "Student added successfully", "Ok");
        StudentAdded?.Invoke(this, EventArgs.Empty);

        await Navigation.PopAsync();
    }
}