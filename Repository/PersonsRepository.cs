using Learn_web.DataBase;
using Learn_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Repository
{
    public class PersonsRepository
    {
        private PersonsContext context;


       public PersonsRepository(PersonsContext context)
        {
            this.context = context;
        }

        public IEnumerable<Person> Get()
        {
            return context.Users;
        }
        
    }
    
}
