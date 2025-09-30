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
            foreach (var i in t)
            {
                var _i = i;
                outArr.Add(_i += 1);
            }

            Console.WriteLine($@"Initial arr: {String.Join(',', arr)}");
            Console.WriteLine($@"Result arr: {String.Join(',', outArr)}");
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
            if (this.isDisposed)
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


    public class CustomEnumerableSimple<T> : IEnumerable<T>
    {
        private T[] _item;

        public CustomEnumerableSimple(T[] item) { _item = item; }

        public IEnumerator<T> GetEnumerator() => new CustomEnumerator<T>(_item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

   
    public class CustomEnumeratorSimple<T> : IEnumerator<T>
    {
        private readonly T[] _items;
        private int _index;
        private T? _current;

        public CustomEnumeratorSimple(T[] items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
            _index = -1;
            _current = default!;
        }

        public bool MoveNext()
        {
            if (++_index >= _items.Length)
                return false;

            _current = _items[_index];
            return true;
        }

        public void Reset()
        {
            // Most enumerators throw NotSupportedException
            // But you can choose to support it
            _index = -1;
            _current = default!;
        }

        public T? Current => _current;
        object IEnumerator.Current => Current;

        public void Dispose()
        {
            // No resources to release
        }
    }



    public class PersonToEqual : IEquatable<PersonToEqual>
    {
        public string Name { get; set; } = String.Empty;
        public string SecondName { get; set; } = string.Empty;

        public bool Equals(PersonToEqual? p)
        {
            if (p is null)
                return false;

            return p.Name == Name && p.SecondName == SecondName;
        }

        public override bool Equals(object? o) => Equals(o as PersonToEqual);
        public override int GetHashCode() => HashCode.Combine(Name, SecondName);

        public static bool operator == (PersonToEqual? personLeft, PersonToEqual? personRight) =>
            Equals(personLeft, personRight);

        public static bool operator !=(PersonToEqual? personLeft, PersonToEqual? personRight) =>
            !Equals(personLeft, personRight);

    }
    
}
