using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientDataModel;

namespace ServerDataModel
{
    public interface ICreateFromClientData
    {
        void ApplyClientData(ClientDataBase clientEntity);
    }
}
