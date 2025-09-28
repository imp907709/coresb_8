using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using InfrastructureCheckers;
using UtilsCustom;

namespace UtilsCustom
{
    // utilities, wrapper
    public class Utils
    {
        public static void Split(int[] arr, out int[] l, out int[] r)
        {
            l = new int[0];
            r = new int[0];

            if (arr.Length <= 1)
            {
                l = arr;
                r = new int[0];
            }
            else
            {
                var m = arr.Length / 2;
                var lL = arr.Length - m;
                l = new int[m];
                r = new int[lL];
                Array.Copy(arr, 0, l, 0, l.Length);
                Array.Copy(arr, m, r, 0, r.Length);
            }
        }

        public static void ArraySwap(int[] arr, int l, int r)
        {
            (arr[l], arr[r]) = (arr[r], arr[l]);
        }

        public static void PrintTrace(string str)
        {
            Trace.WriteLine(str);
        }

        public static void WriteTofile(string contents, string name = "test", string path = @"C:\files\test\",
            string ext = "txt")
        {
            File.WriteAllText($"{path}{name}.{ext}", contents);
        }

        public static void Elapsed(Action method)
        {
            var sw = new Stopwatch();

            sw.Reset();
            sw.Start();

            PrintTrace($"Method started");
            method.Invoke();
            PrintTrace($"Method finised in: {sw.Elapsed.Ticks}");

            sw.Stop();
        }

        public static async Task ElapsedAsync(Func<Task> method)
        {
            var sw = new Stopwatch();

            sw.Reset();
            sw.Start();

            PrintTrace($"Method started");
            await method.Invoke();
            PrintTrace($"Method finised in: {sw.Elapsed.Ticks}");

            sw.Stop();
        }
    }
}

namespace InfrastructureCheckers
{
    public class Check
    {
        public static void GO()
        {
            LINQcheck.GO();
        }
    }

    public class OperatorOverloading
    {
        public int A;
        public int B;

        public OperatorOverloading()
        {
            A = 0;
            B = 0;
        }

        public OperatorOverloading(int a, int b)
        {
            A = a;
            B = b;
        }

        public static OperatorOverloading operator ++(OperatorOverloading o)
        {
            return new OperatorOverloading(o.A + 1, o.B + 1);
        }

        public static OperatorOverloading operator +(OperatorOverloading a)
        {
            return new OperatorOverloading(a.A, a.B);
        }

        public static OperatorOverloading operator +(OperatorOverloading a, int b)
        {
            return new OperatorOverloading(a.A, a.B + b);
        }

        public static OperatorOverloading operator +(OperatorOverloading o1, OperatorOverloading o2)
        {
            return new OperatorOverloading(o1.A + o2.A, o2.B + o2.B);
        }
    }

    public class StringCompare
    {
        public static void GO()
        {
            // == true
            string s0 = "s0";
            string s1 = "s0";

            // == true
            string s2 = "s1";
            object o1 = "s1";
            bool b1 = s2 == o1;

            string s3 = "s2";
            object s4 = new string("s2");
            string s5 = "s2";

            // false
            bool b4 = s3 == s4;

            // true
            bool b5 = s3 == s5;
        }

        public static void SideBySideComparison()
        {
            string a = "some string";
            string b = "some string";
            
            // all true
            var c1 = a == b;
            var c2 = a.Equals(b);
            var c3 = object.ReferenceEquals(a, b);
            
            Console.WriteLine($"str str == {c1}");
            Console.WriteLine($"str str equals {c2}");
            Console.WriteLine($"str str refeq {c3}");
            Console.WriteLine(Environment.NewLine);
            
            string a1 = "some string";
            string b1 = new string("some string");
            
            // true - value comparison
            var c4 = a1 == b1;
            var c5 = a1.Equals(b1);
            // false - different reference
            var c6 = object.ReferenceEquals(a1, b1);
            
            // true
            Console.WriteLine($"str str new == {c4}");
            Console.WriteLine($"str str new equals {c5}");
            
            // false
            Console.WriteLine($"str str new refeq {c6}");
            Console.WriteLine(Environment.NewLine);


            
            string a2 = "some string";
            object b2 = "some string";

            // true - value comparison overloaded
            var c7 = a2 == (string)b2;
            var c8 = a2.Equals(b2);
            // true - value comparison in string overload
            var c9 = b2.Equals(a2);
            // true - both point to same interned string
            var c10 = object.ReferenceEquals(a2, b2);
            
            // all true
            Console.WriteLine($"str obj str == {c7}");
            Console.WriteLine($"str obj str equals {c8}");
            Console.WriteLine($"obj str str equals {c9}");
            Console.WriteLine($"str obj str refeq {c10}");
            Console.WriteLine(Environment.NewLine);


            
            object a3 = "some string";
            object b3 = "some string";

            var b11 = a3 == b3;
            var b12 = a3.Equals(b3);
            var b13 = object.ReferenceEquals(a3, b3);
            
            // all true - as it same reference
            Console.WriteLine($"obj str obj str == {b11}");
            Console.WriteLine($"obj str obj equals == {b12}");
            Console.WriteLine($"obj str obj ref equals == {b13}");
            Console.WriteLine(Environment.NewLine);
            
            
            object a30 = "some string";
            object b30 = new string("some string");

            // false - both objects so ref comparison
            var c20 = a30 == b30;
            
            // true
            var c21 = a30.Equals(b30);
            var c22 = b30.Equals(a30);
            
            // false
            var c23 = object.ReferenceEquals(a30, b30);


            string a4 = "some string";
            object b4 = new string("some string");

            // false - one is object - so ref comparison
            var b14 = a4 == b4;
            
            // true
            var b15 = a4.Equals(b4);
            var b16 = b4.Equals(a4);
            
            // false
            var b17 = object.ReferenceEquals(a4, b4);
            
            Console.WriteLine($"str obj new str == {b14}");
            Console.WriteLine($"str obj new str equals == {b15}");
            Console.WriteLine($"obj new str str obj str equals == {b16}");
            
            // fale - as its another reference
            Console.WriteLine($"str obj new str ref eq {b17}");
            Console.WriteLine(Environment.NewLine);
            
            
            // not created new reference
            string a5 = "String";
            string b5 = nameof(System.String);
            string b6 = string.Intern("String");
            
            var b18 = object.ReferenceEquals(a5, b5);
            var b19 = object.ReferenceEquals(b5, b6);
            
            Console.WriteLine($"str str name refeq :{b18}");
            Console.WriteLine($"str str intern refeq :{b19}");
            
            
            
            // BUT - even with new ref, internal const created with one ref
            var a40 = "hello";
            var a41 = "hel" + "lo";
            var a42 = "he" + "llo";

            // true - same constant
            var b40 = object.ReferenceEquals(a40, a41);
            var b41 = object.ReferenceEquals(a41, a42);
            
            
            // AND another BUT - no const created 
            var a43 = "hel";
            var a44 = "lo";
            var a45 = a43 + a44;
            
            // false - no constant created
            var b42 = object.ReferenceEquals(a40, a45);
            
            
            
            string a50 = new string(new char[] { 'f','o','o' });
            string a51 = string.Intern(a);
            
            // true
            bool b50 = a == b;
            bool b51 = object.ReferenceEquals(a, b);
            
            
            
            string a60 = "hello";
            string b60 = "hel";
            string c60 = b60 + "lo";
            string d60 = string.Intern(c60);

            // false 
            var r1 = object.ReferenceEquals(a60, c60);
            // true - reference to string with this value exists 
            var r2 = object.ReferenceEquals(a60, d60);
            // false - intern ref to another val
            var r5 = object.ReferenceEquals(c60, d60);
            
            // true
            var r3 = a60 == c60;
            var r4 = a60 == d60;


            var a70 = "hel";
            var a71 = "lo";
            var a72 = a70 + a71;
            var a73 = a70 + a71;

            var a74 = string.Intern(a73);
            var a75 = string.Intern(a72);
            
            // false 
            var b72 = object.ReferenceEquals(a72, a73);
            // false
            var b70 = object.ReferenceEquals(a74, a73);

            // true
            var b71 = object.ReferenceEquals(a74, a75);
            
            
            
            // created new reference
            var a7 = new string("hello");
            var a8 = new string(new char[]{'h','e','l','l','o'});
            var a9 = string.Copy("hello");
            var a10 = "hello some".Substring(5);
            var a11 = "hel" + "lo";
            var a12 = "hella".Replace("a", "o");
            
            // all - false - new ref created
            // 1 to 2 ... last to 1 - a
            var b20 = object.ReferenceEquals(a7, a8);
            var b21 = object.ReferenceEquals(a8, a9);
            var b22 = object.ReferenceEquals(a9, a10);
            var b23 = object.ReferenceEquals(a10, a11);
            var b24 = object.ReferenceEquals(a11, a12);
            var b25 = object.ReferenceEquals(a12, a7);
        }
    }


    public class Property
    {
        public int ID { get; set; }
        public string PropertyName { get; set; }
    }

    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<Property> Property { get; set; }
    }

    public class LINQcheck
    {
        public class Student
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }

        public static void GO()
        {
            var intended = new JsonSerializerOptions() {WriteIndented = true};

            var propsIntersectAny = new List<Property>()
            {
                new Property() {ID = 1, PropertyName = "PropName1"},
                new Property() {ID = 2, PropertyName = "PropName2"}
            };

            var itemsList1 = new List<Item>()
            {
                // ALL STRICT
                // intersect all
                new Item() {ID = 1, Name = "Name1", Property = propsIntersectAny},

                // ALL AT LEAST
                // intersect all +
                new Item()
                {
                    ID = 3,
                    Name = "Name3",
                    Property = new List<Property>()
                    {
                        new Property() {ID = 5, PropertyName = "PropName5"},
                        propsIntersectAny[0],
                        propsIntersectAny[1]
                    }
                },

                // ANY AT LEAST
                // intersect 1 +
                new Item()
                {
                    ID = 4,
                    Name = "Name4",
                    Property = new List<Property>()
                    {
                        new Property() {ID = 6, PropertyName = "PropName6"}, propsIntersectAny[0]
                    }
                },

                // ANY STRICT
                // intersect 1
                new Item() {ID = 10, Name = "Name10", Property = new List<Property>() {propsIntersectAny[0]}}

                // ANY NOT INTERSECT
                // intersect non 1
                ,
                new Item()
                {
                    ID = 5,
                    Name = "Name5",
                    Property = new List<Property>()
                    {
                        new Property() {ID = 7, PropertyName = "PropName7"}, propsIntersectAny[0]
                    }
                }

                // ALL NOT INTERSECT
                // intersect non all 
                ,
                new Item()
                {
                    ID = 11,
                    Name = "Name11",
                    Property = new List<Property>()
                    {
                        new Property() {ID = 8, PropertyName = "PropName8"},
                        new Property() {ID = 9, PropertyName = "PropName9"}
                    }
                }
            };

            var itemsList2 = new List<Item>()
            {
                // intersect all
                new Item() {ID = 1, Name = "Name1", Property = propsIntersectAny}
            };

            // any of
            // !11
            var r0 = itemsList1.Where(s =>
                    itemsList2.Any(x => s.Property.Any(c => x.Property.Any(v => v.PropertyName == c.PropertyName))))
                .ToList();

            // all any - any without excess
            //1 10
            var r1 = itemsList1.Where(s =>
                    itemsList2.Any(x => s.Property.All(c => x.Property.Any(v => v.PropertyName == c.PropertyName))))
                .ToList();

            // any excess exist
            //3 4 5 11
            var r2 = itemsList1.Where(s =>
                    itemsList2.Any(x => !s.Property.All(c => x.Property.Any(v => v.PropertyName == c.PropertyName))))
                .ToList();

            // no intersect
            // 11
            var r3 = itemsList1.Where(s =>
                    itemsList2.Any(x => s.Property.All(c => x.Property.All(v => v.PropertyName != c.PropertyName))))
                .ToList();

            var gpjn = itemsList1
                .GroupJoin(itemsList2, lk => lk.Name, rk => rk.Name, (l, r) => new {l, r = r?.DefaultIfEmpty()})
                .SelectMany(k => k.r, (l, r) => new {l.l.ID, l.l.Name, rName = r?.Name ?? "Not found"}).ToList();

            File.WriteAllTextAsync(@"C:\files\test\gpjn.json", JsonSerializer.Serialize(gpjn));

            File.WriteAllTextAsync(@"C:\files\test\r0.json", JsonSerializer.Serialize(r0));
            File.WriteAllTextAsync(@"C:\files\test\r1.json", JsonSerializer.Serialize(r1));
            File.WriteAllTextAsync(@"C:\files\test\r2.json", JsonSerializer.Serialize(r2));

            File.WriteAllTextAsync(@"C:\files\test\r3.json", JsonSerializer.Serialize(r3));


            var a = new List<string>() {"a", "b", "c"};

            var all = new List<string>() {"a", "b", "c"};
            var few = new List<string>() {"a", "b"};
            var fewAndOther = new List<string>() {"a", "b", "d"};
            var notAny = new List<string>() {"e", "f"};

            var a1 = a.Where(s => all.All(c => c == s)).ToList();

            //all [a,b,c]
            var a2 = a.Where(s => all.Any(c => c == s)).ToList();
            var a3 = a.Where(s => all.Exists(c => s == c));
            var a4 = a.Where(s => all.Contains(s));

            //few [a,b]
            var f = a.Where(s => few.Any(c => c == s)).ToList();

            //except [c]
            var fex = a.Where(s => !few.Any(c => c == s)).ToList();

            //!!! not except
            //!any != any (!=)
            var fex2 = a.Where(s => few.Any(c => c != s)).ToList();

            string res = string.Empty;
            res = appendRes(res, JsonSerializer.Serialize(f));
            res = appendRes(res, "=====");
            res = appendRes(res, JsonSerializer.Serialize(fex));
            res = appendRes(res, JsonSerializer.Serialize(fex2));


            for (int i = 0; i < 4; i++)
                res += Environment.NewLine;

            var gp1 = itemsList1.SelectMany(p => p.Property, (l, r) => new {Name = l.Name, Prop = r.PropertyName})
                .ToList();
            var gp2 = itemsList2.SelectMany(p => p.Property, (l, r) => new {Name = l.Name, Prop = r.PropertyName})
                .ToList();

            var jn = gp1.Join(gp2,
                lk => new {lk.Name, lk.Prop},
                rk => new {rk.Name, rk.Prop},
                (l, r) => new {lName = l.Name, lProp = l.Prop, rName = r.Name, rProp = r.Prop}).ToList();

            var gpJn = gp1.GroupJoin(gp2,
                    lk => new {lk.Name, lk.Prop},
                    rk => new {rk.Name, rk.Prop},
                    (l, r) => new {lName = l.Name, lProp = l.Prop, Cnt = r?.Count(s => !string.IsNullOrEmpty(s?.Name))})
                .ToList();

            var gpJnQl =
                (from s1 in gp1
                    join s2 in gp2 on new {s1.Name, s1.Prop}
                        equals new {s2.Name, s2.Prop} into g
                    from s3 in g.DefaultIfEmpty()
                    group new {s3} by new {s1.Name, s1.Prop}
                    into g
                    select new
                    {
                        Name = g.Key.Name, Prop = g.Key.Prop, Cnt = g.Count(s => !string.IsNullOrEmpty(s?.s3?.Name))
                    }).ToList();


            res = appendResFunc(res, Jsonsize(gp1));
            res = appendRes(res, JsonSerializer.Serialize(gp2, intended));
            res = appendRes(res, JsonSerializer.Serialize(jn, intended));
            res = appendRes(res, JsonSerializer.Serialize(gpJn, intended));
            res = appendRes(res, JsonSerializer.Serialize(gpJnQl, intended));

            File.WriteAllTextAsync(@"C:\files\test\a.json", res);


            List<Student> lt = new()
            {
                new() {Name = "abcde", Id = 0},
                new() {Name = "abcdf", Id = 1},
                new() {Name = "abcdg", Id = 2},
                new() {Name = "abcd", Id = 3},
                new() {Name = "abce", Id = 4},
                new() {Name = "abc", Id = 5}
            };

            var stGp =
                lt.GroupBy(NameCount, StudentsGroup);
            var stMn =
                stGp.SelectMany(p => p.r, StudentsSelectMany).ToList();

            var res2 = string.Empty;
            res2 = appendRes(res2, Jsonsize(stGp));
            res2 = appendRes(res2, Jsonsize(stMn));

            File.WriteAllTextAsync(@"C:\files\test\std.json", res2);
        }

        static Func<Student, int> NameCount =
            (i) =>
            {
                return i.Name.Count();
            };

        public class Gp
        {
            public int l { get; set; }
            public IEnumerable<Student> r { get; set; }
        }

        public class GpS
        {
            public Gp l { get; set; }
            public Student r { get; set; }
        }

        public class GpRes
        {
            public string Name { get; set; }
            public int NameLangthGroup { get; set; }
        }

        private static Func<int, IEnumerable<Student>, Gp> StudentsGroup = (l, r) =>
        {
            return new Gp {l = l, r = r};
        };

        private static Func<Gp, Student, GpRes> StudentsSelectMany = (l, r) =>
        {
            return new GpRes {Name = r.Name, NameLangthGroup = l.l};
        };

        static Func<string, string, string> appendResFunc = (string res, string value) =>
        {
            res += value;
            res += Environment.NewLine;
            res += "------";
            res += Environment.NewLine;
            return res;
        };

        private static Func<object, string> Jsonsize = (i) =>
        {
            return JsonSerializer.Serialize(i, new JsonSerializerOptions() {WriteIndented = true});
        };

        static string appendRes(string res, string value)
        {
            res += value;
            res += Environment.NewLine;
            res += "------";
            res += Environment.NewLine;
            return res;
        }
    }
}

namespace Algorithms
{
    public interface GOable
    {
        T[] GO<T>(T[] arr) where T : struct, IComparable;
    }

    public class ArrayCheckNGo : GOable
    {
        public T[] CheckNGo<T>(T[] arr) where T : struct, IComparable
        {
            if (arr?.Length <= 0)
                return arr;

            return GO(arr);
        }

        public virtual T[] GO<T>(T[] arr) where T : struct, IComparable
        {
            return arr;
        }
    }


    public class SelectionSort
    {
        public int[] GO(int[] arr)
        {
            if (arr.Length <= 1)
                return arr;

            int l = arr.Length;

            for (int i = 0; i < l - 1; i++)
            {
                int min_idx = i;

                for (int j = i + 1; j < l; j++)
                {
                    if (arr[j] < arr[min_idx])
                    {
                        min_idx = j;
                    }
                }

                (arr[min_idx], arr[i]) = (arr[i], arr[min_idx]);
            }

            return arr;
        }
    }

    public class SelectionSortOriginal
    {
        public int[] GO(int[] arr)
        {
            int n = arr.Length;

            // One by one move boundary of unsorted subarray
            for (int i = 0; i < n - 1; i++)
            {
                // Find the minimum element in unsorted array
                int min_idx = i;
                for (int j = i + 1; j < n; j++)
                    if (arr[j] < arr[min_idx])
                    {
                        min_idx = j;
                    }

                // Swap the found minimum element with the first
                // element
                (arr[min_idx], arr[i]) = (arr[i], arr[min_idx]);
            }

            return arr;
        }
    }


    public class InsertionSort
    {
        public int[] GO(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                var n = arr[i];
                var j = i - 1;
                while (arr[j] > n && j >= 0)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }

                arr[j + 1] = n;
            }

            return arr;
        }
    }

    public class InsertionSortOriginal
    {
        // Function to sort array
        // using insertion sort
        public int[] GO(int[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; ++i)
            {
                int key = arr[i];
                int j = i - 1;

                // Move elements of arr[0..i-1],
                // that are greater than key,
                // to one position ahead of
                // their current position
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j = j - 1;
                }

                arr[j + 1] = key;
            }

            return arr;
        }
    }


    public class ShellSortInt
    {
        public int[] GO(int[] arr)
        {
            for (var gap = arr.Length / 2; gap > 0; gap /= 2)
            {
                for (var i = gap; i < arr.Length; i++)
                {
                    int j;
                    var pvt = arr[i];
                    for (j = i; j >= gap && arr[j - gap] > pvt; j -= gap)
                    {
                        arr[j] = arr[j - gap];
                    }

                    arr[j] = pvt;
                }
            }

            return arr;
        }
    }

    public class ShellSortOriginal
    {
        public int[] GO(int[] arr)
        {
            int n = arr.Length;

            // Start with a big gap,
            // then reduce the gap
            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                // Do a gapped insertion sort for this gap size.
                // The first gap elements a[0..gap-1] are already
                // in gapped order keep adding one more element
                // until the entire array is gap sorted
                for (int i = gap; i < n; i += 1)
                {
                    // add a[i] to the elements that have
                    // been gap sorted save a[i] in temp and
                    // make a hole at position i
                    int temp = arr[i];

                    // shift earlier gap-sorted elements up until
                    // the correct location for a[i] is found
                    int j;
                    for (j = i; j >= gap && arr[j - gap] > temp; j -= gap)
                        arr[j] = arr[j - gap];

                    // put temp (the original a[i])
                    // in its correct location
                    arr[j] = temp;
                }
            }

            return arr;
        }
    }


    public class QuickSort
    {
        public int[] GO(int[] arr)
        {
            sort(arr, 0, arr.Length - 1);
            return arr;
        }

        void sort(int[] arr, int st, int fn)
        {
            if (st >= fn)
                return;

            var p = partition(arr, st, fn);

            sort(arr, st, p - 1);
            sort(arr, p + 1, fn);
        }

        int partition(int[] arr, int low, int hi)
        {
            var p = hi;
            var i = low - 1;

            for (int j = low; j < hi; j++)
            {
                if (arr[j] < arr[p])
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }

            i++;
            (arr[i], arr[hi]) = (arr[hi], arr[i]);

            return i;
        }
    }

    public class QuickSortIntOriginal
    {
        public int[] GO(int[] arr)
        {
            quickSort(arr, 0, arr.Length - 1);
            return arr;
        }

        // A utility function to swap two elements
        static void swap(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        /* This function takes last element as pivot, places
             the pivot element at its correct position in sorted
             array, and places all smaller (smaller than pivot)
             to left of pivot and all greater elements to right
             of pivot */
        static int partition(int[] arr, int low, int high)
        {
            // pivot
            int pivot = arr[high];

            // Index of smaller element and
            // indicates the right position
            // of pivot found so far
            int i = (low - 1);

            for (int j = low; j <= high - 1; j++)
            {
                // If current element is smaller
                // than the pivot
                if (arr[j] < pivot)
                {
                    // Increment index of
                    // smaller element
                    i++;
                    swap(arr, i, j);
                }
            }

            swap(arr, i + 1, high);
            return (i + 1);
        }

        /* The main function that implements QuickSort
                    arr[] --> Array to be sorted,
                    low --> Starting index,
                    high --> Ending index
           */
        static void quickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                // pi is partitioning index, arr[p]
                // is now at right place
                int pi = partition(arr, low, high);

                // Separately sort elements before
                // partition and after partition
                quickSort(arr, low, pi - 1);
                quickSort(arr, pi + 1, high);
            }
        }
    }

    public class QuickSortNew
    {
        public int[] GO(int[] arr)
        {
            return sort(arr, 0, arr.Length - 1);
        }

        public int[] sort(int[] arr, int l, int h)
        {
            if (l > h)
                return arr;

            var p = partition(arr, 0, arr.Length);

            sort(arr, 0, p - 1);
            sort(arr, p + 1, arr.Length);

            return arr;
        }

        public int partition(int[] arr, int low, int hg)
        {
            var p = hg;
            var i = low - 1;

            for (int j = low; j < hg; j++)
            {
                if (arr[j] < arr[p])
                {
                    i++;
                    Utils.Swap(arr, i, j);
                }
            }

            i++;
            Utils.Swap(arr, i, hg);
            return i;
        }
    }


    public class MergeSortOriginal
    {
        public int[] GO(int[] arr)
        {
            sort(arr, 0, arr.Length - 1);
            return arr;
        }

        // Merges two subarrays of []arr.
        // First subarray is arr[l..m]
        // Second subarray is arr[m+1..r]
        void merge(int[] arr, int l, int m, int r)
        {
            // Find sizes of two
            // subarrays to be merged
            int n1 = m - l + 1;
            int n2 = r - m;

            // Create temp arrays
            int[] L = new int[n1];
            int[] R = new int[n2];
            int i, j;

            // Copy data to temp arrays
            for (i = 0; i < n1; ++i)
                L[i] = arr[l + i];
            for (j = 0; j < n2; ++j)
                R[j] = arr[m + 1 + j];

            // Merge the temp arrays

            // Initial indexes of first
            // and second subarrays
            i = 0;
            j = 0;

            // Initial index of merged
            // subarray array
            int k = l;
            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                {
                    arr[k] = L[i];
                    i++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                }

                k++;
            }

            // Copy remaining elements
            // of L[] if any
            while (i < n1)
            {
                arr[k] = L[i];
                i++;
                k++;
            }

            // Copy remaining elements
            // of R[] if any
            while (j < n2)
            {
                arr[k] = R[j];
                j++;
                k++;
            }
        }

        // Main function that
        // sorts arr[l..r] using
        // merge()
        void sort(int[] arr, int l, int r)
        {
            if (l < r)
            {
                // Find the middle
                // point
                int m = l + (r - l) / 2;

                // Sort first and
                // second halves
                sort(arr, l, m);
                sort(arr, m + 1, r);

                // Merge the sorted halves
                merge(arr, l, m, r);
            }
        }
    }

    public class MergeSort
    {
        public int[] GO(int[] arr)
        {
            return split(arr);
        }

        public int[] split(int[] arr)
        {
            var N = arr.Length;

            if (N <= 1)
                return arr;

            var m = (arr.Length / 2);
            var ll = m;
            var rr = arr.Length - m;
            var l = new int[ll];
            var r = new int[rr];
            Array.Copy(arr, 0, l, 0, ll);
            Array.Copy(arr, ll, r, 0, rr);

            var l2 = arr.Take(m).ToArray();
            var r2 = arr.Skip(m).Take(N - m).ToArray();
            l2 = split(l2);
            r2 = split(r2);

            return merge(l2, r2);
        }

        public int[] merge(int[] lt, int[] rt)
        {
            int l = 0;
            int r = 0;
            int[] res = new int[lt.Length + rt.Length];

            while (l < lt.Length && r < rt.Length)
            {
                if (lt[l] <= rt[r])
                {
                    res[l + r] = lt[l];
                    l++;
                }
                else
                {
                    res[l + r] = rt[r];
                    r++;
                }
            }

            while (l < lt.Length)
            {
                res[l + r] = lt[l];
                l++;
            }

            while (r < rt.Length)
            {
                res[l + r] = rt[r];
                r++;
            }

            return res;
        }
    }

    public class MergeSortNew
    {
        public int[] GO(int[] arr)
        {
            return split(arr);
        }

        int[] split(int[] arr)
        {
            if (arr?.Length <= 1)
                return arr;

            var N = arr.Length;

            var p = N / 2;
            var pl = N - p;

            int[] l = new int[p];
            int[] r = new int[pl];

            Array.Copy(arr, 0, l, 0, l.Length);
            Array.Copy(arr, p, r, 0, r.Length);

            l = split(l);
            r = split(r);

            return merge(l, r);
        }

        int[] merge(int[] lt, int[] rt)
        {
            int[] result = new int[lt.Length + rt.Length];

            int l = 0;
            int r = 0;

            while (l < lt.Length && r < rt.Length)
            {
                if (lt[l] < rt[r])
                {
                    result[l + r] = lt[l];
                    l++;
                }
                else
                {
                    result[l + r] = rt[r];
                    r++;
                }
            }

            while (l < lt.Length)
            {
                result[l + r] = lt[l];
                l++;
            }


            while (r < rt.Length)
            {
                result[l + r] = rt[r];
                r++;
            }

            return result;
        }
    }


    public class HeapSortOriginal
    {
        public int[] GO(int[] arr)
        {
            int N = arr.Length;

            // Build heap (rearrange array)
            for (int i = N / 2 - 1; i >= 0; i--)
                heapify(arr, N, i);

            // One by one extract an element from heap
            for (int i = N - 1; i > 0; i--)
            {
                // Move current root to end
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;

                // call max heapify on the reduced heap
                heapify(arr, i, 0);
            }

            return arr;
        }

        // To heapify a subtree rooted with node i which is
        // an index in arr[]. n is size of heap
        void heapify(int[] arr, int N, int i)
        {
            int largest = i; // Initialize largest as root
            int l = 2 * i + 1; // left = 2*i + 1
            int r = 2 * i + 2; // right = 2*i + 2

            // If left child is larger than root
            if (l < N && arr[l] > arr[largest])
                largest = l;

            // If right child is larger than largest so far
            if (r < N && arr[r] > arr[largest])
                largest = r;

            // If largest is not root
            if (largest != i)
            {
                int swap = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap;

                // Recursively heapify the affected sub-tree
                heapify(arr, N, largest);
            }
        }
    }

    public class HeapSort
    {
        public int[] GO(int[] arr)
        {
            arr = sort(arr);
            return arr;
        }

        int[] sort(int[] arr)
        {
            var N = arr.Length;
            for (int i = (N / 2) - 1; i >= 0; i--)
            {
                arr = heapify(arr, i, N);
            }

            for (int i = N - 1; i > 0; i--)
            {
                (arr[0], arr[i]) = (arr[i], arr[0]);
                arr = heapify(arr, 0, i);
            }

            return arr;
        }

        int[] heapify(int[] arr, int st, int fn)
        {
            var lg = st;
            var l = st * 2 + 1;
            var r = st * 2 + 2;

            if (l < fn && arr[l] > arr[lg])
                lg = l;

            if (r < fn && arr[r] > arr[lg])
                lg = r;

            if (lg != st)
            {
                (arr[lg], arr[st]) = (arr[st], arr[lg]);
                arr = heapify(arr, lg, fn);
            }

            return arr;
        }
    }


    public delegate int[] SortInt(int[] arr);


    //utilities, helpers
    public class Utils
    {
        public static void Split(int[] arr, out int[] l, out int[] r)
        {
            l = new int[0];
            r = new int[0];

            if (arr.Length <= 1)
            {
                l = arr;
                r = new int[0];
            }
            else
            {
                var m = arr.Length / 2;
                var lL = arr.Length - m;
                l = new int[m];
                r = new int[lL];
                Array.Copy(arr, 0, l, 0, l.Length);
                Array.Copy(arr, m, r, 0, r.Length);
            }
        }

        public static void Swap(int[] arr, int l, int r)
        {
            (arr[l], arr[r]) = (arr[r], arr[l]);
        }
    }

    public class SortChecker
    {
        private Random rnd = new Random();
        List<int> _ranges = new List<int>() {10, 10000, 20000, 30000};
        private List<string> rep = new List<string>();
        private StringBuilder sb = new StringBuilder();


        public void Init(List<int> ranges)
        {
            if (ranges?.Any() != true)
                ranges = _ranges;

            var lineGap = "-----";
            rep.Add(
                StatFormat("Alg Name", "Result", "Items cnt", "Time elapsed", "Ratio")
            );
            rep.Add(StatFormat(lineGap, lineGap, lineGap, lineGap, lineGap));

            SelectionSort sst = new SelectionSort();
            InsertionSort iss = new InsertionSort();
            ShellSortInt ssi = new ShellSortInt();
            QuickSort qsi = new QuickSort();
            MergeSort ms = new MergeSort();
            HeapSort hs = new HeapSort();


            SelectionSortOriginal sso = new SelectionSortOriginal();
            InsertionSortOriginal iso = new InsertionSortOriginal();
            ShellSortOriginal slst = new ShellSortOriginal();
            QuickSortIntOriginal qso = new QuickSortIntOriginal();
            MergeSortOriginal mso = new MergeSortOriginal();
            HeapSortOriginal hso = new HeapSortOriginal();

            QuickSortNew qsn = new QuickSortNew();
            MergeSortNew msn = new MergeSortNew();

            // List<SortInt> algs = new List<SortInt>() { ms._GO, mso.GO, sso.GO,sst.GO, iss.GO, iso.GO };
            List<SortInt> algs = new List<SortInt>() {qso.GO, qsn.GO};

            //for (var rng = 5; rng <= 1000; rng += 10)
            foreach (var rng in _ranges)
            {
                var arr = fillRandomArr(rng);
                Trace.WriteLine($"Array under test:{string.Join(",", arr.ToList())} ;");

                // arr = new List<int>() {5, 9, 0, 6, 2, 1};
                var sortedarr = copySorted(arr);

                var sw = new Stopwatch();

                // arr = new List<int>() { 2, 1, 4, 3, 5};
                foreach (var alg in algs)
                {
                    sw.Reset();
                    sw.Start();
                    var sorted = alg(arr.ToArray());
                    var res = sorted.SequenceEqual(sortedarr);
                    var etcks = sw.Elapsed.Ticks;
                    sw.Stop();
                    var ratio = etcks / (rng * Math.Log(rng));

                    //rep.Add($"Algorithm: {ms.GetType().Name}; Result: {res}; Range: {rng}; Elapsed:{sw.Elapsed}; Ratio:{ratio};");
                    //rep.Add($"{alg.Target} {res} {rng} {etcks} {ratio}");
                    rep.Add(
                        //string.Format("{0,45} | {1,7} | {2,7} | {3,7} | {4,7}", alg.Target, res, rng, etcks, ratio)
                        StatFormat(alg.Target.ToString(), res.ToString(), rng.ToString(), etcks.ToString(),
                            ratio.ToString())
                    );
                }

                rep.Add(Environment.NewLine);
            }

            File.WriteAllLines(@"C:\files\test\algTest.txt", rep);
        }

        public List<int> fillRandomArr(int rng = 10)
        {
            var arr = new List<int>(rng);
            for (int i = 0; i < rng; i++)
            {
                arr.Add(rnd.Next(-rng, rng));
            }

            return arr;
        }

        List<int> copySorted(List<int> arr)
        {
            var sortedarr0 = new int[arr.Count];
            arr.CopyTo(sortedarr0, 0);

            //arr.CopyTo(sortedarr0, 0);
            var sorted0 = sortedarr0.ToList().OrderBy(s => s).ToList();
            return sorted0;
        }

        public void SplitArr(int[] arr, out int[] l, out int[] r)
        {
            var ll = (arr.Length / 2);
            var rr = arr.Length - ll;
            l = new int[ll];
            r = new int[rr];
            Array.Copy(arr, 0, l, 0, ll);
            Array.Copy(arr, ll, r, 0, rr);
        }

        public string StatFormat(string name, string res, string rng, string elapsed, string ratio)
        {
            return string.Format($"{name,45} | {res,7} | {rng,11} | {elapsed,12} | {ratio,7}");
        }
    }

    public class AlgCheck
    {
        public class SplitResult
        {
            public int[] ArrayUT { get; set; }
            public int[] Left { get; set; }
            public int[] Right { get; set; }

            public bool IsOK { get; set; }

            public void OkCheckAssign()
            {
                IsOK = false;
                if (ArrayUT.Length == 0)
                {
                    IsOK = Left.Length == 0 && Right.Length == 0;
                }

                if (ArrayUT.Length == 1)
                {
                    IsOK = (Left.Length == 1 && Left[0] == ArrayUT[0] && Right.Length == 0)
                           || (Left.Length == 0 && Right.Length == 1 && Right[0] == ArrayUT[0]);
                }

                if (ArrayUT.Length > 1)
                {
                    var b1 = (Left.Length + Right.Length == ArrayUT.Length);
                    var b2 = new List<bool>();
                    int i = 0;
                    while (i < ArrayUT.Length)
                    {
                        int i2 = 0;
                        int i3 = 0;
                        while (i2 < Left.Length)
                        {
                            b2.Add(ArrayUT[i] == Left[i2]);
                            i++;
                            i2++;
                        }

                        while (i3 < Right.Length)
                        {
                            b2.Add(ArrayUT[i] == Right[i3]);
                            i++;
                            i3++;
                        }
                    }

                    IsOK = b1 && b2.All(s => s);
                }
            }
        }

        public static void SplitCheck()
        {
            SortChecker sc = new SortChecker();
            List<int[]> arrs = new List<int[]>();
            List<SplitResult> results = new List<SplitResult>();
            for (int i = 0; i <= 10; i++)
            {
                var arr = sc.fillRandomArr(i).ToArray();
                arrs.Add(arr);
                int[] l;
                int[] r;
                sc.SplitArr(arr, out l, out r);
                var item = new SplitResult() {ArrayUT = arr, Left = l, Right = r};
                item.OkCheckAssign();
                results.Add(item);
            }
        }

        public static void GO()
        {
            SortChecker ch = new SortChecker();
            ch.Init(null);
        }
    }
}

namespace Datastructures
{
    public class LinkedLists
    {
        public static void FrameworkSingleList()
        {
            var n4 = new LinkedListNode<string>("Node 4");

            LinkedList<string> list = new LinkedList<string>(new List<string>() {"Node 1", "Node 2", "Node 3"});

            var l = string.Join(',', list.Select(s => s));
            Trace.WriteLine(l);

            var n2 = list.Find("Node 2");
            list.AddAfter(n2, n4);

            var n5 = new LinkedListNode<string>("Node 5");
            list.AddFirst(n5);

            var l2 = string.Join(',', list.Select(s => s));
            Trace.WriteLine(l2);

            var reversed = list.Reverse();
            var l3 = string.Join(',', reversed.Select(s => s));
            Trace.WriteLine(l3);
        }
    }


    public class ListHashNode
    {
        public string Message { get; set; }
        public int Prev { get; set; }
        public int Next { get; set; }
    }

    public class LinkedNodeSingle
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public LinkedNodeSingle Next { get; set; }

        public LinkedNodeSingle() { }

        public LinkedNodeSingle(string message)
        {
            Message = message;
        }

        public string Print()
        {
            return $"Id:{Id}; Message:{Message};";
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode()
                   + Message.GetHashCode();
        }
    }

    public class LinkedNodeDouble : LinkedNodeSingle
    {
        public bool visited { get; set; } = false;
        public new LinkedNodeDouble Next { get; set; }
        public LinkedNodeDouble Prev { get; set; }

        public LinkedNodeDouble() { }

        public LinkedNodeDouble(string message) : base(message)
        {
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode()
                   + Message.GetHashCode();
        }
    }

    public class LinkedListSingle
    {
        private LinkedNodeSingle _head;

        private bool check()
        {
            if (_head == null)
                return false;

            return true;
        }

        public int Add(LinkedNodeSingle node)
        {
            if (!check())
            {
                _head = new LinkedNodeSingle() {Id = -1, Message = "_head_"};
                node.Id = 1;
                _head.Next = node;
            }
            else
            {
                var n = _head;
                while (n?.Next != null)
                {
                    var d = n;
                    n = d.Next;
                }

                node.Id = n.Id + 1;
                n.Next = node;
            }

            return node.Id;
        }

        public LinkedNodeSingle SearchById(int id)
        {
            if (!check())
                return null;

            var n = _head.Next;
            while (n.Next != null || n.Id != id)
            {
                var d = n.Next;
                n = d;
            }

            return n;
        }

        public string Print()
        {
            var result = $"{Environment.NewLine}---Empty---{Environment.NewLine}";
            if (!check())
                return result;

            result = string.Empty;
            var n = _head;
            while (n?.Next != null)
            {
                var d = n;
                n = d.Next;
                result += $"{Environment.NewLine}Id: {n.Id} ; Message: {n.Message} ;";
            }

            return result;
        }

        public IEnumerable<LinkedNodeSingle> Nodes()
        {
            if (!check())
                yield return null;

            var n = _head.Next;
            while (n != null)
            {
                var d = n;
                yield return n;
                n = d.Next;
            }
        }
    }

    public class LinkedListDouble
    {
        private IList<LinkedNodeDouble> nodes;
        private LinkedNodeDouble _head;

        private LinkedListHash _loopChecker = new LinkedListHash();

        private string _printEmpty => $"------{Environment.NewLine}Empty{Environment.NewLine}------";

        private string _printNode(LinkedNodeDouble node) =>
            $"{Environment.NewLine} Node :{node.Id}, Message: {node.Message}; Previous:{node.Prev?.Message ?? "Empty"}";

        private void headInit()
        {
            _head = new LinkedNodeDouble() {Id = -1, Message = "_head_", Next = null, Prev = null};
            nodes = new List<LinkedNodeDouble>();
        }

        bool check()
        {
            return (nodes != null && _head != null);
        }

        public int AddLast(LinkedNodeDouble node)
        {
            if (!check())
                headInit();

            var n = _head;
            while (n.Next != null)
            {
                n = n.Next;
            }

            node.Id = n.Id + 1;
            n.Next = node;
            node.Prev = n;

            return node.Id;
        }

        public int AddFirst(LinkedNodeDouble node)
        {
            if (!check())
            {
                headInit();
                return AddLast(node);
            }

            var d = _head.Next;
            node.Id = d.Id + 1;

            _head.Next = node;
            node.Next = d;
            node.Prev = null;

            d.Prev = node;

            return node.Id;
        }

        public LinkedNodeDouble FindById(int id)
        {
            var item = _head;

            while (item?.Next != null && item?.Id != id)
            {
                item = item.Next;
            }

            if (item?.Id == id)
                return item;

            return null;
        }

        public bool Remove(int id)
        {
            var node = FindById(id);

            if (node != null)
            {
                var p = node.Prev;
                var n = node.Next;

                if (p != null)
                    p.Next = node.Next;

                if (n != null)
                    n.Prev = node.Prev;

                node = null;
                return true;
            }

            return false;
        }

        public bool Reverse()
        {
            if (!check())
                return false;

            var n = _head.Next;
            while (n != null)
            {
                var nxt = n.Next;

                (@n.Prev, @n.Next) = (@n.Next, @n.Prev);

                // last node
                if (n.Prev == null)
                {
                    _head.Next = n;
                    n.Prev = _head;
                }

                // first node
                if (n.Next?.Id < 0)
                {
                    n.Next = null;
                }

                n = nxt;
            }

            return true;
        }

        public IList<LinkedNodeDouble> Nodes()
        {
            var result = new List<LinkedNodeDouble>();
            if (!check())
                return result;

            var n = _head.Next;
            while (n != null)
            {
                var d = n;
                result.Add(n);
                n = d.Next;
            }

            return result;
        }


        public bool IsLoopedHash()
        {
            if (!check())
                return false;

            var n = _head.Next;
            while (n != null)
            {
                var b = n;
                if (_loopChecker.Looped(n))
                    return true;

                _loopChecker.Add(n);
                n = b.Next;
            }

            return false;
        }

        public bool IsLoopedVisited()
        {
            var n = _head.Next;
            while (n != null)
            {
                var b = n;

                if (n.visited)
                    return true;

                n.visited = true;

                n = b.Next;
            }

            return false;
        }

        public bool IsLoopFloydsCycle()
        {
            var sl = _head.Next;
            var ft = sl?.Next;
            while (sl != null && ft != null)
            {
                var dSl = sl;
                var dFt = ft;

                if (sl == ft)
                    return true;

                sl = dSl.Next;
                ft = dFt?.Next?.Next;
            }

            return false;
        }


        public string Print(bool reversed = false, bool printHead = false)
        {
            var res = _printEmpty;
            if (!check())
                return res;

            res = string.Empty;
            var n = _head.Next;
            while (n != null)
            {
                res +=
                    $"{Environment.NewLine} Node :{n.Id}, Message: {n.Message}; Previous:{n?.Prev?.Message ?? "empty"}";
                n = n.Next;
            }

            return res;
        }

        public LinkedNodeDouble GetHead()
        {
            return _head;
        }

        public LinkedNodeDouble SearchById(int id)
        {
            var node = _head;
            while (node != null)
            {
                if (node.Id == id)
                    break;

                node = node.Next;
            }

            return node;
        }
    }

    public class LinkedListDoubleNew
    {
        public LinkedNodeDouble _head = new LinkedNodeDouble() {Message = "_head", Id = 0};

        //add
        public LinkedNodeDouble Add(LinkedNodeDouble node)
        {
            var item = _head;

            while (item.Next != null)
            {
                item = item.Next;
            }

            node.Id = item.Id + 1;
            item.Next = node;
            node.Prev = item;

            return node;
        }

        //find
        public LinkedNodeDouble Find(int id)
        {
            var item = _head;

            if (_head.Id == id)
                return _head;

            while (item?.Next != null && item.Id != id)
            {
                item = item?.Next;
            }

            if (item.Id == id)
                return item;

            return null;
        }

        //delete
        public bool Delete(int id)
        {
            var item = _head;

            while (item?.Next != null && item.Id != id)
            {
                item = item.Next;
            }

            if (item.Id == id)
            {
                var p = item.Prev;
                var n = item.Next;

                p.Next = item.Next;

                if (n != null)
                    n.Prev = item.Prev;

                item = null;
                return true;
            }

            return false;
        }

        //reverse
        public void Reverse()
        {
            var item = _head;

            while (item != null)
            {
                var next = item.Next;

                (item.Prev, item.Next) = (item.Next, item.Prev);

                //was first elem
                if (item.Next?.Id == 0)
                {
                    //head detach
                    item.Next = null;
                }

                //was last element
                if (item.Prev == null)
                {
                    //attach as first
                    _head.Next = item;
                    item.Prev = _head;
                }

                item = next;
            }
        }

        //loops
        //hash loop
        //visited prop
        //floydsCycle 2 cycle


        public string print()
        {
            var result = "=== Linked list start";

            var item = _head;

            while (item.Next != null)
            {
                result += $"\n Node message: {item.Message}; Node previous: {item.Prev?.Message ?? "empty"}";
                item = item.Next;
            }

            result += $"\n Node message: {item.Message}; Node previous: {item.Prev?.Message ?? "empty"}";
            result += "\n --- linked list end";
            return result;
        }


        public bool IsLoopedHash()
        {
            LinkedListHash hs = new LinkedListHash();

            var item = _head;
            while (item != null)
            {
                if (hs.Looped(item))
                    return true;

                hs.Add(item);
                item = item.Next;
            }

            return false;
        }

        public bool IsLoppedVisited()
        {
            var item = _head;
            while (item != null)
            {
                if (item.visited)
                    return true;

                item.visited = true;
                item = item.Next;
            }

            return false;
        }

        public bool IsLoopedFloydsCycle()
        {
            var itemFirst = _head;
            var itemNext = _head?.Next?.Next;

            while (itemFirst != null)
            {
                if (itemFirst == itemNext)
                    return true;

                itemFirst = itemFirst.Next;
                itemNext = itemFirst?.Next?.Next;
            }

            return false;
        }
    }


    public class LinkedListHash
    {
        private Hashtable _hashtable = new Hashtable();

        public void Add(LinkedNodeDouble node)
        {
            _hashtable.Add(node.GetHashCode(), node);
        }

        public bool Looped(LinkedNodeDouble node)
        {
            var hs = node.GetHashCode();
            if (_hashtable.ContainsKey(node.GetHashCode()))
                return true;

            return false;
        }
    }


    // A class for Min Heap
    public class MinHeap
    {
        // To store array of elements in heap
        public int[] heapArray { get; set; }

        // max size of the heap
        public int capacity { get; set; }

        // Current number of elements in the heap
        public int current_heap_size { get; set; }

        // Constructor 
        public MinHeap(int n)
        {
            capacity = n;
            heapArray = new int[capacity];
            current_heap_size = 0;
        }

        // Swapping using reference 
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        // Get the Parent index for the given index
        public int Parent(int key)
        {
            return (key - 1) / 2;
        }

        // Get the Left Child index for the given index
        public int Left(int key)
        {
            return 2 * key + 1;
        }

        // Get the Right Child index for the given index
        public int Right(int key)
        {
            return 2 * key + 2;
        }

        // Inserts a new key
        public bool insertKey(int key)
        {
            if (current_heap_size == capacity)
            {
                // heap is full
                return false;
            }

            // First insert the new key at the end 
            int i = current_heap_size;
            heapArray[i] = key;
            current_heap_size++;

            // Fix the min heap property if it is violated 
            while (i != 0 && heapArray[i] <
                heapArray[Parent(i)])
            {
                Swap(ref heapArray[i],
                    ref heapArray[Parent(i)]);
                i = Parent(i);
            }

            return true;
        }

        // Decreases value of given key to new_val. 
        // It is assumed that new_val is smaller 
        // than heapArray[key]. 
        public void decreaseKey(int key, int new_val)
        {
            heapArray[key] = new_val;

            while (key != 0 && heapArray[key] <
                heapArray[Parent(key)])
            {
                Swap(ref heapArray[key],
                    ref heapArray[Parent(key)]);
                key = Parent(key);
            }
        }

        // Returns the minimum key (key at
        // root) from min heap 
        public int getMin()
        {
            return heapArray[0];
        }

        // Method to remove minimum element 
        // (or root) from min heap 
        public int extractMin()
        {
            if (current_heap_size <= 0)
            {
                return int.MaxValue;
            }

            if (current_heap_size == 1)
            {
                current_heap_size--;
                return heapArray[0];
            }

            // Store the minimum value, 
            // and remove it from heap 
            int root = heapArray[0];

            heapArray[0] = heapArray[current_heap_size - 1];
            current_heap_size--;
            MinHeapify(0);

            return root;
        }

        // This function deletes key at the 
        // given index. It first reduced value 
        // to minus infinite, then calls extractMin()
        public void deleteKey(int key)
        {
            decreaseKey(key, int.MinValue);
            extractMin();
        }

        // A recursive method to heapify a subtree 
        // with the root at given index 
        // This method assumes that the subtrees
        // are already heapified
        public void MinHeapify(int key)
        {
            int l = Left(key);
            int r = Right(key);

            int smallest = key;
            if (l < current_heap_size &&
                heapArray[l] < heapArray[smallest])
            {
                smallest = l;
            }

            if (r < current_heap_size &&
                heapArray[r] < heapArray[smallest])
            {
                smallest = r;
            }

            if (smallest != key)
            {
                Swap(ref heapArray[key],
                    ref heapArray[smallest]);
                MinHeapify(smallest);
            }
        }

        // Increases value of given key to new_val.
        // It is assumed that new_val is greater 
        // than heapArray[key]. 
        // Heapify from the given key
        public void increaseKey(int key, int new_val)
        {
            heapArray[key] = new_val;
            MinHeapify(key);
        }

        // Changes value on a key
        public void changeValueOnAKey(int key, int new_val)
        {
            if (heapArray[key] == new_val)
            {
                return;
            }

            if (heapArray[key] < new_val)
            {
                increaseKey(key, new_val);
            }
            else
            {
                decreaseKey(key, new_val);
            }
        }
    }


    public class DataStructuresCheck
    {
        private static DataStructuresCheck inst = new DataStructuresCheck();

        public static void GO()
        {
            inst._go();
        }

        public void _go()
        {
            LinkedListCheck();
            LinkedListNewCheck();
        }

        private void LinkedListCheck()
        {
            LinkedListDouble ll = new LinkedListDouble();

            LinkedNodeDouble n1 = new LinkedNodeDouble("node one");
            LinkedNodeDouble n2 = new LinkedNodeDouble("node two");
            LinkedNodeDouble n3 = new LinkedNodeDouble("node three");

            LinkedNodeDouble n4 = new LinkedNodeDouble("node four");

            ll.AddLast(n1);
            ll.AddLast(n2);
            ll.AddLast(n3);
            Utils.PrintTrace(ll.Print());

            ll.Remove(n2.Id);
            Utils.PrintTrace(ll.Print());

            ll.AddLast(n4);
            Utils.PrintTrace(ll.Print());

            ll.Reverse();
            Utils.PrintTrace(ll.Print());
        }

        private void LinkedListNewCheck()
        {
            var ll = new LinkedListDoubleNew();

            LinkedNodeDouble n1 = new LinkedNodeDouble("node one");
            LinkedNodeDouble n2 = new LinkedNodeDouble("node two");
            LinkedNodeDouble n3 = new LinkedNodeDouble("node three");

            LinkedNodeDouble n4 = new LinkedNodeDouble("node four");

            ll.Add(n1);
            ll.Add(n2);
            ll.Add(n3);
            Utils.PrintTrace(ll.print());

            ll.Delete(n2.Id);
            Utils.PrintTrace(ll.print());

            ll.Add(n4);
            Utils.PrintTrace(ll.print());

            ll.Reverse();
            Utils.PrintTrace(ll.print());

            LinkedNodeDouble n5 = new LinkedNodeDouble("node five");
            LinkedNodeDouble n6 = new LinkedNodeDouble("node six");
            LinkedNodeDouble n7 = new LinkedNodeDouble("node seven");
            n6.Next = n5;

            var notLooped0 = ll.IsLoopedHash();
            var notLooped1 = ll.IsLoppedVisited();
            var notLooped2 = ll.IsLoopedFloydsCycle();

            ll.Add(n5);
            ll.Add(n6);

            var l0 = ll.IsLoopedHash();
            var l1 = ll.IsLoppedVisited();
            var l2 = ll.IsLoopedFloydsCycle();
        }
    }
}

namespace Patterns
{
    namespace StrategyPattern
    {
        //Strategy pattern
        public interface IProduct
        {
            public string Name { get; set; }
        }

        public class Product : IProduct
        {
            public string Name { get; set; }
        }

        public interface IProducer
        {
            public IProduct Produce(string Name);
        }

        public class Producer : IProducer
        {
            public virtual IProduct Produce(string Name) { return new Product() {Name = Name}; }
        }


        // Items for factory creation
        // Item one
        public interface IVessel
        {
            public double Tonnage { get; set; }
        }

        public class Vessel : Product, IVessel
        {
            public double Tonnage { get; set; }
        }

        // Item two
        public interface ICar
        {
            public double Speed { get; set; }
            public string Model { get; set; }
        }

        public class Car : Product, ICar
        {
            public double Speed { get; set; }
            public string Model { get; set; }
        }


        // Producers
        public class ManufacturerOne : Producer
        {
            //!!! override
            public override IProduct Produce(string Name)
            {
                return new Vessel() {Name = Name};
            }

            public new IProduct Produce(string Name, double tonnage)
            {
                return new Vessel() {Name = Name, Tonnage = tonnage};
            }
        }

        public class ManufacturerTwo : Producer
        {
            //!!! override
            public override IProduct Produce(string Name)
            {
                return new Car() {Name = Name, Model = "Model1"};
            }

            public new IProduct Produce(string Name, string Model)
            {
                return new Car() {Name = Name, Model = Model};
            }
        }

        public class ManufacturerThree : Producer
        {
            //!!! override
            public override IProduct Produce(string Name)
            {
                return new Car() {Name = Name, Model = "Model2", Speed = 10};
            }
        }


        // Factory
        public interface IProducerFactory
        {
            IProducer CreateProducer(string name);
            IProducer CreateProducer(IProducer type);
            IProducer CreateProducer(Type type);
        }

        public class ProducerFactory : IProducerFactory
        {
            public IProducer CreateProducer(string name)
            {
                IProducer r = name switch
                {
                    "ManufactureOne" => new ManufacturerOne(),
                    "ManufactureTwo" => new ManufacturerTwo(),
                    "ManufactureThree" => new ManufacturerThree(),
                    _ => new ManufacturerThree(),
                };

                return r;
            }

            public IProducer CreateProducer(IProducer type)
            {
                IProducer r = type switch
                {
                    ManufacturerOne => new ManufacturerOne(),
                    ManufacturerTwo => new ManufacturerTwo(),
                    ManufacturerThree => new ManufacturerThree(),
                    _ => new ManufacturerOne(),
                };

                return r;
            }

            public IProducer CreateProducer(Type type)
            {
                if (type == typeof(ManufacturerOne))
                {
                    return new ManufacturerOne();
                }

                if (type == typeof(ManufacturerTwo))
                {
                    return new ManufacturerTwo();
                }

                if (type == typeof(ManufacturerThree))
                {
                    return new ManufacturerThree();
                }

                return new ManufacturerOne();
            }
        }

        // Factory for producers of items testing
        public class Printer
        {
            public static void Print(IProducer p, IProduct pd)
            {
                Trace.WriteLine($"Producer is: {p.GetType()}");
                Trace.WriteLine($"Product is: {pd.GetType()}");

                PrintProd(pd);
            }

            public static void PrintProd(IProduct p)
            {
                if (p is ICar t)
                    PrintType(t);
                if (p is IVessel t2)
                    PrintType(t2);
            }

            public static void PrintType(ICar c)
            {
                Trace.WriteLine($"Car model is: {c.Model}");
            }

            public static void PrintType(IVessel c)
            {
                Trace.WriteLine($"Vessel tonnage is: {c.Tonnage}");
            }
        }

        public class StrategyPatternCheck
        {
            private static StrategyPatternCheck instance;

            public static void GO()
            {
                instance = new StrategyPatternCheck();
                instance._GO();
            }

            void _GO()
            {
                IProducerFactory factory = new ProducerFactory();

                List<IProducer> items = new List<IProducer>();

                IProducer p1 = new ManufacturerOne();
                IProducer p2 = new ManufacturerTwo();

                items.Add(factory.CreateProducer(p1.GetType()));
                items.Add(factory.CreateProducer(p2.GetType()));
                items.Add(factory.CreateProducer(typeof(ManufacturerThree)));

                foreach (var i in items)
                {
                    var item = i.Produce("Item name");
                    Printer.Print(i, item);
                }
            }
        }
    }
}

namespace Multithreadings
{
    public class Progressions
    {
        public int sum { get; private set; }

        public void Calculate(int gap)
        {
            if (gap >= 1)
                this.sum++;

            for (int i = 2; i <= gap; i++)
            {
                this.sum += i;
            }
        }

        public int CalculateCollectionParallel(List<int> numbers)
        {
            int total = 0;

            Parallel.ForEach(numbers,
                () => 0, // initial value,
                (num, state, localSum) => num + localSum,
                localSum => Interlocked.Add(ref total, localSum));
            return total; // total = 50005000
        }

        public int CalculateCollection(List<int> numbers)
        {
            int total = 0;
            foreach (var number in numbers)
            {
                total += number;
            }

            return total;
        }

        public long SimpleCount(ref long sum, int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                sum += arr[i];

                Utils.PrintTrace($"sum is:{sum} on thread: {Thread.GetCurrentProcessorId()}");
            }

            return sum;
        }

        public void Increaser(List<int> arr, int st, int fn)
        {
            for (int i = st; i <= fn; i++)
            {
                arr[i] *= 2;
            }
        }


        public async Task CountCompare()
        {
            var numbers = Enumerable.Range(1, 100).ToList();
            for (int i = 0; i < numbers.Count; i++)
            {
                numbers[i] = 1;
            }

            Utils.Elapsed(() => CountAll(numbers));

            numbers = Enumerable.Range(1, 100).ToList();
            for (int i = 0; i < numbers.Count; i++)
            {
                numbers[i] = 1;
            }

            Utils.Elapsed(async () =>
            {
                await CountParallelAsync(numbers);
            });
        }

        public void CountAll(List<int> numbers)
        {
            Increaser(numbers, 0, numbers.Count - 1);
        }

        public async Task CountParallelAsync(List<int> numbers)
        {
            var procs = Environment.ProcessorCount;
            var threadsNum = 3;
            var itemsPerThread = numbers.Count / threadsNum;

            var partitions = new List<Partition>()
            {
                new Partition() {from = 0, to = itemsPerThread},
                new Partition() {from = itemsPerThread + 1, to = itemsPerThread + itemsPerThread},
                new Partition() {from = itemsPerThread + itemsPerThread + 1, to = itemsPerThread * 3}
            };

            var tasks = new List<Task>();

            foreach (var p in partitions)
            {
                tasks.Add(Task.Factory.StartNew(() => Increaser(numbers, p.from, p.to)));
            }

            await Task.WhenAll(tasks);
        }
    }

    public class Partition
    {
        public int from { get; set; }
        public int to { get; set; }
        public long result { get; set; }
    }

    public class MultithreadingCheck
    {
        public static void GO()
        {
            Progressions f = new Progressions();

            // f.Calculate(5);
            // UtilsCustom.Utils.PrintTrace($"{f.sum}");

            var numbers = Enumerable.Range(1, 100000).ToList();

            Utils.Elapsed(() => f.CalculateCollection(numbers));
            Utils.Elapsed(() => f.CalculateCollectionParallel(numbers));
        }

        public static async Task GOAsync()
        {
            Progressions f = new Progressions();
            await f.CountCompare();
        }
    }
}

