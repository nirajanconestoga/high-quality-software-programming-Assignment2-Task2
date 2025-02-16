using System.Text.RegularExpressions;

namespace Wiring
{
    // 1.Interface defining the basic structure for a customer
    public interface ICustomer
    {
        string Name { get; }
        int Size { get; }
        int LightBulbs { get; }
        int Outlets { get; }
        void CreateWiringSchema();
        void PurchaseNecessaryParts();
        void CustomerInfo();
    }

    // 2.Abstract base class implementing the ICustomer interface
    public abstract class CustomerBase : ICustomer
    {
        public string Name { get; set; }
        public string BuildingType { get; set; }
        public int Size { get; set; }
        public int LightBulbs { get; set; }
        public int Outlets { get; set; }
        public string CreditCard { get; set; }
        // Constructor to initialize customer properties
        public CustomerBase(string name, string buildingType, int size, int lightBulbs, int outlets, string creditCard)
        {
            Name = name;
            BuildingType = buildingType;
            Size = size;
            LightBulbs = lightBulbs;
            Outlets = outlets;
            CreditCard = creditCard;
        }
        // Method to create a wiring schema
        public void CreateWiringSchema()
        {
            Console.WriteLine($"Creating wiring schema for {Name}'s {BuildingType}.");
        }
        // Method to purchase required parts
        public void PurchaseNecessaryParts()
        {
            Console.WriteLine($"Purchasing parts for {Name}'s {BuildingType}.");
        }
        // Abstract method for building specific task, implemented by derived classes
        public abstract void BuildingSpecificTask();

        // Method to mask credit card number for security
        public string MaskCreditCard()
        {
            return CreditCard.Substring(0, 4) + " XXXX XXXX " + CreditCard.Substring(12, 4);
        }
        // Method to display customer information
        public void CustomerInfo()
        {
            Console.WriteLine($"{Name}, {BuildingType}, {Size} sq.ft, {LightBulbs} bulbs, {Outlets} outlets, CC: {MaskCreditCard()}");
        }
    }
    // Derived class for House customer
    public class HouseCustomer : CustomerBase
    {
        public HouseCustomer(string name, int size, int lightBulbs, int outlets, string creditCard)
            : base(name, "House", size, lightBulbs, outlets, creditCard) { }

        // 3. Overriding abstract method to define special task
        public override void BuildingSpecificTask()
        {
            Console.WriteLine("Installing fire alarms.");
        }
    }

    // Derived class for Barn customer
    public class BarnCustomer : CustomerBase
    {
        public BarnCustomer(string name, int size, int lightBulbs, int outlets, string creditCard)
            : base(name, "Barn", size, lightBulbs, outlets, creditCard) { }

        // 3. Overriding abstract method to define special task
        public override void BuildingSpecificTask()
        {
            Console.WriteLine("Wiring milking equipment.");
        }
    }
    // Derived class for Garage customer
    public class GarageCustomer : CustomerBase
    {
        public GarageCustomer(string name, int size, int lightBulbs, int outlets, string creditCard)
            : base(name, "Garage", size, lightBulbs, outlets, creditCard) { }

        // 3. Overriding abstract method to define special task
        public override void BuildingSpecificTask()
        {
            Console.WriteLine("Installing automatic doors.");
        }
    }
    // 5. Delegate to process wiring tasks
    public delegate void WiringTask(CustomerBase customer);
    public class Program
    {
        static void Main()
        {
            List<CustomerBase> customers = new List<CustomerBase>();
            bool firstCustomerEntered = false;

            while (true)
            {
                string customerName;
                do
                {
                    Console.Write("Enter customer name (or 'done' to finish): ");
                    customerName = Console.ReadLine().Trim();
                    if (string.IsNullOrEmpty(customerName) || Regex.IsMatch(customerName, "^\\d+$"))
                    {
                        Console.WriteLine("Invalid name. Please enter a valid name containing letters.");
                    }
                    else if (customerName.ToLower() == "done" && !firstCustomerEntered)
                    {
                        Console.WriteLine("You must enter at least one customer before finishing.");
                        customerName = "";
                    }
                    else if (customerName.ToLower() == "done")
                    {
                        break;
                    }
                } while (string.IsNullOrEmpty(customerName) || Regex.IsMatch(customerName, "^\\d+$"));

                if (customerName.ToLower() == "done") break;
                firstCustomerEntered = true;

                // Collecting and validating customer details
                string buildingType;
                do
                {
                    Console.Write("Enter building type (house, barn, garage): ");
                    buildingType = Console.ReadLine().Trim().ToLower();
                } while (buildingType != "house" && buildingType != "barn" && buildingType != "garage");

                int buildingSize;
                do
                {
                    Console.Write("Enter size (1000 - 50000 sq.ft): ");
                } while (!int.TryParse(Console.ReadLine(), out buildingSize) || buildingSize < 1000 || buildingSize > 50000);

                int lightBulbCount;
                do
                {
                    Console.Write("Enter number of light bulbs (max 20): ");
                } while (!int.TryParse(Console.ReadLine(), out lightBulbCount) || lightBulbCount < 0 || lightBulbCount > 20);

                int outletCount;
                do
                {
                    Console.Write("Enter number of outlets (max 50): ");
                } while (!int.TryParse(Console.ReadLine(), out outletCount) || outletCount < 0 || outletCount > 50);

                string creditCardNumber;
                do
                {
                    Console.Write("Enter credit card number (16-digit): ");
                    creditCardNumber = Console.ReadLine();
                } while (creditCardNumber.Length != 16 || !long.TryParse(creditCardNumber, out _));

                // Creating the appropriate customer type
                CustomerBase customer = buildingType switch
                {
                    "house" => new HouseCustomer(customerName, buildingSize, lightBulbCount, outletCount, creditCardNumber),
                    "barn" => new BarnCustomer(customerName, buildingSize, lightBulbCount, outletCount, creditCardNumber),
                    "garage" => new GarageCustomer(customerName, buildingSize, lightBulbCount, outletCount, creditCardNumber),
                };
                customers.Add(customer);
            }

            // 5. Using delegate to process wiring tasks
            WiringTask task = ProcessWiring;
            foreach (var customer in customers)
            {
                task(customer);
            }
        }
        static void ProcessWiring(CustomerBase customer)
        {
            Console.WriteLine("\nCustomer Details:");
            customer.CreateWiringSchema();
            customer.PurchaseNecessaryParts();
            customer.BuildingSpecificTask();
            customer.CustomerInfo();
        }
    }
}

