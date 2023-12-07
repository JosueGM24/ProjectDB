using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using MySqlConnector;
using ProjectDB.Resources.Pages;


namespace ProjectDB
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            var backgroundImage = new Image
            {
                Source = "tu_imagen.png", 
                Aspect = Aspect.AspectFill 
            };
            ImageSource imgs = "newbg.jpg";
            
            contentPageLogin.BackgroundImageSource = imgs;
        }

        [Obsolete]
        private async void Log_in(object sender, EventArgs e)
        {
            loader.IsVisible = true;

                string connectionString = "Server=MYSQL8003.site4now.net;Database=db_aa282a_mauidb;Uid=aa282a_mauidb;Pwd=winterfell11";

                using MySqlConnection connection = new MySqlConnection(connectionString);

                try
                {
                    await connection.OpenAsync();
                    string localUsername = userEntry.Text;
                    string localPassword = passwordEntry.Text;
                    if (string.IsNullOrEmpty(localUsername))
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await DisplayAlert("You are baboso?", "Enter your username.", "Ok");
                        });
                    }
                    else
                    {
                        string query = $"SELECT * FROM USERS WHERE USERNAME = '{localUsername}' AND PASSWORD = '" + localPassword + "'";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        using MySqlDataReader reader = await command.ExecuteReaderAsync();

                        if (await reader.ReadAsync())
                        {
                            PageAdministrator pageAdministrator = new PageAdministrator(reader.GetInt32(0));
                            await Navigation.PushAsync(pageAdministrator);
                            imageLogin.Source = "";
                            userEntry.Text = "";
                            passwordEntry.Text = "";
                            loader.IsVisible = false;
                            errorMessage.Text = "";
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                errorMessage.Text = "User or password incorrect";
                                imageLogin.Source = "error.png";
                            });

                        }
                    }
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        errorMessage.Text = ex.Message;
                    });
                }
                connection.Close();
        }

        [Obsolete]
        private async void continueBtn_Clicked(object sender, EventArgs e)
        {
            loader.IsVisible = true;

            await Task.Run(async () =>
            {
                string connectionString = "Server=MYSQL8003.site4now.net;Database=db_aa282a_mauidb;Uid=aa282a_mauidb;Pwd=winterfell11";

                using MySqlConnection connection = new MySqlConnection(connectionString);

                try
                {
                    await connection.OpenAsync();
                    string localUsername = userEntry.Text;
                        string query = $"SELECT * FROM USERS WHERE USERNAME = '{localUsername}'";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        using MySqlDataReader reader = await command.ExecuteReaderAsync();

                        if (await reader.ReadAsync())
                        {
                            string picture = "picture" + reader.GetInt32(4).ToString() + ".png";
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                errorMessage.Text = "";
                                imageLogin.Source = picture;
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                errorMessage.Text = "User no found";
                                imageLogin.Source = "error.png";
                            });

                        }
                    
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        errorMessage.Text = ex.Message;
                    });
                }
                finally
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        loader.IsVisible = false; 
                    });
                }
                connection.Close();
            });
        }

        private void signUpBtn_Clicked(object sender, EventArgs e)
        {

        }
    }

}
