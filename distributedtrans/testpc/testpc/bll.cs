using System;
using System.Collections.Generic;
using System.Text;

namespace testpc
{
    public class bll
    {
        private readonly ShawnDbContext _dbContext;

        public bll(ShawnDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void DoTrans()
        {

        }
    }
}
