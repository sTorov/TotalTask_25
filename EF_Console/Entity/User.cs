namespace EF_Console.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is User user)
            {
                if (user.Id != Id) return false;
                if (user.Name != Name) return false;
                if (user.Email != Email) return false;
                return true;
            }
            return false;
        }
    }
}
