using System;

namespace OrderProcessing.Shipment
{
    public static class Printer
    {
        private static string CutHereLine = "--------------------------------------------------------";

        public static void PrintLabel(string label)
        {
            Console.WriteLine(CutHereLine);
            Console.Write(label);
            Console.WriteLine(CutHereLine);
        }

        public static void PrintParcel(IParcel parcel)
        {
            Console.WriteLine();
            Console.WriteLine($"Shipment provider: {parcel.ShipmentProviderName}");
            Console.WriteLine($"TotalPrice: {parcel.BundlePrice,10:N2}, TotalTax: {parcel.BundleTax,10:N2}, TotalPriceWithTax: {parcel.BundlePriceWithTax, 10:N2}");
            PrintParcelPrintableFormat(parcel.BundleHeader, parcel.Summary);
            Console.WriteLine();
        }
        private static void PrintParcelPrintableFormat(string header, string summary)
        {
            Console.WriteLine(CutHereLine);
            Console.WriteLine(header);
            Console.WriteLine(summary);
            Console.WriteLine(CutHereLine);
        }
    }
}