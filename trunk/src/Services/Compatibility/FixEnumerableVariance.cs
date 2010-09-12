// 
//  FixEnumerableVariance.cs
//  
//  Author:
//       vvassilev <vasil.georgiev.vasilev@cern.ch>
//  
//  Copyright (c) 2010 vvassilev
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections;
using System.Collections.Generic;

namespace SolidOpt.Services.Compatibility
{
	public static class FixEnumerableVariance
	{
	
	    // Simple workaround for single method
	    // Variance in one direction only
	    public static void Add<S, D>(List<S> source, List<D> destination)
	        where S : D
	    {
	        foreach (S sourceElement in source)
	        {
	            destination.Add(sourceElement);
	        }
	    }
	
	    // Workaround for interface
	    // Variance in one direction only so type expressinos are natural
	    public static IEnumerable<D> Convert<S, D>(IEnumerable<S> source)
	        where S : D
	    {
	        return new EnumerableWrapper<S, D>(source);
	    }
	
	    private class EnumerableWrapper<S, D> : IEnumerable<D>
	        where S : D
	    {
	        public EnumerableWrapper(IEnumerable<S> source)
	        {
	            this.source = source;
	        }
	
	        public IEnumerator<D> GetEnumerator()
	        {
	            return new EnumeratorWrapper(this.source.GetEnumerator());
	        }
	
	        IEnumerator System.Collections.IEnumerable.GetEnumerator()
	        {
	            return this.GetEnumerator();
	        }
	
	        private class EnumeratorWrapper : IEnumerator<D>
	        {
	            public EnumeratorWrapper(IEnumerator<S> source)
	            {
	                this.source = source;
	            }
	
	            private IEnumerator<S> source;
	
	            public D Current
	            {
	                get { return this.source.Current; }
	            }
	
	            public void Dispose()
	            {
	                this.source.Dispose();
	            }
	
	            object IEnumerator.Current
	            {
	                get { return this.source.Current; }
	            }
	
	            public bool MoveNext()
	            {
	                return this.source.MoveNext();
	            }
	
	            public void Reset()
	            {
	                this.source.Reset();
	            }
	        }
	
	        private IEnumerable<S> source;
	    }
	
	    // Workaround for interface
	    // Variance in both directions, causes issues
	    // similar to existing array variance
	    public static ICollection<D> Convert<S, D>(ICollection<S> source)
	        where S : D
	    {
	        return new CollectionWrapper<S, D>(source);
	    }
	
	
	    private class CollectionWrapper<S, D> 
	        : EnumerableWrapper<S, D>, ICollection<D>
	        where S : D
	    {
	        public CollectionWrapper(ICollection<S> source)
	            : base(source)
	        {
	        }
	
	        // variance going the wrong way ... 
	        // ... can yield exceptions at runtime
	        public void Add(D item)
	        {
	            if (item is S)
	            {
	                this.source.Add((S)item);
	            }
	            else
	            {
	                throw new Exception(@"Type mismatch exception, due to type hole introduced by variance.");
	            }
	        }
	
	        public void Clear()
	        {
	            this.source.Clear();
	        }
	
	        // variance going the wrong way ... 
	        // ... but the semantics of the method yields reasonable semantics
	        public bool Contains(D item)
	        {
	            if (item is S)
	            {
	                return this.source.Contains((S)item);
	            }
	            else
	            {
	                return false;
	            }
	        }
	
	        // variance going the right way ... 
	        public void CopyTo(D[] array, int arrayIndex)
	        {
	            foreach (S src in this.source)
	            {
	                array[arrayIndex++] = src;
	            }
	        }
	
	        public int Count
	        {
	            get { return this.source.Count; }
	        }
	
	        public bool IsReadOnly
	        {
	            get { return this.source.IsReadOnly; }
	        }
	
	        // variance going the wrong way ... 
	        // ... but the semantics of the method yields reasonable  semantics
	        public bool Remove(D item)
	        {
	            if (item is S)
	            {
	                return this.source.Remove((S)item);
	            }
	            else
	            {
	                return false;
	            }
	        }
	        
	        private ICollection<S> source;
	    }
	
	    // Workaround for interface
	    // Variance in both directions, causes issues similar to existing array variance
	    public static IList<D> Convert<S, D>(IList<S> source)
	        where S : D
	    {
	        return new ListWrapper<S, D>(source);
	    }
	
	    private class ListWrapper<S, D> : CollectionWrapper<S, D>, IList<D>
	        where S : D
	    {
	        public ListWrapper(IList<S> source) : base(source)
	        {
	            this.source = source;
	        }
	        
	        public int IndexOf(D item)
	        {
	            if (item is S)
	            {
	                return this.source.IndexOf((S) item);
	            }
	            else
	            {
	                return -1;
	            }
	        }
	
	        // variance the wrong way ...
	        // ... can throw exceptions at runtime
	        public void Insert(int index, D item)
	        {
	            if (item is S)
	            {
	                this.source.Insert(index, (S)item);
	            }
	            else
	            {
	                throw new Exception("Invalid type exception");
	            }
	        }
	    
	        public void RemoveAt(int index)
	        {
	            this.source.RemoveAt(index);
	        }
	
	        public D this[int index]
	        {
	            get
	            {
	                return this.source[index];
	            }
	            set
	            {
	                if (value is S)
	                    this.source[index] = (S)value;
	                else
	                    throw new Exception("Invalid type exception.");
	            }
	        }
	        
	        private IList<S> source;
	    }
	}

}

