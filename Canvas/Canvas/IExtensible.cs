using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas
{
    public interface IExtensible<T> where T : IExtensible<T>
    {
        // Token: 0x1700003E RID: 62
        // (get) Token: 0x06000217 RID: 535
        ExtensionCollection<T> Extensions { get; }
    }
}
