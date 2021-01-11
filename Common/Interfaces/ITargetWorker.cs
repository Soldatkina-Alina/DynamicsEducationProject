using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ITargetWorker
    {
        bool Contains(string attribute);
        T GetAttribute<T>(string attribute);
        int GetOptionSetValue(string attribute);
    }
}
