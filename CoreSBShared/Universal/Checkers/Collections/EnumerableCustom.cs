using System;
using System.Collections;
using System.Collections.Generic;

namespace InfrastructureCheckers.Collections
{
    public class CustomEnumerableCheck
    {
        public static void GO()
        {
            var arr = new int[] {1, 2, 3};
            var t = new CustomEnumerable<int>(arr);

            var outArr = new List<int>();
            foreach (var i in t) {
                var _i=i;
                outArr.Add(_i+=1);
            }

            Console.WriteLine($@"Initial arr: {String.Join(',',arr)}");
            Console.WriteLine($@"Result arr: {String.Join(',',outArr)}");
        }
    }
    
    public class CustomEnumerable<T> : IEnumerable<T>
    {
        private T[] _item;

        public CustomEnumerable(T[] item)
        {
            _item = item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CustomEnumerator<T>(_item);
        }

        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }
    }

    public class CustomEnumerator<T> : IEnumerator<T>
    {
        private T[] _col;
        private int idx = -1;

        public CustomEnumerator(T[] item)
        {
            _col = item;
        }
        public bool MoveNext()
        {
            idx++;
            if (idx >= _col.Length)
                return false;

            _current = _col[idx];
            return true;
        }

        public void Reset()
        {
            idx = -1;
            _current = default;
        }

        private T _current;

        public T Current => this._current;
        
        private object Current1 { get { return this.Current; } }
        object IEnumerator.Current { get { return Current1; } }

        private bool isDisposed = false;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if(this.isDisposed)
                return;

            if (disposing)
            {
                // managed
            }

            _col = null;
            _current = default;

            this.isDisposed = true;
        }

        ~CustomEnumerator()
        {
            this.Dispose(false);
        }
    }
}
