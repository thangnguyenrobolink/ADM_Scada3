using ADM_Scada.Cores.Models;
using ADM_Scada.Cores.PubEvent;
using ADM_Scada.Modules.User.Repositories;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADM_Scada.Modules.User.ViewModels
{
    public class UserLoginViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private string password;
        private string resCode;
        private string loginname;
        private string imagePath;
        private UserModel selectedUser;
        private UserModel currentUser;
        private ObservableCollection<UserModel> users;
        private string resLoginName;
        private int resLevel;
        private string resPassword;
        private string resFullName;

        public ObservableCollection<UserModel> Users
        {
            get => users;
            set => SetProperty(ref users, value);
        }
        public UserModel CurrentUser { get => currentUser ?? new UserModel() { FullName = "Guest", LoginName = "Guest", Level = 0, Avatar = "Avatar.jpeg" }; set => SetProperty(ref currentUser, value); }
        public UserModel SelectedUser { get => selectedUser; set => SetProperty(ref selectedUser, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string LoginName { get => loginname; set => SetProperty(ref loginname, value); }
        public string ResLoginName { get => resLoginName; set => SetProperty(ref resLoginName, value); }
        public int ResLevel { get => resLevel; set => SetProperty(ref resLevel, value); }
        public string ResCode { get => resCode; set => SetProperty(ref resCode, value); }
        public string ResPassword { get => resPassword; set => SetProperty(ref resPassword, value); }
        public string ResFullName { get => resFullName; set => SetProperty(ref resFullName, value); }
        public string ImagePath { get => imagePath ?? "Avatar.jpeg"; set => SetProperty(ref imagePath, value); }

        private readonly UserRepository userRepository;

        public DelegateCommand<UserModel> EditCommand { get; private set; }
        public DelegateCommand<UserModel> DeleteCommand { get; private set; }
        public DelegateCommand ImageBrowseCommand { get; private set; }
        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand AddUserCommand { get; set; }
        private void Edit(UserModel selectedModel)
        {
            // Handle the Edit button click
            if (selectedModel != null)
            {
                int index = Users.IndexOf(selectedModel);
                Users[index] = selectedModel;
                Task task1 = Task.Run(async () =>
                    {
                        _ = await userRepository.Update(selectedModel);
                        Users = new ObservableCollection<UserModel>((IEnumerable<UserModel>)await userRepository.GetAll());
                    });
                task1.Wait();
            }
            UpdateSecur();
        }
        private void Delete(UserModel selectedModel)
        {
            if (selectedModel != null)
            {
                _ = Users.Remove(selectedModel);
                _ = userRepository.Delete(selectedModel.Id);
                Users = new ObservableCollection<UserModel>();
                _ = Task.Run(async () => Users = new ObservableCollection<UserModel>((IEnumerable<UserModel>)await userRepository.GetAll()));
            }
        }
        private bool CanDelete(UserModel selectedModel)
        {
            // CanExecute condition for DeleteCommand
            return Users.Count > 1;
        }
        private void ImageBrowse()
        {

            var openFileDialog1 = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff;*.bmp|All files|*.*",
                InitialDirectory = "\\"
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<string> ImagePathList = new List<string>
                {
                    openFileDialog1.FileName
                };
                ImagePath = ImagePathList[0];
            }
        }
        private void UpdateSecur()
        {
            foreach (UserModel User in Users)
            {
                User.IsEnable = CurrentUser.Level > User.Level || User.LoginName == CurrentUser.LoginName;
            }
            Users = Users;
        }
        private async void AddUser()
        {
            // Validation: Check if required properties are not empty
            if (string.IsNullOrWhiteSpace(ResLoginName)
                || string.IsNullOrWhiteSpace(ResFullName)
                || string.IsNullOrWhiteSpace(ResCode)
                || string.IsNullOrWhiteSpace(ResPassword))
            {
                // Handle validation error (e.g., show a message)
                // You can also throw an exception or handle it based on your application's requirements.
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Create a new user based on the provided properties
            UserModel newUser = new UserModel
            {
                LoginName = ResLoginName,
                FullName = ResFullName,
                Code = ResCode,
                Level = ResLevel, // Assuming ResLevel is a string, convert it to int
                Password = ResPassword,
                Avatar = ImagePath ?? "Avatar.jpeg"
                // You may need to set other properties based on your UserModel definition
            };

            int newUserId = await userRepository.Create(newUser);

            // Set the generated Id to the new user
            newUser.Id = newUserId;

            // Add the new user to the Users collection
            Users.Add(newUser);
            // Optionally, you can reset the properties after adding a new user
            ResLoginName = string.Empty;
            ResFullName = string.Empty;
            ResCode = string.Empty;
            ResLevel = 0;
            ResPassword = string.Empty;
        }
        private void Login()
        {
            // Validation: Check if login credentials are valid
            if (string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(Password))
            {
                // Handle validation error (e.g., show a message)
                MessageBox.Show("Please enter both login name and password.");
                return;
            }

            // Check if a user with the provided login name and password exists
            UserModel user = Users.FirstOrDefault(u =>
                u.LoginName.Equals(LoginName, StringComparison.OrdinalIgnoreCase) &&
                u.Password == Password);

            if (user != null)
            {
                // Successful login, perform the login action (e.g., navigate to the main application screen)
                _ = MessageBox.Show("Login successful!");
                CurrentUser = user;
                UpdateSecur();
                // You can add additional logic here, such as navigation to the main application screen
                eventAggregator.GetEvent<UserLoginEvent>().Publish(CurrentUser);
            }
            else
            {
                // Invalid login credentials, handle accordingly (e.g., show an error message)
                _ = MessageBox.Show("Invalid login credentials. Please try again.");
            }
        }
        public UserLoginViewModel(IEventAggregator ea)
        {
            eventAggregator = ea;
            userRepository = new UserRepository();
            Users = new ObservableCollection<UserModel>();
            _ = Task.Run(async () => Users = new ObservableCollection<UserModel>((IEnumerable<UserModel>)await userRepository.GetAll()));
            EditCommand = new DelegateCommand<UserModel>(Edit);
            DeleteCommand = new DelegateCommand<UserModel>(Delete, CanDelete)
                .ObservesProperty(() => Users.Count);
            AddUserCommand = new DelegateCommand(AddUser);
            ImageBrowseCommand = new DelegateCommand(ImageBrowse);
            LoginCommand = new DelegateCommand(Login);
        }

    }
}
