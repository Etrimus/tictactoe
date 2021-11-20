namespace TicTacToe.Domain
{
    public class UserEntity : DbEntityBase
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}