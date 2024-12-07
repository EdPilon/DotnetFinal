using System;
using NorthwindApp.Services;

namespace NorthwindApp
{
    class Program
    {
        static void Main()
        {
            var productService = new ProductService();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Display All Products");
                Console.WriteLine("2. Add a Product");
                Console.WriteLine("3. Edit a Product");
                Console.WriteLine("4. Display a Specific Product");
                Console.WriteLine("0. Exit");

                switch (Console.ReadLine())
                {
                    case "1":
                        productService.DisplayAllProductsWithFilter(); // Updated method
                        break;
                    case "2":
                        productService.AddProduct();
                        break;
                    case "3":
                        productService.EditProduct();
                        break;
                    case "4":
                        productService.DisplaySpecificProduct();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }


        }
    }
}
