using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace testpc.Entity
{
    [Table("table1",Schema = "core")]
    public class Table1
    {
        [Column("id"), Key]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("score")]
        public int Score { get; set; }
    }
}
