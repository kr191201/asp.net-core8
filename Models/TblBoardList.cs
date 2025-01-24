using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardStudy.Models;

[Table("TBL_BOARD_LIST")]
public partial class TblBoardList
{
    [Key]
    [Column("ID")]
    public int ID { get; set; }

    [Required]
    [Column("BOARD_TITLE")]
    public string? BOARD_TITLE { get; set; }

    [Required]
    [Column("BOARD_BODY")]
    public string? BOARD_BODY { get; set; }

    [Column("BOARD_USER_ID")]
    public string? BOARD_USER_ID { get; set; }

    [Column("BOARD_WRITE_NM")]
    public string? BOARD_WRITE_NM { get; set; }

    [Column("BOARD_REG_DATE")]
    public DateTime BOARD_REG_DATE { get; set; }

    [Column("BOARD_CHG_DATE")]
    public DateTime BOARD_CHG_DATE { get; set; }

    [Column("BOARD_DEL_DATE")]
    public DateTime BOARD_DEL_DATE { get; set; }

    [Column("BOARD_TYPE")]
    public string? BOARD_TYPE { get; set; }

    [Column("BOARD_RES_WRITE_NM")]
    public string? BOARD_RES_WRITE_NM { get; set; }

    [Column("BOARD_RES_WRITE_ID")]
    public string? BOARD_RES_WRITE_ID { get; set; }

    [Column("BOARD_RES_REG_DATE")]
    public DateTime BOARD_RES_REG_DATE { get; set; }

    [Column("BOARD_TITLE")]
    public DateTime BOARD_RES_CHG_DATE { get; set; }

    [Column("BOARD_TITLE")]
    public DateTime BOARD_RES_DEL_DATE { get; set; }
}