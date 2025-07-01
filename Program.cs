using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new LibraryContext())
            {
               

                while (true)
                {
                    Console.WriteLine("\nLibrary Management System:");
                    Console.WriteLine("1. Add Resource");
                    Console.WriteLine("2. View All Resources");
                    Console.WriteLine("3. Borrow Resource");
                    Console.WriteLine("4. Return Resource");
                    Console.WriteLine("5. Search by Title/Author");
                    Console.WriteLine("6. Overdue Report");
                    Console.WriteLine("7. Resources by Category");
                    Console.WriteLine("0. Exit");
                    Console.Write("Choose an option: ");
                    var input = Console.ReadLine();

                    switch (input)
                    {
                        case "1": AddResource(db); break;
                        case "2": ViewAll(db); break;
                        case "3": BorrowResource(db); break;
                        case "4": ReturnResource(db); break;
                        case "5": Search(db); break;
                        case "6": OverdueReport(db); break;
                        case "7": ReportByCategory(db); break;
                        case "0": return;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
            }
        }

        static void AddResource(LibraryContext db)
        {
            var res = new Resource();

            Console.Write("Title: ");
            res.Title = Console.ReadLine();

            Console.Write("Author: ");
            res.Author = Console.ReadLine();

            Console.Write("Publication Year: ");
            if (int.TryParse(Console.ReadLine(), out int year))
                res.PublicationYear = year;
            else
                res.PublicationYear = DateTime.Now.Year;

            Console.Write("Category: ");
            res.Category = Console.ReadLine();

            res.IsAvailable = true;
            res.DueDate = null;

            db.Resources.Add(res);
            db.SaveChanges();
            Console.WriteLine("Resource added.");
        }

        static void ViewAll(LibraryContext db)
        {
            var list = db.Resources.ToList();

            if (list.Count == 0)
            {
                Console.WriteLine("No resources found.");
                return;
            }

            foreach (var r in list)
            {
                Console.WriteLine($"{r.Id}. {r.Title} - {r.Author} ({r.PublicationYear}) | {r.Category} | Available: {r.IsAvailable}");
            }
        }

        static void BorrowResource(LibraryContext db)
        {
            Console.Write("Enter ID to borrow: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            var res = db.Resources.Find(id);
            if (res == null || !res.IsAvailable)
            {
                Console.WriteLine("Resource not available.");
                return;
            }

            res.IsAvailable = false;
            res.DueDate = DateTime.Now.AddDays(14);
            db.SaveChanges();
            Console.WriteLine($"Borrowed. Due date: {res.DueDate?.ToShortDateString()}");
        }

        static void ReturnResource(LibraryContext db)
        {
            Console.Write("Enter ID to return: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            var res = db.Resources.Find(id);
            if (res == null || res.IsAvailable)
            {
                Console.WriteLine("Resource not borrowed.");
                return;
            }

            res.IsAvailable = true;
            res.DueDate = null;
            db.SaveChanges();
            Console.WriteLine("Returned successfully.");
        }

        static void Search(LibraryContext db)
        {
            Console.Write("Enter keyword: ");
            string keyword = Console.ReadLine()?.ToLower() ?? "";

            var results = db.Resources
                .Where(r =>
                    (!string.IsNullOrEmpty(r.Title) && r.Title.ToLower().Contains(keyword)) ||
                    (!string.IsNullOrEmpty(r.Author) && r.Author.ToLower().Contains(keyword)))
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No matches found.");
                return;
            }

            foreach (var r in results)
            {
                Console.WriteLine($"{r.Id}. {r.Title} - {r.Author}");
            }
        }

        static void OverdueReport(LibraryContext db)
        {
            var overdue = db.Resources
                .Where(r => r.DueDate.HasValue && r.DueDate.Value < DateTime.Now && !r.IsAvailable)
                .ToList();

            if (overdue.Count == 0)
            {
                Console.WriteLine("No overdue items.");
                return;
            }

            foreach (var r in overdue)
            {
                Console.WriteLine($"Overdue: {r.Title} (Due: {r.DueDate?.ToShortDateString()})");
            }
        }

        static void ReportByCategory(LibraryContext db)
        {
            Console.Write("Enter category: ");
            string category = Console.ReadLine()?.ToLower() ?? "";

            var results = db.Resources
                .Where(r => !string.IsNullOrEmpty(r.Category) && r.Category.ToLower() == category)
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No resources found for this category.");
                return;
            }

            foreach (var r in results)
            {
                Console.WriteLine($"{r.Id}. {r.Title} - {r.Author} ({r.PublicationYear})");
            }
        }
    }
}
