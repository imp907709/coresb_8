using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveCodingPrep
{
    // ================== Models ==================
    public class UserLive
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
    }

    public class ProductLive
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public decimal Price { get; set; }
    }

    public class Program
    {
        public static async Task Main()
        {
            // ================== Sample Data ==================
            var users = new List<UserLive>
            {
                new() { Id = 1, Name = "Alice", State = "NY", Country = "USA" },
                new() { Id = 2, Name = "Bob", State = "CA", Country = "USA" },
                new() { Id = 3, Name = "Charlie", State = "NY", Country = "USA" },
                new() { Id = 4, Name = "Diana", State = "TX", Country = "USA" },
                new() { Id = 5, Name = "Eve", State = "CA", Country = "USA" }
            };

            var products = new List<ProductLive>
            {
                new() { Id = 1, Title = "Laptop", Price = 1200 },
                new() { Id = 2, Title = "Phone", Price = 800 },
                new() { Id = 3, Title = "Book", Price = 20 },
                new() { Id = 4, Title = "Tablet", Price = 500 },
                new() { Id = 5, Title = "Headphones", Price = 150 }
            };

            // ================== Sorting ==================
            // TASK: Sort users alphabetically by name
            var sortedUsers = users.OrderBy(u => u.Name).ToList();
            Console.WriteLine("Sorted Users:");
            sortedUsers.ForEach(u => Console.WriteLine(u.Name));

            // TASK: Sort products by descending price and take top 2
            var topProducts = products.OrderByDescending(p => p.Price).Take(2).ToList();
            Console.WriteLine("Top 2 Expensive Products:");
            topProducts.ForEach(p => Console.WriteLine($"{p.Title} - {p.Price}"));

            // ================== Filtering ==================
            // TASK: Filter users from NY state
            var nyUsers = users.Where(u => u.State == "NY").ToList();
            Console.WriteLine("NY Users:");
            nyUsers.ForEach(u => Console.WriteLine(u.Name));

            // ================== Mapping ==================
            // TASK: Map users to simplified objects (name + Id)
            var simpleUsers = users.Select(u => new { u.Name, u.Id }).ToList();
            Console.WriteLine("Simplified Users:");
            simpleUsers.ForEach(u => Console.WriteLine($"{u.Id} - {u.Name}"));

            // ================== Grouping ==================
            // TASK: Group users by single key (State) using method syntax
            var usersByState = users.GroupBy(u => u.State);
            foreach (var group in usersByState)
            {
            Console.WriteLine($"State: {group.Key}");
            foreach (var u in group)
            Console.WriteLine($" - {u.Name}");
            }

            // TASK: Group users by multiple keys (State + Country) using query syntax
            var usersByStateCountry =
            from u in users
            group u by new { u.State, u.Country } into g
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
                from u in users
                join p in products on u.Id equals p.Id
                select new { u.Name, Product = p.Title };
            Console.WriteLine("Inner Join (User ID = Product ID):");
            foreach (var up in userProductJoin)
            Console.WriteLine($"{up.Name} - {up.Product}");

            // TASK: Left join users + products (users may not have matching product)
            var leftJoin =
                from u in users
                    join p in products on u.Id equals p.Id 
                        // left part
                        into upGroup
                        from p in upGroup.DefaultIfEmpty()
                select new { u.Name, Product = p?.Title ?? "No Product" };
            
            Console.WriteLine("Left Join (User -> Product):");
            foreach (var up in leftJoin)
            Console.WriteLine($"{up.Name} - {up.Product}");

            // ================== Async / Task ==================
            // TASK: Implement async method to fetch all products
            async Task<List<ProductLive>> GetAllProductsAsync()
            {
                await Task.Delay(500); // simulate async network delay
                return products;
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
            var matchingIds = products.Where(p => users.Any(u => u.Id == p.Id)).ToList();
            Console.WriteLine("Products matching user IDs:");
            matchingIds.ForEach(p => Console.WriteLine($"{p.Title} - ID {p.Id}"));

            // TASK: Map + filter users in CA state
            var mappedFilteredUsers = users
            .Where(u => u.State == "CA")
            .Select(u => new { u.Name, u.Country })
            .ToList();
            Console.WriteLine("Mapped + Filtered Users (CA):");
            mappedFilteredUsers.ForEach(u => Console.WriteLine($"{u.Name} - {u.Country}"));

            // ================== Utility / Placeholder ==================
            // TASK: Demonstrate readonly / record usage
            var ru = new ReadonlyUser(1, "Alice", "NY");
            // ru.Name = "Bob"; // ❌ compilation error (readonly simulation)
            
            
            
            // inner join 
            var innerJoin = from u in users
                join p in products on u.Id equals p.Id
                select new {user = u.Name, product = p.Title};

            // left join
            var lefJOin = from u in users
                join p in products on u.Id equals p.Id
                    into g
                from p in g.DefaultIfEmpty()
                select new {user = u.Name, title = p.Title};
        }

        // Top-level record declaration inside namespace/class scope
        public record ReadonlyUser(int Id, string Name, string State);
    }
}
