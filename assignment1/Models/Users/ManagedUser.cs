using System.Data;
using assignment1.Libs;
using assignment1.Models.Generics;

namespace assignment1.Models.Users
{
    public class ManagedUser : UserBase
    {
        public string username;

        public ManagedUser(DataRow _row) : base(_row) { }

        public override int Id { get => base.Id; set => throw new InvalidOperationException(); }

        public override string Username { get => username; set => throw new InvalidOperationException(); }
        public override string Password { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        public override DateTime DateOfBirth { get => base.DateOfBirth; set => throw new InvalidOperationException(); }
        public override string Email { get => base.Email; set => throw new InvalidOperationException(); }
        public override string FirstName { get => base.FirstName; set => base.FirstName = value; }
        public override string LastName { get => base.LastName; set => base.LastName = value; }
        public override int UserTypeId { get => base.UserTypeId; set => base.UserTypeId = value; }
        public string UserTypeName { get => Persistent.user_type_table.FirstOrDefault(x => x.UserTypeId.Equals(this.UserTypeId)).UserTypeName; }
    }
}
