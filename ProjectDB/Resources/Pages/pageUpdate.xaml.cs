using MySqlConnector;

namespace ProjectDB.Resources.Pages;

public partial class pageUpdate : ContentPage
{
    public event EventHandler StudentUpdated;
    public pageUpdate()
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

    private async void updateBtn_Clicked(object sender, EventArgs e)
    {
        string connectionString = "Server=MYSQL8003.site4now.net;Database=db_aa282a_mauidb;Uid=aa282a_mauidb;Pwd=winterfell11";

        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        string query = "UPDATE STUDENTS SET GROUP_S = " + (pickerGroup.SelectedIndex + 1) + " WHERE ID = " + Convert.ToInt32(keySearch.Text);
        MySqlCommand command = new MySqlCommand(query, connection);
        int rowsAffected = await command.ExecuteNonQueryAsync();
        await DisplayAlert("Success", "Student added successfully", "Ok");
        StudentUpdated?.Invoke(this, EventArgs.Empty);

        await Navigation.PopAsync();
    }

    private async void keySearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string connectionString = "Server=MYSQL8003.site4now.net;Database=db_aa282a_mauidb;Uid=aa282a_mauidb;Pwd=winterfell11";

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "SELECT * FROM STUDENTS WHERE ID = " + Convert.ToInt32(keySearch.Text);
            MySqlCommand command = new MySqlCommand(query, connection);
            using MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                name.Text = reader.GetString(1);    
                lastname1.Text  = reader.GetString(2);
                lastname2.Text = reader.GetString(3);   
                age.Text = reader.GetInt32(4) + "";
                pickerGroup.SelectedIndex = reader.GetInt32(6) - 1;
                updateBtn.IsEnabled = true;
            } else
            {
                name.Text = "";
                lastname1.Text = "";
                lastname2.Text = "";
                age.Text = "";
                pickerGroup.SelectedIndex = 0;  
                updateBtn.IsEnabled = false;
            }
        } catch {
            if (keySearch.Text.Length > 0) {
                await DisplayAlert("Error", "Enter a correct ID", "Ok");
            } else
            {
                name.Text = "";
                lastname1.Text = "";
                lastname2.Text = "";
                age.Text = "";
                pickerGroup.SelectedIndex = 0;
                updateBtn.IsEnabled = false;
            }
        }

    }
}