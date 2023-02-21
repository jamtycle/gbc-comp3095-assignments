using System.Data;

namespace assignment1.Models.Generics
{
    public abstract class UserBase : ModelBase
    {
        private int id;

        public UserBase() : base() { }

        public UserBase(DataRow _row) : base(_row)
        {
            this.FromDataRow(_row);
        }

        public virtual int Id { get => id; set => id = value; }

        public abstract string Username { get; set; }
        public abstract string Password { get; set; }

        public virtual int UserTypeId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string SessionCookie { get; set; }
        public virtual string MachineName { get; set; }
        public virtual byte[] ProfilePic { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
    }
}