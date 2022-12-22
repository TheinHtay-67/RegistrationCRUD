using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationTest20221221.Models.User
{
    [Table("tblUser")]
    public class UserModel : BaseRespModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserViewModel : BaseRespModel
    {
        public int Id { get; set; }
        [DisplayName("Name")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Phone Number")]
        [Required]
        public string Phone { get; set; }
        [DisplayName("User Name")]
        [Required]
        public string UserName { get; set; }
        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Confirm Password")]
        [Required]
        public string ConfirmPassword { get; set; }
    }

    public class BaseRespModel
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class ResponeModel
    {
        public string respCode { get; set; }
        public string respMsg { get; set; }
    }
}
