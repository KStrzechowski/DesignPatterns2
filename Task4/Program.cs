﻿using System.Linq;
using System;
using System.Collections.Generic;
using OrderProcessing.Databases;
using OrderProcessing.Orders;
using OrderProcessing.Payments;
using OrderProcessing.Shipment;

namespace OrderProcessing
{
    class Program
    {
        public static IOrdersDatabaseIterator GetOrdersIterator(LocalOrdersDB localOrdersDB) =>
            new LocalOrdersDBIterator(localOrdersDB);
        public static IOrdersDatabaseIterator GetOrdersIterator(GlobalOrdersDB globalOrdersDB) =>
            new GlobalOrdersDBIterator(globalOrdersDB);
        public static IOrdersDatabaseIterator Filter(LocalOrdersDB localOrdersDB) =>
            new FilterIterator(GetOrdersIterator(localOrdersDB), new FilterReadyForShipment());
        public static IOrdersDatabaseIterator Filter(GlobalOrdersDB globalOrdersDB) =>
            new FilterIterator(GetOrdersIterator(globalOrdersDB), new FilterReadyForShipment());

        public static IPaymentHandler PreparePayments()
        {
            var paypal = new PayPalPaymentHandler();
            var invoice = new InvoicePaymentHandler();
            var creditCard = new CreditCardPaymentHandler();
            paypal.SetNext(invoice).SetNext(creditCard);
            return paypal;
        }

        public static IShipmentHandler PrepareShipments()
        {
            var localShipment = new LocalPostShipmentHandler();
            var globalShipment = new GlobalShipmentHandler();
            localShipment.SetNext(globalShipment);
            return localShipment;
        }

        public static void PayForEveryOrder(IOrdersDatabaseIterator iterator)
        {
            var payments = PreparePayments();
            var order = iterator.Next();
            while (order != null)
            {
                Console.WriteLine($"Processing order {order.OrderId} with total amount: {order.DueAmount}");
                payments.Handle(order);
                if (order.DueAmount <= 0)
                {
                    order.Status = OrderStatus.ReadyForShipment;
                    Console.WriteLine($"Order {order.OrderId} is ready for shipment");
                }
                else
                {
                    Console.WriteLine($"Order {order.OrderId} has insufficient paid amount " +
                        $"{order.AmountToBePaid - order.DueAmount}");
                }
                Console.WriteLine();
                order = iterator.Next();
            }
        }

        public static void RegisterShipments(IOrdersDatabaseIterator iterator, List<IShipmentProvider> shipmentProviders, TaxRatesDB taxRatesDB)
        {
            var Shipments = PrepareShipments();
            var taxRateProvider = new TaxRateProvider(taxRatesDB);
            var order = iterator.Next();
            while (order != null)
            {
                var provider = Shipments.Handle(order, taxRateProvider);
                if (!shipmentProviders.Any(x => x.Name == provider.Name))
                {
                    shipmentProviders.Add(provider);
                }
                provider.RegisterForShipment(order);

                var label = provider.GetLabelForOrder(order);
                Printer.PrintLabel(label);

                order = iterator.Next();
            }
        }

        static void Main(string[] args)
        {
            var localOrdersDB = ProvideLocalOrders();
            var globalOrdersDB = ProvideGlobalOrders();
            var taxesDB = ProvideTaxRates();

            Console.WriteLine("-------------PAYMENT_PROCESSING----------------------");
            var iterator = GetOrdersIterator(localOrdersDB);
            PayForEveryOrder(iterator);
            iterator = GetOrdersIterator(globalOrdersDB);
            PayForEveryOrder(iterator);

            Console.WriteLine("-------------SHIPMENT_PROCESSING----------------------");
            Console.WriteLine("Register orders and print order labels");
            Console.WriteLine("--------------------------------------------------------");

            var usedProviders = new List<IShipmentProvider>();
            iterator = Filter(localOrdersDB);
            RegisterShipments(iterator, usedProviders, taxesDB);
            iterator = Filter(globalOrdersDB);
            RegisterShipments(iterator, usedProviders, taxesDB);

            Console.WriteLine("Print parcels summaries with printable labels");
            foreach (var provider in usedProviders)
            {
                var parcels = provider.GetParcels();
                foreach (var parcel in parcels)
                {
                    Printer.PrintParcel(parcel);
                }
            }
        }

        private static LocalOrdersDB ProvideLocalOrders()
        {
            var order3 = new Order(3, new[]
                {
                    new LineItem("Lilies", 100, 2m)
                })
            {
                Recipient = new Address
                {
                    Name = "Grzegorz Brzęczyszczykiewicz",
                    Line1 = "Chrząszczyrzewoszyce 13",
                    Line2 = "Łękołody",
                    PostalCode = "77-777",
                    Country = "Polska"
                }
            }
                .SetPayment(PaymentMethod.PayPal, 20m)
                .SetPayment(PaymentMethod.Invoice, 20m)
                .SetPayment(PaymentMethod.CreditCard, 20m);

            var order4 = new Order(4, new[]
                {
                    new LineItem("Orchid", 50, 1.50m),
                    new LineItem("Calathea", 50, 15m)
                })
            {
                Recipient = new Address
                {
                    Name = "Grażyna Girlanda",
                    Line1 = "Jaroszewice 34/112",
                    Line2 = "Janów Podlaski",
                    PostalCode = "98-876",
                    Country = "Polska"
                }
            }
                .SetPayment(PaymentMethod.CreditCard, 100)
                .SetPayment(PaymentMethod.PayPal, 100);

            var order5 = new Order(5, new[]
                {
                    new LineItem("Leon's Fern", 1, 50m),
                })
            {
                Recipient = new Address
                {
                    Name = "Janusz Pasmankiewicz",
                    Line1 = "Kozietuły 54",
                    Line2 = "Dobry początek",
                    PostalCode = "90-123",
                    Country = "Polska"
                }
            }
                .SetPayment(PaymentMethod.CreditCard, 100)
                .SetPayment(PaymentMethod.Invoice, 100)
                .SetPayment(PaymentMethod.PayPal, 100);

            var order8 = new Order(8, new[]
                {
                    new LineItem("Nasiona kwietna łąka", 5, 20)
                })
            {
                Recipient = new Address
                {
                    Name = "Janina Osiwiecka",
                    Line1 = "Kamionka 3/34",
                    Line2 = "Lipsko",
                    PostalCode = "25-895",
                    Country = "Polska"
                }
            }
                .SetPayment(PaymentMethod.Invoice, 1000);

            var orders = new[] { order3, order4, order5, order8, };
            return new LocalOrdersDB(orders);
        }

        private static GlobalOrdersDB ProvideGlobalOrders()
        {
            var order9 = new OrderNode(null, null, new Order(9, new[]
                {
                    new LineItem("Porcelain set", 20, 100)
                })
            {
                Recipient = new Address
                {
                    Name = "Julie Lerman",
                    Line1 = "Ana Moana Blvd",
                    Line2 = "Honolulu",
                    PostalCode = "96814",
                    Country = "Hawaii"
                }
            }
                .SetPayment(PaymentMethod.Invoice, 1000)
                .SetPayment(PaymentMethod.PayPal, 1000));

            var order7 = new OrderNode(order9, null, new Order(7, new[]
                {
                    new LineItem("Lego Technics - Discovery Nasa ", 1, 700)
                })
            {
                Recipient = new Address
                {
                    Name = "Sherlock Holmes",
                    Line1 = "221B Baker Street",
                    Line2 = "London",
                    PostalCode = "NW1 6XE",
                    Country = "United Kingdom"
                }
            }
                .SetPayment(PaymentMethod.CreditCard, 1000));

            var order6 = new OrderNode(null, null, new Order(6, new[]
                {
                    new LineItem("Sport bike", 1, 600)
                })
            {
                Recipient = new Address
                {
                    Name = "Lea Mathias",
                    Line1 = "9054 Share-wood",
                    Line2 = "Manhattan",
                    PostalCode = "90561",
                    Country = "USA"
                }
            }
                .SetPayment(PaymentMethod.Invoice, 1000));

            var order2 = new OrderNode(null, order6, new Order(2, new[]
                {
                    new LineItem("Flowers - tulips", 25, 1.50m)
                })
            {
                Recipient = new Address
                {
                    Name = "Ely Winkl",
                    Line1 = "1147  Sherwood Circle",
                    Line2 = "Leesville",
                    PostalCode = "71446",
                    Country = "USA"
                }
            }
                .SetPayment(PaymentMethod.CreditCard, 100));

            var order1 = new OrderNode(order2, order7, new Order(1, new[]
                {
                    new LineItem("The Pragmatic Programmer", 1, 47.50m),
                    new LineItem("Code Complete", quantity: 1, 48.50m),
                    new LineItem("Winds of Winter", quantity: 1, 150.00m)
                })
            {
                Recipient = new Address
                {
                    Name = "Krzysztof Krawczyk",
                    Line1 = "3042  Rowes Lane",
                    Line2 = "TRIPLER ARMY MEDICAL CENTER",
                    PostalCode = "96859",
                    Country = "Hawaii"
                }
            }
                .SetPayment(PaymentMethod.CreditCard, 100m)
                .SetPayment(PaymentMethod.Invoice, 200m));

            return new GlobalOrdersDB(order1);
        }

        private static TaxRatesDB ProvideTaxRates()
        {
            return new TaxRatesDB(new Dictionary<string, int>
            {
                { "Polska", 23 },
                { "USA", 0 },
                { "United Kingdom", 20 }
            });
        }
    }
}