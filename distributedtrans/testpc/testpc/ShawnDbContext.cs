using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using testpc.Entity;

namespace testpc
{
    public class ShawnDbContext:DbContext
    {  /// <summary>
        /// 构造函数
        /// </summary>
        public ShawnDbContext()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options">参数</param>
        public ShawnDbContext(DbContextOptions<ShawnDbContext> options)
            : base(options)
        {

        }

        public DbSet<Table1> Table1 { get; set; }
        public DbSet<Table2> Table2 { get; set; }
    }
}
