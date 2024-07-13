using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Presentation.Shared
{
    public class ErrorCode(string ErrorDesctription, int ErrorCode)
    {
        public string Desctription { get; } = ErrorDesctription;
        public int Code { get; } = ErrorCode;
    }
}
