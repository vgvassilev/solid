// /*
//  * $Id: $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using System;
using System.Collections.Generic;
using NUnit.Framework;

using SolidOpt.Services;

namespace SolidOpt.Services.Tests
{
    
    public interface IMyGenericDictionaryService<TKey, TValue>
    {
        void Set(TKey key, TValue value);
        TValue Get(TKey key);
    }

}
