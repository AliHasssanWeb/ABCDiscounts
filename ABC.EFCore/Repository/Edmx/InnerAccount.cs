using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class InnerAccount
    {

    }
    [MetadataType(typeof(InnerAccount))]
    public partial class Account
    {
    }
}
