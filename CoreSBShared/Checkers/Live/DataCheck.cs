using System.Linq;
using CoreSBShared.Checkers.LINQ;
using InfrastructureCheckers.Vit;
using Microsoft.EntityFrameworkCore;

namespace Live
{
    public class LINQcheck
    {
        public static async Task GO()
        {
            // ================== Sorting ==================
            // TASK: Sort users alphabetically by name
            var sortedUsers = SampleData.users.OrderBy(u => u.Name).ToList();
            Console.WriteLine("Sorted Users:");
            sortedUsers.ForEach(u => Console.WriteLine(u.Name));

            // TASK: Sort products by descending price and take top 2
            var topProducts = SampleData.products.OrderByDescending(p => p.Price).Take(2).ToList();
            Console.WriteLine("Top 2 Expensive Products:");
            topProducts.ForEach(p => Console.WriteLine($"{p.Title} - {p.Price}"));

            // ================== Filtering ==================
            // TASK: Filter users from NY state
            var nyUsers = SampleData.users.Where(u => u.State == "NY").ToList();
            Console.WriteLine("NY Users:");
            nyUsers.ForEach(u => Console.WriteLine(u.Name));

            // ================== Mapping ==================
            // TASK: Map users to simplified objects (name + Id)
            var simpleUsers = SampleData.users.Select(u => new {u.Name, u.Id}).ToList();
            Console.WriteLine("Simplified Users:");
            simpleUsers.ForEach(u => Console.WriteLine($"{u.Id} - {u.Name}"));

            // ================== Grouping ==================
            // TASK: Group users by single key (State) using method syntax
            var usersByState = SampleData.users.GroupBy(u => u.State);
            foreach (var group in usersByState)
            {
                Console.WriteLine($"State: {group.Key}");
                foreach (var u in group)
                    Console.WriteLine($" - {u.Name}");
            }

            // TASK: Group users by multiple keys (State + Country) using query syntax
            var usersByStateCountry =
                from u in SampleData.users
                group u by new {u.State, u.Country}
                into g
                select g;
            foreach (var g in usersByStateCountry)
            {
                Console.WriteLine($"State: {g.Key.State}, Country: {g.Key.Country}");
                foreach (var u in g)
                    Console.WriteLine($" - {u.Name}");
            }


            // ================== Join / Left Join ==================
            // TASK: Inner join users + products by Id
            var userProductJoin =
                from u in SampleData.users
                join p in SampleData.products on u.Id equals p.Id
                select new {u.Name, Product = p.Title};
            Console.WriteLine("Inner Join (User ID = Product ID):");
            foreach (var up in userProductJoin)
                Console.WriteLine($"{up.Name} - {up.Product}");

            // TASK: Left join users + products (users may not have matching product)
            var leftJoin =
                from u in SampleData.users
                join p in SampleData.products on u.Id equals p.Id
                    // left part
                    into upGroup
                from p in upGroup.DefaultIfEmpty()
                select new {u.Name, Product = p?.Title ?? "No Product"};

            Console.WriteLine("Left Join (User -> Product):");
            foreach (var up in leftJoin)
                Console.WriteLine($"{up.Name} - {up.Product}");

            // ================== Async / Task ==================
            // TASK: Implement async method to fetch all products
            async Task<List<ProductLive>> GetAllProductsAsync()
            {
                await Task.Delay(500); // simulate async network delay
                return SampleData.products;
            }

            // TASK: Call async method and await results
            var asyncProducts = await GetAllProductsAsync();
            Console.WriteLine("Async Products:");
            asyncProducts.ForEach(p => Console.WriteLine(p.Title));

            // TASK: Combine async fetch with filtering/mapping
            var expensiveProducts = (await GetAllProductsAsync())
                .Where(p => p.Price > 500)
                .ToList();
            Console.WriteLine("Expensive Products (>500):");
            expensiveProducts.ForEach(p => Console.WriteLine(p.Title));

            // ================== Additional Exercises ==================
            // TASK: Find top products whose IDs match a user ID
            var matchingIds = SampleData.products.Where(p => SampleData.users.Any(u => u.Id == p.Id)).ToList();
            Console.WriteLine("Products matching user IDs:");
            matchingIds.ForEach(p => Console.WriteLine($"{p.Title} - ID {p.Id}"));

            // TASK: Map + filter users in CA state
            var mappedFilteredUsers = SampleData.users
                .Where(u => u.State == "CA")
                .Select(u => new {u.Name, u.Country})
                .ToList();
            Console.WriteLine("Mapped + Filtered Users (CA):");
            mappedFilteredUsers.ForEach(u => Console.WriteLine($"{u.Name} - {u.Country}"));

            // ================== Utility / Placeholder ==================
            // TASK: Demonstrate readonly / record usage
            var ru = new ReadonlyUser(1, "Alice", "NY");
            // ru.Name = "Bob"; // ❌ compilation error (readonly simulation)


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
        }

        // Top-level record declaration inside namespace/class scope
        public record ReadonlyUser(int Id, string Name, string State);
    }

    public class EmployeeCheck
    {
        public static void Filter()
        {
            var res = SampleData.employees.GroupBy(d => new {d.Department, d.Name})
                .Select(s => new {s.Key.Name, s.Key.Department})
                .ToList();

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

            var largestSlPerDep = SampleData.employees.GroupBy(g => g.Department)
                .Select(s => s.OrderByDescending(v => v.Salary).First()).ToList();


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

            var str = "aaabbcc";
            var maxCnt = str.GroupBy(g => g).Select(s => new {ch = s.Key, cnt = s.Count(c => c != null)})
                .OrderByDescending(o => o.cnt).FirstOrDefault();
        }
    }

    public class DataCheck
    {
        public static void GO()
        {
            var lj1 =
                from s1 in SampleData.Customers
                join s2 in SampleData.Orders on s1.ExternalId equals s2.CustomerId
                    into g
                from s3 in g.DefaultIfEmpty()
                select new {s3.Amount, s3.Status, s1.Name};

            var gp = SampleData.Orders
                .GroupBy(p => p.CustomerId)
                .SelectMany(s => s, (g, c) => new {k = g.Key, a = c.Amount})
                .ToList();

            var gp2 = SampleData.Orders
                .GroupBy(p => new {p.CustomerId, p.Status})
                .SelectMany(s => s, (g, c) => new {cId = g.Key.CustomerId, status = g.Key.Status, a = c.Amount})
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
    }
}
