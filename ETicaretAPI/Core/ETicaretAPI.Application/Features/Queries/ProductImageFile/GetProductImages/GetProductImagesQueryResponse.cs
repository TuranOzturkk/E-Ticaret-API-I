using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryResponse
    {
       public Guid Id { get; set; }
       public string FileName { get; set; }
       public string Path { get; set; }
        
    }
}
