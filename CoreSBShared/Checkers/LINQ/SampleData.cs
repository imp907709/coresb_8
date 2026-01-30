
namespace CoreSBShared.Checkers.LINQ
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
    
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
    }
    
    public class Order
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
    }

    public class Customer
    {
        public int ExternalId { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
    }
    
    public static class SampleData
    {
        public static List<Order> Orders = new List<Order>
        {
            new Order { Id = 1, CustomerId = 101, Amount = 150.5m, Status = "Open" },
            new Order { Id = 2, CustomerId = 102, Amount = null, Status = "Closed" },
            new Order { Id = 3, CustomerId = 101, Amount = 75m, Status = null },
            new Order { Id = 4, CustomerId = 103, Amount = 200m, Status = "Open" },
            new Order { Id = 5, CustomerId = null, Amount = 50m, Status = "Open" },
            new Order { Id = 6, CustomerId = 104, Amount = 120m, Status = "Closed" },
            new Order { Id = 7, CustomerId = 102, Amount = null, Status = null },
            new Order { Id = 8, CustomerId = 101, Amount = 300m, Status = "Open" },
            new Order { Id = 9, CustomerId = 105, Amount = 80m, Status = "Closed" }
        };

        public static List<Customer> Customers = new List<Customer>
        {
            new Customer { ExternalId = 101, Name = "Alice", Region = "East" },
            new Customer { ExternalId = 102, Name = "Bob", Region = "West" },
            new Customer { ExternalId = 103, Name = "Charlie", Region = null },
            new Customer { ExternalId = 104, Name = "Diana", Region = "North" },
            new Customer { ExternalId = 105, Name = "Evan", Region = "South" }
        };
        
        // Example data
        public static List<Employee>  employees = new List<Employee> {
            new Employee { Id = 1, Name = "Alice", Department = "HR", Salary = 55000 },
            new Employee { Id = 2, Name = "Bob", Department = "IT", Salary = 75000 },
            new Employee { Id = 3, Name = "Charlie", Department = "IT", Salary = 80000 },
            new Employee { Id = 4, Name = "Diana", Department = "Finance", Salary = 65000 },
            new Employee { Id = 5, Name = "Evan", Department = "Finance", Salary = 70000 },
            new Employee { Id = 6, Name = "Fiona", Department = "HR", Salary = 60000 },
            new Employee { Id = 7, Name = "George", Department = "IT", Salary = 72000 },
            new Employee { Id = 8, Name = "Hannah", Department = "Marketing", Salary = 50000 },
            new Employee { Id = 9, Name = "Ian", Department = "Marketing", Salary = 52000 },
            new Employee { Id = 10, Name = "Jane", Department = "Finance", Salary = 68000 }
        };
        
        // ================== Sample Data ==================
        public static List<UserLive> users = new List<UserLive>
        {
            new() { Id = 1, Name = "Alice", State = "NY", Country = "USA" },
            new() { Id = 2, Name = "Bob", State = "CA", Country = "USA" },
            new() { Id = 3, Name = "Charlie", State = "NY", Country = "USA" },
            new() { Id = 4, Name = "Diana", State = "TX", Country = "USA" },
            new() { Id = 5, Name = "Eve", State = "CA", Country = "USA" }
        };

        public static List<ProductLive>  products = new List<ProductLive>
        {
            new() { Id = 1, Title = "Laptop", Price = 1200 },
            new() { Id = 2, Title = "Phone", Price = 800 },
            new() { Id = 3, Title = "Book", Price = 20 },
            new() { Id = 4, Title = "Tablet", Price = 500 },
            new() { Id = 5, Title = "Headphones", Price = 150 }
        };
    }
    
  
}
