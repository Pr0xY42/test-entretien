using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TestEntretien
{
    public class TestAlgo
    {

        /// <summary>
        /// grouper les anagrammes 
        /// 10 min
        /// </summary>
        public static void GroupAnagram()
        {
            Console.WriteLine("GroupAnagram start");
            List<string> input = new List<string> { "reza","eat", "tea", "tan", "ate", "nat", "bat", "bta", "azer" };
            var anagrams = new Dictionary<string, List<string>>();

            var i = 0;

            while (input.Count != 0)
            {
                var possibleAnagrams = input.Where(x => x.Length == input[i].Length && x != input[i]).ToList();
                foreach (var possibleAnagram in possibleAnagrams)
                {
                    if (possibleAnagram.All(x => input[i].Contains(x)))
                    {
                        if (anagrams.ContainsKey(input[i]))
                            anagrams[input[i]].Add(possibleAnagram);
                        else
                        {
                            anagrams.Add(input[i], new List<string> { possibleAnagram });
                        }

                        input.Remove(possibleAnagram);
                    }
                }

                if (input.Count <= i)
                    break;
                
                input.Remove(input[i]);

                i++;
            }

            foreach (var anagram in anagrams)
            {
                Console.WriteLine($"{anagram.Key} : ");
                foreach (var anagramValue in anagram.Value)
                {
                    Console.WriteLine($"    {anagramValue}");
                }
            }
        }

        public static void SortInt()
        {
            int[] numbers = Enumerable.Range(0, 10).OrderBy(xp => Guid.NewGuid()).ToArray();
           
           
            



            Console.WriteLine(string.Join(",", numbers));
           
        }


        /// <summary>
        /// Reconstruire une liste en fusionnant les infos, de manière performante ( < 200ms pour 100_000 itérations)
        /// 10 min
        /// </summary>
        public static void PerfSeach()
        {
            int count = 100_000;
            IEnumerable<(string Id, string Name)> persons = Enumerable.Range(0, count)
                .Select(i =>  (i.ToString(), $"username_{i}"));

            IEnumerable<(string Id, string Email)> persons2 = Enumerable.Range(0, count)
                .Select(i => (i.ToString(),  $"mail_{i}@citeo.com" ));

            List<(string Id, string Email, string Name)> result = new List<(string Id, string Email, string Name)>();
            Stopwatch watch = Stopwatch.StartNew();

            // 
            Console.WriteLine(watch.Elapsed.TotalSeconds);


        }
    }
}
