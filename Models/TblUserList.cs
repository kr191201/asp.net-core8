using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardStudy.Models;

[Table("TBL_USER_LIST")]
public partial class TblUserList
{
    [Key]
    [Column("ID")]
    public int ID { get; set; }

    [Required]
    [Column("USERID")]
    public string? USERID { get; set; }

    [Required]
    [Column("USERNAME")]
    public string? USERNAME { get; set; }

    [Required]
    [Column("USEREMAIL")]
    public string? USEREMAIL { get; set; }

    [Required]
    [Column("USERPASSWORD")]
    public string? USERPASSWORD { get; set; }

    [Required]
    [Column("USERPASSWORDRE")]
    [Compare("USERPASSWORD")]
    public string? USERPASSWORDRE { get; set; }

    [Required]
    [Column("REG_DATE")]
    public DateTime REG_DATE { get; set; }

    [Column("CHG_DATE")]
    public DateTime CHG_DATE { get; set; }

    [Column("LOGIN_DATE")]
    public DateTime LOGIN_DATE { get; set; }

    [Column("LOGOUT_DATE")]
    public DateTime LOGOUT_DATE { get; set; }


}