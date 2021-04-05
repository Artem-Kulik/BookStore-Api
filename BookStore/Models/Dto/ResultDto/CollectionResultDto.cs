using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Dto.ResultDto
{
    public class SingleResultDto<T>: ResultDto
    {
        public T Data { get; set; }
    }
}
