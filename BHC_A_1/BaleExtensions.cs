using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHC_A_1
{
    public static class BaleExtensions
    {
        public static IEnumerable<(Bale Bale1, Bale Bale2)> GetOptimalCombinations(this IEnumerable<Bale> source)
        {
            return source.GroupBy(bale => bale.grade)
                .SelectMany(group => group.SelectMany((bale1, i) => group.Skip(i + 1).Select(bale2 => (bale1, bale2))))
                .Where(pair => pair.bale1.mass + pair.bale1.mass <= 120)
                .Where(pair => pair.bale1.grade == pair.bale2.grade);
        }

        public static void Reworks(this IEnumerable<Bale> source)
        {
            var optimalCombinations = source.GetOptimalCombinations().ToList();
            var rejectedBales = source.Except(optimalCombinations.SelectMany(pair => new[] { pair.Bale1, pair.Bale2 })).ToList();
            var totalMass = optimalCombinations.Sum(pair => pair.Bale1.mass + pair.Bale2.mass);
            var totalGross = optimalCombinations.Sum(pair => pair.Bale1.price * pair.Bale1.mass + pair.Bale2.price * pair.Bale2.mass);
            var newAveragePrice = totalGross / totalMass;

            Console.WriteLine("Optimal Combinations:");
            foreach (var (bale1, bale2) in optimalCombinations)
            {
                Console.WriteLine($"({bale1.barcode}, {bale2.barcode})");
            }

            Console.WriteLine("Rejected Bales:");
            foreach (var rejectedBale in rejectedBales)
            {
                Console.WriteLine(rejectedBale.barcode);
            }

            Console.WriteLine($"New Average Price: {newAveragePrice}");
        }
    }
}
