using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public interface IRecord
    {
        string NAME
        {
            get;
        }
        int TYPE
        {
            get;
        }
        int CLASS
        {
            get;
        }
    }
}
