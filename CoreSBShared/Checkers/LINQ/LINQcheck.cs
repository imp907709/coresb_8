using System.Linq;
using System.Runtime.Intrinsics.X86;
using CoreSBShared.Checkers.LINQ;
using InfrastructureCheckers;
using InfrastructureCheckers.Vit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace Live
{
    public class LINQcheck
    {

        public static async Task SampleTemplates()
        {
            // ================== Sorting ==================
            // TASK: Sort users alphabetically by name
            var sortedUsers = SampleData.users.OrderBy(u => u.Name).ToList();

            // TASK: Sort products by descending price and take top 2
            var topProducts = SampleData.products.OrderByDescending(p => p.Price).Take(2).ToList();

            // ================== Filtering ==================
            // TASK: Filter users from NY state
            var nyUsers = SampleData.users.Where(u => u.State == "NY").ToList();

            // ================== Mapping ==================
            // TASK: Map users to simplified objects (name + Id)
            var simpleUsers = SampleData.users.Select(u => new {u.Name, u.Id}).ToList();

            // ================== Grouping ==================
            // TASK: Group users by single key (State) using method syntax
            var usersByState = SampleData.users
                .GroupBy(u => u.State)
                .SelectMany(s => s, (l,r) => new {
                    k = l.Key, cntr = r.Name
                });

            // TASK: Group users by multiple keys (State + Country) using query syntax
            var usersByStateCountry =
                from u in SampleData.users
                group u by new {u.State, u.Country}
                into g
                select new                 {
                    k = g.Key.Country, c = g.Key.State
                    , u = g.ToList()
                };
            

            // ================== Join ==================
            // TASK: Inner join users + products by Id
            var userProductJoin =
                from u in SampleData.users
                join p in SampleData.products on u.Id equals p.Id
                select new {u.Name, Product = p.Title};


            // TASK: Left join users + products (users may not have matching product)
            var leftJoin =
                from u in SampleData.users
                join p in SampleData.products on u.Id equals p.Id
                    // left part
                    into upGroup
                from p in upGroup.DefaultIfEmpty()
                select new {u.Name, Product = p?.Title ?? "No Product"};
            

            // ================== Async / Task ==================
            // TASK: Implement async method to fetch all products
            async Task<List<ProductLive>> GetAllProductsAsync()
            {
                await Task.Delay(500); // simulate async network delay
                return SampleData.products;
            }

            // TASK: Call async method and await results
            var asyncProducts = await GetAllProductsAsync();

            // TASK: Combine async fetch with filtering/mapping
            var expensiveProducts = (await GetAllProductsAsync())
                .Where(p => p.Price > 500)
                .ToList();

            // ================== Additional Exercises ==================
            // TASK: Find top products whose IDs match a user ID
            var matchingIds = SampleData.products
                .Where(p => SampleData.users.Any(u => u.Id == p.Id)).ToList();

           
            // inner join 
            var innerJoinTwo = from u in SampleData.users
                join p in SampleData.products on u.Id equals p.Id
                select new {user = u.Name, product = p.Title};

            // left join
            var leftJoinTwo = from u in SampleData.users
                join p in SampleData.products on u.Id equals p.Id
                    into g
                from p in g.DefaultIfEmpty()
                select new {user = u.Name, title = p.Title};
       
            
            // --------------------------
            // employee templates 
             // sample grouping
            var res = SampleData.employees.GroupBy(d => new {d.Department, d.Name})
                .Select(s => new { name = s.Key.Name, dep = s.Key.Department, itms = s.ToList() })
                .ToList();

            // max salary by dep
            var maxDep = SampleData.employees.GroupBy(d => d.Department)
                .Select(s => new {Dep = s.Key, Sal = s.Max(v => v.Salary)})
                .ToList();

            // max avg salary
            var maxAvgDep = SampleData.employees.GroupBy(d => d.Department)
                .Select(s => new {Dep = s.Key, Sal = s.Average(v => v.Salary)})
                .OrderByDescending(o => o.Sal)
                .FirstOrDefault();
            
            // largest payed per dep
            var topEmployeesPerDept = SampleData.employees
                .GroupBy(e => e.Department)
                .Select(g => g.OrderByDescending(e => e.Salary).First())
                .ToList();

            // avarage sal
            var emp = SampleData.employees
                .GroupBy(g => g.Department)
                .Select(s => new {dep = s.Key, avg = s.Average(v => v.Salary)})
                .OrderByDescending(o => o.avg)
                .FirstOrDefault();

            // Select the top 3 highest paid employees in each department. Mode: GroupBy + projection. Focus: Grouping, ordering, Take.
            var largest3PerDep = SampleData.employees.GroupBy(g => g.Department)
                .Select(s => s.OrderByDescending(v => v.Salary).Take(3)).ToList();

            var largest3PerDeps = SampleData.employees.GroupBy(g => g.Department)
                .SelectMany(s => s.OrderByDescending(v => v.Salary).Take(3)).ToList();

            var largest = SampleData.employees.GroupBy(g => g.Department)
                .Select(s => s.OrderByDescending(k => k.Salary).First()).ToList();

            
            // inner join 
            var innerJoin = from u in SampleData.users
                join p in SampleData.products on u.Id equals p.Id
                select new {user = u.Name, product = p.Title};

            // left join
            var lefJOin = from u in SampleData.users
                join p in SampleData.products on u.Id equals p.Id
                    into g
                from p in g.DefaultIfEmpty()
                select new {user = u.Name, title = p.Title};

            var str = "aaabbcc";
            var maxCnt = str.GroupBy(g => g)
                .Select(s => new {ch = s.Key, cnt = s.Count(c => c != null)})
                .OrderByDescending(o => o.cnt).FirstOrDefault();
            
            
            var gp3 = SampleData.employees
                .GroupBy(p => new{p.Department})
                .SelectMany(s => s, (g, c) => new {
                     g.Key, c.Department
                });

            var gpM = SampleData.employees
                .GroupBy(p => p.Department)
                .Select(c => c.OrderByDescending(k => k.Salary).First());

            var lj1 =
                from s1 in SampleData.Customers
                join s2 in SampleData.Orders on s1.ExternalId equals s2.CustomerId
                    into g
                from s3 in g.DefaultIfEmpty()
                select new {s3.Amount, s3.Status, s1.Name};

            var gp = SampleData.Orders
                .GroupBy(p => p.CustomerId)
                .SelectMany(s => s, (g, c) 
                    => new {k = g.Key, a = c.Amount})
                .ToList();

            var gp2 = SampleData.Orders
                .GroupBy(p => new {p.CustomerId, p.Status})
                .SelectMany(s => s, (g, c) 
                    => new {cId = g.Key.CustomerId, status = g.Key.Status, a = c.Amount})
                .ToList();


            var gpj = SampleData.Orders
                .GroupBy(g => g.CustomerId)
                .Select(s => s.OrderByDescending(o => o.Amount).First())
                .ToList();

            var _context = new SampleDataVit.VitContext(new DbContextOptions<DbContext>());

            var min = 15;
            var max = 27;
            _context.prices.Include(s => s.Price)
                .GroupBy(c => c.Product)
                .Select(s => s.OrderByDescending(c => c.CreatedDate).First())
                .Where(s => s.Price >= min && s.Price <= max);

            var prices = _context.prices.ToList();
            var products = _context.products.ToList();

            var lj = from s1 in prices
                join s2 in products on s1.ProductId equals s2.Id
                    into g
                from s3 in g.DefaultIfEmpty()
                select new {s1.Price, s3.Name};
        }

        // sample data empl orders check
        public static void SampleDataChecks()
        {
            
            // max sal by dep
            var sumBygp = SampleData.employees.GroupBy(g => g.Department)
                .Select(s => new {dep = s.Key, amt = s.Max(c => c?.Salary ?? 0)});
            
            var largSalEmplWithDep = SampleData.employees.GroupBy(g=> g.Department)
                .Select(c=>c.OrderByDescending(o=>o.Salary).First());
            
            // avarage
            // AVG: hr 57500 it 756666 fin 67666
            var avgbyDep = SampleData.employees
                .GroupBy(g => new {dep = g.Department})
                .Select(s => new {Department = s.Key.dep, avg = s.Average(c => c.Salary)});

            // max in group
            // max by cutId : 101-300, 103-270, 104-180
            var maxBuCustom = SampleData.Orders.GroupBy(s => s.CustomerId)
                .Select(c => c.OrderByDescending(s => s.Amount).First());

            var maxCustSum = SampleData.Orders.GroupBy(g => new {Cust = g.CustomerId})
                .Select(c => new {Cust = c.Key.Cust, Sum = c.Sum(v => v.Amount)})
                .OrderByDescending(o => o.Sum).First();

            // left join 
            // group by sum
            var lj = 
                from s1 in SampleData.Customers
                    join s2 in SampleData.Orders 
                        on s1.ExternalId equals s2.CustomerId into j
                        from s3 in j.DefaultIfEmpty()
                group s3 by new {s1.ExternalId} into g
                select new { g.Key, amt = g.Sum(c=> c?.Amount ?? 0)};

            var gpJn = SampleData.Customers
                .Join(SampleData.Orders, l => l.ExternalId, r => r.CustomerId,
                    (l, r) => new {l.ExternalId, l.Name, r.Amount})
                .GroupBy(g => new {g.ExternalId, g.Name})
                .Select(s => new {
                    Gp = s.Key.Name, amt = s.Sum(c=>c?.Amount ?? 0)
                });
            
            
            var total = SampleData.employees.Count();
            var ordrd = SampleData.employees.OrderByDescending(s => s.Salary);
            var percentileGeneral = SampleData.employees.Select(s => new {
                s.Name, s.Salary, perc = (double)ordrd.Count(c => c.Salary < s.Salary) / total * 100
            }).OrderByDescending(c=>c.perc).ToList();
            
            var percentileByDepartment = SampleData.employees.Select(s => new {
                s.Name, s.Salary, s.Department, perc = (double)ordrd.Where(c=>c.Department == s.Department)
                   .Count(c => c.Salary < s.Salary) 
               / SampleData.employees.Where(c=>c.Department == s.Department).Count()
               * 100
                
            }).OrderByDescending(c=>c.Department).ToList();
            
            // group by select selectmany - unwrap
            var emp = SampleData.employees
                .GroupBy(g => new {g.Department, g.Name})
                .Select(s => new {
                        s.Key.Department, s.Key.Name, 
                        sal = s.Select(c => new {c.Salary, c.Id})
                })
                .SelectMany(c=> c.sal, 
                    (l,r) => new { l.Department,l.Name, r.Salary,r.Id })
                .ToList();

           
        }


        public static void GO()
        {
            var str = "aaaabbbcceeeeeeffff";

            var topFreq = str.GroupBy(c => c)
                .Select(s => new {s.Key, cnt = s.Count(x => true)})
                .OrderByDescending(c => c.cnt).Take(3).ToList();

            var charMap = new HashMaps();
        }
    }
}
