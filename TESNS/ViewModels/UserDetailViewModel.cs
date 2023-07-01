using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TESNS.Data.Enum;

namespace TESNS.ViewModels
{
    public class UserDetailViewModel
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string ProfilePhoto { get; set; }
    }
}
