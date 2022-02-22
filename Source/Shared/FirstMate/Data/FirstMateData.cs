using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

////////////////////////////////////////
// entity framework notes
//add-migration <name> -cotext=fmd or adc
//update-database

namespace FirstMate
{
    public class FirstMateData : DbContext
    {
        //public FirstMateData(DbContextOptions options) : base(options) { }

        public FirstMateData() { }



    }
}
