using System;
using System.Linq;
using NLog;
using NorthwindApp.Models;
using NorthwindApp.Repositories;

namespace NorthwindApp.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository = new();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void DisplayAllProductsWithFilter()
        {
            try
            {
                Console.WriteLine("\n--- Display Products ---");
                Console.WriteLine("1. All Products");
                Console.WriteLine("2. Discontinued Products");
                Console.WriteLine("3. Active Products");
                Console.WriteLine("0. Cancel");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                // Filter decision tree
                IEnumerable<Product> productsToDisplay = null;

                switch (choice)
                {
                    case "1": // All Products
                        productsToDisplay = _repository.GetAllProducts();
                        break;

                    case "2": // Discontinued Products
                        productsToDisplay = _repository.GetAllProducts().Where(p => p.Discontinued);
                        break;

                    case "3": // Active Products
                        productsToDisplay = _repository.GetAllProducts().Where(p => !p.Discontinued);
                        break;

                    case "0": // Cancel
                        Console.WriteLine("Cancelled.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Returning to the main menu.");
                        return;
                }

                if (productsToDisplay == null || !productsToDisplay.Any())
                {
                    Console.WriteLine("No products found.");
                    return;
                }

                // Display the filtered products
                Console.WriteLine("\n--- Products List ---");
                foreach (var product in productsToDisplay)
                {
                    string status = product.Discontinued ? "(Discontinued)" : "(Active)";
                    Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName} {status}");
                }
                Console.WriteLine("----------------------");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error displaying products");
                Console.WriteLine("An error occurred while displaying products.");
            }
        }


        public void AddProduct()
        {
            try
            {
                var product = new Product
                {
                    ProductName = Prompt("Enter Product Name:"),
                    CategoryId = int.Parse(Prompt("Enter Category ID:")),
                    UnitPrice = decimal.Parse(Prompt("Enter Unit Price:")),
                    UnitsInStock = short.Parse(Prompt("Enter Units in Stock:")),
                    Discontinued = Prompt("Is Discontinued? (yes/no):").ToLower() == "yes"
                };

                _repository.AddProduct(product);
                Logger.Info($"Product added: {product.ProductName}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error adding product");
                Console.WriteLine("An error occurred.");
            }
        }

        public void EditProduct()
        {
            try
            {
                // Display all products with their IDs
                Console.WriteLine("\n--- Products List ---");
                var allProducts = _repository.GetAllProducts(); // Renamed from 'products'
                if (allProducts == null || !allProducts.Any())
                {
                    Console.WriteLine("No products available to edit.");
                    return;
                }

                foreach (var p in allProducts) // Renamed 'product' to 'p' in loop
                {
                    string status = p.Discontinued ? "(Discontinued)" : "(Active)";
                    Console.WriteLine($"ID: {p.ProductId}, Name: {p.ProductName} {status}");
                }
                Console.WriteLine("----------------------\n");

                Console.Write("Enter the Product ID to edit: ");
                if (!int.TryParse(Console.ReadLine(), out int productId))
                {
                    Console.WriteLine("Invalid Product ID.");
                    return;
                }

                // Fetch the selected product
                var selectedProduct = _repository.GetProductById(productId); // Renamed from 'product'
                if (selectedProduct == null)
                {
                    Console.WriteLine($"No product found with ID {productId}.");
                    return;
                }

                Console.WriteLine($"\nEditing Product: {selectedProduct.ProductName}");
                Console.WriteLine("Select the field to edit by entering the corresponding number:");
                Console.WriteLine("1. Product Name");
                Console.WriteLine("2. Category ID");
                Console.WriteLine("3. Unit Price");
                Console.WriteLine("4. Units in Stock");
                Console.WriteLine("5. Discontinued Status");
                Console.WriteLine("0. Save and Exit");

                bool editing = true;

                while (editing)
                {
                    Console.Write("\nEnter your choice: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": // Edit Product Name
                            Console.Write("Enter new Product Name (current: " + selectedProduct.ProductName + "): ");
                            string newName = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newName))
                            {
                                selectedProduct.ProductName = newName;
                                Console.WriteLine("Product Name updated.");
                            }
                            else
                            {
                                Console.WriteLine("No changes made to Product Name.");
                            }
                            break;

                        case "2": // Edit Category ID
                            Console.Write("Enter new Category ID (current: " + selectedProduct.CategoryId + "): ");
                            string newCategory = Console.ReadLine();
                            if (int.TryParse(newCategory, out int categoryId))
                            {
                                selectedProduct.CategoryId = categoryId;
                                Console.WriteLine("Category ID updated.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. No changes made to Category ID.");
                            }
                            break;

                        case "3": // Edit Unit Price
                            Console.Write("Enter new Unit Price (current: " + selectedProduct.UnitPrice + "): ");
                            string newPrice = Console.ReadLine();
                            if (decimal.TryParse(newPrice, out decimal unitPrice))
                            {
                                selectedProduct.UnitPrice = unitPrice;
                                Console.WriteLine("Unit Price updated.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. No changes made to Unit Price.");
                            }
                            break;

                        case "4": // Edit Units in Stock
                            Console.Write("Enter new Units in Stock (current: " + selectedProduct.UnitsInStock + "): ");
                            string newStock = Console.ReadLine();
                            if (short.TryParse(newStock, out short unitsInStock))
                            {
                                selectedProduct.UnitsInStock = unitsInStock;
                                Console.WriteLine("Units in Stock updated.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. No changes made to Units in Stock.");
                            }
                            break;

                        case "5": // Edit Discontinued Status
                            Console.Write("Is the product discontinued? (yes/no, current: " + (selectedProduct.Discontinued ? "yes" : "no") + "): ");
                            string newDiscontinued = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newDiscontinued))
                            {
                                selectedProduct.Discontinued = newDiscontinued.Trim().ToLower() == "yes";
                                Console.WriteLine("Discontinued Status updated.");
                            }
                            else
                            {
                                Console.WriteLine("No changes made to Discontinued Status.");
                            }
                            break;

                        case "0": // Save and Exit
                            editing = false;
                            break;

                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }
                }

                // Update product in repository
                _repository.UpdateProduct(selectedProduct);
                Console.WriteLine("\nProduct updated successfully!");
                Logger.Info($"Product edited: {selectedProduct.ProductName}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error editing product");
                Console.WriteLine("An error occurred while updating the product.");
            }
        }

        public void DisplaySpecificProduct()
        {
            try
            {
                // Prompt user for the Product ID
                Console.Write("\nEnter the Product ID to view details: ");
                if (!int.TryParse(Console.ReadLine(), out int productId))
                {
                    Console.WriteLine("Invalid Product ID.");
                    return;
                }

                // Fetch the product
                var product = _repository.GetProductById(productId);
                if (product == null)
                {
                    Console.WriteLine($"No product found with ID {productId}.");
                    return;
                }

                // Display product details
                Console.WriteLine("\n--- Product Details ---");
                Console.WriteLine($"ID: {product.ProductId}");
                Console.WriteLine($"Name: {product.ProductName}");
                Console.WriteLine($"Category ID: {product.CategoryId}");
                Console.WriteLine($"Supplier ID: {product.SupplierId}");
                Console.WriteLine($"Quantity Per Unit: {product.QuantityPerUnit}");
                Console.WriteLine($"Unit Price: {product.UnitPrice:C}");
                Console.WriteLine($"Units in Stock: {product.UnitsInStock}");
                Console.WriteLine($"Units on Order: {product.UnitsOnOrder}");
                Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
                Console.WriteLine($"Discontinued: {(product.Discontinued ? "Yes" : "No")}");
                Console.WriteLine("------------------------");
                Logger.Info($"Displayed details for Product ID: {productId}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error displaying specific product");
                Console.WriteLine("An error occurred while retrieving the product.");
            }
        }


        private string Prompt(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
    }
}
