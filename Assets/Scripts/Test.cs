using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class Test
    {
        static void Main()
        {
            Dictionary<string, int> myDict = new Dictionary<string, int>
            {
                {"a", 1},
                {"b", 2},
                {"c", 3},
                {"d", 4}
            };

            KeyValuePair<string, int> randomElement = GetRandomElement(myDict);

            Console.WriteLine($"Random element: Key = {randomElement.Key}, Value = {randomElement.Value}");
        }

        static KeyValuePair<TKey, TValue> GetRandomElement<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                // Trả về một KeyValuePair mặc định nếu dictionary rỗng
                return default(KeyValuePair<TKey, TValue>);
            }

            // Sử dụng System.Random để tạo số ngẫu nhiên
            Random random = new Random();
        
            // Lấy một index ngẫu nhiên từ keys của dictionary
            int randomIndex = random.Next(dictionary.Count);

            // Chuyển đổi keys thành một danh sách để có thể truy cập theo index
            List<TKey> keys = new List<TKey>(dictionary.Keys);

            // Lấy key và value tương ứng với index ngẫu nhiên
            TKey randomKey = keys[randomIndex];
            TValue randomValue = dictionary[randomKey];

            return new KeyValuePair<TKey, TValue>(randomKey, randomValue);
        }
    }
}