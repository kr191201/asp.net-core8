using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardStudy.Models;

public partial class LoginModel
{
    [Required]
    [Column("USEREMAIL")]
    public string? USEREMAIL { get; set; }

    [Column("USERNAME")]
    public string? USERNAME { get; set; }

    [Column("USERID")]
    public string? USERID { get; set; }


    [Required]
    [Column("USERPASSWORD")]
    public string? USERPASSWORD { get; set; }

	[Column("ROLE")]
	public string? ROLE { get; set; }
    public bool IsAuthenticated { get; set; }

}