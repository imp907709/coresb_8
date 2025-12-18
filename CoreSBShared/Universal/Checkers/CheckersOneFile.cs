
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace coreSB
{
    public class SandBox
    {
        static SandBox item = new SandBox();
        public static void GO()
        {
            item._GO();
        }
        public void _GO()
        {
            Trace.WriteLine($"{ MethodBase.GetCurrentMethod().DeclaringType };{MethodBase.GetCurrentMethod().Name}");
        }

        public static Task GOasync()
        {
            return item._GOasync();
        }
        public static void GOsync()
        {
            item._GOasync();
        }

        public async Task _GOasync()
        {
            await Task.Delay(100);
        }
    }

    public static class ImportExport
    {
        public static string defaultPath => "C:\\files\\test";
        
        public static async Task<string> GetAsync(string url)
        {
            return await new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync();
        }

        public static async Task ParseExport<T>(List<T> items, string path)
        {
            var str = JsonSerializer.Serialize(items);
            await File.WriteAllTextAsync(path, str);
        }

        public static async Task<T> Import<T>(string path)
        {
            var str = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<T>(str, new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
        }
    }
}

namespace InfrastructureCheckers
{
    
    public class ConnectionStrings 
    {
        public static string MsSQlCoreSBConnection => "Server=192.168.0.100,1433;Database=coreSB;User Id=sa;password=QwErTy_1;MultipleActiveResultSets=true";
        public static string DockerMsSQlCoreSBConnection => "Server=WINDOWS-VCTMMRG\\SQLEXPRESS;Database=coreSB;Trusted_Connection=True;";
        public static string PgSQlCoreSBConnection =>  "Host=localhost;Port=5433;Database=coreSB;Username=postgres;Password=postgres";
    }

    public class HostBuilder
    {
        public static IHost Build()
        {
            var host = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(builder =>
            {
                builder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    env.ContentRootPath = Directory.GetCurrentDirectory();
                    env.EnvironmentName = "Development";
                });

                //builder.UseStartup<Startup>();
            }).Build();
            
            return host;
        }
    }

    public class ExpressionsPOCCheck
    {
        //ExpressionsPOC.GO();
    }
}

namespace NetPlatformCheckers
{
    public static class Check
    {

        public static void GO()
        {
            StringObjectEquality.GO();
            StringsCheck.GO();

            AbstractClassCheck.GO();
            // FakeThreadSleepForTest.GO();

            EqualIsCheck.GO();
            OverrideCheck.GO();
            GenericSwapCheck.GO();
            DelegateCheck.GO();
            EventsCheck.GO();
            ReflectionsCheck.GO();
        }
    }


    public static class Operators
    {
        public class NumenatorDenumenator
        {
            public NumenatorDenumenator()
            {
            }
            public NumenatorDenumenator(int numenator, int denumenator)
            {
                _numenator = numenator;
                _denumenator = denumenator;
            }

            public static NumenatorDenumenator operator +(NumenatorDenumenator operandB) => operandB;
            public static NumenatorDenumenator operator -(NumenatorDenumenator operandB) => new NumenatorDenumenator(-operandB._numenator, operandB._denumenator);

            public static NumenatorDenumenator operator +(NumenatorDenumenator a, NumenatorDenumenator b) =>
                new NumenatorDenumenator(a._numenator * b._denumenator + b._numenator * a._denumenator, a._denumenator * b._denumenator);
            public static NumenatorDenumenator operator -(NumenatorDenumenator a, NumenatorDenumenator b) =>
                a + (-b);
            public static NumenatorDenumenator operator *(NumenatorDenumenator a, NumenatorDenumenator b) =>
                new NumenatorDenumenator(a._numenator * b._numenator, a._denumenator * b._denumenator);
            public static NumenatorDenumenator operator /(NumenatorDenumenator a, NumenatorDenumenator b) =>
                new NumenatorDenumenator(a._numenator * b._denumenator, b._numenator * a._denumenator);


            public override string ToString() => $"{_numenator}/{_denumenator}";
            
            public int _numenator { get; set; }
            public int _denumenator { get; set; }
        }
        public static void GO()
        {

            int? a = null;
            a ??= 1;

            NumenatorDenumenator dn1 = new NumenatorDenumenator(3, 4);
            NumenatorDenumenator dn2 = new NumenatorDenumenator(5, 7);

            var t0 = -dn1;
            var t1 = +dn1;

            var t2 = (dn2 + dn1).ToString();
            var t3 = (dn2 - dn1).ToString();
            var t4 = (dn1 * dn2).ToString();
            var t5 = (dn1 / dn2).ToString();
        }
        
    }
    
    public static class ExtensionsDIY
    {
        public static void GO()
        {
            IEnumerable<int> listUT = new List<int>(){1,2,3};
            var listAdded = listUT.ForEachCustom(s=> s + 1);
        }
        public static IEnumerable<T> ForEachCustom<T>(this IEnumerable<T> items, 
            Func<T,T> exp)
        {
            var cnt = items.Count();
            if (cnt <= 0)
                return items;

            IEnumerable<T> result = new List<T>();
            
            for (int i = 0; i < cnt; i++)
            {
                var item = items.ToList()[i];
                var res = exp.Invoke(item);
                result = result.Append(res);
            }

            return result;
        }
    }

    /* strings concatenation check */
    /*--------------------------------------------- */
    public class StringsCheck
    {

        /*
         * str + char - str code
         * 
         * sum only +
         * str + int -> str
         * int + str -> int concat with str
         * 
         * char int -> char int codes to int
         * 
         * sum only + 
         * str and char and int -> str
         * 
         * */

        public static void GO()
        {
            StringCharIntConcatCheck();
            stringRefValCheck();
        }

        public static void StringCharIntConcatCheck()
        {

            var strAndChar = "1" + '2'; // str 12
            var charAndStr = '1' + "2"; // str 12

            var strAndInt = "1" + 2 + 3; // 123
            var intAndStr = 1 + 2 + "3";  // 33

            var cahrsAndStrAndInt1 = "1" + "2" + '3' + 4 + 5; // to str 12345
            var cahrsAndStrAndInt2 = 0 + "1" + "2" + '3' + 4 + 5; // to str 012345

        }

        public static void stringRefValCheck()
        {
            var stringNotChanged = "string";

            notChangesString(stringNotChanged);

            var changedString1 = newString(stringNotChanged);
            var changedString2 = newString2(stringNotChanged);
            stringChanges(ref stringNotChanged);

        }
        public static void notChangesString(string str)
        {
            str = str + "changes";
        }
        public static string newString(string str)
        {
            return str + "changes";
        }
        public static string newString2(string str)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(str);
            sb.Append("changes");

            return sb.ToString();
        }
        public static void stringChanges(ref string str)
        {
            str = str + "changes";
        }
    }

    /* String and object ==, equals, reference equals check */
    public static class StringObjectEquality
    {
        public static void GO()
        {

            string a = new string(new char[] { 'a' });
            string b = new string(new char[] { 'a' });

            object c = new string(new char[] { 'a' });
            object d = new string(new char[] { 'a' });

            object e = "str";
            string r = "str";

            var result = false;

            string st0 = "String";
            string st1 = nameof(String);
            string st2 = new string(new char[] { 'S', 't', 'r', 'i', 'n', 'g' });
            string st3 = new string(new char[] { 'S', 't', 'r', 'i', 'n', 'g' });

            object obr = st0;

            object ob0 = "String";
            object ob1 = nameof(String);
            object ob2 = new string(new char[] { 'S', 't', 'r', 'i', 'n', 'g' });

            dynamic dr = st0;

            dynamic d0 = "String";
            dynamic d1 = nameof(String);
            dynamic d2 = new string(new char[] { 'S', 't', 'r', 'i', 'n', 'g' });

            var isTrue = (st0 == st2) && (st2 == st3) && (st0 == ob0) && (st0 == st1) && (st1 == obr) && (obr == ob0) && (ob0 == ob1)
                         && (ob1 == dr) && (dr == d0) && (d0 == d1);
            var isFalse = (st2 == obr) && (ob0 == ob2) && (ob2 == dr) && (dr == d2);

            //true, equals by val
            var eqB = st0.Equals(st3);
            var eqObb = obr.Equals(ob2);

            var obDb = ob2.Equals(d2);

            result = a.Equals(b); //true && ()
            result = a == b; //true
            result = ReferenceEquals(a, b); //false (not from same string)
            result = ReferenceEquals(c, d); //false

            result = c.Equals(d); //true str equals by val
            result = c == d; //false obj equals by ref

            result = a.Equals(c); //true str equals by val
            result = a == c;//false by ref
            result = e == r; //true by ref

            result = ReferenceEquals(a, b); //false
            result = ReferenceEquals(c, d); //false


            object objStr = "String";
            object objRef = objStr;

            string strNew = typeof(string).Name;

            object objStr2 = "String";



            object o1 = new string("String");
            object o2 = o1;
            object o3 = new string("String");
            object o4 = "String";
            object o5 = "String";

            string s1 = new string("String");
            string s2 = s1;
            string s3 = new string("String");
            string s4 = "String";
            string s5 = "String";

            List<object> objs = new List<object>() { o1, o2, o3, o4, o5 };
            List<string> strs = new List<string>() { s1, s2, s3, s4, s5 };

            //ReferenceEquals
            //obj(str)||str""
            //true
            result = ReferenceEquals(o1, o2);
            result = ReferenceEquals(o4, o5);
            result = ReferenceEquals(s1, s2);
            result = ReferenceEquals(s4, s5);

            //false
            result = ReferenceEquals(o1, o3);
            result = ReferenceEquals(o1, o4);
            result = ReferenceEquals(o1, o5);
            result = ReferenceEquals(o2, o3);
            result = ReferenceEquals(o2, o4);
            result = ReferenceEquals(o2, o5);
            result = ReferenceEquals(o3, o4);
            result = ReferenceEquals(o3, o5);
            result = ReferenceEquals(s1, s3);
            result = ReferenceEquals(s1, s4);
            result = ReferenceEquals(s1, s5);
            result = ReferenceEquals(s2, s3);
            result = ReferenceEquals(s2, s4);
            result = ReferenceEquals(s2, s5);
            result = ReferenceEquals(s3, s4);
            result = ReferenceEquals(s3, s5);


            //==
            //obj(new) obj(ref)
            //obj(str) obj(str)
            //true
            result = o1 == o2;
            result = o4 == o5;

            //obj(ref) obj(new), obj(str)
            //obj(new) obj(str) obj(new)
            //false
            result = o1 == o3;
            result = o1 == o4;
            result = o2 == o3;
            result = o2 == o4;
            result = o2 == o5;
            result = o3 == o4;
            result = o3 == o5;

            //obj(new) str(new),str(ref),str
            //false
            result = o1 == s1;
            result = o1 == s2;
            result = o1 == s3;
            result = o1 == s4;
            result = o1 == s5;
            result = o2 == s1;
            result = o2 == s2;
            result = o2 == s3;
            result = o2 == s4;
            result = o2 == s5;
            result = o3 == s1;
            result = o3 == s2;
            result = o3 == s3;
            result = o3 == s4;
            result = o3 == s5;
            result = o4 == s1;
            result = o4 == s2;
            result = o4 == s3;

            //obj(str) str
            //true
            result = o4 == s4;
            result = o4 == s5;
            result = o5 == s4;
            result = o5 == s5;

            //str str(new)
            //true
            result = s1 == s2;
            result = s1 == s3;
            result = s1 == s4;
            result = s1 == s5;
            result = s2 == s3;
            result = s2 == s4;
            result = s2 == s5;
            result = s3 == s4;
            result = s3 == s5;
            result = s4 == s5;


            //equals
            //all objects from string all by val
            //true
            result = o1.Equals(o2);
            result = o4.Equals(o5);
            result = o1.Equals(o3);
            result = o1.Equals(o4);
            result = o1.Equals(o5);
            result = o2.Equals(o3);
            result = o2.Equals(o4);
            result = o2.Equals(o5);
            result = o3.Equals(o4);
            result = o3.Equals(o5);

            //obj str - all by val
            //true
            result = o1.Equals(s1);
            result = o1.Equals(s2);
            result = o1.Equals(s3);
            result = o1.Equals(s4);
            result = o1.Equals(s5);
            result = o2.Equals(s1);
            result = o2.Equals(s2);
            result = o2.Equals(s3);
            result = o2.Equals(s4);
            result = o2.Equals(s5);
            result = o3.Equals(s1);
            result = o3.Equals(s2);
            result = o3.Equals(s3);
            result = o3.Equals(s4);
            result = o3.Equals(s5);
            result = o4.Equals(s1);
            result = o4.Equals(s2);
            result = o4.Equals(s3);
            result = o4.Equals(s4);
            result = o4.Equals(s5);
            result = o5.Equals(s1);
            result = o5.Equals(s2);
            result = o5.Equals(s3);
            result = o5.Equals(s4);
            result = o5.Equals(s5);

        }
    }


    //IEnumerable IEnumerator 
    public class Person
    {
        public string Name;
        public string SecondName;
    }
    public class Persons : IEnumerable
    {
        public Person[] _persons;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
        public PersonEnum GetEnumerator()
        {
            return new PersonEnum(_persons);
        }

    }
    public class PersonEnum : IEnumerator
    {
        int position = -1;
        public Person[] _persons;
        public PersonEnum(Person[] list)
        {
            _persons = list;
        }

        object IEnumerator.Current => Current;
        public Person Current
        {
            get
            {
                try
                {
                    return _persons[position];
                }
                catch (Exception) { throw; }
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            position++;
            return (position < _persons.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }


    /* PatternMatching check */

    /*--------------------------------------------- */
    public static class PatternMatching
    {

        public static void GO()
        {

        }

        public static void Match()
        {
            Stream s = new FileStream(string.Empty, FileMode.Open);
            var message = string.Empty;
            switch (s)
            {
                case FileStream c when s.CanRead:
                    break;
                case FileStream c:
                    break;
                case MemoryStream c:
                    break;
                case null:
                    break;
                default:
                    break;
            }


        }

        public static void MatchExp()
        {
            Stream s = new FileStream(string.Empty, FileMode.Open);
            var message = s switch
            {
                FileStream c when s.CanRead => "FS can read"
                ,
                FileStream c => "FS"
                ,
                MemoryStream c => "MS"
                ,
                null => "null"
                ,
                _ => "default"
            };
        }

        //Extended property pattern
        public static void MatchParam(ParentParameter input)
        {
            var a = input switch
            {
                {param.Name : "Name1"} => input.param.Id * 1,
                {param.Name : "Name3"} => input.param.Id *3,
                _ => input.param.Id * 2
            };
            
        }
    }

    public class ParentParameter
    {
        public NestedParameter param { get; set; }
    }

    public class NestedParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    /* Abstract class initialization check */

    /*--------------------------------------------- */
    public abstract class ABS
    {

        public static string ParentProp { get; set; } = "inited from autoprop in abs";
        public ABS() { }
        //private ABS(){}
        static ABS()
        {
            ParentProp = "inited in parent static const";
        }
    }

    public class ABSchild : ABS
    {
        public static string AbsChildProp { get; set; } = "inited in child autoprop";
        public ABSchild() : base() { }
        public ABSchild(int val) { }

        static ABSchild()
        {
            AbsChildProp = "inited in child static constr";
        }
    }
    public class AbstractClassCheck
    {
        static AbstractClassCheck acch;
        public static void GO()
        {
            acch = new AbstractClassCheck();
            acch._GO();
        }

        private void _GO()
        {
            ABSchild aschild = new ABSchild();
            Trace.WriteLine(ABSchild.ParentProp);
            Trace.WriteLine(ABSchild.AbsChildProp);
        }
    }




    /* overriding */

    // not falls to base but overrides in cases
    // base as child override
    // base as childlvlN override
    // interface of base as child

    /*--------------------------------------------- */

    public interface IMethodA
    {
        string MethodA();
    }
    public interface IMethodB
    {
        string MethodB();
    }
    public class newParent
    {
        public int ID { get; set; }
        public string MethodA() => $"A in parent";
        public virtual string MethodB() => $"B in parent";
    }
    public class newParentI : IMethodA, IMethodB
    {
        public string MethodA() => $"A in parentI";
        public virtual string MethodB() => $"B in parentI";
    }
    //classic override
    public class newChild1 : newParent
    {
        public new string MethodA() => $"A in child1";
        public override string MethodB() => $"B in child1";
    }

    //classic new new
    public class newChild2 : newParent
    {
        public new string MethodA { get; set; }
        public new string MethodB { get; set; }
    }

    //new new bu imtplements A
    public class newChild3 : newParent, IMethodA
    {
        public new string MethodA { get; set; }
        public new string MethodB { get; set; }
    }
    //new new but implements B
    public class newChild4 : newParent, IMethodB
    {
        public new string MethodA { get; set; }
        public new string MethodB { get; set; }
    }
    //classic override new but implements both
    public class newChild5 : newParent, IMethodA, IMethodB
    {
        public new string MethodA { get; set; }
        public override string MethodB() => $"B in child5";
    }

    public class InheritanceAndInterfacesCheck
    {

        public static void GO()
        {
            check();
        }

        static void check()
        {

        }
    }



    public class parent
    {

        public int ID { get; set; }
        public parent() { }
        public virtual string printV() { return "printed from base " + this.GetType().Name; }
        public string log() { return "logged from base " + this.GetType().Name; }
    }
    class child1 : parent
    {
        public child1() { }
        public override string printV() { return "printed from child1 " + this.GetType().Name; }
        public new string log() { return "logged from child1 " + this.GetType().Name; }
    }
    class child2 : parent
    {
        public child2() { }
        public new string printV() { return "printed from child2 " + this.GetType().Name; }
        public new string log() { return "logged from child2 " + this.GetType().Name; }
    }
    public static class OverrideCheck
    {
        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            parent parent = new parent();
            parent parentAsChild1 = new child1();
            parent parentAsChild2 = new child2();
            child1 child = new child1();


            Trace.WriteLine(parent.printV()); //base
            Trace.WriteLine(parent.log()); //base
            Trace.WriteLine(parentAsChild1.printV()); //child1
            Trace.WriteLine(parentAsChild1.log()); //base
            Trace.WriteLine(parentAsChild2.printV()); //base
            Trace.WriteLine(parentAsChild2.log()); //base
            Trace.WriteLine(child.printV()); //child1
            Trace.WriteLine(child.log()); //child1
        }
    }
    public static class EqualIsCheck
    {
        public static void GO()
        {

            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            parent parent = new parent();
            parent parentAsChild1 = new child1();
            parent parentAsChild2 = new child2();
            child1 child = new child1();

            bool parentAsChild2IsEqualsParentAsChild1 = parentAsChild2.Equals(parentAsChild1);

            bool parentAsChild1IsParent = parentAsChild1 is parent;
            bool parentAsChild1IsChild1 = parentAsChild1 is child1;

            bool childIsParent = child is parent;

            Trace.WriteLine($"parentAsChild2IsEqualsParentAsChild1: {parentAsChild2IsEqualsParentAsChild1}");

            Trace.WriteLine($"parentAsChild1IsParent: {parentAsChild1IsParent}");
            Trace.WriteLine($"parentAsChild1IsChild1: {parentAsChild1IsChild1}");
            Trace.WriteLine($"childIsParent: {childIsParent}");
        }
    }



    //Linear inheritance from interface 

    interface IA { void A(); }

    public class AC : IA
    {
        public void A() { Trace.WriteLine("A"); }
    }

    public class BC : AC
    {
        public new void A() { Trace.WriteLine("B"); }
    }
    //overrides 
    public class CC : BC, IA
    {
        public new void A() { Trace.WriteLine("C"); }
    }
    //Also overrides
    public class DD : AC, IA
    {
        public new void A() { Trace.WriteLine("D"); }
    }
    public static class LinearInheritance
    {
        public static void GO()
        {
            AC a = new AC();
            AC b = new BC();
            AC c = new CC();
            AC d = new DD();

            a.A(); b.A(); c.A(); d.A();

            IA iac = new AC();
            IA ibc = new BC();
            IA icc = new CC();
            IA idd = new DD();

            iac.A(); ibc.A(); icc.A(); idd.A();
        }
    }



    //Multiple Interfaces and childs of childs
    public interface ia
    {
        void mA();
    }
    public interface ib
    {
        void mB();
    }

    public class A : ia, ib
    {
        public void mA()
        {
            Trace.WriteLine("a in A");
        }
        public virtual void mB()
        {
            Trace.WriteLine("b in A");
        }
    }


    public class B : A
    {
        public new void mA()
        {
            Trace.WriteLine("a in B");
        }
        public void mB()
        {
            Trace.WriteLine("b in B");
        }
    }
    public class C : A, ia
    {
        public new void mA()
        {
            Trace.WriteLine("a in C");
        }
        public new void mB()
        {
            Trace.WriteLine("b in C");
        }
    }
    public class D : A
    {
        public new void mA()
        {
            Trace.WriteLine("a in D");
        }
        public override void mB()
        {
            Trace.WriteLine("b in D");
        }
    }
    public class E : A, ib
    {
        public new void mA()
        {
            Trace.WriteLine("a in E");
        }
        public new void mB()
        {
            Trace.WriteLine("b in E");
        }
    }
    public class Eo : A, ib
    {

        public new void mA()
        {
            Trace.WriteLine("a in E");
        }
        public override void mB()
        {
            Trace.WriteLine("b in Eo");
        }
    }

    public class D1 : D
    {

    }
    public class D2 : D
    {
        public override void mB()
        {
            Trace.WriteLine("b in D2");
        }
    }

    public static class InheritanceOverridingWithInterfaces
    {
        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            A aFromB = new B();
            A aFormC = new C();
            A aFromD = new D();
            A aFromE = new E();

            A aFromEo = new Eo();

            ia iaFromB = new B();
            ia iaFromC = new C();
            ia iaFromD = new D();
            ia iaFromE = new E();

            ib ibFromB = new B();
            ib ibFromC = new C();
            ib ibFromD = new D();
            ib ibFromE = new E();

            A aFromD1 = new D1();
            A aFromD2 = new D2();

            //all others to base a in A and b in A
            aFromB.mA();
            aFromB.mB();
            aFormC.mA();
            aFormC.mB();
            aFromD.mA();
            aFromD.mB(); // b in D
            aFromE.mA();
            aFromE.mB();
            aFromD1.mA();
            aFromD1.mB(); // b in D
            aFromD2.mA();
            aFromD2.mB(); // b in D2

            aFromEo.mB(); //b in a

            //all others to base
            iaFromB.mA();
            iaFromC.mA(); // a in C
            iaFromD.mA();
            iaFromE.mA();

            //all others to base
            ibFromB.mB();
            ibFromC.mB();
            ibFromD.mB(); // b in D
            ibFromE.mB(); // b in E

        }
    }

    public static class TypesCheck
    {
        public static void GO()
        {

            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            D1 d1 = new D1();
            A dfromA = new D1();

            var tD = typeof(D1).ToString();
            var tiD = d1.GetType().ToString();

            var tA = typeof(A).ToString();
            var tiA = dfromA.GetType().ToString();

            int i = 1;
            object o = i;
            var oRtTp = o.GetType().ToString();

            int i2 = (int)o;

        }
    }


    /* Inheritance with interfaces */


    public interface IMethodD
    {
        void MethodD();
    }
    public interface IMethodE
    {
        void MethodE();
    }

    public interface IMethodF
    {
        void MethodF();
    }
    public interface IMethodG
    {
        void MethodG();
    }

    public class ParentClass : IMethodD, IMethodE
    {
        public void MethodA()
        {
            Console.WriteLine("MethodA from ParentClass");
        }
        public virtual void MethodB()
        {
            Console.WriteLine("MethodB from ParentClass");
        }
        public virtual void MethodC()
        {
            Console.WriteLine("MethodC from ParentClass");
        }

        public virtual void MethodD()
        {
            Console.WriteLine("MethodD from ParentClass");
        }
        public void MethodE()
        {
            Console.WriteLine("MethodE from ParentClass");
        }

        public virtual void MethodF()
        {
            Console.WriteLine("MethodF from ParentClass");
        }
        public void MethodG()
        {
            Console.WriteLine("MethodG from ParentClass");
        }
    }

    public class ChildClass : ParentClass, IMethodF, IMethodG
    {
        public new void MethodB()
        {
            Console.WriteLine("MethodB from ChildClass");
        }
        public override void MethodC()
        {
            Console.WriteLine("MethodC from ChildClass");
        }

        public override void MethodD()
        {
            Console.WriteLine("MethodD from ChildClass");
        }
        public new void MethodE()
        {
            Console.WriteLine("MethodE from ChildClass");
        }

        public override void MethodF()
        {
            Console.WriteLine("MethodF from ChildClass");
        }
        public new void MethodG()
        {
            Console.WriteLine("MethodG from ChildClass");
        }
    }

    public static class InheritanceWithInterfacesCheck
    {

        public static void Go()
        {

            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            ParentClass parentFromChild = new ChildClass();

            parentFromChild.MethodD(); //from child
            parentFromChild.MethodE(); //from parent

            parentFromChild.MethodF(); //from child
            parentFromChild.MethodG(); //from parent

            IMethodD methodDfromChild = new ChildClass();
            IMethodE methodEfromChild = new ChildClass();
            IMethodF methodFfromChild = new ChildClass();
            IMethodG methodGfromChild = new ChildClass();

            methodDfromChild.MethodD(); //from child
            methodEfromChild.MethodE(); //from parent

            methodFfromChild.MethodF(); //from child
            methodGfromChild.MethodG(); //from child

        }

    }




    /** generic delegate swap*/
    /*--------------------------------------------- */
    public interface IEntityID
    {
        int ID { get; set; }
    }
    public class EntityForSwap : IEntityID
    {
        public int ID { get; set; }
    }
    public static class SwapG
    {
        public static void Sort<T>(IList<T> arr, Func<T, T, int> cmpr) where T : IEntityID
        {
            bool sort = true;
            while (sort)
            {
                sort = false;
                for (int i = 0; i < arr.Count - 1; i++)
                {
                    if (cmpr(arr[i], arr[i + 1]) > 0)
                    {
                        sort = true;
                        swap<T>(arr, arr.IndexOf(arr[i]), arr.IndexOf(arr[i + 1]));
                    }
                }
            }
        }

        static void swap<T>(IList<T> arr, int i1, int i2)
        {
            T item;
            item = arr[i1];
            arr[i1] = arr[i2];
            arr[i2] = item;
        }

    }
    static class Comparers
    {
        public static int desc<T>(T itm1, T itm2) where T : IEntityID
        {
            if (itm1.ID > itm2.ID) { return 1; }
            if (itm1.ID < itm2.ID) { return -1; }
            return 0;
        }
        public static int asc<T>(T itm1, T itm2) where T : IEntityID
        {
            if (itm1.ID < itm2.ID) { return 1; }
            if (itm1.ID > itm2.ID) { return -1; }
            return 0;
        }
    }
    public static class GenericSwapCheck
    {
        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            List<EntityForSwap> arr =
            new List<EntityForSwap>(){
                new EntityForSwap(){ID=0},
                new EntityForSwap(){ID=3},
                new EntityForSwap(){ID=5},
                new EntityForSwap(){ID=2},
                new EntityForSwap(){ID=1}
            };

            Trace.WriteLine("before swap:");
            foreach (EntityForSwap p in arr) { Console.Write(p.ID); }

            SwapG.Sort<EntityForSwap>(arr, Comparers.desc<IEntityID>);

            Trace.WriteLine("after swap desc:");
            foreach (EntityForSwap p in arr) { Console.Write(p.ID); }

            SwapG.Sort<EntityForSwap>(arr, Comparers.asc<IEntityID>);

            Trace.WriteLine("after swap asc:");
            foreach (EntityForSwap p in arr) { Console.Write(p.ID); }

        }
    }




    /** delegate */
    /*--------------------------------------------- */
    public delegate string Del1(int i);

    /** shows named, anonimous and lambda anonimous delegate type invokation */
    public class DelegateInvokation
    {
        public static void GO()
        {
            //named method instance
            Del1 d11 = print;
            Trace.WriteLine(d11.Invoke(2));
            Trace.WriteLine(d11(3));

            //anonimous method instance
            Del1 d12 = delegate (int i) { return "Anonimous to str: " + i.ToString(); };
            Trace.WriteLine(d12.Invoke(4));
            Trace.WriteLine(d12(5));

            //lambda instance
            Del1 d13 = s => "Lambd to str:" + s.ToString();
            Trace.WriteLine(d13.Invoke(6));
            Trace.WriteLine(d13(7));

        }

        static string print(int i)
        {
            return "Int of str:" + i.ToString();
        }

    }

    /** binds unbinds methods to delegate varible handler */
    public class DelegateReceiver
    {
        Del1 delHandler;

        public void RegisterDel(Del1 del_)
        {
            this.delHandler += del_;
        }
        public void UnregisterDel(Del1 del_)
        {
            this.delHandler -= del_;
        }

        public void Fire(int i)
        {
            this.delHandler.Invoke(i);
        }
    }

    /** displays delegate receiver work with print methods */
    public class DelegateEmitter
    {
        public void GO()
        {

            DelegateReceiver du = new DelegateReceiver();
            du.RegisterDel(print2);
            du.RegisterDel(print3);

            du.Fire(2);
            du.UnregisterDel(print2);
            du.Fire(3);
        }
        string print2(int i)
        {
            string ret = "Print2 of str:" + i.ToString();
            Trace.WriteLine(ret);
            return ret;
        }
        string print3(int i)
        {
            string ret = "Print3 of str:" + i.ToString();
            Trace.WriteLine(ret);
            return ret;
        }
    }


    public class DelegatesArray
    {
        //delegate type declaration
        delegate void GetStringDel(string i);
        //variables of delegate type declaratoin
        GetStringDel PrintString;
        GetStringDel PrintString2;
        GetStringDel PrintString3;
        Random rnd = new Random();
        GetStringDel[] arr;
        public DelegatesArray()
        {

        }

        public void GO()
        {

            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            //delegate initializations
            //with method name		  
            PrintString = PrintStringA;

            //#2.0 anonimous init
            PrintString2 = delegate (string i) { Trace.WriteLine(@"Anonimous init for: " + i); };

            //#3.0 lambda
            PrintString3 = (x) => { Trace.WriteLine(@"Lambda init for: " + x); };

            //change method order at runtime from Rand values
            arr = new GetStringDel[rnd.Next(1, 10)];
            for (int i = 0; i < arr.Length; i++)
            {
                int val = rnd.Next(1, 1000);
                if (val % 2 == 0)
                {
                    arr[i] = PrintString;
                }
                if (val % 3 == 0)
                {
                    arr[i] = PrintString2;
                }
                if (val % 5 == 0)
                {
                    arr[i] = PrintString3;
                }
                if (arr[i] == null)
                {
                    arr[i] = PrintString3;
                }
            }
        }
        public void DelegatesInvoke()
        {
            PrintString3.Invoke(@"Invoke");

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].Invoke(" invoked from Arr in position " + i);
            }
        }
        public void PrintStringA(string i)
        {
            Trace.WriteLine(" Method init for  : " + @" " + i);
        }

    }

    public static class DelegateCheck
    {
        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            DelegateInvokation.GO();

            DelegateEmitter dr = new DelegateEmitter();
            dr.GO();

            DelegatesArray delArray = new DelegatesArray();
            delArray.GO();
        }

    }



    /*events*/
    /*--------------------------------------------- */
    /* event argument classes */
    public class SpeedChangedEventArgs : EventArgs { public float speed { get; set; } }
    public class EngineBrokeEventArgs : EventArgs { public bool broken { get; set; } }



    /*events with named classes */


    /*
    class ->
    eventType : EventArgs {}

    class ->
    EventHadler<eventType> eventToSubscribeAndFire;

    handler method(eventType e) ->
    EventHadler<eventType> handler=eventToSubscribeAndFire;
    handler(this,e)

    emitter method ->
    eventType et = new eventType();
    handler method(et) 


    method ->
    Listen(this,e)


    eventToSubscribeAndFire+=Listen;
    */


    public class CreateEvent : EventArgs
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Emitter
    {
        int ID { get; set; }
        string Name { get; set; }

        public event EventHandler<CreateEvent> _handler;
        public Emitter()
        {

        }
        public void Add(int id_, string name_)
        {
            CreateEvent event_ = new CreateEvent();
            this.ID = id_;
            this.Name = name_;
            event_.ID = id_;
            event_.Name = Name;
            OnCreateEvent(event_);
        }

        protected virtual void OnCreateEvent(CreateEvent e)
        {
            EventHandler<CreateEvent> handler_ = _handler;
            if (handler_ != null)
            {
                handler_(this, e);
            }
        }

    }
    public class Receiver
    {
        public void ReceiveEvent(object sender, CreateEvent e)
        {
            Trace.WriteLine(@"Event raised for sender: " + sender + @"; e: " + e.ID + @" " + e.Name);
        }
    }
    public static class Event
    {
        public static void Check()
        {
            Receiver rc = new Receiver();
            Emitter em = new Emitter();
            em._handler += rc.ReceiveEvent;
            em.Add(1, @"Added");
        }
    }



    /* event emitter class */
    public class Car
    {

        //event handlers by classes
        public event EventHandler<SpeedChangedEventArgs> speedChangeEvent;
        public event EventHandler<EngineBrokeEventArgs> brokeChangeEvent;

        //event handler method
        public virtual void OnSpeedChanged(SpeedChangedEventArgs e)
        {
            EventHandler<SpeedChangedEventArgs> handler = speedChangeEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public virtual void OnEngineBroke(EngineBrokeEventArgs e)
        {
            EventHandler<EngineBrokeEventArgs> handler = brokeChangeEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        float speed { get; set; }
        public string name { get; set; }

        //event emitting method
        public void speedIncrease(float speed_)
        {
            this.speed += speed_;

            SpeedChangedEventArgs args = new SpeedChangedEventArgs();
            args.speed = this.speed;

            OnSpeedChanged(args);
            engineCheck();
        }
        public void engineCheck()
        {
            if (this.speed > 100)
            {
                EngineBrokeEventArgs args = new EngineBrokeEventArgs();
                args.broken = true;
                OnEngineBroke(args);
            }
        }
    }
    public class SpeedListener
    {
        public void ListenToSpeed(object cr_, SpeedChangedEventArgs e)
        {
            if (cr_.GetType().Equals(typeof(Car)))
            {
                Car cr = (Car)cr_;
                Trace.WriteLine($"Speed changed: { cr.name}  {e.speed}");
            }
        }
    }
    public class EngineListener
    {
        public void ListenToEngine(object o, EngineBrokeEventArgs e)
        {
            if (o.GetType().Equals(typeof(Car)))
            {
                Car cr = (Car)o;
                Trace.WriteLine($"Car broke: { cr.name}  {e.broken}");
            }
        }
    }

    /*events with cancellation*/
    public class PrintOrCancell : EventArgs { public string toPrint { get; set; } public bool stop { get; set; } = false; }
    public class PrinterEmitter
    {
        public event EventHandler<PrintOrCancell> onPrint;

        public void print(List<string> toPrint)
        {
            foreach (string str_ in toPrint)
            {
                PrintOrCancell args = new PrintOrCancell() { toPrint = str_ };
                onPrint?.Invoke(this, args);
                if (args.stop)
                {
                    break;
                }
            }

        }

    }
    public class PrintListener
    {
        public void lsiten(object e, PrintOrCancell args)
        {
            Trace.WriteLine(args.toPrint);
            if (args.toPrint.Length > 6) { args.stop = true; }
        }
    }

    /*updated event from core, not need to inherit from EventArgs*/
    public class CountOrCall { public int toCount { get; set; } public bool stop { get; set; } = false; }
    public class CountEmitter
    {
        public event EventHandler<CountOrCall> onCount;
        public void count(List<int> cnt_)
        {
            EventHandler<CountOrCall> handler = onCount;
            foreach (int i_ in cnt_)
            {
                CountOrCall args = new CountOrCall() { toCount = i_ };
                handler?.Invoke(this, args);
                if (args.stop)
                {
                    break;
                }
            }
        }
    }
    public class CountListener
    {
        public void listen(object o, CountOrCall e)
        {
            for (int i = 0; i < e.toCount; i++)
            {
                Console.Write(i);
            }

            if (e.toCount > 2) { e.stop = true; }
        }
    }

    public class Countlarge { public int? sum { get; set; } public List<int> arr { get; set; } public int[] arr_ { get; set; } }
    public class CountAsync
    {
        static int st = 0;
        public event EventHandler<Countlarge> onCnt;
        public void toCount(List<int> arr_)
        {
            EventHandler<Countlarge> handler = onCnt;
            Countlarge args = new Countlarge() { arr = arr_ };
            handler?.Invoke(this, args);
            while (args.sum == null)
            {
                logToConsole();
            }
            logResult(args.sum);
        }
        public void toCount(int[] arr__)
        {
            EventHandler<Countlarge> handler = onCnt;
            Countlarge args = new Countlarge() { arr_ = arr__ };
            handler?.Invoke(this, args);
            while (args.sum == null)
            {
                logToConsole();
            }
            logResult(args.sum);
        }

        void logToConsole()
        {
            char[] progress = new char[] { '\\', '|', '/', '-' };
            Console.Write("\r Solving: {0}, {1}", DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"), progress[st].ToString());
            if (st < progress.Length - 1) { st += 1; } else { st = 0; }
        }
        void logResult(int? res)
        {
            Trace.Write($"result={res}");
        }
    }
    public class ListenerAsync
    {
        public async void Listen(object o, Countlarge args)
        {
            //arr vs arr_ for list , int[] inmpl
            try
            {
                int i = await Countlarge(args.arr);
                args.sum = i;
            }
            catch (Exception e) { Trace.WriteLine(e.Message); }
        }
        async Task<int> Countlarge(List<int> amt)
        {
            int res = 0;
            await Task.Run(() =>
            {
                foreach (int i in amt)
                {
                    for (int i_ = 0; i_ < i; i_++)
                    {
                        res += 1;
                    }
                }
            });

            return res;
        }
        async Task<int> Countlarge(int[] amt)
        {
            int res = 0;
            int amtLen = amt.Length;
            await Task.Run(() =>
            {
                for (int i = 0; i < amtLen; i++)
                {
                    for (int i_ = 0; i_ < i; i_++)
                    {
                        res += 1;
                    }
                }
            });

            return res;
        }
    }

    /*events check*/
    public class EventsCheck
    {
        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            SampleEventCheck();
            CancelationCheck();
            UpdatedCoreEventCheck();
            //AsyncEventsCheck();
            //NamedEventsCheck();
        }

        static void SampleEventCheck()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            Car car0 = new Car() { name = "car0" };
            Car car1 = new Car() { name = "car1" };
            SpeedListener sl = new SpeedListener();
            EngineListener el = new EngineListener();

            car0.speedChangeEvent += sl.ListenToSpeed;
            car0.brokeChangeEvent += el.ListenToEngine;

            car0.speedIncrease(10.0F);
            car0.speedIncrease(80.0F);
            car0.speedIncrease(11.0F);
        }
        static void CancelationCheck()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            List<string> strs = new List<string>() { "a", "aa", "aaa", "aaaa", "aaaaa", "aaaaaa" };
            PrinterEmitter pe = new PrinterEmitter();
            PrintListener pl = new PrintListener();
            pe.onPrint += pl.lsiten;
            pe.print(strs);
        }
        static void UpdatedCoreEventCheck()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            CountEmitter ce = new CountEmitter();
            CountListener cl = new CountListener();
            List<int> cnt = new List<int>() { 1, 2, 3, 4, 5, 6 };
            ce.onCount += cl.listen;
            ce.count(cnt);
        }
        static void AsyncEventsCheck()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            CountAsync ca = new CountAsync();
            ListenerAsync la = new ListenerAsync();
            ca.onCnt += la.Listen;

            List<int> intL = new List<int>();
            int[] intArr = new int[100001];

            for (int i = 0; i < 100000; i++) { intL.Add(i); intArr[i] = i; }

            //list implementation, need to e.arr change
            ca.toCount(intL);

            //arr implementation, need to e.arr_ change
            //ca.toCount(intArr);
        }

        static void NamedEventsCheck()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            Receiver rc = new Receiver();
            Emitter em = new Emitter();
            em._handler += rc.ReceiveEvent;
            em.Add(1, @"Added");
        }
    }



    /*Interfaces */
    /*--------------------------------------------- */
    /*Explicit Initialization */
    interface IPrintInt
    {
        void Print(int i);
    }
    interface IPrintDouble
    {
        void Print(Double i);
    }

    public class PrintIntDouble : IPrintInt, IPrintDouble
    {
        void IPrintInt.Print(int i) { Console.WriteLine(i); }
        void IPrintDouble.Print(double i) { Console.WriteLine(i); }
    }
    static class ExplicitInterfacesCheck
    {
        public static void Check()
        {
            IPrintDouble pri = (IPrintDouble)new PrintIntDouble();
            IPrintInt prd = (IPrintInt)new PrintIntDouble();

            pri.Print(0.321);
            prd.Print(123);

        }
    }



    /* Indexer */
    /*--------------------------------------------- */
    interface IIndexer_
    {
        int this[int index] { get; set; }
    }
    class Indexer_ : IIndexer_
    {
        int[] arr = new int[100];

        public int this[int index]
        {
            get { return arr[index]; }
            set { this.arr[index] = value; }
        }
    }
    public static class IndexerInterfaceCheck
    {
        public static void Check()
        {
            Indexer_ ind = new Indexer_();
            ind[0] = 1;
            Console.WriteLine(ind[0]);
        }
    }



    /* Linked List */
    /*--------------------------------------------- */
    public class Document
    {
        internal string Title { get; set; }
        string Content { get; set; }
        internal byte Priority { get; set; }

        public Document(string title, string content, byte priority)
        {
            this.Title = title;
            this.Content = content;
            this.Priority = priority;
        }

    }
    public class PriorityDocumentManager
    {
        internal readonly LinkedList<Document> documentList;
        internal readonly List<LinkedListNode<Document>> nodeList;

        public PriorityDocumentManager()
        {
            documentList = new LinkedList<Document>();

            nodeList = new List<LinkedListNode<Document>>(10);

            for (int i = 0; i < 10; i++)
            {
                nodeList.Add(new LinkedListNode<Document>(null));
            }
        }

        public void AddDocument(Document doc)
        {
            if (doc == null) throw new ArgumentNullException("doc");

            AddDocumentToPriorityNode(doc, doc.Priority);
        }

        public void AddDocumentToPriorityNode(Document document, int priority)
        {

            if (priority > 9 || priority < 0)
            {
                throw new ArgumentException("Priority no in 1-9 range");
            }

            if (nodeList[priority].Value == null)
            {
                --priority;
                if (priority >= 0)
                {
                    AddDocumentToPriorityNode(document, priority);
                }
                else
                {
                    documentList.AddLast(document);
                    nodeList[document.Priority] = documentList.Last;
                }
                return;
            }
            else
            {
                LinkedListNode<Document> prioNode = nodeList[priority];
                if (priority == document.Priority)
                {
                    documentList.AddAfter(prioNode, document);
                    nodeList[document.Priority] = prioNode.Next;
                }
                else
                {
                    LinkedListNode<Document> firstPrioNode = prioNode;
                    while (firstPrioNode.Previous != null &&
                        firstPrioNode.Previous.Value.Priority == prioNode.Value.Priority)
                    {
                        firstPrioNode = prioNode.Previous;
                        prioNode = firstPrioNode;
                    }

                    documentList.AddBefore(firstPrioNode, document);

                    nodeList[document.Priority] = firstPrioNode.Previous;
                }

            }
        }

        public void DisplayAllNodes()
        {
            foreach (Document doc in documentList)
            {
                Debug.WriteLine("Document: {0}, with priority: {1}", doc.Title, doc.Priority);
            }
        }

        public static void NodesInit()
        {
            var pdm = new PriorityDocumentManager();

            pdm.AddDocument(new Document("One", "sample", 9));
            pdm.AddDocument(new Document("Two", "sample", 7));
            pdm.AddDocument(new Document("Three", "sample", 9));
            pdm.AddDocument(new Document("Four", "sample", 7));

            pdm.DisplayAllNodes();
        }
    }
    public class LinkedListSwitch
    {
        public LinkedListSwitch()
        {
            string[] words = { "A", "B", "C", "D", "E" };
            LinkedList<string> sentence = new LinkedList<string>(words);

            display(sentence);
            LinkedListNode<string> node = sentence.Find("C");

            sentence.AddAfter(node, "C2");
            sentence.AddBefore(node, "C0");

            node = sentence.Find("C");
            sentence.Remove(node);
            sentence.AddFirst(node);

            node = sentence.Find("C0");
            LinkedListNode<string> nodep = node.Previous;
            LinkedListNode<string> noden = node.Next;

            sentence.Remove(node.Previous);
            sentence.Remove(node.Next);
            sentence.Remove(node);

            sentence.AddFirst(node);
            sentence.AddFirst(nodep);
            sentence.AddFirst(noden);

            display(sentence);

        }

        public void display(LinkedList<string> ls)
        {
            foreach (string st in ls)
            {
                Debug.WriteLine(st);
            }
            Debug.WriteLine("-----");
        }
        public void displayNode(LinkedListNode<string> node_)
        {
            try
            {
                Debug.WriteLine(node_.Value);
                Debug.WriteLine(node_.Previous.Value);
                Debug.WriteLine(node_.Next.Value);
            }
            catch (Exception e)
            {
                Trace.Write(e.Message);
            }
        }
    }

    public static class LinkedListsCheck
    {
        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            PriorityDocumentManager.NodesInit();

            LinkedListSwitch ls = new LinkedListSwitch();
        }
    }



    /*--------------------------------------------- */
    public class StringBuilderChecker
    {

    }



    /*Async,Multithreading,Parallel*/
    /*--------------------------------------------- */
    //ADD new

    public class AsyncCheck
    {
        static string result = string.Empty;

        public async static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            AsyncCheck asCheck = new AsyncCheck();
            await asCheck.SumResultOfCollectionOfTasks();
        }

        public async Task<int> GO_async()
        {
            Trace.WriteLine("GO async started");
            Trace.WriteLine($"result = {result}");
            var r = await GetStringAsync();
            Trace.WriteLine("GO async finished");
            Trace.WriteLine($"result = {result}");
            Trace.WriteLine($"r = {r}");
            return 1;
        }
        public async Task<string> GetStringAsync()
        {
            await ChangeStringAsync();
            return result;
        }
        public async Task<int> ChangeStringAsync()
        {
            await Task.Delay(200);
            result = "finished";
            return 1;
        }


        /*Summs result of several counting tasks */
        public async Task<string> SumResultOfCollectionOfTasks()
        {

            List<Task<int>> tasks = new List<Task<int>>()
            {
                DoSomeDelayedrWork(1000)
                , DoSomeDelayedrWork(1000)
                , DoSomeDelayedrWork(500)
            };

            var awaitedresult = await Task.WhenAll(tasks);

            var res = awaitedresult.Sum();

            return String.Empty;
        }



        public async Task<int> DoSomeOtherWork()
        {

            await Task.Delay(30);
            return 1;

        }
        public async Task<int> DoSomeDelayedrWork(int delay)
        {

            await Task.Delay(delay);
            return 1;

        }
    }

    public class FakeThreadSleepForTest
    {
        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            for (int i = 0; i < 2000; i++)
            {
                new Thread(o => { Thread.Sleep(100); }).Start();
            }
        }
        public async Task FakeLongRunningTask()
        {
            await Task.Factory.StartNew(() => Thread.Sleep(5000));
        }
    }


    /*Reflections */
    /*--------------------------------------------- */
    public class ModelForReflectionOne
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class ModelForReflectionTwo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [New(AttrType = true)]
        public string Sername { get; set; }
    }
    //custom attribute class
    public class NewAttribute : Attribute
    {
        public bool AttrType { get; set; }
    }
    public class Reflections
    {

        public void LoopThroughtAssemblyReflections()
        {

            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            Assembly asm = Assembly.GetCallingAssembly();
            Type[] types_ = asm.GetTypes();
            List<Type> types_filtered = types_.Where(s => s.IsAbstract == false && s.IsSealed == false)
            .Where(t => !t.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute), true)).ToList();

            foreach (Type b in types_filtered)
            {
                //defining non static type
                if (b.IsClass && b.IsAbstract == false && b.IsSealed == false && b.IsGenericType == false)
                {
                    var constructor = b.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, Type.EmptyTypes, null);

                    if (constructor != null)
                    {
                        var c = Activator.CreateInstance(b);
                        Trace.WriteLine($"TypeName instatiated from asm: {c.GetType()}");
                    }
                }
            }
        }

        public void LoopThroughtProperties()
        {

            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            List<ModelForReflectionTwo> ClassCollection = new List<ModelForReflectionTwo>(){
                new ModelForReflectionTwo(){Id= new Guid(), Name ="Name1", Sername="Sername2"}
            };

            Type type = typeof(ModelForReflectionTwo);
            PropertyInfo[] b = type.GetProperties();

            foreach (PropertyInfo c in b)
            {
                IEnumerable<Attribute> pa = c.GetCustomAttributes();
                var ca = c.GetCustomAttributes(typeof(NewAttribute), false).FirstOrDefault();

                foreach (Attribute v in pa)
                {
                    NewAttribute na = v as NewAttribute;
                    bool val = (bool)na.AttrType;
                }

                Trace.WriteLine(c.Name);
                foreach (ModelForReflectionTwo item in ClassCollection)
                {
                    var d = item.GetType().GetProperty(c.Name).GetValue(item);
                    Trace.WriteLine($"Property Name, Value: {c.Name}, {d}");
                }
            }
        }
    }

    public class ReflectionsCheck
    {
        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            Reflections r = new Reflections();
            r.LoopThroughtAssemblyReflections();
            r.LoopThroughtProperties();
        }
    }



    /*Sockets */
    /*--------------------------------------------- */
    public class SocketListener
    {
        public void Listen()
        {

            Socket listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.IPv6Any, 2112));
            listener.Listen(10);

            while (true)
            {
                Console.WriteLine(@"Waiting...");
                Socket socket = listener.Accept();
                string receive = string.Empty;
                string reply = string.Empty;

                while (true)
                {
                    try
                    {
                        byte[] bytes = new byte[1024];
                        int numBytes = socket.Receive(bytes);

                        Console.WriteLine(@"Receiving");
                        receive += Encoding.ASCII.GetString(bytes, 0, numBytes);

                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                    }

                    if (receive.IndexOf("[FINAL]") > -1)
                    {
                        break;
                    }


                }

                reply = @"Received: " + receive;
                Console.WriteLine(@"Reply: " + reply);
                byte[] response = Encoding.BigEndianUnicode.GetBytes(reply);
                try
                {
                    socket.Send(response);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

            }
        }

    }

    public class SocketClient
    {
        public void Send()
        {
            byte[] receivedBytes = new byte[1024];
            IPHostEntry ipHost = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAdress = ipHost.AddressList[0];

            while (true)
            {
                IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, 2112);

                Socket sender = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(ipEndpoint);
                    Console.WriteLine("Connected");
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }

                string sendMessage = Console.ReadLine();
                byte[] sendBytes = Encoding.ASCII.GetBytes(sendMessage + @"[FINAL]");

                try
                {
                    sender.Send(sendBytes);
                    int receivedNum = sender.Receive(receivedBytes);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }

                if (sendMessage.IndexOf("[FINAL]") > -1)
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }

        }

    }



    /*IO */
    /*--------------------------------------------- */
    /*drive read*/
    public static class DriveRead
    {
        private static DriveInfo[] drives_;
        private static DirectoryInfo[] directories_;
        private static FileInfo[] fileInfo_;

        public static void Disk()
        {
            BindDrives();
            ReadDrives(drives_);
            ReadDirectories(directories_);
        }

        private static void BindDrives()
        {
            drives_ = DriveInfo.GetDrives();
        }
        private static void ReadDrives(DriveInfo[] drives_)
        {
            foreach (DriveInfo di in drives_)
            {
                DirectoryInfo rootDir_ = di.RootDirectory;
                directories_ = rootDir_.GetDirectories();
            }
        }
        private static void ReadDirectories(DirectoryInfo[] dirs_)
        {
            foreach (DirectoryInfo dirInf_ in dirs_)
            {
                if (dirInf_.GetDirectories().Length != 0)
                {
                    ReadDirectories(dirInf_.GetDirectories());
                }
                else
                {
                    fileInfo_ = dirInf_.GetFiles();
                }
            }
        }
        private static void ReadFiles(FileInfo[] files_)
        {
            foreach (FileInfo fi_ in files_)
            {

            }
        }
    }



    /*--------------------------------------------- */
    /*Overriding comparer and gethashcode methods*/
    public class ComparerOverride
    {
        public class InstrumentComparerCheck
        {
            public static void GO()
            {
                Instrumnent i0 = new Instrumnent() { };
                i0.AddCode("cd1");
                i0.AddCode("cd2");
                Instrumnent i2 = new Instrumnent() { };
                i2.AddCode("cd1");
                i2.AddCode("cd2");
                Instrumnent i3 = new Instrumnent() { };
                i3.AddCode("cd1");
                i3.AddCode("cd3");
                Instrumnent i4 = new Instrumnent() { };
                i4.AddCode("cd2");
                i4.AddCode("cd1");

                var a0 = i0.Equals(i2);
                var a1 = i0.Equals(i3);
                var a2 = i0.Equals(i4);

                InstrumentComparer ic = new InstrumentComparer();
                var b0 = ic.Equals(i0, i2);
                var b1 = ic.Equals(i0, i3);
                var b2 = ic.Equals(i0, i4);

            }
        }

        //Equals GetHashCode overide
        public class Instrumnent
        {
            private List<string> instrumentCodes = new List<string>();
            public List<string> InstrumentCodes
            {
                get
                {
                    return instrumentCodes;
                }
            }

            public void AddCode(string code)
            {
                instrumentCodes.Add(code);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Instrumnent);
            }
            public bool Equals(Instrumnent item)
            {
                if (item.instrumentCodes.Where(s => item.instrumentCodes.Contains(s)).Any())
                {
                    return true;
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hashCode = 13;
                unchecked
                {
                    foreach (string i in this.InstrumentCodes)
                    {
                        hashCode = (hashCode * 397) ^ i.GetHashCode();
                    }

                    return hashCode;
                }
            }
        }

        //Equlas and GetHashcode basic realization
        public class InstrumentComparer : IEqualityComparer<Instrumnent>
        {

            public bool Equals(Instrumnent i1, Instrumnent i2)
            {
                if (i1.InstrumentCodes.SequenceEqual(i2.InstrumentCodes))
                {
                    return true;
                }
                return false;
            }

            public int GetHashCode(Instrumnent ic)
            {
                return ic.GetHashCode();
            }

        }

        public static class HashCodeCheck
        {
            public static void GO()
            {

                //all return different hashes 

                char[] ch1 = new char[] { 'a' };
                char[] ch2 = new char[] { 'a' };
                string s1 = string.Join("", ch1);
                string s2 = string.Join("", ch2);
                int h1 = ch1.GetHashCode();
                int h2 = ch2.GetHashCode();
                int h21 = s1.GetHashCode();
                int h22 = s2.GetHashCode();

                byte[] bt1 = Encoding.UTF8.GetBytes(ch1);
                byte[] bt2 = Encoding.UTF8.GetBytes(ch2);
                int h31 = bt1.GetHashCode();
                int h32 = bt2.GetHashCode();

                using (MD5 m = MD5.Create())
                {
                    byte[] h41 = m.ComputeHash(bt1);
                    byte[] h42 = m.ComputeHash(bt2);
                }

            }
        }

        public class GenericHashComparer<T>
        : IEqualityComparer<T> where T : class, IEnumerable<T>
        {
            public bool Equals(T x, T y)
            {
                HashSet<T> t1 = x as HashSet<T>;
                HashSet<T> t2 = y as HashSet<T>;
                if (t1.Intersect(t2).Any())
                {
                    return true;
                }
                return false;
            }

            public int GetHashCode(T obj)
            {
                var hashCode = 13;
                unchecked
                {
                    foreach (T i in obj)
                    {
                        hashCode = (hashCode * 397) ^ i.GetHashCode();
                    }
                }
                return hashCode;
            }
        }

    }
    
    //hash from different collections compare check
    //---------------------------------------------
    public static class HashCodeCheck
    {
        public static void GO()
        {

            //all return different hashes 

            char[] ch1 = new char[] { 'a' };
            char[] ch2 = new char[] { 'a' };
            string s1 = string.Join("", ch1);
            string s2 = string.Join("", ch2);
            int h1 = ch1.GetHashCode();
            int h2 = ch2.GetHashCode();
            int h21 = s1.GetHashCode();
            int h22 = s2.GetHashCode();

            byte[] bt1 = Encoding.UTF8.GetBytes(ch1);
            byte[] bt2 = Encoding.UTF8.GetBytes(ch2);
            int h31 = bt1.GetHashCode();
            int h32 = bt2.GetHashCode();

            using (MD5 m = MD5.Create())
            {
                byte[] h41 = m.ComputeHash(bt1);
                byte[] h42 = m.ComputeHash(bt2);
            }

        }
    }
    
}

namespace LINQtoObjectsCheck
{
    /*Models*/
    public class Racer
    {
        public string Name { get; set; }
        public string Sername { get; set; }
        public string Car { get; set; }
        public int Year { get; set; }
    }
    public class Cup
    {
        public string Competition { get; set; }
        public int Year { get; set; }
        public int Position { get; set; }
        public string RacerName { get; set; }
    }
    public class PetOwners
    {
        public string OwnerName { get; set; }
        public int OwnerInt { get; set; }
        public List<string> PetsStr { get; set; }
        public List<PetForOwner> Pets { get; set; }
    }
    public class PetForOwner
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public PetOwners Owner { get; set; }
    }


    public class Instrument
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Instrument Underlying { get; set; }
    }
    public class Position
    {
        public Instrument instrument { get; set; }
        public decimal Quantity { get; set; }
    }
    public class Price
    {
        public int InstrumentID { get; set; }
        public decimal Value { get; set; }
    }


    public interface Iid
    {
        int ID { get; set; }
    }

    public static class Log
    {
        public static void ToLog(IEnumerable<object> item)
        {
            foreach (var i in item)
            {
                Console.WriteLine(i);
            }
        }
    }


    public class Facility
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<ServiceTypes> Services { get; set; }

    }


    public class ServiceTypes : Iid
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime SomeDate { get; set; }
    };

    class Person
    {
        public string Name { get; set; }
    }

    class Pet
    {
        public string Name { get; set; }
        public Person Owner { get; set; }
    }


    public class Customer
    {
        public string Name { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
    }


    public class User
    {
        public int ID { get; set; }
        public string name { get; set; }
        public IList<Address> Address { get; set; }

        private Address addresPriv;
        public readonly Address addrRdn;

        public void BindPrivAddr(string name)
        {
            this.addresPriv.name = name;
        }
    }

    public class Address : Iid
    {
        public int ID { get; set; }
        public string name { get; set; }

    }



    /*
        Item1 1-8 prop1
        8-8 prop2

        Item2 1-8 prop1
            1-8 item1
    */
    public class Item1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }

        public List<Property1> properties { get; set; }
    }
    public class Property1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Property2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Item1ToP2
    {
        public class itemToP { public Item1 item { get; set; } public Property2 prop { get; set; } }
        public List<itemToP> itemsToProp { get; set; }
    }

    public class Item2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        
        public TimeSpan? Duration { get; set; }
        

        public List<Property1> properties { get; set; }

        public List<int> Items1Ids { get; set; }
    }

    public record struct r(string name, int id);

    public struct st
    {
        public string name { get; set; }
        public int id { get; set; }
    }
    
    
    public class LinqCheck
    {
        
        public static List<Racer> racers = new List<Racer>();
        public static List<Cup> cups = new List<Cup>();

        static LinqCheck lc = new LinqCheck();

        public static void GO()
        {
            r r0 = new ("a", 1);
            r r1 = new ("a", 1);

            var rer = r0 == r1;
            r1 = new ("st2",2);
            var r3 = r0 with {name = "b"};
            var r4 = r3 with {name = "c", id = 2};
            var r5 = r3 with {name = "d", id = 5};

            st st0 = new st() {name = "a", id = 1};
            st st1 = new st() {name = "a", id = 1};

            var st2 = st0 with {name = "b", id = 3};
            
            var ses = st0.Equals(st1);
            st0.id = 2;

            string intStr = "123";


            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

            NewOverallCasesCheck();

            racers.Add(new Racer { Name = @"Racer1", Sername = @"sername1", Year = 1990, Car = @"car1" });
            racers.Add(new Racer { Name = @"Racer2", Sername = @"sername2", Year = 1991, Car = @"car1" });
            racers.Add(new Racer { Name = @"Racer3", Sername = @"sername3", Year = 1990, Car = @"car2" });
            racers.Add(new Racer { Name = @"Racer4", Sername = @"sername4", Year = 1990, Car = @"car3" });
            racers.Add(new Racer { Name = @"Racer5", Sername = @"sername5", Year = 1990, Car = @"car2" });
            racers.Add(new Racer { Name = @"Racer6", Sername = @"sername6", Year = 1990, Car = @"car3" });
            racers.Add(new Racer { Name = @"Racer7", Sername = @"sername7", Year = 1991, Car = @"car1" });
            racers.Add(new Racer { Name = @"Racer8", Sername = @"sername8", Year = 1991, Car = @"car3" });
            racers.Add(new Racer { Name = @"Racer9", Sername = @"sername9", Year = 1991, Car = @"car1" });

            cups.Add(new Cup { Competition = @"Cup1", Year = 1990, Position = 1, RacerName = @"Racer3" });
            cups.Add(new Cup { Competition = @"Cup1", Year = 1990, Position = 2, RacerName = @"Racer2" });
            cups.Add(new Cup { Competition = @"Cup2", Year = 1991, Position = 1, RacerName = @"Racer2" });
            cups.Add(new Cup { Competition = @"Cup2", Year = 1991, Position = 2, RacerName = @"Racer3" });
            cups.Add(new Cup { Competition = @"Cup2", Year = 1991, Position = 3, RacerName = @"Racer1" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 1, RacerName = @"Racer4" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 2, RacerName = @"Racer5" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 3, RacerName = @"Racer3" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 4, RacerName = @"Racer1" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 5, RacerName = @"Racer7" });
            cups.Add(new Cup { Competition = @"Cup4", Year = 1989, Position = 1, RacerName = @"Racer1" });
            cups.Add(new Cup { Competition = @"Cup5", Year = 1988, Position = 1, RacerName = @"Racer2" });
            cups.Add(new Cup { Competition = @"Cup6", Year = 1985, Position = 1, RacerName = @"Racer10" });
            cups.Add(new Cup { Competition = @"Cup7", Year = 1991, Position = 1, RacerName = @"Racer11" });

            var txt = @"aa b c das asdaa sd aa s aas";
            string src = "aa";
            var schcnt = txt.Split().Where(s => s.ToLowerInvariant() == src.ToLowerInvariant()).Count();


            //where any
            var racersWhereAny = racers
                .Where(s => cups.Any(c => s.Name == c.RacerName))
                .ToList();

            //where exist
            var cupsWhereExist = cups?
                .Where(s => racers.Exists(c => s.RacerName == c.Name))
                .ToList();

            //Aggregate
            var list = new List<int>() { 1, 2, 3, 4, 5, 6 };
            var aggregate = list.Aggregate(
                seed: 0,
                func: (result, item) => result + item,
                resultSelector: res => res / list.Count()
            );

            /* Group by count on join of Anonymous dynamic left join */
            var adventures = new List<dynamic>() {
                    new { Id = 1, Name = "nm1" }, new { Id = 2, Name = "nm2" }, new { Id = 3, Name = "nm3" }
                };
            var events = new List<dynamic>() {
                    new { Id = 0, Evnt = "evnt1", adventurerId = 1 },
                    new { Id = 1, Evnt = "evnt2", adventurerId = 1 },
                    new { Id = 2, Evnt = "evnt3", adventurerId = 2 },
                    new { Id = 3, Evnt = "evnt4", adventurerId = 0 },
                    new { Id = 4, Evnt = "evnt5", adventurerId = 0 }
                };

            /*Query group by*/
            var joinGroupCnt =
            (
                from s1 in adventures
                join s2 in events on s1.Id equals s2.adventurerId into jn
                from s3 in jn.DefaultIfEmpty()
                group new { s3 } by new { name = s1.Name } into g
                select new
                {
                    name = g.Key,
                    adv = g.Count(o => !string.IsNullOrEmpty(o?.s3?.Evnt))
                }
            ).ToList();

            /*Method join group count*/
            var gp = adventures.Join(events,
                adv => adv.Id,
                evt => evt.adventurerId,
                (adv, evt) =>
                    new { Name = adv.Name, Evt = evt.Evnt }
                )
                .GroupBy(q => q.Name)
                .Select(s => new
                {
                    Name = s.Key,
                    Count = s.Count(o => !string.IsNullOrEmpty(o?.Evt))
                })
                .ToList();

            Person magnus = new Person { Name = "Hedlund, Magnus" };
            Person terry = new Person { Name = "Adams, Terry" };
            Person charlotte = new Person { Name = "Weiss, Charlotte" };

            Pet barley = new Pet { Name = "Barley", Owner = terry };
            Pet boots = new Pet { Name = "Boots", Owner = terry };
            Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
            Pet daisy = new Pet { Name = "Daisy", Owner = magnus };

            List<Person> people = new List<Person> { magnus, terry, charlotte };
            List<Pet> pets = new List<Pet> { barley, boots, whiskers, daisy };

            // Create a list of Person-Pet pairs where
            // each element is an anonymous type that contains a
            // Pet's name and the name of the Person that owns the Pet.
            var query =
                people.Join(pets,
                            p => p,
                            pt => pt.Owner,
                            (p, pt) =>
                                new { OwnerName = p.Name, Pet = pt.Name })
                .GroupBy(q => q.OwnerName, w => w.Pet.Count())
                .ToList();

            List<PetOwners> petownersWithPets = new List<PetOwners>() {
                    new PetOwners {OwnerName=@"Owner1",OwnerInt=0,PetsStr= new List<string>() { @"Pet1",@"Pet2"}
                    , Pets = new List<PetForOwner>(){ new PetForOwner(){Name=@"Pet1"}, new PetForOwner() { Name = @"Pet2" }}},
                    new PetOwners {OwnerName=@"Owner2",OwnerInt=1,PetsStr= new List<string>() { @"Pet3",@"Pet4", @"Pet5" }
                     , Pets = new List<PetForOwner>(){ new PetForOwner(){Name=@"Pet3"}, new PetForOwner() { Name = @"Pet4" }, new PetForOwner() { Name = @"Pet5" }}},
                    new PetOwners {OwnerName=@"Owner3",OwnerInt=2,PetsStr= new List<string>() { @"Pet6" }
                    , Pets = new List<PetForOwner>() { new PetForOwner() { Name = @"Pet6" }}},
                    new PetOwners {OwnerName=@"Owner4",OwnerInt=3,PetsStr= new List<string>() { @"Pet7",@"Pet8", @"Pet9", @"Pet10" }
                     , Pets = new List<PetForOwner>() { new PetForOwner() { Name = @"Pet7" }, new PetForOwner() { Name = @"Pet8" } ,new PetForOwner() { Name = @"Pet9" },new PetForOwner() { Name = @"Pet10" }}}
                };

            List<PetForOwner> petsWithoutOwners = new List<PetForOwner>(){
                    new PetForOwner(){Name=@"Pet3"}
                    ,new PetForOwner() { Name = @"Pet7" }
                    ,new PetForOwner() { Name = @"Pet11" }
                };

            List<PetForOwner> petsWithOwners = new List<PetForOwner>(){
                    new PetForOwner(){Name = "Pet20", Owner = petownersWithPets[0]}
                    ,new PetForOwner(){Name = "Pet21", Owner = petownersWithPets[0]}

                    ,new PetForOwner(){Name = "Pet22", Owner = petownersWithPets[1]}

                    ,new PetForOwner(){Name = "Pet23", Owner = petownersWithPets[2]}
                    ,new PetForOwner(){Name = "Pet24", Owner = petownersWithPets[2]}
                    ,new PetForOwner(){Name = "Pet25", Owner = petownersWithPets[2]}
                };

            var ownersOfPets =
                from s in petownersWithPets
                join c in petsWithOwners on s equals c.Owner into t
                from c in t.DefaultIfEmpty()
                select new
                {
                    Owner = s?.OwnerName ?? "",
                    Pet = c?.Name ?? ""
                };

            var owersOfPetsAPI =
                petownersWithPets.Join(petsWithOwners, petowner => petowner, pet => pet.Owner, (petowner, pet) => new
                {
                    Owner = petowner.OwnerName,
                    Pet = pet.Name
                });

            //where Any
            var petOwnersWithPetsInList =
                petownersWithPets.Where(s => s.Pets.Any(x => petsWithoutOwners.Any(c => c.Name == x.Name)))
                .ToList();

            //where exists
            var petOwnersWithPetsInListExist =
                petownersWithPets.Where(s => s.Pets.Exists(x => petsWithoutOwners.Exists(c => c.Name == x.Name)))
                .ToList();

            //Join 
            var joinRacerCups =
                racers.Join(cups, r => r.Name, c => c.RacerName, (r, c) => new
                {
                    racer = r.Name,
                    cup = c.Competition
                });

            IEnumerable<string> racerNameIntersect = (from s in cups select s.RacerName).Intersect(from r in racers select r.Name);
            IEnumerable<string> racerNameExcept = (from r in racers select r.Name).Except(from s in cups select s.RacerName);
            IEnumerable<string> cupsZipRacers = (from s in cups select new { s.RacerName, s.Position })
                .Zip(from r in racers select new { r.Car }, (z, x) => (z.RacerName + x.Car));

            IEnumerable<string> d =
                from t in petownersWithPets
                from t2 in t.PetsStr
                select t2;

            IEnumerable<string> e = petownersWithPets.SelectMany(s => s.PetsStr);

            var cupsByYearQuery =
            from t in cups
            group t by new { t.Year } into g
            select new
            {
                Year = g.Key.Year,
                Cnt = g.Count()
            };

            var cupsByYearAPI = cups.GroupBy(p => p.Year, (z, x) => new
            {
                Year = z,
                Cnt = x.Count()
            });

            //group by several columns
            var f =
                from t in cups
                group t by new { t.Competition, t.Year } into t2
                orderby t2.Key.Year, t2.Key.Competition descending
                select new { Comp = t2.Key.Competition, Racer = t2.Key.Year, Count = t2.Count() };

            var RacersCountByCar =
                from s in racers
                group s by new { s.Car } into g
                select new
                {
                    Car = g.Key.Car,
                    RacersCountAll = g.Count()
                };

            var RacersCountByCarAndYear =
                from s in racers
                group s by new { s.Year, s.Car } into g
                select new { Car = g.Key.Car, Year = g.Key.Year, Racers = g.Count() };


            //Aggregate
            var MaxPetsInOnerCnt =
            petownersWithPets.Aggregate<PetOwners, int, int>(0, (s, next) =>
                   next.PetsStr.Count() > s ? next.PetsStr.Count() : s
            , cp => cp);

            //seed is temprary store
            //next is iteratable item
            var MaxPetsPetOwner =
                petownersWithPets.Aggregate<PetOwners, PetOwners, PetOwners>(new PetOwners(),
                    (seed, next) =>
                    (seed?.PetsStr?.Any() != true)
                        ? next
                        :
                            next.PetsStr.Count() > seed?.PetsStr?.Count() ? next : seed,
                po => po);

            //left join from different sources
            var a = (
                from h1 in racers
                join i1 in cups on new { Name = h1.Name } equals new { Name = i1.RacerName } into jn
                from s3 in jn.DefaultIfEmpty()
                select new
                {
                    Name = h1?.Name ?? "",
                    Car = h1?.Car ?? "",
                    Competition = s3?.Competition ?? "",
                    Position = s3?.Position
                }).Skip(2 * 2).Take(2).ToList();

            //join from different sources
            var h = from s in racers select s;
            var i = from s in cups select s;
            var j =
                (from t in h
                 join t2 in i on new { Name = t.Name } equals new { Name = t2.RacerName }
                 select new
                 {
                     t.Name,
                     t.Car,
                     t2.Competition,
                     t2.Position
                 }).Skip(2 * 2).Take(2).ToList();
            ;


            var leftJoin =
            (from s in cups
             join c in racers on s.RacerName equals c.Name into t
             from c in t.DefaultIfEmpty()
             select new
             {
                 Cup = s?.Competition ?? "",
                 Name = s?.RacerName ?? "",

                 NameRigth = c?.Name ?? "",
                 CarRigth = c?.Car ?? ""
             }).ToList();


            //???
            var k =
                from t in h
                join t2 in i on t.Name equals t2.RacerName into t3
                from t2 in t3.DefaultIfEmpty()
                select new
                {
                    t2.Competition,
                    t2.Position,
                    Name = t == null ? @"" : t.Name,
                    Car = t == null ? @"" : t.Car
                };

            var l =
                from t in racers
                let cnt = t.Name.Count()
                orderby cnt descending, t.Car
                select new
                {
                    Name = t.Car,
                    Count = cnt
                };

            var m =
                from t in racers
                group t by t.Car into r
                select new
                {
                    Car = r.Key,
                    Racers = (from t2 in r select t2.Name).Count()
                };


            Func<string, IEnumerable<Cup>> CarByCup =
                cup => from s in cups where s.Competition == cup select s;

            var n = from s in CarByCup("Cup2").Intersect(CarByCup("Cup3")) select s;

            bulkCheck();
            UpdateCheck();
            UpdateNestedCheck();
            MaxDateCheck();
            LinqSumGroupByNew();
            SequenceCheck();
            DeferredCheck();
        }

        static void NewOverallCasesCheck()
        {
            var props2 = new List<Property2>() {
                new Property2(){ Id = 0, Name = "prop1"}
                ,new Property2(){ Id = 1, Name = "prop3"}
                ,new Property2(){ Id = 2, Name = "prop5"}
                ,new Property2(){ Id = 3, Name = "prop6"}
                ,new Property2(){ Id = 4, Name = "prop7"}
            };
            var props1 = new List<Property1>() {
                new Property1(){ Id = 0, Name = "prop1"}
                ,new Property1(){ Id = 1, Name = "prop3"}
                ,new Property1(){ Id = 2, Name = "prop8"}
                ,new Property1(){ Id = 3, Name = "prop9"}
                ,new Property1(){ Id = 4, Name = "prop10"}
            };
            var items1 = new List<Item1>() {
                new Item1(){Id = 0 , Name = "item1", Amount = 2,
                    properties = new List<Property1>(){
                    props1[0]
                }},
                new Item1(){Id = 1 , Name = "item2", Amount = 1,
                    properties = new List<Property1>(){
                    new Property1(){ Id = 0, Name = "prop1"}
                }},
                new Item1(){Id = 2 , Name = "item3", Amount = 3,
                    properties = new List<Property1>(){
                    props1[0],props1[1],props1[2]
                }},
                new Item1(){Id = 3 , Name = "item4", Amount = 2,
                    properties = new List<Property1>(){
                    props1[0],props1[1],new Property1(){ Id = 2, Name = "prop8"}
                }},
                new Item1(){Id = 4 , Name = "item5", Amount = 0,
                    properties = new List<Property1>(){
                    props1[0],props1[1],props1[2]
                }}
                ,
                new Item1(){Id = 5 , Name = "item6",
                    properties = new List<Property1>(){
                    new Property1(){ Id = 5, Name = "prop11"}
                }}
            };
            var items2 = new List<Item2>() {
                new Item2(){Id=0,Name="item1", Amount = 0,
                    properties = new List<Property1>(){
                    props1[0]
                }, Items1Ids = new List<int>(){ 0 } },
                new Item2(){Id=1,Name="item2", Amount = 2,
                    properties = new List<Property1>(){
                    new Property1(){ Id = 0, Name = "prop1"}
                }, Items1Ids = new List<int>(){ 0,1,2 }},
                new Item2(){Id=2,Name="item3", Amount = 1,
                    properties = new List<Property1>(){
                    props1[2],new Property1(){ Id = 2, Name = "prop5"}
                }}
            };
            var itemsToProp1 = new Item1ToP2()
            {
                itemsToProp = new List<Item1ToP2.itemToP>()
                {
                   new Item1ToP2.itemToP(){item = items1[0], prop=props2[0]},
                   new Item1ToP2.itemToP(){item = items1[0], prop=props2[1]},
                   new Item1ToP2.itemToP(){item = items1[0], prop=props2[2]},
                   new Item1ToP2.itemToP(){item = items1[1], prop=props2[2]},
                   new Item1ToP2.itemToP(){item = items1[3], prop=props2[3]}
                }
            };

            var propsToSearch = new List<Property1>() {
                props1[0],new Property1(){ Id = 1, Name = "prop3"},props1[2]
            };
            var propsToSearch2 = new List<Property1>() {
                new Property1(){ Id = 1, Name = "prop8"},props1[2]
            };
            var newProp = new Property1() { Id = 2, Name = "prop8" };
            var refProp = props1[2];

            var propsCol1 = new List<Property1>() { props1[0], props1[1], props1[4] };
            var propsCol2 = new List<Property1>() { props1[0], props1[3], props1[4] };
            var propsToUpdate = new List<Property1>() { props1[2], props1[3] };

            var sm0 = items1.SelectMany(s
                => s.properties, (l, r) => new { item = l.Name, prop = r.Name, amtl = l.Amount }).ToList();
            var sm1 = items2.SelectMany(s
                => s.properties, (l, r) => new { item = l.Name, prop = r.Name, amtr = l.Amount }).ToList();

            var il = items1.SelectMany(k => k.properties, (l, r) => new {l, propName = r.Name});
            var ir = items2.SelectMany(k => k.properties, (l, r) => new {l, propName= r.Name});


            var itemsLeft = new List<Item1>()
            {
                new Item1(){ Id = 0, Name = "Name1"},
                new Item1(){ Id = 1, Name = "Name2"},
                new Item1(){ Id = 2, Name = "Name3"},
                new Item1(){ Id = 7, Name = ""},
                new Item1(){ Id = 8, Name = null},
            };
            
            var itemsRight = new List<Item1>()
            {
                new Item1(){ Id = 4, Name = ""},
                new Item1(){ Id = 5, Name = null},
                new Item1(){ Id = 6, Name = "Name3"}
            };
            var lj0 = itemsLeft.Join(itemsRight, lk => lk.Name, rk => rk.Name, (l,r) => new
            {
                lName = l.Name, lid= l.Id,
                rName = r.Name,rId= r.Id
            }).ToList();

            var lj = itemsLeft
                .GroupJoin(itemsRight, lk => lk.Name, rk => rk.Name,
                    (l, r) => new {lName = l.Name, lid = l.Id, r = r.DefaultIfEmpty()}).SelectMany(k => k.r,
                    (l, r) => new {l.lName, l.lid, rName = r?.Name ?? "Not found"}).ToList();

            File.WriteAllText(@"C:\files\test\leftJoin.json", JsonSerializer.Serialize(lj));
            
            //left join
            var leftGroupJoin = props1.GroupJoin(
                    props2,
                    lk => lk.Name,
                    rk => rk.Name,
                    (l, r) => new {l.Id, l.Name, r})
                //critical
                .SelectMany(p => p?.r?.DefaultIfEmpty(),
                    (l, r) => new {l.Id, l.Name, rName = r?.Name ?? string.Empty })
                .ToList();
            
            //not any
            var propsNotExist = props1.Where(c => !props2.Any(x => x.Name == c.Name))
                .ToList();
            //not all
            var propsNotExistAll = props1.Where(c => props2.All(x => x.Name != c.Name))
                .ToList();

            //0 4
            var intersect = propsCol2.Intersect(propsCol1).ToList();
            //3 
            var except = propsCol2.Except(propsCol1).ToList();
            //0 3 4 1
            var union = propsCol2.Union(propsCol1).ToList();

            //0 3 4
            var fullUpdate = propsToUpdate.Except(propsToUpdate.Except(propsCol2)).Union(propsCol2).ToList();
            //2 3 0 4
            var addUnique = propsToUpdate.Union(propsCol2).ToList();

            //3
            var toRemove = propsToUpdate.Intersect(propsCol2).ToList();
            //0 4 
            var toAdd = propsCol2.Except(propsToUpdate).ToList();
            //2 0 4
            var updateUnique = propsToUpdate.Except(toRemove).Union(toAdd).ToList();

            //intersect - 2 items as by ref            
            var propsIntersec = props1.Intersect(propsToSearch).ToList();


            //0 2 3 4 
            var whereIntersectAny = items1.Where(s => propsToSearch.Intersect(s.properties).Any());
            // 0 1 2 3 4 
            var whereExistsAny = items1.Where(s => propsToSearch.Exists(c => s.properties.Any(x => x.Name == c.Name))).ToList();

            // where any by val compar
            var propsWhereAny = props1.Where(s => propsToSearch.Any(c => c.Name == s.Name)).ToList();
            // where exists
            var propsWhereExists = props1.Where(s => propsToSearch.Exists(c => c.Name == s.Name)).ToList();


            //true 
            var colsEq = propsWhereExists.SequenceEqual(propsWhereAny);

            // exist any to bool
            var propsExist = props1.Exists(s => propsToSearch.Any(c => c.Name == s.Name));


            //where contains nested prop
            var whereAnyContains = items1
                .Where(s => propsToSearch.Any(c => s.properties.Contains(c))).ToList();

            //no items - > val equals
            var newPropItems = items1
                .Where(s => s.properties.Contains(newProp)).ToList();
            //2 items val equals
            var refPropItems = items1
                .Where(s => s.properties.Contains(refProp)).ToList();

            var groupBy = (
                from s1 in items1
                join s2 in items2 on s1.Name equals s2.Name into jn
                from s3 in jn.DefaultIfEmpty()
                group new { s3 } by new { name1 = s1.Name } into g
                select new
                {
                    name = g.Key.name1,
                    count = g.Count(s => !string.IsNullOrEmpty(s?.s3?.Name))
                }
            ).ToList();

            var selectMany = items1.SelectMany(i => i.properties, (l, r) => new { item = l.Name, name = r.Name, amtl = l.Amount })
                .ToList();
            var selectManySelect = items1.SelectMany(i => i.properties, (l, r) => new { item = l, property = r, amtr = l.Amount })
                .Select(s => new
                {
                    item = s.item.Name,
                    name = s.property.Name
                }).ToList();

            var selectManyItems2 = items2.SelectMany(i => i.properties, (l, r) => new { item = l.Name, name = r.Name, amtr = l.Amount });

            //multiple join multiple group by
            var groupByMultipleLeftjoinMultiple =
                (
                from s1 in selectManySelect
                join s2 in selectManyItems2 on new { s1.item, s1.name } equals new { s2.item, s2.name } into jn
                from s3 in jn.DefaultIfEmpty()
                    //group (s3) by new { name = s1.item, prop = s1.name} into g
                    // || or
                group new { s3 } by new { iteml = s1.item, propl = s1.name, itemr = s3?.item, propr = s3?.item } into g
                select new
                {
                    iteml = g.Key.iteml,
                    propl = g.Key.propl,
                    itemr = g.Key.itemr,
                    propr = g.Key.propr,

                    items2Cnt = g.Count(c => !string.IsNullOrEmpty(c?.s3?.name)),
                    items2Sum = g.Sum(c => c?.s3?.amtr)
                }).ToList();

            var groupByMultipleInnerJoin =
                (
                from s1 in selectManySelect
                join s2 in items2 on s1.item equals s2.Name
                group new { s2 } by new { iteml = s1.item, propl = s1.name, itemr = s2?.Name } into g
                select new
                {
                    iteml = g.Key.iteml,
                    propl = g.Key.propl,
                    itemr = g.Key.itemr,

                    item2 = g.Count(c => !string.IsNullOrEmpty(c?.s2?.Name))
                }).ToList();


        }

        public static void bulkCheck()
        {

            List<Address> addresses = new List<Address>(){
            new Address(){ID=0,name="Name_0"},
            new Address(){ID=1,name="Name_1"},
            new Address(){ID=4,name="Name_4"},
            new Address(){ID=5,name="Name_5"},
            };

            List<User> users = new List<User>(){
            new User(){ID=0,name="User_0",Address=new List<Address>(){ addresses[0] } },
            new User(){ID=1,name="User_1",Address=new List<Address>(){ addresses[1], addresses[2] }},
            new User(){ID=2,name="User_2",Address=null},
            new User(){ID=3,name="User_3",Address=new List<Address>(){ addresses[3], addresses[2] }},
            new User(){ID=4,name="User_4",Address=new List<Address>(){ addresses[3], addresses[1] }}
            };

            var query =
                addresses.Join(users,
                            u => u.ID,
                            a => a.ID,
                            (u, a) =>
                                new { OwnerName = u.name, Pet = a.name });

            List<Address> usersNUll = null;
            if (usersNUll?.Any() == true)
            {

            }
            if (usersNUll?.Any() == false)
            {

            }
            if (usersNUll?.Any() != true)
            {

            }
            usersNUll = new List<Address>();
            if (usersNUll?.Any() == true)
            {

            }

            if (usersNUll?.Any() == false)
            {

            }
            if (usersNUll?.Any() != true)
            {

            }
            usersNUll.Add(new Address());
            if (addresses?.Any() == true)
            {

            }

            //testListA.Where(a => !testListB.Any(b => a.ProductID == b.ProductID && a.Category == b.Category));

            List<Address> intersect = addresses.Where(a => users.Any(b => b.ID == a.ID)).ToList();
            List<Address> all = addresses.Where(a => users.Any(b => b.ID == a.ID)).ToList();
            List<Address> except = addresses.Where(a => !users.Any(b => b.ID == a.ID)).ToList();

            var u = users.Where(s => s?.Address != null && s.Address.Count() > 1 && s.Address.Any(c => c.name == "Name_1"))
                .ToList();

            Log.ToLog(query);
        }

        public static void UpdateCheck()
        {

            List<Address> addrLess = new List<Address>(){
                new Address(){ID=0,name="Name_0"},
                new Address(){ID=1,name="Name_1"},
                new Address(){ID=2,name="Name_2"},
                new Address(){ID=3,name="Name_3"},
            };
            List<Address> addrMore = new List<Address>(){
                new Address(){ID=3,name="Name_3"},
                new Address(){ID=4,name="Name_4"},
                new Address(){ID=5,name="Name_5"},
                new Address(){ID=6,name="Name_6"}
            };

            var upd = lc.UpdateExceptAdd<Address>(addrMore, addrLess);
        }

        public static void UpdateNestedCheck()
        {
            List<Facility> fac = lc.GenFacilities(5);

            List<ServiceTypes> serv = new List<ServiceTypes>(){
                new ServiceTypes(){ID=4,Name="name 4"}
                ,new ServiceTypes(){ID=5,Name="name 5"}
                ,new ServiceTypes(){ID=6,Name="name 6"}
            };

            fac.ForEach(s =>
                s.Services = lc.UpdateExceptAdd<ServiceTypes>(serv, s.Services)
            );

        }

        public List<ServiceTypes> GenServices(int num)
        {
            List<ServiceTypes> ret = new List<ServiceTypes>();
            for (int i = 0; i < num; i++)
            {
                ret.Add(new ServiceTypes() { ID = i, Name = i.ToString() });
            }
            return ret;
        }
        public List<Facility> GenFacilities(int num)
        {
            List<Facility> ret = new List<Facility>();
            for (int i = 0; i < num; i++)
            {
                ret.Add(new Facility() { ID = i, Name = i.ToString(), Services = GenServices(i) });
            }
            return ret;
        }

        public IEnumerable<T> UpdateExceptAdd<T>(IEnumerable<T> from, IEnumerable<T> into)
            where T : class, Iid
        {
            var ret = new List<T>();
            var toDelete = into.Where(c => !from.Any(s => c.ID == s.ID)).ToList();
            var toAdd = from.Where(c => !into.Any(s => c.ID == s.ID)).ToList();

            into = into.Except(into.Where(c => !from.Any(s => c.ID == s.ID))).ToList();
            into.ToList().AddRange(toAdd.ToList());

            return into;
        }

        public class MaxDateByName
        {
            public string Name { get; set; }
            public DateTime MaxDate { get; set; }
        }
        public static void MaxDateCheck()
        {
            List<Item> items = new List<Item>(){
                new Item(){Name="Name0", SomeDate = new DateTime(2019,01,01)}
                ,new Item(){Name="Name0", SomeDate = new DateTime(2019,01,01)}
                ,new Item(){Name="Name0", SomeDate = new DateTime(2019,02,01)}
                ,new Item(){Name="Name0", SomeDate = new DateTime(2019,01,01)}

                ,new Item(){Name="Name1", SomeDate = new DateTime(2019,02,01)}
                ,new Item(){Name="Name1", SomeDate = new DateTime(2019,03,01)}
            };

            //select max date
            var maxDate = items.OrderByDescending(c => c.SomeDate).First().SomeDate;

            //select max date by group
            var maxDateByName =
            items
            .Where(v => v.SomeDate != null && v.Name != null)
            .GroupBy(s => s.Name)
            .Select(c => new
            {
                c.Key,
                LastDate = c.OrderByDescending(z => z.SomeDate)
                .Select(x => x.SomeDate)
                .FirstOrDefault()
            });

            //selecting name from annonimous
            var Name = maxDateByName.Where(s => s.Key != null && s.LastDate != null).OrderByDescending(c => c.LastDate).FirstOrDefault()?.Key;

        }

        //if a?.Any() == true  
        //if a?.Any() != true  
        public static void AnyCheckBool()
        {
            List<Item> items2 = new List<Item>()
            {
                new Item{Id=0,Name="nm0"}
                ,new Item{Id=1,Name="nm1"}
            };

            List<Item> itemsEmpty = new List<Item>();
            List<Item> itemsNull = null;


            if (items2?.Any() == true)
            {

            }

            if (itemsEmpty?.Any() == true)
            {

            }
            if (itemsEmpty?.Any() != true)
            {

            }

            if (itemsNull?.Any() == true)
            {

            }
            if (itemsNull?.Any() != true)
            {

            }

        }

        //if (!servicesBefore?.SequenceEqual(toUpdate?.ServiceTypes) == true)
        public static void SequenceCheck()
        {
            Item i0 = new Item { Id = 0, Name = "nm0" };
            Item i1 = new Item { Id = 1, Name = "nm1" };
            Item i2 = new Item { Id = 2, Name = "nm2" };

            List<Item> checkList = new List<Item>()
            {
                i0,i1,i2
            };

            List<Item> listNotEqual = new List<Item>()
            {
                i0,i2
            };

            List<Item> listEqual = new List<Item>()
            {
                i0,i1,i2
            };

            if (!checkList?.SequenceEqual(listEqual) == true)
            {

            }
            //OK
            if (checkList?.SequenceEqual(listEqual) == true)
            {

            }
            if (checkList?.SequenceEqual(listEqual) != true)
            {

            }

            if (!checkList?.SequenceEqual(listNotEqual) == true)
            {

            }
            if (checkList?.SequenceEqual(listNotEqual) == true)
            {

            }
            //OK
            if (checkList?.SequenceEqual(listNotEqual) != true)
            {

            }

        }


        public static void ContainsCheck()
        {
            List<Item> itemsToUpdate = new List<Item>(){
                new Item(){Id=0, Name = "Nm0"}
                ,new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=2, Name = "Nm2"}
            };

            List<Item> newItems = new List<Item>(){
                new Item(){Id=0, Name = "Nm0"}
                ,new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=3, Name = "Nm3"}
                ,new Item(){Id=4, Name = "Nm4"}
            };

            //3 4
            var itemsToAdd = newItems.Where(s => !itemsToUpdate.Exists(c => c.Id == s.Id));

            //2
            var itemsToRemove = itemsToUpdate.Where(s => !newItems.Exists(c => c.Id == s.Id));

            //0 1 3 4
            var newList = itemsToUpdate.Except(itemsToRemove).Union(itemsToAdd);

        }

        public static void WhereAnyContains()
        {

            List<Item> allIds = new List<Item>(){
                new Item(){Id=0, Name = "Nm0"}
                ,new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=2, Name = "Nm2"}
                ,new Item(){Id=3, Name = "Nm3"}
                ,new Item(){Id=4, Name = "Nm4"}
            };

            List<Item> parentIds = new List<Item>(){
                new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=2, Name = "Nm2"}
                ,new Item(){Id=3, Name = "Nm3"}
            };

            List<Item> servicesIds = new List<Item>(){
                    new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=2, Name = "Nm2"}
            };

            var res = allIds.Where(x =>
                servicesIds.Where(s =>
                parentIds.Exists(c => c.Id == s.Id)).ToList()
                    .Exists(z => x.Id == z.Id)
            );

        }

        public static void DeferredCheck()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            List<Item> items = new List<Item>(){
                new Item(){Id=0, Name="nm1"},new Item(){Id=1,Name="nm2"},new Item(){Id=2,Name="nm3"},new Item(){Id=1,Name="nm4"}
            };
            int id = 2;
            var itemById = items.Where(s => s.Id == id);

            id = 1;
            foreach (var i in itemById)
            {
                Trace.WriteLine(i.Name);
                //nm2 nm4
            };
        }

        public static void LinqSumGroupByNew()
        {
            var under1 = new Instrument() { ID = 5, Name = "Eq1" };
            var under2 = new Instrument() { ID = 6, Name = "Eq2" };

            var instr1 = new Instrument() { Underlying = under1 };
            var instr2 = new Instrument() { Underlying = under1 };
            var instr3 = new Instrument() { Underlying = under2 };

            var prices = new[]{
                    new Price {InstrumentID =5, Value = 2},
                    new Price {InstrumentID =6, Value = 10}
                };

            var positions = new[]{
                    new Position {instrument = instr1, Quantity=10}
                    ,new Position {instrument = instr2, Quantity=90}
                    ,new Position {instrument = instr3, Quantity=200}
                };

            //eq1 , amt=200
            //eq2 , amt=2000
            //amt=sum(quant)*price

            var q0 = from t1 in (from s in prices select s)
                     join t2 in (from c in positions select c) on t1.InstrumentID equals t2.instrument.Underlying.ID
                     select new
                     {
                         Id = t2.instrument.Underlying.ID,
                         Price = t2.Quantity * t1.Value
                     };
            q0 = q0.ToList();

            var qt =
            from s2 in (from s in (from z in new List<Instrument>() { under1, under2 } select z).ToList() select new { ID = s.ID })
            join c2 in (from c in positions select new { ID = c.instrument.Underlying.ID, Qnt = c.Quantity, Name = c.instrument.Underlying.Name }) on s2.ID equals c2.ID
            group c2 by new
            {
                c2.Name,
                c2.ID
            } into g
            select new
            {
                Name = g.Key.Name,
                ID = g.Key.ID,
                Amt = g.Sum(t => t.Qnt)
            };

            var pr =
            (from s2 in (from s in (from z in new List<Instrument>() { instr1, instr2, instr3 } select z).ToList() select new { ID = s.Underlying.ID })
             join c2 in (from c in prices select new { ID = c.InstrumentID, Val = c.Value }) on s2.ID equals c2.ID
             select new { s2.ID, c2.Val }).GroupBy(x => x.ID).Select(s => s.First());

            var qt2 = from q in (
            from s2 in (from s in (from z in new List<Instrument>() { under1, under2 } select z).ToList() select new { ID = s.ID })
            join c2 in (from c in positions select new { ID = c.instrument.Underlying.ID, Qnt = c.Quantity, Name = c.instrument.Underlying.Name }) on s2.ID equals c2.ID
            group c2 by new
            {
                c2.Name,
                c2.ID
            } into g
            select new
            {
                Name = g.Key.Name,
                ID = g.Key.ID,
                Amt = g.Sum(t => t.Qnt)
            }
            )
                      join p in
                      (from s2 in (from s in (from z in new List<Instrument>() { instr1, instr2, instr3 } select z).ToList() select new { ID = s.Underlying.ID })
                       join c2 in (from c in prices select new { ID = c.InstrumentID, Val = c.Value }) on s2.ID equals c2.ID
                       select new { s2.ID, c2.Val }).GroupBy(x => x.ID).Select(s => s.First())
                      on q.ID equals p.ID
                      select new { Name = q.Name, Amt = q.Amt * p.Val }
            ;

        }

    }

}

namespace TipsAndTricks
{
    public static class TnT
    {
        public static void Check()
        {
            NullEquality();
            StructsCompare();
        }

        public static void NullEquality()
        {
            string a = null;
            //->throws System.NullReferenceException
            a.Equals(null);
        }

        public struct Str1
        {
            public string a;
            public int b;
        }
        public struct Str2
        {
            public string a;
            public int b;
        }
        public static void StructsCompare()
        {
            Str1 str1 = new Str1() { a = "Str1", b = 5 };
            Str1 str2 = str1;
            //true
            bool equalsResult = str1.Equals(str2);
            //false
            bool referenceResult = ReferenceEquals(str1, str2);

        }

        /*Prints current executing class and method names */
        public static void PrintCurrentMethodAndClassName()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
        }

        /// <summary>
        /// Eternal lopp if unchecked
        /// </summary>
        static void OwerflowException()
        {
            int max = 500;
            checked
            {
                for (byte i = 0; i < max; i++)
                {
                    Console.WriteLine(i);
                }
            }
        }

    }

    //hash from different collections compare check
    //---------------------------------------------
    public static class HashCodeCheck
    {
        public static void GO()
        {

            //all return different hashes 

            char[] ch1 = new char[] { 'a' };
            char[] ch2 = new char[] { 'a' };
            string s1 = string.Join("", ch1);
            string s2 = string.Join("", ch2);
            int h1 = ch1.GetHashCode();
            int h2 = ch2.GetHashCode();
            int h21 = s1.GetHashCode();
            int h22 = s2.GetHashCode();

            byte[] bt1 = Encoding.UTF8.GetBytes(ch1);
            byte[] bt2 = Encoding.UTF8.GetBytes(ch2);
            int h31 = bt1.GetHashCode();
            int h32 = bt2.GetHashCode();

            using (MD5 m = MD5.Create())
            {
                byte[] h41 = m.ComputeHash(bt1);
                byte[] h42 = m.ComputeHash(bt2);
            }

        }
    }

}

namespace KATAS
{
    public class Prop
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Item1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Item2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amt { get; set; }
        public IList<Prop> properties { get; set; }
    }

    public class Check
    {
         public static void GO()
        {
            var items1 = new List<Item1>()
            {
                new Item1(){Id = 1, Name ="name1"}
                ,new Item1(){Id = 3, Name ="name3"}
                ,new Item1(){Id = 5, Name ="name5"}
            };
            var items2 = new List<Item2>()
            {
                new Item2(){ Id = 1, Name = "name1", Amt = 1, properties = new List<Prop>() {
                    new Prop(){Id=1, Name = "p1"}, new Prop(){Id=2, Name = "p2"}
                }},
                new Item2(){ Id = 2, Name = "name2", Amt = 2, properties = new List<Prop>() {
                    new Prop(){Id=1, Name = "p1"}
                }},
                new Item2(){ Id = 3, Name = "name3", Amt = 3, properties = new List<Prop>() {
                    new Prop(){Id=1, Name = "p1"},new Prop(){Id=3, Name = "p3"}
                }},
                 new Item2(){ Id = 3, Name = "name4", Amt = 4, properties = new List<Prop>() {
                    new Prop(){Id=3, Name = "p4"}
                }}
            };

            var names = new List<string>() { "p1" };

            var itm0 = items1.Join(items2,
                l => new { l.Id, l.Name },
                r => new { r.Id, r.Name },
                (l, r) => new { lName = l.Name, rName = r.Name, amt = r.Amt }
            ).ToList();
            var itm1 = items1.GroupJoin(items1,
                l => new { l.Id, l.Name },
                r => new { r.Id, r.Name },
                (l, r) => new { lName = l.Name, r = r.DefaultIfEmpty() }
            ).ToList();
            var itm2 = items2.SelectMany(s => s.properties, (l, r) => new { l.Name, l.Amt, prop = r.Name });
            var itm3 = items2.Where(s => s.properties.Any(c => names.Exists(z => z == c.Name)))
            .ToList();

            var unsorted = new int[] { 9, 2, 4, 1, 6, 8, 7, 5 };
            var arrTosort = new int[unsorted.Length];
            var sorted = new int[arrTosort.Length];
            Array.Copy(unsorted, sorted, unsorted.Length);
            Array.Sort(sorted);

            Array.Copy(unsorted, arrTosort, unsorted.Length);
            var b0 = arrTosort.SequenceEqual(sorted);

            Array.Copy(unsorted, arrTosort, unsorted.Length);
            var b1 = arrTosort.SequenceEqual(sorted);

            Array.Copy(unsorted, arrTosort, unsorted.Length);
            var b2 = arrTosort.SequenceEqual(sorted);

            var hs = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("string"));
            var bts0 = Encoding.UTF8.GetBytes("string");
            var st = "string";
            var bts1 = st.Select(s => BitConverter.GetBytes(s)).SelectMany(c => c).Where(s => s != 0).ToList();

            var b3 = bts0.SequenceEqual(bts1);
        }
    }

    public class TNine
    {
        //https://code.google.com/codejam/contest/351101/dashboard#s=p2
        //T9  

        //running custom test cases
        public static class tNineCheck
        {
            public static void GO()
            {
                check1();
            }
            public static void check1()
            {
                List<CaseList> cl = new List<CaseList>() {
                    new CaseList(){Case="ab cff",Exp="2 220222333 333",Act=null}
                    , new CaseList("hg e a","44 403302",null)
                };

                foreach (CaseList cl_ in cl)
                {
                    cl_.Act = tNineChecks.GO(new KeyPadStrait(), cl_.Case);
                    cl_.check();
                }
            }

        }

        //class for test  cases usage
        public class CaseList
        {
            public CaseList() { }

            public CaseList(string @case, string exp, string act)
            {
                Case = @case;
                Exp = exp;
                Act = act;
            }

            public void check()
            {
                if (this.Exp == this.Act) { this.isOK = true; } else { this.isOK = false; }
                //or 
                //this.isOK=this.Exp == this.Act ?   true :  false;
            }
            public string Case { get; set; } = string.Empty;
            public string Exp { get; set; } = string.Empty;
            public string Act { get; set; } = null;
            public bool? isOK { get; private set; } = null;
        }

        //key presser interface handler
        public static class tNineChecks
        {
            public static string GO(IKeyPresser kp_, string case_)
            {
                return kp_.print(case_);
            }
        }

        //key presser interface with base realization
        public interface IKeyPresser
        {
            string print(string input);
        }
        public class KeyPresser : IKeyPresser
        {
            public string print(string input)
            {
                return null;
            }
        }

        //straightforward "naive" approach with char arrays
        public class KeyPadStrait : IKeyPresser
        {

            public static Dictionary<char, char?[]> keyPad = new Dictionary<char, char?[]>()
            {
                {'a', new char?[]{'2'} },{'b', new char?[]{'2','2'} },{'c', new char?[]{'2','2','2'} }
                ,{'d', new char?[]{'3'} },{'e', new char?[]{'3','3'} },{'f', new char?[]{'3','3','3'} }
                ,{'g', new char?[]{'4'} },{'h', new char?[]{'4','4'} },{'i', new char?[]{'4','4','4'} }
                ,{ ' ', new char?[]{'0'}}

            };
            public static List<char> presser(char[] str_)
            {

                //"".ToCharArray().First();
                char?[] foundPrev = null;

                List<char> res = new List<char>();
                for (int i = 0; i < str_.Count(); i++)
                {
                    char?[] found = null;
                    if (keyPad.ContainsKey(str_[i]))
                    {
                        keyPad.TryGetValue(str_[i], out found);
                        if (foundPrev != null)
                        {
                            if (foundPrev[0] == found[0]) { res.Add(' '); }
                        }

                        foreach (char ch in found)
                        {
                            res.Add(ch);
                        }
                        foundPrev = found;
                    }
                }
                return res;
            }

            public string print(string input_)
            {
                return string.Join(string.Empty, presser(input_.ToCharArray()));

            }
        }

    }


    public static class ReqwindKATA
    {
        public static string GO(string input_)
        {

            List<char> arr = new List<char>();
            Stack<char> st = new Stack<char>();
            Stack<char> st2 = new Stack<char>();

            for (int i = 0; i < input_.ToArray().Length; i++)
            {
                char ch = input_.ToArray()[i];

                if (ch != ' ')
                {
                    st.Push(ch);
                }
                else
                {
                    if (st.Count >= 5)
                    {
                        while (st.Count > 0)
                        {
                            arr.Add(st.Pop());
                        }

                    }
                    else
                    {
                        while (st.Count > 0)
                        {
                            st2.Push(st.Pop());
                        }
                        while (st2.Count > 0)
                        {
                            arr.Add(st2.Pop());
                        }

                    }
                    arr.Add(' ');
                }
            }

            if (st.Count >= 5)
            {
                while (st.Count > 0)
                {
                    arr.Add(st.Pop());
                }

            }
            else
            {
                while (st.Count > 0)
                {
                    st2.Push(st.Pop());
                }
                while (st2.Count > 0)
                {
                    arr.Add(st2.Pop());
                }

            }

            return string.Join(null, arr);
        }
    }

    public static class FindKata
    {
        public static char GO(char[] input)
        {
            byte[] arr = Encoding.ASCII.GetBytes(input);
            char result = ' ';
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] + 1 < arr[i + 1])
                {
                    result = (char)(arr[i] + 1);
                    break;
                }
            }

            return result;
        }

    }

    public static class DivideKATA
    {
        public static int[] Divisors(int n)
        {
            if (n < 2) { return null; }


            List<int> divisors = new List<int>();

            for (int i = 2; i < n; i++)
            {
                if (n % i == 0) { divisors.Add(i); }
            }
            if (divisors.Count == 0)
            {
                return null;
            }
            else
            {
                return divisors.ToArray();
            }

        }
    }

    public static class FormatRearrange
    {
        public static void GO()
        {
            StringsCheck();
        }

        static void StringsCheck()
        {

            string input = "{0}{1} {2}";
            string r1 = Rearrange(input);
        }

        static string Rearrange(string input_)
        {
            string result = input_;
            char[] chr = input_.ToCharArray();
            int lng = chr.Length;
            char[] prevDigit = null;
            char[] currDigit = null;

            for (int i = 0; i < lng; i++)
            {

                int i2 = i;

                if (char.IsDigit(chr[i2]))
                {

                    if (i2 + 1 < lng)
                    {
                        while (char.IsDigit(chr[i2 + 1]))
                        {
                            i2++;
                        }

                    }


                    if (prevDigit == null)
                    {
                        prevDigit = ChArrFill(i, i2, chr);
                    }
                    else
                    {
                        currDigit = ChArrFill(i, i2, chr);

                        if (!check(currDigit, prevDigit))
                        {
                            currDigit = intRecount(currDigit, prevDigit);

                            char[] chrN = new char[chr.Length + currDigit.Length - prevDigit.Length];

                            for (int i4 = 0; i4 < i; i4++)
                            {
                                chrN[i4] = chr[i4];
                            }
                            for (int i4 = i; i4 < i2; i4++)
                            {
                                chrN[i4] = chr[i4];
                            }
                            for (int i4 = i2; i4 <= lng; i4++)
                            {
                                chrN[i4] = chr[i4];
                            }

                            result = charArrToInteger(chrN).ToString();
                        }
                        else
                        {
                            prevDigit = intToCharArr(charArrToInteger(currDigit));
                        }

                    }

                }

            }

            return result;
        }

        static int charArrToInteger(char[] arr_)
        {
            int res = 0;
            int i = 1;
            for (int i2 = arr_.Length - 1; i2 >= 0; i2--)
            {
                res += (int)(char.GetNumericValue(arr_[i2]) * i);
                i *= 10;
            }
            return res;
        }
        static char[] intToCharArr(int i_)
        {
            return i_.ToString().ToCharArray();
        }
        static char[] intRecount(char[] currDig_, char[] prevDigit_)
        {
            if (charArrToInteger(currDig_) == charArrToInteger(prevDigit_) + 1)
            {
                return currDig_;
            }
            else
            {
                return intToCharArr(charArrToInteger(prevDigit_) + 1);
            }
        }
        static bool check(char[] currDig_, char[] prevDigit_)
        {
            if (charArrToInteger(currDig_) == charArrToInteger(prevDigit_) + 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static char[] ChArrFill(int i_, int i2_, char[] chFrom_)
        {
            char[] chTo_ = new char[(i2_ - i_) + 1];

            for (int i3_ = 0; i3_ <= (i2_ - i_); i3_++)
            {
                chTo_[i3_] = chFrom_[i_ + i3_];
            }
            return chTo_;
        }

    }

    public static class DigitSumm
    {
        public static void GO()
        {
            List<int> nums = new List<int>();
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                nums.Add(rnd.Next(1, 100000));
            }
            nums.ForEach(s =>
            {
                var sum = sumOfDigits(s);
                Trace.WriteLine($"digit: {s}; sum = {sum};");
            });
        }

        static int sumOfDigits(int number)
        {
            int result = 0;
            while (number != 0)
            {
                result += number % 10;
                number /= 10;
            }
            return result;
        }
    }


    //Kasper
    public class StringCount
    {
        public static void GO(string input)
        {
            var inp = "aaabbcc";
            inp = input;

            //group by query
            var counts = (
            from s in inp
            group s by new { s } into c
            select new
            {
                Key = c.Key,
                count = c.Count()
            }).ToList();


            //group by API
            var countsTwo = inp
            .GroupBy(p => p, (Key, g) => new
            {
                K = Key,
                C = g.Count()
            }).ToList();

            //foreach with dictionary
            Dictionary<char, int> result = new Dictionary<char, int>();
            foreach (char ch in input)
            {
                if (!result.ContainsKey(ch))
                {
                    result.Add(ch, 1);
                }
                else
                {
                    result[ch] += 1;
                }
            }
        }
    }

    //OZONe
    public static class WordsCount
    {
        static List<string> input = new List<string>() { "ABC", "ACB", "ABCD", "ABD", "ABCE", "CBA" };

        internal class Item
        {
            internal string itemRef { get; set; }
            internal int count { get; set; }
        }
        static Dictionary<int, Item> items = new Dictionary<int, Item>();
        public static void GO()
        {

            //foreach
            foreach (var str in input)
            {
                string newStr = new string(str.OrderBy(c => c).ToArray());
                var alg = SHA256.Create();
                byte[] hashBytes = alg.ComputeHash(Encoding.UTF8.GetBytes(newStr));
                int hashNew = BitConverter.ToInt32(hashBytes);

                if (!items.ContainsKey(hashNew))
                {
                    items.Add(hashNew, new Item { itemRef = str, count = 1 });
                }
                else
                {
                    items[hashNew].count += 1;
                }
            }

            items.Select(s => new { s.Value.itemRef, s.Value.count })
            .ToList()
            .ForEach(s =>
                Trace.WriteLine($"Itme entry count: {s.itemRef} {s.count}")
            );

        }

    }

    //chars
    public class Convertions
    {
        //System.Security.Cryptography.SHA256.Create().ComputeHash
        //byte[] => hash

        //BitConverter.GetBytes
        //Type !str => byte[]
        //BitConverter.GetBytes
        //byte[] => types|str

        //Encoding.UTF8.GetBytes
        //type => byte[]

        //Convert.ToString()
        //var -> totype

        //System.Environment.CurrentDirectory
        //Directory.GetCurrentDirectory()  

        public static void GO()
        {

            var str = "abcdefg";

            var hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(str));

            try
            {
                string input = "abcd123";

                //computeHash
                byte[] bytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(input));


                List<byte> bytesFromString = new List<byte>();
                foreach (char ch in input)
                {
                    byte[] tempBytes = BitConverter.GetBytes(ch);
                    bytesFromString.AddRange(tempBytes);
                }

                //converted bytes
                string result = new string(BitConverter.ToString(bytesFromString.ToArray()));
                //original chars
                List<char> charsFromByte = new List<char>(bytesFromString.ToList().Where(s => s != 0).Select(Convert.ToChar));
                //original string
                string resultOne = new string(charsFromByte.ToArray());

                //new string from converted bytes
                string[] byteString = result.Split("-");
                List<char> charsFromBiteString = new List<char>();
                foreach (string st in byteString)
                {
                    byte bt;
                    byte.TryParse(st, out bt);

                    char ch = Convert.ToChar(bt);
                    charsFromBiteString.Add(ch);
                }
                string resultTwo = new string(charsFromBiteString.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Bulk()
        {
            StreamWriter sw = new StreamWriter("output1.txt");

            byte[] bites = { 0X0000, 0X0001, 0X0002, 0X0003, 0X0003, 0X0061 };
            char[] charsFromBite = BitConverter.ToString(bites).ToCharArray();

            char[] chars = { '\u0061', '\u0308' };

            chars = chars.Union(charsFromBite).ToArray();
            string str = new string(chars);

            byte[] bytesFromChars = Encoding.UTF8.GetBytes(chars);
            string stringFromByte = BitConverter.ToString(bytesFromChars);

            sw.WriteLine(str);

            sw.WriteLine($"Encoding.UTF8: {stringFromByte}");
            sw.WriteLine($"Encoding.UTF32: {BitConverter.ToString(Encoding.UTF32.GetBytes(chars))}");
            sw.WriteLine($"Encoding.Unicode: {BitConverter.ToString(Encoding.Unicode.GetBytes(chars))}");
            sw.WriteLine($"Encoding UTF8: {BitConverter.ToString(bites)}");

            StringBuilder sb = new StringBuilder();
            int width = 0;
            for (int i = 0; i < 65535; i++)
            {
                try
                {
                    char chnew = Convert.ToChar(i);
                    char ch = Convert.ToChar(i);
                    if (!char.IsDigit(chnew) && !char.IsHighSurrogate(chnew) && !char.IsLowSurrogate(chnew) && !char.IsSurrogate(chnew))
                    {
                        sb.Append(Convert.ToChar(i));
                        width += 1;
                        if (width >= 50)
                        {
                            sb.Append(Environment.NewLine);
                            width = 0;
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
            sw.WriteLine(Char.ConvertFromUtf32(0x1D160));
            sw.WriteLine(sb.ToString());
            sw.Close();
        }

    }

    public static class Bites
    {
        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");
            BitEncoding();
            BitesTest();
        }

        public static void BitesTest()
        {
            byte bt = 0X80;
            int a = 0X80;
            int res = a & a;
            var str0 = Convert.ToString(bt, 2);

            byte[] bytes = { 0X0001, 0X0011, 0XF1, 0, 1 };
            foreach (byte b in bytes)
            {
                Trace.WriteLine(Convert.ToString(b, 2));
            }

            string str = "Value to bytes";

            List<char> charsFromCharBits = new List<char>();
            List<char> charsFromIntBits = new List<char>();

            foreach (char ch in str)
            {
                string str1 = char.ToString(ch);

                int intFromChar = Convert.ToInt32(ch);
                byte byteFromInt = Convert.ToByte(intFromChar);
                byte byteFromCh = Convert.ToByte(ch);

                byte[] btArrCh = BitConverter.GetBytes(ch);
                byte[] btArrInt = BitConverter.GetBytes(intFromChar);

                charsFromCharBits.Add(BitConverter.ToChar(btArrCh));
                charsFromIntBits.Add(BitConverter.ToChar(btArrInt));

                string byteStringRep = BitConverter.ToString(btArrCh);
            }

            string stringFromCharBits = new string(charsFromCharBits.ToArray());
            string stringFromIntBits = new string(charsFromIntBits.ToArray());

            bool eq0 = stringFromCharBits == stringFromIntBits;
            bool eq1 = stringFromCharBits.Equals(stringFromIntBits);

            List<char> charsFromStringOfBites = new List<char>();

            string hexValue = "56 61 6C"; //"48 65 6C 6C 6F 20 57 6F 72 6C 64 21";
            string[] hexValues = hexValue.Split(' ');
            foreach (string hS in hexValues)
            {

                try
                {
                    int intFromString = Convert.ToInt32(hS, 16);
                    byte byteFromString = Convert.ToByte(hS, 16);

                    byte[] bytesFromInt = BitConverter.GetBytes(intFromString);
                    byte bfs;

                    byte.TryParse(hS, out bfs);
                    charsFromStringOfBites.Add(BitConverter.ToChar(bytesFromInt));
                }
                catch (Exception e)
                {

                }
            }

            string stringFromStringOfBytes = new string(charsFromStringOfBites.ToArray());

        }

        public static void BitEncoding()
        {

            string str = "string123";
            List<byte> getBytes = str.Select(s => BitConverter.GetBytes(s))
                .Aggregate(new List<byte>(), (acc, i) =>
                {
                    foreach (byte bt in i)
                    {
                        acc.Add(bt);
                    }

                    return acc;
                });
            getBytes = getBytes.Except(getBytes.Where(s => s == 0)).ToList();
            byte[] encBytes = Encoding.UTF8.GetBytes(str);

            var hashFromStr = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            var hashBytesBitconv = SHA256.Create().ComputeHash(getBytes.ToArray());


            var hashesAreEqual = hashFromStr.SequenceEqual(hashBytesBitconv);

            var strFromBitBytes = Encoding.UTF8.GetString(getBytes.ToArray());
            var strFromEncBytes = Encoding.UTF8.GetString(encBytes);

            var strnigsAreEqual = (str == strFromBitBytes) && (str == strFromEncBytes);

        }
    }

    public class FactorialCount
    {
        public int Count(int upperGap)
        {
            if (upperGap == 0) { return 0; }

            int result = 1;
            for (int i = 1; i <= upperGap; i++)
            {
                result *= i;
            }

            return result;
        }
    }
    
    public class BracketsChecker
{
public static void GO()
{
    List<string> strsOK = new List<string>() {
        "c * [ (a+b) / d]",  "()[][()]","([])[()]", "()","[]"
    };
    List<string> strsNotOK = new List<string>() {
        "(c * [a+b) / d]", ")[]", "]()", "[(())])" , "(([[])"
    };
    var isOK = strsOK.Select(s => braketsCount(s)).All(s => s == true);
    var isNotOK = strsNotOK.Select(s => braketsCount(s)).All(s => s == false);

    var ok2 = strsOK.Select(s => bracketsCheck(s)).All(c => c == true);
    var notOk = strsNotOK.Select(s => bracketsCheck(s)).All(c => c == false);

}

static Func<string, bool> braketsCount = (s) =>
{

    Stack<char> brackets = new Stack<char>();

    foreach (char i in s)
    {
        if (brackets.Count == 0)
        {
            if (i == ')' || i == ']') { return false; }
            if (i == '(' || i == '[') { brackets.Push(i); };
        }
        else
        {
            if (i == ')') { if (brackets.Pop() != '(') { return false; } }
            if (i == ']') { if (brackets.Pop() != '[') { return false; } }

            if (i == '(' || i == '[') { brackets.Push(i); };
        }
    }

    return brackets.Count == 0;

};

public static bool bracketsCheck(string input)
{
    List<char> opened = new List<char>() { '(', '[', '{' };
    List<char> closed = new List<char>() { ')', ']', '}' };
    Stack<char> cntr = new Stack<char>();

    if (string.IsNullOrEmpty(input) || input?.Any() != true) { return false; }
    if (closed.Contains(input[0])) { return false; }

    foreach (var ch in input)
    {
        if (opened.Contains(ch)) { cntr.Push(ch); }
        if (closed.Contains(ch))
        {
            if (cntr.Count() <= 0) { return false; }
            var previous = cntr.Pop();
            if (opened.IndexOf(previous) != closed.IndexOf(ch)) { return false; }
        }
    }

    return cntr.Count() == 0;
}

}

    public class RoomNum
    {
        public int Section { get; set; }
        public int RoomNumber { get; set; }
    }
    public class TrainRooms
    {

        public static List<RoomNum> rooms;
        public static void GO()
        {
            init();

            var section1 = rooms.Where(s => s.RoomNumber == 4).FirstOrDefault().Section;
            var section2 = rooms.Where(s => s.RoomNumber == 52).FirstOrDefault().Section;
        }

        public static void init()
        {
            rooms = new List<RoomNum>();

            int section = 1;

            //fill rooms
            for (int i = 1; i <= 36; i++)
            {
                rooms.Add(new RoomNum() { Section = section, RoomNumber = i });
                if (i % 4 == 0) { section += 1; }
            }

            section = 1;
            for (int i = 54; i >= 37; i--)
            {
                rooms.Add(new RoomNum() { Section = section, RoomNumber = i });
                if (i % 2 == 0) { section += 1; }
            }
        }
    }

    public static class BalancedDelimeter
    {

        public static void GO()
        {
            Trace.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType}.{MethodBase.GetCurrentMethod().Name}----------");

        }

    }

    public class DelimeterChecker
    {

        public static void GO()
        {
            var nums = new List<int>()
            {
                Convert.ToInt32("01000000", 2),
                Convert.ToInt32("010", 2),
                Convert.ToInt32("0100100010", 2),
                Convert.ToInt32("010010001", 2),
                Convert.ToInt32("01001", 2),
            };

            var results = new List<int>();
            foreach (var n in nums)
            {
                results.Add(Count(n));
            }
        }
        public static int Count(int i)
        {
            var binary = Convert.ToString(i, 2);

            int max = 0;
            int cnt = 0;
            foreach (var b in binary)
            {
                if (b == '0')
                {
                    cnt++;
                }

                if (b != '0' && cnt > max)
                {
                    max = cnt;
                    cnt = 0;
                }
            }
            
            // if (max == 0)
            //     max = cnt;

            return max;
        }
    }

    public class FurryRoad
    {

        public static void GO()
        {
            var sut = new FurryRoad();
            var roads = new List<string>() {"ASAASS", "ASAA",  "SSA", "SSSSAAA"};

            var results = new List<int>();

            foreach (var road in roads)
            {
                var result = sut.solution(road);
                results.Add(result);
            }
        }
        
        public int solution(string R)
        {
            var result = 0;
            var res = 0;
            var sRes = 0;
            var fRes = 0;

            var time = 0;
            if (string.IsNullOrEmpty(R))
                return 0;

            var Acnt = R.Count(s => s == 'A');
            var Scnt = R.Count(s => s == 'S');

            var idx = 0;
            
            var fr = footRes(R.ToList());
            var sr = scooterRes(R.ToList());

            if (fr < sr)
                return fr;

         
            for (int i = 0; i < R.Length; i++)
            {

                if (R[i] == 'A')
                {
                    res += 5;
                }
                else
                {
                    var stayed = R.Skip(i).Take(R.Length -(i));
                    
                    sr = scooterRes(stayed.ToList());
                    fr = footRes(stayed.ToList());

                    if (fr < sr)
                    {
                        res += fr;
                        break;
                    }
                    else
                    {
                        res += 40;
                    }
                }
               
            }

            return res;
        }

        int scooterRes(List<char> list)
        {
            var Ascnt = list.Count(s => s == 'A');
            var Sscnt = list.Count(s => s == 'S');
            return ((Ascnt * 5) + (Sscnt * 40));
        }
        int footRes(List<char> list)
        {
            var Ascnt = list.Count(s => s == 'A');
            var Sscnt = list.Count(s => s == 'S');
            return ((Ascnt * 20) + (Sscnt * 30));
        }
    }
    
    public class RotateArray
    {
        internal class Suts
        {
             public int[] arrays { get; set; }
             public int rotate { get; set; }
        }
        
        public static void GO()
        {
            var sut = new RotateArray();
            var results = new List<int[]>();
            
            var arrays = new List<Suts>()
            {
                new Suts() { arrays = new List<int>().ToArray(), rotate = 7},
                new Suts() { arrays = new int[]{1,2,3}, rotate = 7},
                new Suts() { arrays = new int[]{1,2,3,4,5,6}, rotate = 2},
                new Suts() { arrays = new int[]{1,2,3,4,5,6}, rotate = 3},
                new Suts() { arrays = new int[]{1,2,3,4,5,6,7,8,9}, rotate = 3}
            };

            foreach (var a in arrays)
            {
                var r = sut.Rotate(a.arrays, a.rotate);
                results.Add(r);
            }
        }
        public int[] Rotate(int[] A, int K)
        {
            if (K == A.Length || A.Length <=0)
                return A;

            if (K > A.Length)
                K = (  K % A.Length);
            
            var result = new int[A.Length];

            for (int i = A.Length-1; i > (A.Length-1) - K; i--)
            {
                result[(i+1)-(A.Length-(K-1))] = A[i];
            }

            for (int i = 0; i < A.Length - K; i++)
            {
                result[i+K] = A[i];
            }

            return result;
        }
        
    }
    
    public class ReadWrite
    {

        public static async Task GO()
        {
            var _fixture = new Fixture();
            var item = new ReadWrite();
            var path = "C:\\files\\test\\fs.txt";
            
            if(File.Exists(path))
                File.Delete(path);
            
            var str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            Random rnd = new Random();

            int len = 15;
            int offset = 3;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= len; i++)
            {
                sb.Append(str[rnd.Next(0, str.Length)]);
            }

            var inf = sb.ToString();
            
            var bt = Encoding.UTF8.GetBytes(inf);

            for (int i = 0; i < len; i += offset)
            {
                await item.Write(path, bt, i, offset);
            }
        }
        public async Task Write(string path, byte[] arr, int offset, int count)
        {
            using FileStream fs = File.OpenWrite(path);
            
            await fs.WriteAsync(arr,offset,count);
        }
        
        
    }

    public class HackerRank
    {
        public static void GO()
        {
            var _f = new Fixture();
            var words = _f.CreateMany<string>(5).ToList();
            
            var words0 = new List<string> {"a","aa","aab","aac","aacd"};
            var words1 = new List<string> {"aab", "defgab", "abcde", /**/"aabcde", "bbbbbbbbbb", "jabjjjad"};
            var words2 = new List<string> {"aab", "aac", /**/"aacghgh", "aabghgh"};
            var words3 = new List<string> {"aab", "aac", "aaghgh" ,/**/"aac"};
            var words4 = new List<string> {"abe","de","abc","abcde","def"};
            var words5 = new List<string> {"abe","deq","abcde","abc","def"};

            Trie trie = new Trie();

            TireSecond tireSecond = new TireSecond();

            var result = "";
            var res = false;
            foreach (var w in words5)
            {
                if (tireSecond.SearchAndAdd(w) && !res)
                {
                    res = true;
                    result = w;
                    break;
                }
            }
            
            if(!res)
                Console.WriteLine("GOOD SET");
            
            if (res)
                Console.WriteLine("BAD SET" + "\r\n" + result);
            
            foreach (var w in words)
            {
                var index = trie.Traverse(w);
                if (!index.OK)
                {
                    trie._nodes = trie.Build(index.NodeIndex.Index, w, index.NodeIndex.Nodes).ToList();
                }
                var newIdx = trie.Traverse(w);
            }
           
            
            NoPrefix(words3);
        }

        public static void NoPrefix(List<string> words)
        {
            var items = words;

            bool toContinue = true;
            bool isBad = false;
         

            if (items.Count == 1)
            {
                toContinue = false;
                Console.WriteLine("GOOD SET");
            }

            if (items?.Any()!=true)
            {
                toContinue = false;
                Console.WriteLine("BAD SET");
            }
            var i = 0;
            var j = i+1;
            
            while (toContinue && j<items.Count())
            {
                i = 0;
                //new input
                while (i < j && toContinue)
                {
 
                    var contains = true;
                    var len = items[i].Length > items[j].Length ? items[j].Length : items[i].Length;

                    //compare i j exclude i 
                    for (var c = 0; c < len; c++)
                    {
                        if (items[j][c] != items[i][c])
                        {
                            contains = false;
                        }

                        c += 1;
                    }

                    if (contains)
                    {
                        var longestBadItem = items[i].Length > items[j].Length ? items[i] : items[j];
                        Console.WriteLine("BAD SET" + "\r\n" + items[j]);
                        toContinue = false;
                        isBad = true;
                        break;
                    }

                    i += 1;
                }

                j += 1;
            }

            if(!isBad)
                Console.WriteLine("GOOD SET");
        }
        
    }
    
    
    
    //tire tree first version

    public class TrieNode
    {
        public TrieNode(char ch)
        {
            CH = ch;
        }
        public char CH { get; set; }

        public List<TrieNode> Childs { get; set; } = new List<TrieNode>();
    }
    
    public class Result
    {
        public bool OK { get; set; }
        
        public NodeIndex NodeIndex { get; set; }
    }
    public class NodeIndex
    {
        public int Index { get; set; }
        public IEnumerable<TrieNode> Nodes { get; set; }
    }
    public class Trie
    {
        public List<TrieNode> _nodes { get; set; } = new List<TrieNode>();
        
        public TrieNode AddParent(IEnumerable<TrieNode> nodes, char ch)
        {
            var result = new TrieNode(ch);
            nodes.ToList().Add(result);
            return result;
        }
        
        public TrieNode AddChild(TrieNode node, char ch)
        {
            var result = new TrieNode(ch);
            node.Childs.Add(result);
            return result;
        }

        public IEnumerable<TrieNode> GetParents(IEnumerable<TrieNode> nodes, char ch)
        {
            return nodes.Where(s => s.CH == ch);
        }
        public IEnumerable<TrieNode> GetByChilds(IEnumerable<TrieNode> nodes, char ch)
        {
            return nodes.Where(s => s.Childs.Any(c=>c.CH == ch));
        }
        public IEnumerable<TrieNode> GetChilds(IEnumerable<TrieNode> nodes, char ch)
        {
            return nodes.SelectMany(p=>p.Childs)
                .Where(s => s.CH== ch);
        }
        
        public IEnumerable<TrieNode> TraverseAndBuild(string str)
        {
            IEnumerable<TrieNode> nds = _nodes;
            if (string.IsNullOrEmpty(str))
                return null;

            var parent = GetParents(_nodes, str[0]);
            if (parent?.Any() != true)
            {
                AddParent(_nodes,str[0]);
            }
            
            if (str.Length == 1)
                return parent;

            nds = parent;
            int i = 1;
            while (i < str.Length)
            {
                var ch = str[i];
                var childs = GetByChilds(nds, ch);
                
                //build
                if (childs?.Any() != true)
                {
                    nds = Build(i, str, nds);
                    break;
                }

                nds = childs;
                i++;
            }

            return nds;
        }

        public Result Traverse(string str)
        {
            IEnumerable<TrieNode> nds = _nodes;
            if (string.IsNullOrEmpty(str))
                return null;

            var parent = GetParents(_nodes, str[0]);
            if (parent?.Any() != true)
            {
                return new Result() {OK = false, NodeIndex = new NodeIndex() {Index = 0, Nodes = _nodes}};
            }
            
            for (int i = 1; i < str.Length; i++)
            {
                var ch = str[i];
                nds = parent.SelectMany(p => p.Childs);
                if (nds?.Any() != true)
                {
                    return new Result() {OK = false, NodeIndex = new NodeIndex() {Index = 1, Nodes = parent}};
                }
            }
            return new Result() {OK = true, NodeIndex = new NodeIndex() {Index = str.Length, Nodes = nds}};
        }

        public List<TrieNode> Build(int index, string str, IEnumerable<TrieNode> nodes)
        {
            var nds = nodes;
            for (int i = index; i < str.Length; i++)
            {
                var ch = str[i];
                var parent = AddParent(nds, ch);
                nds = parent.Childs;
            }

            return nodes.ToList();
        }
    }
    
    
    
    //tire tree naive
    public class TireNode
    {
        public TireNode(char ch){ Value = ch; }

        public char Value {get;set;}
        public List<TireNode> Childs {get;set;} = new List<TireNode>();
        public bool End {get;set;} = true;
    }

    public class TireSecond
    {
        public List<TireNode> _nodes { get; set; } = new List<TireNode>();
        
        public bool SearchAndAdd(string str)
        {
            var nds = _nodes;
            TireNode parent = null;
            TireNode newNode = null;
            var result = false;

            for (var i = 0; i < str.Length; i++)
            {

                var ch = str[i];
                if (parent?.Childs?.Any() == true)
                {
                    parent.End = false;
                }
                parent = nds.FirstOrDefault(s => s.Value == ch);
                
                if (parent != null)
                {
                    if (parent.End)
                        result = true;

                    nds = parent.Childs;
                }
                else
                {
                    if (newNode != null)
                        newNode.End = false;
                    
                    //add
                    newNode = new TireNode(ch);
                    nds.Add(newNode);
                    nds = newNode.Childs;
                }
                
            }
            
            return result;
        }
    }
    
    
    
    public class HTTPserializeSave
    {

        // get
        // https://catfact.ninja/facts
        // https://api.worldremit.com/api/countries
        // https://catfact.ninja/fact
        // https://api.coindesk.com/v1/bpi/currentprice.json
        // https://api.nationalize.io/?name=lilu
        // https://datausa.io/api/data?drilldowns=Nation&measures=Population

        //C:\files\test

        public static async Task<IEnumerable<T>> HttpReqSaveSinglelineSyntax<T>()
        {
            await File.WriteAllTextAsync($"{Directory.GetCurrentDirectory()}\\slExp.json",
                JsonSerializer.Serialize(
                    JsonSerializer.Deserialize<IEnumerable<T>>(
                        await new HttpClient()
                            .GetAsync("https://api.worldremit.com/api/countries")?
                            .Result
                            .Content
                            .ReadAsStringAsync()
                    )
                )
            );
            return JsonSerializer.Deserialize<IEnumerable<T>>(await new HttpClient()
                .GetAsync("")?.Result.Content.ReadAsStringAsync());
        }
        
    }
    
}

namespace AlgorithmsOld
{
    
    /* Collection for testing value collections */
    public class TestListsStructs<T> where T : struct, IComparable
    {
        //test list
        public List<T> Arrange { get; set; }
        //expected sorted list
        public List<T> Expected { get; set; }
        //sorting method SUT
        public string MethodName { get; set; }
        //sorting time 
        public long Elapsed { get; set; }
        //checks sequence equality of arrange and expected
        public bool result
        {
            get
            {
                if (this.Arrange == null || this.Expected == null) { return false; }
                return this.Arrange.SequenceEqual(this.Expected);
            }
            private set { value = false; }
        }
    }
    /* Collection for testing class collections */
    public class TestListsClasses<T> where T : class, IComparable
    {
        public List<T> Arrange { get; set; }
        public List<T> Expected { get; set; }
        public bool result
        {
            get
            {
                if (this.Arrange == null || this.Expected == null) { return false; }
                return this.Arrange.SequenceEqual(this.Expected);
            }
            private set { value = false; }
        }
    }

    
    // Delegate for sorting method
    public delegate IList<T> SortingDelegate<T>(IList<T> arr);

    public class AlgorithmTest<T>
        where T : struct, IComparable
    {

        private Stat stat = new Stat();
        
        public AlgorithmTest(List<List<T>> testArr, List<SortingDelegate<T>> methods)
        {
            BindItems(testArr, methods);
        }

        // Lists under test
        public List<TestListsStructs<T>> Results { get; set; }
        // Property for sorting method
        public IEnumerable<SortingDelegate<T>> MethodsUT { get; set; }
        // Lists under test
        public List<List<T>> CollectionsUT { get; set; }

        
        public void RunTests()
        {
            Results = new List<TestListsStructs<T>>();

            foreach (var m in MethodsUT)
            {
                foreach (var t in CollectionsUT)
                {
                    var item = new TestListsStructs<T>();

                    item.MethodName = m.Method.Name;
                    item.Arrange = t.OrderBy(s=>s).ToList();
                    
                    var watch = Stopwatch.StartNew();
                    //test run
                        item.Expected = m.Invoke(item.Arrange).ToList();
                    watch.Stop();
                    
                    item.Elapsed = watch.ElapsedMilliseconds;

                    Results.Add(item);
                    stat.ToStat($"{m.GetMethodInfo().DeclaringType} {m.Method.Name }: {item.Arrange.Count} : {item.Elapsed} : {item.result}");
                    
                }
            }
            
            stat.Export();
        }

        public void BindItems(
            List<List<T>> lists,
            IEnumerable<SortingDelegate<T>> methods)
        {
            if (lists?.Any() == true)
                CollectionsUT = lists;
            
            if (methods?.Any() == true)
                this.MethodsUT = methods;
        }


    
    }

    public class Stat
    {
        private ExportType export = ExportType.File;
        private StringBuilder sb = new StringBuilder();

        public void ToStat(string input)
        {
            sb.AppendLine(input);
        }

        public void Export()
        {
            var output = sb.ToString();
            if (export == ExportType.Console)
                Trace.WriteLine(output);

            if (export == ExportType.File)
            {
                File.WriteAllText(@"C:\files\test\algRes.txt",output);
            }
        }

        enum ExportType
        {
            Console,
            File
        }
    }

    /// <summary>
    /// Good for generic Sort(Array<T>) methods
    /// not very clear and addaptive
    /// </summary>
    public class SortingTests
    {
        AlgorithmTest<int> test;
        Random rnd = new Random();

        public static void GO()
        {
            SortingTests st = new SortingTests();
            st.SortingTestsArrangeAndRun();
        }
        
        /// <summary>
        /// Create collections of ints bettwen ranges
        /// </summary>
        List<int> getLongCollection(int count, int maxRandomGap)
        {
            List<int> longList = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                longList.Add(rnd.Next(0, maxRandomGap));

            }
            return longList;
        }
        
        //generated collection
        List<List<int>> createGeneratedIntCollection()
        {

            List<int> longList = new List<int>(10000);
            for (int i = 0; i < 10000; i++)
            {
                longList.Add(rnd.Next(0, 10000));

            }
  
            return new List<List<int>>()
            {
                getLongCollection(100,100)
                , getLongCollection(10000,10000)
                , getLongCollection(50000,50000)
                , getLongCollection(100000,100000)
            };
        }
        
        //manual collection tests
        List<List<int>> createManualIntCollection()
        {
            return new List<List<int>>(){
                new List<int>(){3,1,2,1,3,1,2},
                new List<int>{1,1,2,3,3},
                new List<int>{1,2,3},
                new List<int>(){15,25,3,9,34,8,18,6,16}
                , getLongCollection(100,100)
                , getLongCollection(500,500)
                , getLongCollection(1000,1000)
                , getLongCollection(10000,10000)
                , getLongCollection(30000,30000)
            };
        }
        
        
        
        void SortingTestsArrangeAndRun()
        {
            InsertionSort<int> insertionSort = new InsertionSort<int>();
            QuickSort<int> quickSort = new QuickSort<int>();
            HeapSort<int> heapSort = new HeapSort<int>();
            MergeSort<int> mergeSort = new MergeSort<int>();
            MergeSortInt mergeSortInt = new MergeSortInt();
            HeapSortInt heapSortInt = new HeapSortInt();

            var  lists = createManualIntCollection(); //createIntCollection();
            //var  lists = createGeneratedIntCollection();
            var methods = new List<SortingDelegate<int>>()
            {
                // insertionSort.Sort,
                // quickSort.Sort,
              
                mergeSort.Sort,
                mergeSortInt.Sort,
                
                heapSort.Sort,
                heapSortInt.Sort
                
            };
            var alg = new AlgorithmTest<int>(lists, methods);
            
            alg.RunTests();
        }

        void sampleIntCheck()
        {

            var arr = new int[] { 5, 1, 7, 3, 5, 9, 3, 5, 8 };
            var expArr = arr.OrderBy(s => s).ToArray();
            ShellSort.Sort(arr);
            //InsertionSortInt.Sort(arr);
            var res = arr.SequenceEqual(expArr);
        }
    }


    public class InsertionSortInt
    {
        public static void GO()
        {
            var arrToSort = new int[] { 1, 9, 15, 8, 7, 10 };

            var arrExpect = (int[])arrToSort.Clone();
            Array.Sort(arrExpect);
            sort(arrToSort);
            var sorted = arrToSort.SequenceEqual(arrExpect);
        }
        public static void sort(int[] arr)
        {
            var i = 1;
            while (i < arr.Length)
            {
                var x = arr[i];
                var j = i - 1;
                while (j >= 0 && arr[j] > x)
                {
                    arr[j + 1] = arr[j];
                    j -= 1;
                }
                arr[j + 1] = x;
                i += 1;
            }
        }

    }
    public class InsertionSort<T> where T : struct, IComparable
    {
        public IList<T> Sort(IList<T> arr)
        {
            IList<T> result = new List<T>();
            if (arr == null || arr?.Count == 0)
            {
                return result;
            }
            if (arr.Count == 1)
            {
                result.Add(arr[0]);
            }

            return sort(arr);
        }
        IList<T> sort(IList<T> arr)
        {

            for (int i = 1; i <= arr.Count - 1; i++)
            {
                T pivot = arr[i];

                int j = i - 1;

                while (j >= 0 && Comparer<T>.Default.Compare(pivot, arr[j]) < 1)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = pivot;
            }

            return arr;
        }
    }

    public class ShellSort
    {
        public static void Sort(int[] arr)
        {
            var n = arr.Length;
            for (var gap = n / 2; gap > 0; gap /= 2)
            {
                for (var i = gap; i < arr.Length; i++)
                {
                    var x = arr[i];
                    int j;
                    for (j = i; j >= gap && arr[j - gap] > x; j -= gap)
                    {
                        arr[j] = arr[j - gap];
                    }
                    arr[j] = x;
                }
            }
        }
    }

    public class ShellSortInt
    {
        public static void GO()
        {
            var arr = new int[] { 8, 2, 14, 6, 3, 13, 15, 4, 11, 7, 1, 12, 5, 9, 10 };
            var arrExpected = ((int[])arr.Clone()).OrderBy(s => s).ToArray<int>();
            Sort(arr, arr.Length * 2);
            bool equ = arr.SequenceEqual(arrExpected);
        }
        public static void Sort(int[] arr, int n)
        {
            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < arr.Length; i += 1)
                {
                    var s = arr[i];

                    int j;
                    for (j = i; j >= gap && arr[j - gap] > s; j -= gap)
                    {
                        arr[j] = arr[j - gap];
                    }
                    arr[j] = s;
                }
            }
        }
    }



    public class QuickSortTest
    {
        public static void GO()
        {
            QuickSortTest qt = new QuickSortTest();
            qt.QuickSortGenericTest();
        }

        void QuickSortGenericTest()
        {
            QuickSort<int> qs = new QuickSort<int>();
            List<TestListsStructs<int>> array = new List<TestListsStructs<int>>(){
                new TestListsStructs<int>(){Arrange =  new List<int>(){5,4,3,2,6}, Expected = new List<int>() { 2,3,4,5,6 } },
                new TestListsStructs<int>(){Arrange =  new List<int>(){1,5,3,4,2,7}, Expected = new List<int>() { 1, 2, 3, 4, 5, 7 } },
                new TestListsStructs<int>(){Arrange =  new List<int>(){15, 25, 3, 9, 34, 8, 18, 6, 16 }, Expected = new List<int>() { 3,6,8,9,15,16,18,25,34} }
            };

            foreach (var item in array)
            {
                qs.Sort(item.Arrange);
            }
        }
    }
    public class QuickSort<T> where T : struct, IComparable
    {
        public IList<T> Sort(IList<T> arr)
        {
            return sort(arr, 0, arr.Count - 1);
        }
        IList<T> sort(IList<T> arr, int idxLw, int idxHg)
        {
            if (idxHg > 0 && idxLw < idxHg)
            {
                int p = partition(arr, idxLw, idxHg);

                sort(arr, idxLw, p - 1);
                sort(arr, p + 1, idxHg);
            }
            return arr;
        }
        int partition(IList<T> arr, int idxLw, int idxHg)
        {
            T pivot = arr[idxHg];
            int i = idxLw - 1;

            for (int j = idxLw; j <= idxHg - 1; j++)
            {
                if (Comparer<T>.Default.Compare(pivot, arr[j]) > 0)
                {
                    i++;
                    Swap(arr, i, j);
                }
            }

            i++;
            Swap(arr, i, idxHg);
            return i;
        }
        void Swap(IList<T> arr, int idxFt, int idxLt)
        {
            T item = arr[idxFt];
            arr[idxFt] = arr[idxLt];
            arr[idxLt] = item;
        }
    }



    public class HeapSortTest
    {
        protected class TestLists
        {
            public List<int> Arrange { get; set; }
            public List<int> Expected { get; set; }
            public bool result { get; set; } = false;
        }

        public static void GO()
        {
            HeapSortIntCheck();
            HeapSortGenericCheckInt();
            HeapSortGenericCheckChars();

        }
        static void HeapSortIntCheck()
        {
            HeapSortInt hs = new HeapSortInt();

            List<TestLists> arrange = new List<TestLists>(){
                new TestLists(){ Arrange = new List<int>() { 4, 5, 3, 2, 1 }, Expected = new List<int>() { 5, 4, 3, 2, 1 }}
                ,new TestLists(){ Arrange = new List<int>() { 4, 10, 3, 5, 1 }, Expected = new List<int>() { 10, 5, 3, 4, 1 }}
                ,new TestLists(){ Arrange = new List<int>() {1, 3, 5, 4, 6, 13, 10, 9, 8, 15, 17}, Expected = new List<int>() {17,15,13,9,6,5,10,4,8,3,1}}
                };

            foreach (var list in arrange)
            {
                hs.Sort(list.Arrange);
                list.result = list.Arrange.SequenceEqual(list.Expected);
            };

        }

        static void HeapSortGenericCheckInt()
        {
            HeapSort<int> hsInt = new HeapSort<int>();
            List<TestListsStructs<int>> arrange = new List<TestListsStructs<int>>(){
                new TestListsStructs<int>(){Arrange = new List<int>(){4,5,3,2,1}, Expected = new List<int>(){1,2,3,4,5} }
                ,new TestListsStructs<int>(){ Arrange = new List<int>() { 4, 10, 3, 5, 1 }, Expected = new List<int>() {1,3,4,5,10 }}
                ,new TestListsStructs<int>(){ Arrange = new List<int>() {1, 3, 5, 4, 6, 13, 10, 9, 8, 15, 17}, Expected = new List<int>() {1,3,4,5,6,8,9,10,13,15,17}}
            };

            foreach (var list in arrange)
            {
                hsInt.Sort(list.Arrange);
            }

        }
        static void HeapSortGenericCheckChars()
        {
            HeapSort<char> hsInt = new HeapSort<char>();
            List<TestListsStructs<char>> arrange = new List<TestListsStructs<char>>(){
                new TestListsStructs<char>(){Arrange = "adfbec".ToArray().ToList(), Expected = "abcdef".ToArray().ToList() }
            };

            foreach (var list in arrange)
            {
                hsInt.Sort(list.Arrange);
            }

        }

    }
    public class HeapSortInt
    {

        public IList<int> Sort(IList<int> arr)
        {
            int lastNotLeafNode = arr.Count / 2 - 1;

            for (int i = lastNotLeafNode; i >= 0; i--)
            {
                CheckChildsAndSwap(arr.ToList(), i);
            }

            return arr;
        }
        void CheckChildsAndSwap(List<int> arr, int i)
        {
            var maxChildIdx = GetMaxNodeIndex(arr, i * 2 + 1, i * 2 + 2);
            if (maxChildIdx >= 0 && arr[maxChildIdx] > arr[i])
            {
                Swap(arr, maxChildIdx, i);
                CheckChildsAndSwap(arr, maxChildIdx);
            }
        }
        int GetMaxNodeIndex(List<int> arr, int idxSt, int idxFn)
        {
            int result = 0;
            int idx = -1;
            for (int i = idxSt; i <= idxFn; i++)
            {
                if (i <= (arr.Count - 1) && arr[i] > result)
                {
                    result = arr[i];
                    idx = i;
                }
            }
            return idx;
        }
        List<int> Swap(List<int> arr, int a, int b)
        {
            var item = arr[a];
            arr[a] = arr[b];
            arr[b] = item;
            return arr;
        }

    }
    public class HeapSort<T> where T : struct, IComparable
    {
        private readonly int nodesPerLvlv = 2;

        public IList<T> Sort(IList<T> arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                int newIdxHg = (arr.Count - i) - 1;
                heapify(arr, 0, newIdxHg);

                if (Comparer<T>.Default.Compare(arr[0], arr[newIdxHg]) > 0)
                {
                    Swap(arr, 0, newIdxHg);
                }
            }

            return arr;
        }
        public IList<T> heapify(IList<T> arr, int idxLw, int idxHg)
        {
            if (arr != null && idxHg - idxLw >= 0)
            {
                int lastNotLeafNodeIndex = (idxHg - idxLw) / nodesPerLvlv - 1;

                for (int i = lastNotLeafNodeIndex; i >= idxLw; i--)
                {
                    siftDown(arr, i, idxHg);
                }

            }
            return arr;
        }

        void siftDown(IList<T> arr, int index, int maxBorder)
        {
            int maxParentOrChildIndex = GetMaxNodesIndex(arr, index, maxBorder);
            if (maxParentOrChildIndex >= 0 && maxParentOrChildIndex > index)
            {
                Swap(arr, maxParentOrChildIndex, index);
                siftDown(arr, maxParentOrChildIndex, maxBorder);
            }
        }
        int GetMaxNodesIndex(IList<T> arr, int idx, int maxBorder)
        {
            int resultindex = -1;

            if (
                idx < (arr.Count - 1)
                &&
                //There are childs in array with lowerbound index
                ((arr.Count - 1) - (idx * nodesPerLvlv + 1) > 0)
            )
            {
                int nodesLwInds = idx * nodesPerLvlv + 1;
                int nodesHgIdx = idx * (nodesPerLvlv) + nodesPerLvlv;
                nodesHgIdx = nodesHgIdx <= maxBorder ? nodesHgIdx : maxBorder;

                T tempResult = arr[idx];
                for (int nodesIndex = nodesLwInds; nodesIndex <= nodesHgIdx; nodesIndex++)
                {
                    if (Comparer<T>.Default.Compare(arr[nodesIndex], tempResult) > 0)
                    {
                        tempResult = arr[nodesIndex];
                        resultindex = nodesIndex;
                    }
                }
            }

            return resultindex;
        }
        void Swap(IList<T> arr, int idxLw, int idxHg)
        {
            T item = arr[idxLw];
            arr[idxLw] = arr[idxHg];
            arr[idxHg] = item;
        }

    }

    public class HeapSortArr
    {
        //https://www.tutorialspoint.com/heap-sort-in-chash#:~:text=Heap%20Sort%20is%20a%20sorting,then%20the%20heap%20is%20reestablished.
        static void heapSort(int[] arr, int n)
        {
            for (int i = n / 2 - 1; i >= 0; i--)
                heapify(arr, n, i);
            for (int i = n - 1; i >= 0; i--)
            {
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;
                heapify(arr, i, 0);
            }
        }
        static void heapify(int[] arr, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            if (left < n && arr[left] > arr[largest])
                largest = left;
            if (right < n && arr[right] > arr[largest])
                largest = right;
            if (largest != i)
            {
                int swap = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap;
                heapify(arr, n, largest);
            }
        }

        public static void GO()
        {
            var arr = new int[] { 3, 5, 6, 4, 7, 9, 1 };
            var avtHeap = new int[] { 9, 6, 7, 4, 1 };
            var avtSort = new int[] { 1, 3, 4, 5, 6, 7, 9 };

            Sort(arr);

            var bol = arr.SequenceEqual(avtSort);

        }
        static void Sort(int[] arr)
        {
            BuildHeap(arr, arr.Length);
        }

        /// <summary>
        /// From middle to border build heap bu comparing node parents with node
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="len"></param>
        static void BuildHeap(int[] arr, int len)
        {
            for (int i = len / 2 - 1; i >= 0; i--)
            {
                MaxHeap(arr, i, len);
            }
            for (int i = len - 1; i >= 0; i--)
            {
                Swap(arr, 0, i);
                MaxHeap(arr, 0, i);
            }
        }

        /// <summary>
        /// Compare parents with node
        /// and swap if node larger
        /// recursive
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="i">int is 1 so decrease 1 for index</param>
        static void MaxHeap(int[] arr, int i, int len)
        {
            var l = 2 * i + 1;
            var r = 2 * i + 2;
            var lg = i;

            if (l < len && arr[l] > arr[lg]) { lg = l; }

            if (r < len && arr[r] > arr[lg]) { lg = r; }

            if (lg != i)
            {
                Swap(arr, lg, i);
                MaxHeap(arr, lg, len);
            }
        }
        static void Swap(int[] arr, int a, int b)
        {
            var s = arr[a];
            arr[a] = arr[b];
            arr[b] = s;
        }


    }



    public class MergeSortTest
    {
        public static void GO()
        {
            MergeSortTest mst = new MergeSortTest();
            mst.MergeSortIntTest();
        }

        void MergeSortIntTest()
        {
            List<TestListsStructs<int>> arr = new List<TestListsStructs<int>>()
            {
                new TestListsStructs<int>(){Arrange = new List<int>(){ 7, 3, 5, 6, 1, 2, 4, 8 } ,Expected = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 }}
                ,new TestListsStructs<int>(){Arrange = new List<int>(){ 3,1 } ,Expected = new List<int>() { 1, 3 }}
                ,new TestListsStructs<int>(){Arrange = new List<int>(){ 3,1,4,2 } ,Expected = new List<int>() { 1, 2, 3,4 }}
                ,new TestListsStructs<int>(){Arrange = new List<int>(){ 3,1,2 } ,Expected = new List<int>() { 1, 2, 3 }}

            };

            MergeSort<int> ms = new MergeSort<int>();
            foreach (var i in arr)
            {
                i.Arrange = ms.Sort(i.Arrange).ToList();
            }

        }
    }
    public class MergeSort<T> where T : struct, IComparable
    {
        public IList<T> Sort(IList<T> arr)
        {
            IList<T> result = new List<T>(arr.Count);
            if (arr?.Count == 1)
            {
                return arr;
            }
            if (arr?.Count > 1)
            {
                //result = split(arr, 0, arr.Count-1);
                result = splitNoConditions(arr, 0, arr.Count - 1);
            }
            return result;
        }

        /* Less if switches, no swap and sorting down to arrays of 1 element*/
        IList<T> splitNoConditions(IList<T> arr, int idxLw, int idxHg)
        {
            IList<T> result = new List<T>() { };
            if (idxHg == idxLw)
            {
                return arr.Skip(idxLw).Take(1).ToList();
            }
            if (idxHg > idxLw)
            {
                int p = idxLw + (idxHg - idxLw) / 2;
                var left = splitNoConditions(arr, idxLw, p);
                var right = splitNoConditions(arr, p + 1, idxHg);

                result = sortArr(left, right);
            }

            return result;
        }

        IList<T> sortArr(IList<T> arrA, IList<T> arrB)
        {
            IList<T> result = new List<T>();

            int i = 0, i2 = 0;

            while (i < arrA.Count && i2 < arrB.Count)
            {

                if (Comparer<T>.Default.Compare(arrA[i], arrB[i2]) >= 0)
                {
                    result.Add(arrB[i2]);
                    i2++;
                }
                else if (Comparer<T>.Default.Compare(arrA[i], arrB[i2]) < 0)
                {
                    result.Add(arrA[i]);
                    i++;
                }

            }

            while (i < arrA.Count)
            {
                result.Add(arrA[i]);
                i++;
            }

            while (i2 < arrB.Count)
            {
                result.Add(arrB[i2]);
                i2++;
            }

            return result;
        }

    }
    
    public class MergeSortInt
    {

        public IList<int> Sort(IList<int> arr)
        {
            return split(arr, 0, arr.Count);
        }

        public IList<int> split(IList<int> arr, int lwIdx, int hgIdx)
        {
            if (hgIdx == lwIdx)
            {
                arr = arr.Skip(lwIdx).Take(1).ToList();;
            } 
            if (hgIdx > lwIdx)
            {
                var middle = (hgIdx-lwIdx)/2;
                
                var left = split(arr, lwIdx, (lwIdx+middle));
                var right = split(arr, (lwIdx+middle) + 1 , hgIdx);
                arr = merge(left, right);
            }
            return arr;
        }

        private IList<int> merge(IList<int> l, IList<int> r)
        {
            var result = new List<int>(l.Count + r.Count);
            int i = 0, i2 = 0;

            while (i < l.Count && i2 < r.Count)
            {
                if (i < l.Count && i2 < r.Count
                                  && l[i] <= r[i2])
                {
                    result.Add(l[i]);
                    i++;
                }
                if (i < l.Count && i2 < r.Count
                                  && l[i] > r[i2])
                {
                    result.Add(r[i2]);
                    i2++;
                }
            }

            while (i < l.Count)
            {
                result.Add(l[i]);
                i++;
            }

            while (i2 < r.Count)
            {
                result.Add(r[i2]);
                i2++;
            }

            return result;
        }

    }


    public class LinkedListSortTest
    {
        public static void GO()
        {
            LinkedListSortTest ls = new LinkedListSortTest();
            ls.ReverseAndPrintCheck();
        }

        void ReverseAndPrintCheck()
        {
            var node0 = new Node<int>() { Id = 1, Value = 'a', Previous = null, Next = null };
            var node1 = new Node<int>() { Id = 2, Value = 'b', Previous = node0, Next = null };
            var node2 = new Node<int>() { Id = 3, Value = 'c', Previous = node1, Next = null };
            var node3 = new Node<int>() { Id = 4, Value = 'c', Previous = node2, Next = null };
            var node4 = new Node<int>() { Id = 5, Value = 'b', Previous = node3, Next = null };
            var node5 = new Node<int>() { Id = 6, Value = 'a', Previous = node4, Next = null };

            node0.Next = node1;
            node1.Next = node2;
            node2.Next = node3;
            node3.Next = node4;
            node4.Next = node5;

            List<Node<int>> LkdList = new List<Node<int>>(){
                node0,node1,node2,node3,node4,node5
            };

            LinkedListSort<int> ls = new LinkedListSort<int>();

            ls.PrintLine(LkdList);
            ls.Reverse(LkdList);
            ls.PrintLine(LkdList);




            var pn0 = new Node<int>() { Id = 1, Value = 'a', Previous = null, Next = null };
            var pn1 = new Node<int>() { Id = 2, Value = 'b', Previous = pn0, Next = null };
            var pn2 = new Node<int>() { Id = 3, Value = 'b', Previous = pn1, Next = null };
            var pn3 = new Node<int>() { Id = 4, Value = 'b', Previous = pn2, Next = null };
            var pn4 = new Node<int>() { Id = 5, Value = 'a', Previous = pn3, Next = null };

            pn0.Next = pn1;
            pn1.Next = pn2;
            pn2.Next = pn3;
            pn3.Next = pn4;

            List<Node<int>> LkdListp = new List<Node<int>>(){
                pn0,pn1,pn2,pn3,pn4
            };


            bool isPolindrome = ls.PolindromeCheck(LkdListp);
            ls.PrintLine(LkdListp);
            ls.Reverse(LkdListp);
            ls.PrintLine(LkdListp);
            bool isPolindromeAfter = ls.PolindromeCheck(LkdListp);
        }
    }
    public class Node<T> where T : struct, IComparable
    {
        public T Id { get; set; }
        public char Value { get; set; }

        public Node<T> Previous { get; set; }
        public Node<T> Next { get; set; }
    }
    public class LinkedListSort<T> where T : struct, IComparable
    {

        public static void GO()
        {

        }

        public void Sort()
        {

        }

        public void Reverse(IList<Node<T>> list)
        {
            foreach (Node<T> node in list)
            {
                Node<T> prev = node.Previous;
                node.Previous = node.Next;
                node.Next = prev;
            }
        }

        public bool PolindromeCheck(IList<Node<T>> list)
        {
            Node<T> st = list.Where(s => s.Previous == null).FirstOrDefault();
            Node<T> fn = list.Where(s => s.Next == null).FirstOrDefault();

            while (fn != null)
            {
                if (st.Value != fn.Value) { return false; }

                if (st != fn.Previous && fn != st.Next)
                {
                    st = st.Next;
                    fn = fn.Previous;
                }
                else
                {
                    fn = null;
                }

            }

            return true;
        }

        public void Print(IList<Node<T>> list)
        {
            foreach (Node<T> node in list)
            {
                Trace.WriteLine($"Node: {node.Id}; In reference: {node.Previous?.Id}-{node.Id}->{node.Next?.Id};");
            }
        }
        public string PrintLine(IList<Node<T>> list)
        {

            string result = String.Empty;
            Node<T> item = list.Where(s => s.Previous == null).FirstOrDefault();

            while (item != null)
            {
                result += $"-{item?.Id}-";
                item = item.Next;
            }
            Trace.WriteLine(result);
            return result;
        }
        public string PrintValue(IList<Node<T>> list)
        {

            string result = String.Empty;
            Node<T> item = list.Where(s => s.Previous == null).FirstOrDefault();

            while (item != null)
            {
                result += $"-{item?.Value}-";
                item = item.Next;
            }
            Trace.WriteLine(result);
            return result;
        }
    }


}

namespace Rewrite
{
    //custom linq

    /*StreamReadWrite */
    /*--------------------------------------------- */
    /// CopyPast class
    /// Parses class to txt with JSON
    /// Reads result JSON file to class and creates files from content
    /// Detects minimal single directory and recreates folder structure in new folder
    public class StreamCheck
    {
        public void Check()
        {
            ReadFilesCheck();
            ReadCheck();
        }

        public void ReadFilesCheck()
        {
            StreamIO sio3 = new StreamIO();
            string pathResult_ = @"C:\FILES\SHARE\debug\moove\new\result.txt";
            string pathFolders_ = @"C:\FILES\SHARE\debug\moove\1";

            sio3.pathResult_ = pathResult_;
            sio3.pathFolders_ = pathFolders_;

            sio3.DeleteFileChecked(sio3.pathResult_);
            sio3.PathsToLIstAddRecursive(sio3.pathFolders_, 0);

            foreach (string str_ in sio3.fileList)
            {
                PathDict pd = new PathDict();
                sio3.DictionaryInit(pd);
                sio3.FileToDcitionary(str_);
                sio3.DictionaryListAdd();
            }

            //File.WriteAllText(sio3.pathResult_, JsonConvert.SerializeObject(sio3.pathDictList, Formatting.Indented));
            sio3.AddBytesToFile(Encoding.UTF8.GetBytes( JsonSerializer.Serialize(sio3.pathDictList)), sio3.pathResult_);

        }
        public void ReadCheck()
        {
            StreamIO sio3 = new StreamIO();
            string pathResult_ = @"C:\FILES\SHARE\debug\moove\new\result.txt";
            //string pathResult_ = @"C:\111\moove\result.txt";

            sio3.pathResult_ = pathResult_;

            List<PathDict> pd = JsonSerializer.Deserialize<List<PathDict>>(File.ReadAllText(sio3.pathResult_));

            foreach (IPathDictSaearch pd_ in pd)
            {
                sio3.pathDictList.Add(pd_);
            }

            sio3.PathSimplify(sio3.PathRemoveFileName(sio3.pathResult_));

            foreach (IPathDictSaearch pd_ in sio3.pathDictList)
            {
                sio3.CreateFoldersExplicitly(sio3.PathRemoveFileName(pd_.Path));
                if (!pd_.Path.Contains(@"Thumbs"))
                {
                    File.WriteAllText(pd_.Path, Encoding.UTF8.GetString(Encoding.UTF8.GetBytes((pd_.Text))));
                }
            }

        }
    }

    public class StreamIO
    {
        internal IPathDictSaearch pathDict;
        public List<IPathDictSaearch> pathDictList = new List<IPathDictSaearch>();
        internal List<string> fileList = new List<string>();

        public string pathResult_ = null;
        public string pathFolders_ = null;

        public string path = @"";

        internal void DictionaryInit(IPathDictSaearch dict_)
        {
            this.pathDict = dict_;
        }
        internal void DictionaryListAdd()
        {
            this.pathDictList.Add(pathDict);
        }
        private void DictionaryVal(string path_ = null, string text_ = null)
        {
            if (path_ != null)
            {
                pathDict.Path = path_;
            }
            if (text_ != null)
            {
                pathDict.Text = text_;
            }
        }

        internal void DeleteFileChecked(string input_)
        {
            if (File.Exists(input_))
            {
                File.Delete(input_);
            }
        }
        internal byte[] FileToBytesChecked(string input_)
        {
            if (input_ == null)
            {
                throw new NullReferenceException();
            }
            if (input_.Length == 0)
            {
                throw new InvalidOperationException(@"Empty file");
            }
            if (!File.Exists(input_))
            {
                throw new FileNotFoundException();
            }

            //FileInfo fi = new FileInfo(input_);
            //BinaryReader br = new BinaryReader(fi.OpenRead(),Encoding.UTF8);
            //byte[] _result = new byte[br.BaseStream.Length];
            //br.Read(_result, 0, _result.Length);

            FileStream fs = File.OpenRead(input_);
            byte[] _result = new byte[fs.Length];
            fs.Read(_result, 0, _result.Length);

            return _result;
        }
        internal void AddBytesToFile(byte[] input_, string output_)
        {
            if (!File.Exists(output_))
            {
                using (FileStream fs = new FileStream(output_, FileMode.Create))    //Path.GetExtension(input_)))
                {

                    fs.Seek(0, SeekOrigin.End);
                    fs.Write(input_, 0, input_.Length);
                }
            }
            else
            {
                using (FileStream fs = new FileStream(output_, FileMode.Append))    //Path.GetExtension(input_)))
                {
                    fs.Seek(0, SeekOrigin.End);
                    fs.Write(input_, 0, input_.Length);
                }
            }

        }

        internal void PathSimplify(string NewPath_)
        {
            if (pathDictList.Count != 0)
            {

                while (pathDictList.Where(s => !s.Searched).Any())
                {
                    string oldMInimal = null;

                    if (pathDictList.Count != 1)
                    {
                        for (int i = 0; i < pathDictList.Count; i++)
                        {
                            for (int i2 = i; i2 < pathDictList.Count; i2++)
                            {
                                if (i2 != i && !pathDictList[i2].Searched)
                                {
                                    string newMinimal = CompareMinimal(PathRemoveFileName(pathDictList[i].Path), PathRemoveFileName(pathDictList[i2].Path));
                                    if (oldMInimal == null || oldMInimal.Length > newMinimal.Length)
                                    {
                                        oldMInimal = newMinimal;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        oldMInimal = PathRemoveFileName(pathDictList[0].Path);
                    }

                    if (oldMInimal != null)
                    {
                        PathReplace(oldMInimal, NewPath_);
                        FilePathDictionaryPrint();
                    }
                }

            }

            //search shortest path

            //remember
            //remember lowest level
            //mark as searched

            //remove path from all others

            //repeat for all
        }
        private string CompareMinimal(string a_, string b_)
        {
            string _minimalString = null;

            string[] arr_a = a_.Split('\\');
            string[] arr_b = b_.Split('\\');

            if (arr_a.Length < arr_b.Length)
            {
                for (int i = 0; i < arr_a.Length; i++)
                {
                    if (arr_b[i] != arr_a[i]) { break; }

                    if (_minimalString == null)
                    {
                        _minimalString += arr_a[i];
                    }
                    else
                    {
                        _minimalString += @"\" + arr_a[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < arr_a.Length; i++)
                {
                    if (arr_b[i] != arr_a[i]) { break; }

                    if (_minimalString == null)
                    {
                        _minimalString += arr_b[i];
                    }
                    else
                    {
                        _minimalString += @"\" + arr_b[i];
                    }
                }
            }

            return _minimalString;
        }
        internal string PathRemoveFileName(string input_)
        {
            string _result = null;
            if (Path.GetExtension(input_) != "")
            {
                _result = Path.GetDirectoryName(input_);
            }
            else
            {
                _result = input_;
            }
            return _result;
        }
        private void PathReplace(string old_, string new_)
        {
            foreach (IPathDictSaearch pd in pathDictList)
            {
                if (pd.Path.Contains(old_))
                {
                    pd.Path = pd.Path.Replace(old_, new_);
                    pd.Searched = true;
                }
            }
        }

        internal void CreateFoldersExplicitly(string _input)
        {
            string[] arr = _input.Split('\\');
            StringBuilder sb = new StringBuilder();

            foreach (string str in arr)
            {
                if (sb.Length == 0)
                {
                    sb.Append(str);
                }
                else
                {
                    sb.Append(@"\" + str);
                }

                if (!Directory.Exists(sb.ToString()))
                {
                    try
                    {
                        Directory.CreateDirectory(sb.ToString());
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                    }
                }
            }
        }

        internal void PathsToLIstAddRecursive(string input_, int level_)
        {
            level_ += 1;

            string[] directories = Directory.GetDirectories(input_);
            string[] files = Directory.GetFiles(input_);
            if (files.Length != 0)
            {
                foreach (string str_ in files)
                {
                    fileList.Add(str_);
                }
            }

            if (directories.Length != 0)
            {
                foreach (string str_ in directories)
                {
                    PathsToLIstAddRecursive(str_, level_);
                }
            }

            level_ -= 1;
        }

        private void FilePathDictionaryPrint()
        {
            Trace.WriteLine(@"---------------------------");
            foreach (IPathDictSaearch pd in pathDictList)
            {
                Trace.WriteLine(pd.Searched + @" " + pd.Path);
            }
        }

        internal void FileToDcitionary(string str)
        {
            pathDict.Path = str;
            pathDict.Text = Encoding.UTF8.GetString(FileToBytesChecked(str));
        }
    }

    public class PathDict : IPathDict, IPathDictSaearch
    {
        public void Init(string path_ = null, string text_ = null)
        {
            if (path_ != null)
            {
                this.Path = path_;
            }
            if (text_ != null)
            {
                this.Text = text_;
            }
        }
        public string Path { get; set; }
        public string Text { get; set; }
        public bool Searched { get; set; }
    }
    public interface IPathDict
    {
        void Init(string path_ = null, string text_ = null);
        string Path { get; set; }
        string Text { get; set; }
    }
    public interface IPathDictSaearch : IPathDict
    {
        bool Searched { get; set; }
    }





    /*ConsoleParameters */
    /*--------------------------------------------- */
    public static class ConsoleParametersCheck
    {
        public static void Check(string[] args)
        {
            ConsoleParameters.Check(args);
        }
    }

    public static class ConsoleParameters
    {
        public static List<ParametersForConsole> parameters_;

        static ConsoleParameters()
        {
            Initialize();
        }

        public static void Check(string[] args)
        {
            foreach (string str_ in args)
            {
                Console.WriteLine(str_);
            }
            Console.ReadLine();
        }
        public static void Initialize()
        {
            parameters_ = new List<ParametersForConsole>();
            parameters_.Add(new ParametersForConsole()
            {
                ID = 0,
                ParameterName = @"-m",
                Description = @"months"
            });
        }
        public static void ExportJson()
        {

        }
        public static void ImportJson()
        {

        }
        private static string[] StringToArgs(string input_)
        {
            string[] args = null;
            args = input_.Replace(@"""", @"").Split(' ');
            return args;
        }
    }

    public class ParametersForConsole
    {
        public int ID { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string Description { get; set; }
    }




    /*StreamsTesting */
    /*--------------------------------------------- */

    public delegate string encodeStringDel(byte[] arr);
    public delegate byte[] encodeArrDel(string input_);

    public delegate string encodeStringCodePageDel(byte[] arr, int codepage_);
    public delegate byte[] encodeArrCodePageDel(string input_, int codepage_);

    public static class StreamTesting
    {
        static string FilePathIn { get; set; }
        static string DirectoryIn { get; set; }
        static string FilePathOut { get; set; }
        static string FileNameOut { get; set; }
        static string DirectoryOut { get; set; }
        static FileStream fsIn { get; set; }
        static FileStream fsOut { get; set; }
        static byte[] arr { get; set; }
        static int length { get; set; }
        static int pos { get; set; }

        static StreamReader sr;
        static int cnt;
        static int b = 0;
        static string a = null;

        static encodeStringDel encodingStr { get; set; }
        static encodeArrDel encodingArr { get; set; }

        static encodeStringCodePageDel encodingStrCP { get; set; }
        static encodeArrCodePageDel encodingArrCP { get; set; }

        static Encoding encoding { get; set; }
        static int codepage { get; set; }

        static Encoding localEncoding_ = null;

        static List<PathText> pathTextList = new List<PathText>();
        public static void Check()
        {

            StartDefault();

            //SerializeOne();
            //DeserializeOne();

        }

        public static void SerializeOne()
        {
            string FileToParse = @"C:\FILES\SHARE\debug\moove\1\send.7z";
            string FileToExportBytesStr = @"C:\FILES\SHARE\debug\moove\new\send_bytes_str.txt";
            //string FileToExportBytes = @"C:\FILES\SHARE\debug\moove\new\send_bytes.txt";
            string FileToExport = @"C:\FILES\SHARE\debug\moove\new\out.txt";
            byte[] arr;
            string name;
            string res_;
            StreamReader sr_;
            StringBuilder sb;

            using (FileStream fs_ = new FileStream(FileToParse, FileMode.Open))
            {
                sr_ = new StreamReader(fs_, true);
                name = sr_.CurrentEncoding.BodyName;
                arr = new byte[fs_.Length];
                fs_.Read(arr, 0, arr.Length);
            }

            sb = new StringBuilder();

            foreach (byte bt_ in arr)
            {
                sb.Append(bt_);
                sb.Append(@",");
            }
            res_ = sb.ToString();

            File.WriteAllText(FileToExportBytesStr, res_);

            using (FileStream fsWrite_ = new FileStream(FileToExport, FileMode.OpenOrCreate))
            {
                fsWrite_.Write(arr, 0, arr.Length);
            }
        }
        public static void DeserializeOne()
        {
            string FileToParse = @"C:\FILES\SHARE\debug\moove\new\out.txt";

            string FileToExport = @"C:\FILES\SHARE\debug\moove\new\out_de.txt";
            byte[] arr;

            FileStream fs = new FileStream(FileToParse, FileMode.Open);
            arr = new byte[fs.Length];
            fs.Read(arr, 0, arr.Length);

            using (FileStream fsWrite_ = new FileStream(FileToExport, FileMode.OpenOrCreate))
            {
                fsWrite_.Write(arr, 0, arr.Length);
            }

        }

        public static void StartDefault()
        {
            StringEncoder();
            InitFromCode();
            SetDefaultEncoding();
            Serialize_();
            Deserialize_();
        }
        public static void InitFromCode()
        {
            DirectoryIn = @"C:\FILES\SHARE\debug\moove\1";
            DirectoryOut = @"C:\FILES\SHARE\debug\moove\new";
            FileNameOut = @"out.txt";
            FilePathOut = Path.Combine(DirectoryOut, FileNameOut);
        }
        public static void StreamWrite()
        {
            arr = new byte[length];
            fsIn.Read(arr, 0, length);
            ByteArrayAsIs(arr, FilePathOut + @"_bytesASIS.txt");
            fsOut.Write(arr, 0, length);
            BytesExport();
        }
        public static void BytesExport()
        {
            File.WriteAllBytes(DirectoryOut + @"\_bytes.txt", arr);
        }
        public static void ArrWrite(byte[] arr_)
        {
            fsOut.Write(arr_, 0, arr_.Length);
        }
        public static void WriteNewLine()
        {
            fsOut.WriteByte(13);
        }
        public static void Write_(byte[] arr_)
        {
            ArrWrite(arr);
            WriteNewLine();
        }

        public static void ByteArrayAsIs(byte[] arr_, string path_)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte bt_ in arr_)
            {
                sb.Append(bt_);
                sb.Append(@",");
            }
            string res_ = sb.ToString();

            File.WriteAllText(path_, res_);
        }

        public static void Serialize_()
        {
            fsOut = new FileStream(FilePathOut, FileMode.OpenOrCreate);
            foreach (string filename in Directory.GetFiles(DirectoryIn))
            {
                if (!filename.Contains(@"Thumbs"))
                {

                    FilePathIn = filename;
                    fsIn = new FileStream(FilePathIn, FileMode.Open);

                    //new filename in output directory
                    FileNameOut = Path.GetFileNameWithoutExtension(FilePathIn) + @"_copy" + Path.GetExtension(FilePathIn);
                    length = filename.Length;


                    //write length of next array and array
                    arr = encodingArr(length.ToString());
                    Write_(arr);

                    arr = encodingArr(filename);
                    Write_(arr);

                    //set default string encoding UTF-8
                    StringEncoder();

                    //write encoding length and name                
                    arr = encodingArr(EncodingGetFS(fsIn).CodePage.ToString().Length.ToString());
                    Write_(arr);

                    arr = encodingArr(EncodingGetFS(fsIn).CodePage.ToString());
                    Write_(arr);

                    //write length of array and array
                    length = (int)fsIn.Length;
                    arr = encodingArr(length.ToString());
                    Write_(arr);

                    //get encoding
                    EncodingGet(fsIn);
                    //change encoding for file text
                    codepage = encoding.CodePage;

                    //write file text
                    StreamWrite();
                    WriteNewLine();

                    fsIn.Close();
                    fsIn.Dispose();
                }
            }
            fsOut.Close();
            fsOut.Dispose();
        }
        public static void Deserialize_()
        {
            PathTextFill();
            PathtextRead();
        }

        public static void PathTextFill()
        {
            fsIn = new FileStream(FilePathOut, FileMode.Open);
            sr = new StreamReader(fsIn);
            int encode = 0;
            while (fsIn.Position + 1 < fsIn.Length)
            {
                string path = ParsePathText();
                Int32.TryParse(ParsePathText(), out encode);
                localEncoding_ = Encoding.UTF8; //Encoding.GetEncoding(encode);
                string text = ParsePathText();
                if (path != "" && encode != 0 && text != "")
                {
                    pathTextList.Add(new PathText(Path.Combine(DirectoryOut, Path.GetFileName(path)), arr, codepage));
                }
            }
        }
        public static void PathtextRead()
        {
            foreach (PathText pt_ in pathTextList)
            {
                if (true == true)
                {
                    using (FileStream fss = new FileStream(pt_.path, FileMode.OpenOrCreate))
                    {
                        try
                        {
                            fss.Write(pt_.text, 0, pt_.text.Length);
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine(e.Message);
                        }
                        fss.Close();
                    }
                }
            }
        }

        public static string ParsePathText()
        {
            string _Newpath = null;
            cnt = 0;
            while (fsIn.ReadByte() != 13)
            {
                if (fsIn.Position >= fsIn.Length) break;
                cnt += 1;
            }
            fsIn.Position -= cnt + 1;
            arr = new byte[cnt];
            fsIn.Read(arr, 0, cnt);
            a = encodingStrCP(arr, codepage);
            Int32.TryParse(a, out b);
            arr = new byte[b];
            fsIn.Position += 1;
            fsIn.Read(arr, 0, b);
            if (localEncoding_ != null)
            {
                _Newpath = encodingStrCP(arr, localEncoding_.CodePage);
                localEncoding_ = null;
            }
            else
            {
                _Newpath = encodingStrCP(arr, codepage);
            }
            fsIn.Position += 1;

            return _Newpath;
        }

        public static void EncodingGet(FileStream fs_)
        {
            StreamReader sr = new StreamReader(fs_, true);
            encoding = sr.CurrentEncoding;
            if (Path.GetExtension(fs_.Name) == ".7z")
            {
                encoding = Encoding.BigEndianUnicode;
            }
            codepage = encoding.CodePage;
        }
        public static Encoding EncodingGetFS(FileStream fs_)
        {
            StreamReader sr = new StreamReader(fs_, true);
            Encoding result_ = sr.CurrentEncoding;
            if (Path.GetExtension(fs_.Name) == ".")
            {
                result_ = Encoding.BigEndianUnicode;
            }
            return result_;
        }

        public static void StringEncoder()
        {
            BEenc();
            CPenc();
        }
        public static void SetDefaultEncoding()
        {
            encoding = Encoding.UTF8;
        }

        static void UTFenc()
        {
            encodingStr = EncodeStringUTF8;
            encodingArr = EncodeArrUTF8;
        }
        static void BEenc()
        {
            encodingStr = EncodeStringBE;
            encodingArr = EncodeArrBE;
        }
        static void CPenc()
        {
            encodingStrCP = EncodeStringCodepage;
            encodingArrCP = EncodeArrCodepage;
        }

        public static string EncodeStringUTF8(byte[] arr)
        {
            return Encoding.UTF8.GetString(arr);
        }
        public static string EncodeStringBE(byte[] arr)
        {
            return Encoding.ASCII.GetString(arr);
        }

        public static byte[] EncodeArrUTF8(string input_)
        {
            return Encoding.UTF8.GetBytes(input_);
        }
        public static byte[] EncodeArrBE(string input_)
        {
            return Encoding.ASCII.GetBytes(input_);
        }

        public static string EncodeStringCodepage(byte[] arr, int codepage_)
        {
            return Encoding.GetEncoding(codepage_).GetString(arr);
        }
        public static byte[] EncodeArrCodepage(string input_, int codepage_)
        {
            return Encoding.GetEncoding(codepage_).GetBytes(input_);
        }

    }

    public class PathText
    {
        public string path { get; set; }
        public byte[] text { get; set; }
        public int? codepage { get; set; }

        public PathText(string path_, byte[] text_, int? codepage_ = null)
        {
            this.path = path_;
            this.text = text_;
            this.codepage = codepage_;
        }
    }






    /*CutomLinq */
    /*--------------------------------------------- */
    public static class LinqToContextCheck
    {
        static TestContext ts = new TestContext();
        public static void GO()
        {
            ts.ExpressionBuild();

            string st0 = ts.TraverseExpression<TestEntity>(s => s.tp.isTrue == true);

            string st4 = ts.VisitLeftRightFromExpressionTypes<TestEntity>(s => s.tp.isTrue == false);
            string st1 = ts.VisitLeftRightFromExpressionTypes<TestEntity>(s => s.Id >= 1);

            string st2 = ts.VisitLeftRightFromExpressionTypes<TestEntity>(s => s.name == "test name");
            string st3 = ts.VisitLeftRightFromExpressionTypes<TestEntity>(s => s.intrinsicIsTrue == true);

        }
    }
    //check Linq to context
    public class TestContext
    {
        ParameterExpression leftParamExpr;
        Type leftType_ = null;

        ConstantExpression constExpr;

        public TestContext()
        {

        }
        public string VisitLeftRightFromExpressionTypes<T>(Expression<Func<T, bool>> expr_)
          where T : TestEntity
        {
            var b = expr_;
            var c = b.Body;
            var gtp = c.GetType();
            var nt = c.NodeType;
            var pm = expr_.Parameters;
            var nm = expr_.Name;
            var tp = expr_.Type;
            var typeName = typeof(T);

            //straight convertsion
            string straight = expr_.ToString();
            BinaryExpression binaryE = (BinaryExpression)expr_.Body;

            //conversion from nested class
            string straightNested = binaryE.ToString();

            if (binaryE.Left != null) { VisitConditional(binaryE.Left); }
            if (binaryE.Right != null) { VisitConditional(binaryE.Right); }

            Expression leftParameter = Expression.Parameter(leftType_, leftType_.Name);
            Type tp0 = leftParameter.GetType();
            Expression leftExpr = Expression.Property(leftParameter, leftParamExpr.Name);
            Type tp1 = leftExpr.GetType();
            ExpressionType nodeType = c.NodeType;
            Expression rightParameter = Expression.Constant(constExpr.Value, constExpr.Type);
            Type tp2 = rightParameter.GetType();
            Expression e0 = Expression.Assign(leftExpr, rightParameter);
            Type tp3 = e0.GetType();

            string ets = e0.ToString();

            string lb = this.VisitBinary((MemberExpression)binaryE.Left, "oper", binaryE);
            string lb2 = this.VisitBinary(binaryE, "converted", leftExpr, nodeType, rightParameter);

            //variable not invoked
            //var a = Expression.Lambda(e0).Compile().DynamicInvoke();

            return ets;
        }
        public Expression VisitConditional(Expression expr)
        {
            Type type_ = expr.GetType().BaseType;
            if (type_ == typeof(MemberExpression))
            {
                MemberExpression memberExpr = (MemberExpression)expr;
                leftType_ = memberExpr.Expression.Type;
                /*
                MemberExpression mn=(MemberExpression)memberExpr.Expression;
                memberName=mn.Member.Name;
                */
                leftParamExpr = Expression.Parameter(memberExpr.Type, memberExpr.Member.Name);
            }
            if (type_ == typeof(ConstantExpression) || type_ == typeof(Expression))
            {
                constExpr = (ConstantExpression)expr;
                Expression rightExpr = Expression.Constant(constExpr.Value, constExpr.Type);
            }
            return expr;
        }

        public string VisitBinary(MemberExpression binary, string @operator, BinaryExpression expression) =>
          $"{@operator}({binary},{expression})";
        public string VisitBinary(BinaryExpression expression, string op, Expression left, ExpressionType nt, Expression right) =>
          $"{expression}({op}),{left}|{nt}|{right}";

        public string TraverseExpression<T>(Expression<Func<T, bool>> expr_)
        {
            //traversing expression
            Pv pv = new Pv();
            string traversed = pv.VisitBody(expr_);
            return traversed;
        }

        //build in function compile
        internal static void BuiltInCompile()
        {
            Expression<Func<double, double, double, double, double, double>> infix =
                (a, b, c, d, e) => a + b - c * d / 2 + e * 3;
            Func<double, double, double, double, double, double> function = infix.Compile();
            double result = function(1, 2, 3, 4, 5); // 12
        }
        public void ExpressionBuild()
        {

            BinaryExpression be = Expression.Power(Expression.Constant(2D), Expression.Constant(3D));
            Expression<Func<double>> fd = Expression.Lambda<Func<double>>(be);
            Func<double> ce = fd.Compile();
            double res = ce();
        }

    }

    public class TestEntity
    {
        public string name { get; set; }
        static int id { get; set; } = 0;
        public ToggleProp tp { get; set; }
        public int Id
        {
            get
            {
                if (id == 0) { id += 1; }
                return id;
            }
            set { id = value; }
        }
        public bool intrinsicIsTrue { get; set; }
    }
    public class ToggleProp
    {
        public bool isTrue { get; set; }
    }

    //https://weblogs.asp.net/dixin/functional-csharp-function-as-data-and-expression-tree
    internal abstract class Ba<TResult>
    {

        internal virtual TResult VisitBody(LambdaExpression expression) => this.VisitNode(expression.Body, expression);

        protected TResult VisitNode(Expression node, LambdaExpression expression)
        {

            switch (node.NodeType)
            {

                case ExpressionType.Equal:
                    return this.VisitEqual((BinaryExpression)node, expression);

                case ExpressionType.GreaterThanOrEqual:
                    return this.VisitGreaterorEqual((BinaryExpression)node, expression);

                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)node, expression);

                case ExpressionType.Constant:
                    return this.VisitConstatnt((ConstantExpression)node, expression);

                default:
                    throw new ArgumentOutOfRangeException(nameof(node));
            }

        }

        protected abstract TResult VisitEqual(BinaryExpression equal, LambdaExpression expression);
        protected abstract TResult VisitGreaterorEqual(BinaryExpression equal, LambdaExpression expression);
        protected abstract TResult VisitMemberAccess(MemberExpression equal, LambdaExpression expression);
        protected abstract TResult VisitConstatnt(ConstantExpression equal, LambdaExpression expression);
    }
    internal class Pv : Ba<string>
    {

        protected override string VisitEqual
          (BinaryExpression add, LambdaExpression expression) => this.VisitBinary(add, "Equal", expression);

        protected override string VisitGreaterorEqual
          (BinaryExpression add, LambdaExpression expression) => this.VisitBinary(add, "Greater", expression);

        protected override string VisitMemberAccess
          (MemberExpression add, LambdaExpression expression) => this.VisitBinary(add, "Member", expression);

        protected override string VisitConstatnt
          (ConstantExpression add, LambdaExpression expression) => this.VisitBinary(add, "Constant", expression);

        private string VisitBinary( // Recursion: operator(left, right)
          BinaryExpression binary, string @operator, LambdaExpression expression) =>
          $"{@operator}({this.VisitNode(binary.Left, expression)},{this.VisitNode(binary.Right, expression)})";

        private string VisitBinary( // Recursion: operator(left, right)
          MemberExpression binary, string @operator, LambdaExpression expression) =>
          $"{binary}";

        private string VisitBinary( // Recursion: operator(left, right)
          ConstantExpression binary, string @operator, LambdaExpression expression) =>
          $"{binary}";
    }


}

