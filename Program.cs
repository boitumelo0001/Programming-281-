using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IManageable
{
    string GetDetails();
}

namespace AdministrationOptimization
{
    public class Activity : IManageable
    {
        private static int nextActivityId = 1;
        public int ActivityId { get; private set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public Activity()
        {
            ActivityId = nextActivityId++;
        }

        public string GetDetails()
        {
            return $"Activity ID: {ActivityId}, Description: {Description}, Date: {Date}";
        }
    }

    public class Expense : IManageable
    {
        private static int nextExpenseId = 1;
        public int ExpenseId { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public Expense()
        {
            ExpenseId = nextExpenseId++;
        }

        public string GetDetails()
        {
            return $"Expense ID: {ExpenseId}, Name: {Name}, Description: {Description}, Amount: R{Amount}, Date: {Date}, Due Date: {DueDate}";
        }
    }

    public class Invoice : IManageable
    {
        private static int nextInvoiceId = 1;
        public int InvoiceId { get; private set; }
        public string Client { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime DueDate { get; set; }

        public Invoice()
        {
            InvoiceId = nextInvoiceId++;
        }

        public string GetDetails()
        {
            return $"Invoice ID: {InvoiceId}, Client: {Client}, Description: {Description}, Amount: R{Amount}, Due Date: {DueDate}";
        }
    }

    public class AdministrationManager
    {
        private List<Activity> activities = new List<Activity>();
        private List<Expense> expenses = new List<Expense>();
        private List<Invoice> invoices = new List<Invoice>();

        // Define events
        public event Action<Activity> ActivityTracked;
        public event Action<Expense> ExpenseAdded;
        public event Action<Invoice> InvoiceCreated;

        public void TrackActivity(Activity activity)
        {
            activities.Add(activity);
            ActivityTracked?.Invoke(activity);
            Console.WriteLine("Activity has been tracked successfully.");
        }

        public void AddExpense(Expense expense)
        {
            expenses.Add(expense);
            ExpenseAdded?.Invoke(expense);
            Console.WriteLine("Expense added successfully.");
        }

        public void CreateInvoice(Invoice invoice)
        {
            invoices.Add(invoice);
            InvoiceCreated?.Invoke(invoice);
            Console.WriteLine("Invoice created successfully.");
        }

        public void GenerateReports()
        {
            Console.WriteLine("Generating reports...");
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("\nActivities Report:");
                foreach (var activity in activities)
                {
                    Console.WriteLine(activity.GetDetails());
                }

                Console.WriteLine("\nExpenses Report:");
                foreach (var expense in expenses)
                {
                    Console.WriteLine(expense.GetDetails());
                }

                Console.WriteLine("\nInvoices Report:");
                foreach (var invoice in invoices)
                {
                    Console.WriteLine(invoice.GetDetails());
                }
            });
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AdministrationManager manager = new AdministrationManager();
            manager.ActivityTracked += (activity) => Console.WriteLine($"Event: Activity tracked - {activity.GetDetails()}");
            manager.ExpenseAdded += (expense) => Console.WriteLine($"Event: Expense added - {expense.GetDetails()}");
            manager.InvoiceCreated += (invoice) => Console.WriteLine($"Event: Invoice created - {invoice.GetDetails()}");

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Administration Optimization ---");
                Console.WriteLine("1. Track Activity");
                Console.WriteLine("2. Add Expense");
                Console.WriteLine("3. Create Invoice");
                Console.WriteLine("4. Generate Reports");
                Console.WriteLine("5. Exit");

                Console.Write("Select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Enter Description: ");
                        string activityDescription = Console.ReadLine();
                        manager.TrackActivity(new Activity { Description = activityDescription, Date = DateTime.Now });
                        break;

                    case "2":
                        Console.Write("Enter Name: ");
                        string expenseName = Console.ReadLine();
                        Console.Write("Enter Description: ");
                        string expenseDescription = Console.ReadLine();
                        Console.Write("Enter Amount: ");
                        if (double.TryParse(Console.ReadLine(), out double expenseAmount))
                        {
                            Console.Write("Enter Due Date (yyyy-mm-dd): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime expenseDueDate))
                            {
                                manager.AddExpense(new Expense
                                {
                                    Name = expenseName,
                                    Description = expenseDescription,
                                    Amount = expenseAmount,
                                    Date = DateTime.Now,
                                    DueDate = expenseDueDate
                                });
                            }
                            else
                            {
                                Console.WriteLine("Invalid Due Date. Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Amount. Please try again.");
                        }
                        break;

                    case "3":
                        Console.Write("Enter Client Name: ");
                        string client = Console.ReadLine();
                        Console.Write("Enter Description: ");
                        string invoiceDescription = Console.ReadLine();
                        Console.Write("Enter Amount: ");
                        if (double.TryParse(Console.ReadLine(), out double invoiceAmount))
                        {
                            Console.Write("Enter Due Date (yyyy-mm-dd): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime invoiceDueDate))
                            {
                                manager.CreateInvoice(new Invoice
                                {
                                    Client = client,
                                    Description = invoiceDescription,
                                    Amount = invoiceAmount,
                                    DueDate = invoiceDueDate
                                });
                            }
                            else
                            {
                                Console.WriteLine("Invalid Due Date. Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Amount. Please try again.");
                        }
                        break;

                    case "4":
                        manager.GenerateReports();
                        break;

                    case "5":
                        Console.WriteLine("Exiting program...");
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}