using System.Data;
using static assignment1.Libs.Persistent;

namespace assignment1.Models.Generics
{
    public abstract class UserBase : ModelBase
    {
        private int user_id = -1;
        private int user_type_id;
        private string first_name;
        private string last_name;
        private string email;
        private string session;
        private string machine_name;
        private byte[] profile_pic;
        private DateTime date_of_birth;
        private string validation_key;
        private bool two_factor_auth;
        private string two_factor_code;

        public UserBase() : base() { }

        public UserBase(DataRow _row) : base(_row)
        {
            this.FromDataRow(_row);
        }

        public virtual int Id { get => user_id; set => user_id = value; }

        public abstract string Username { get; set; }
        public abstract string Password { get; set; }

        public virtual int UserTypeId { get => user_type_id; set => user_type_id = value; }
        public virtual string FirstName { get => first_name; set => first_name = value; }
        public virtual string LastName { get => last_name; set => last_name = value; }
        public virtual string Email { get => email; set => email = value; }
        public virtual string SessionCookie { get => session; set => session = value; }
        public virtual string MachineName { get => machine_name; set => machine_name = value; }
        public virtual byte[] ProfilePic { get => profile_pic; set => profile_pic = value; }
        public virtual DateTime DateOfBirth { get => date_of_birth; set => date_of_birth = value; }
        public virtual string ValidationKey { get => validation_key; set => validation_key = value; }

        public virtual bool TwoFactorAuth { get => two_factor_auth; set => two_factor_auth = value; }
        public virtual string TwoFactorCode { get => two_factor_code; set => two_factor_code = value; }

        public string UserType => user_type_table.FirstOrDefault(x => x.UserTypeId.Equals(this.user_type_id))?.UserTypeName ?? string.Empty;
    }
}