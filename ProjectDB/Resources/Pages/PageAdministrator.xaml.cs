using Microsoft.Maui.Controls;
using MySqlConnector;

namespace ProjectDB.Resources.Pages;

public partial class PageAdministrator : ContentPage
{
    public PageAdministrator()
    {
        InitializeComponent();
    }
    public List<string> listColors = new List<string>() { "#3F56EC", "#EC7E3F", "#EC3F76", "#3F95EC", "#ECBB3F" };

    [Obsolete]
    public PageAdministrator(int id)
    {
        InitializeComponent();
        string connectionString = "Server=MYSQL8003.site4now.net;Database=db_aa282a_mauidb;Uid=aa282a_mauidb;Pwd=winterfell11";

        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        string query = "SELECT * FROM USERS WHERE ID = " + id;
        MySqlCommand command = new MySqlCommand(query, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            welcomed.Text = "Hola " + reader.GetString(1);
            if (reader.GetInt32(3) >= 1)
            {
                priviliges.Text = "User";
                PanelPage.Title = "Page User";
                btnDelete.IsVisible = false;
                btnUpdate.IsVisible = false;
                nodePrivileges.BackgroundColor = Color.FromArgb("#FAC99B");
            } else
            {
                priviliges.Text = "Administrator";
                PanelPage.Title = "Page Administrator";
                nodePrivileges.BackgroundColor = Color.FromArgb("#B89BFA");
            }
        } else
        {
            priviliges.Text = ("Error");
        }
        reader.Close();
        connection.Close();
        RefreshData();
        
    }

    private MySqlConnection getConnection()
    {
        string connectionString = "Server=MYSQL8003.site4now.net;Database=db_aa282a_mauidb;Uid=aa282a_mauidb;Pwd=winterfell11";

        MySqlConnection connection = new MySqlConnection(connectionString);
        return connection;
    }

    [Obsolete]
    public void RefreshData()
    {
        ContentStudents.Clear();
        MySqlConnection connection = getConnection();
        connection.Open();
        string query = "SELECT * FROM STUDENTS JOIN GROUPS_S ON GROUPS_S.ID = STUDENTS.GROUP_S";
        MySqlCommand command = new MySqlCommand(query, connection);
        using MySqlDataReader readerStudents = command.ExecuteReader();
        while (readerStudents.Read())
        {
            Frame fr1 = new Frame();
            Label lb = new Label();
            lb.Text = readerStudents.GetInt32(0) +
                "\n" + readerStudents.GetString(1) +
                "\n" + readerStudents.GetString(2) +
                " " + readerStudents.GetString(3) + "\n"
                + readerStudents.GetInt32(4) 
                + "\n" + readerStudents.GetString(8);
            lb.VerticalOptions = LayoutOptions.CenterAndExpand;
            lb.HorizontalOptions = LayoutOptions.CenterAndExpand;
            lb.FontSize = 18;
            fr1.Content = lb;
            fr1.Margin = 20;
            fr1.BackgroundColor = Color.FromArgb("#" + readerStudents.GetString(5));
            fr1.WidthRequest = 200;
            fr1.HeightRequest = 180;
            ContentStudents.Add(fr1);
        }
        readerStudents.Close();
        connection.Close();
    }

    [Obsolete]
    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        pageAdd pageAdd = new pageAdd();

        pageAdd.StudentAdded += async (s, args) =>
        {
            RefreshData();
        };

        await Navigation.PushAsync(pageAdd);
    }

    [Obsolete]
    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        string idToDelete = await DisplayPromptAsync("Delete student", "Enter to ID of student", "Ok");
        try
        {
            MySqlConnection connection = getConnection();
            connection.Open();
            string update = "DELETE FROM STUDENTS WHERE ID = " + Convert.ToInt32(idToDelete);
            MySqlCommand command = new MySqlCommand(update, connection);
            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                await DisplayAlert("Success", "Student deleted successfully", "Ok");
            }
            else
            {
                await DisplayAlert("Error", "ID not found or incorrect", "Ok");
            }
            connection.Close();
        } catch (Exception ex) {
            await DisplayAlert("Error", "ID no found or incorrect", "Ok");
        }
        RefreshData();

    }   

    private async void btnUpdate_Clicked(object sender, EventArgs e)
    {
        pageUpdate pageUpdate = new pageUpdate();

        pageUpdate.StudentUpdated += async (s, args) =>
        {
            RefreshData();
        };

        await Navigation.PushAsync(pageUpdate);
    }
}