using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities
{
    public class InvoiceFile : FileDocumun
    {
        public decimal Price { get; set; }
    }
}
