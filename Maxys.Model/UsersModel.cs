namespace ERP_MaxysHC.Maxys.Model
{
    public class UsersModel
    {
        public int Id_user { get; set; }
        public string User_name { get; set; }
        public string User_lastname { get; set; }
        public string Position { get; set; }
        public string User { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UsersLogin
    {
        public string? User { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }
}
