using ABC.EFCore.Repository.Edmx;
using ABC.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.Services
{
   public class GenericPrograms
    {
        protected readonly ABCDiscountsContext _db;
        public readonly IMailService mailService;

        public GenericPrograms(IMailService mailService)
        {
            this.mailService = mailService;

        }
    }
}
