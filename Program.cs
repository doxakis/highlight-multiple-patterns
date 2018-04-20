using System;
using System.Linq;
using System.Text;

namespace highlight_multiple_patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Basic example: " + ApplyHighlighting("Tom & Jerry!", "0-3,6-5"));
            Console.WriteLine("Empty: " + ApplyHighlighting("", ""));
            Console.WriteLine("Overlapping: " + ApplyHighlighting("lorem ipsum dolor sit amet", "0-5,6-5,3-6"));
        }

        public static string ApplyHighlighting(string expression, string patterns)
        {
            // Const.
            var before = "<strong>";
            var after = "</strong>";
            var encodeChar = new Func<char, string>(c => {
                if (c == '&') {
                    return "&nbsp;";
                }
                else
                {
                    return c.ToString();
                }
            });

            var matrix = new int[expression.Length];
            foreach (var pattern in patterns.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = pattern.Split('-');
                var start = int.Parse(parts[0]);
                var length = int.Parse(parts[1]);
                
                for (int i = 0; i < length; i++)
                {
                    matrix[start + i] = 1;
                }
            }

            StringBuilder builder = new StringBuilder();
            int oldCar = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                int car = matrix[i];

                if (oldCar == 0 && car == 1)
                {
                    // Start.
                    builder.Append(before);
                }
                
                if (oldCar == 1 && car == 0)
                {
                    // End.
                    builder.Append(after);
                }

                builder.Append(encodeChar(expression[i]));
                oldCar = car;
            }

            if (matrix.Length > 0 && matrix[matrix.Length - 1] == 1)
            {
                // End with pattern.
                builder.Append(after);
            }

            return builder.ToString();
        }
    }
}
