using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekzamen.Data
{
    public partial class DbEntities: DbContext
    {
        private static DbEntities context;
        public static DbEntities GetContext()
        {
            if (context == null)
            {
                context = new DbEntities();
            }
            return context;
        }
    }
}
