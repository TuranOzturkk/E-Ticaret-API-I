using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities
{
    public class ProductImageFile : FileDocumun
    {
        
        public bool Showcase { get; set; }
        public ICollection<Product> Products { get; set; }


        //public ICollection<FileDocumun> FileDocumuns { get; set; }
    }
}
