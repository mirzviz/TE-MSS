using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_MSS
{
    public interface ITE_MSS
    {
        bool CreateMssObject(string mssString, double xCoord, double yCoord);
    }
}
