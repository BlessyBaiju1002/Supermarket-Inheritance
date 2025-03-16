using System;
using System.Collections.Generic;

// Base class for all supermarket products
class Product
{
    public string SKU { get; set; }  
    public string Brand { get; set; }  
    public string ProductName { get; set; }  
    public int Size { get; set; }  // Size in grams
    public DateTime DateStocked { get; set; } 
    public int ShelfLife { get; set; }  
    public double BaseRetailPrice { get; set; }  // Original price

    // Constructor to initialize common product details
    public Product(string sku, string brand, string productName, int size, DateTime dateStocked, int shelfLife, double basePrice)
    {
        SKU = sku;
        Brand = brand;
        ProductName = productName;
        Size = size;
        DateStocked = dateStocked;
        ShelfLife = shelfLife;
        BaseRetailPrice = basePrice;
    }

    // Method to calculate discount - Child classes will override this
    public virtual double GetDiscountedPrice()
    {
        return BaseRetailPrice;
    }

    // Displaying all the product details
    public override string ToString()
    {
        return $"SKU: {SKU}, Name: {ProductName}, Brand: {Brand}, Price: ${BaseRetailPrice:F2}";
    }
}

// Dairy subclass - Milk, Cheese, Yogurt, etc.
class Dairy : Product
{
    public bool LactoseFree { get; set; }  // Is this product lactose-free?

    public Dairy(string sku, string brand, string productName, int size, DateTime dateStocked, int shelfLife, double basePrice, bool lactoseFree)
        : base(sku, brand, productName, size, dateStocked, shelfLife, basePrice)
    {
        LactoseFree = lactoseFree;
    }

    // Dairy products get 50% off if they expire in 5 days or less
    public override double GetDiscountedPrice()
    {
        int daysLeft = (DateStocked.AddDays(ShelfLife) - DateTime.Today).Days;
        return daysLeft <= 5 ? Math.Round(BaseRetailPrice * 0.5, 2) : BaseRetailPrice;
    }
}

// Produce subclass - Fruits and Vegetables
class Produce : Product
{
    public string SoldBy { get; set; }  // Sold by unit, weight, or package
    public string Category { get; set; }  // Fruit or Vegetable

    public Produce(string sku, string brand, string productName, int size, DateTime dateStocked, int shelfLife, double basePrice, string soldBy, string category)
        : base(sku, brand, productName, size, dateStocked, shelfLife, basePrice)
    {
        SoldBy = soldBy;
        Category = category;
    }

    // Produce gets 50% off if expiring in <5 days, 20% off if 5-10 days left
    public override double GetDiscountedPrice()
    {
        int daysLeft = (DateStocked.AddDays(ShelfLife) - DateTime.Today).Days;
        if (daysLeft < 5)
            return Math.Round(BaseRetailPrice * 0.5, 2);
        if (daysLeft <= 10)
            return Math.Round(BaseRetailPrice * 0.8, 2);
        return BaseRetailPrice;
    }
}

// Cereal subclass - Packaged breakfast cereals
class Cereal : Product
{
    public double SugarPercentage { get; set; }  // Sugar content in % of daily intake

    public Cereal(string sku, string brand, string productName, int size, DateTime dateStocked, int shelfLife, double basePrice, double sugarPercentage)
        : base(sku, brand, productName, size, dateStocked, shelfLife, basePrice)
    {
        SugarPercentage = sugarPercentage;
    }

    // Cereal gets 50% off if it's expired
    public override double GetDiscountedPrice()
    {
        return DateTime.Today > DateStocked.AddDays(ShelfLife) ? Math.Round(BaseRetailPrice * 0.5, 2) : BaseRetailPrice;
    }
}

class Program
{
    static void Main()
    {
        // Creating some sample products with realistic stock dates
        Dairy milk = new Dairy("D001", "BrandA", "Milk", 1000, DateTime.Today.AddDays(-5), 10, 5.99, false);
        Produce apple = new Produce("P001", "BrandB", "Apple", 500, DateTime.Today.AddDays(-3), 7, 2.99, "Weight", "Fruit");
        Cereal cornFlakes = new Cereal("C001", "BrandC", "Corn Flakes", 750, DateTime.Today.AddDays(-12), 30, 4.99, 10);

        // Store all products in a list for easy management
        List<Product> inventory = new List<Product> { milk, apple, cornFlakes };

        // Displaying all products and their discounted prices
        Console.WriteLine("Supermarket Inventory:\n");
        foreach (var product in inventory)
        {
            Console.WriteLine(product);
            Console.WriteLine($"Discounted Price: ${product.GetDiscountedPrice():F2}\n");
        }
    }
}
