namespace MessengerRepositories.Dtos
{
    public class UserDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public UserStates? State { get; set; } = UserStates.Offline;
    }
}