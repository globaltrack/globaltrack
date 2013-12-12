using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerDataModel
{
    public interface IConvertToClientData
    {
        T ToClientData<T>() where T : class; 
    }
}
