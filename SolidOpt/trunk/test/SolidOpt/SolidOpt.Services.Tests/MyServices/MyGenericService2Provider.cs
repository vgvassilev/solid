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
    
    public class MyGenericService2Provider<TKey,TValue>: AbstractServiceProvider, IMyGenericDictionaryService<TKey,TValue>
    {
        protected Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
        
        public void Set(TKey key, TValue value) {
            dict.Add(key,value);
        }

        public TValue Get(TKey key) {
            return dict[key];
        }

        public override string ToString() {
            string sb = "";
            foreach (KeyValuePair<TKey, TValue> item in dict) sb += item.Key + "=" + item.Value + ";"; 
            return sb;
        }
    }
}