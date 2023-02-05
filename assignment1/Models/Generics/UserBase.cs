using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Interfaces;

namespace assignment1.Models.Generics
{
    public abstract class UserBase : IDataRow
    {
        public UserBase()
        {

        }

        public UserBase(DataRow _row)
        {
            if (!this.FromDataRow(_row)) throw new FormatException("The row was not in the correct format for this class.");
        }

        public abstract string Username { get; set; }
        public abstract string Password { get; set; }
        
        public virtual int UserTypeId { get; set; }
        public virtual string? FirstName { get; set; }
        public virtual string? LastName { get; set; }
        public virtual string? Email { get; set; }
        public virtual string? SessionCookie { get; set; }
        public virtual string? MachineName { get; set; }
        public virtual byte[]? ProfilePic { get; set; }
        public virtual DateTime DateOfBirth { get; set; }


        public bool FromDataRow(DataRow _row)
        {
            throw new NotImplementedException();
        }

        public DataRow ToDataRow(DataTable _skeleton)
        {
            throw new NotImplementedException();
        }
    }
}