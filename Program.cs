using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AshokaExamSystem
{
    class Checker
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: dotnet run <filename.json>");
                return;
            }

            string filePath = args[0];

            // --- MASTER KEY (Question ID : Correct Option Number) ---
            Dictionary<int, int> masterKey = new Dictionary<int, int>
            {
                {1, 1}, {2, 3}, {3, 2}, {4, 1}, {5, 3},
                {6, 2}, {7, 2}, {8, 2}, {9, 3}, {10, 2},
                {11, 2}, {12, 2}, {13, 3}, {14, 2}, {15, 1},
                {16, 2}, {17, 1}, {18, 2}, {19, 2}, {20, 3}
            };

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File '{filePath}' not found.");
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                
                // Using Regex to parse the JSON format {"1": 2, "2": 4...}
                // This matches "key": value
                var matches = Regex.Matches(jsonContent, @"""(\d+)"":\s*(\d+)");

                int score = 0;
                int totalQuestions = masterKey.Count;

                Console.WriteLine("\n==============================================");
                Console.WriteLine("    ASHOKA UNIVERSAL SCHOOL - EXAM CHECKER");
                Console.WriteLine("==============================================");
                Console.WriteLine($"Student File: {Path.GetFileName(filePath)}");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("{0,-10} | {1,-10} | {2,-10} | {3,-8}", "Q.No", "Student", "Correct", "Result");
                Console.WriteLine("-----------|------------|------------|--------");

                // Create a dictionary for student answers to handle out-of-order questions
                Dictionary<int, int> studentAnswers = new Dictionary<int, int>();
                foreach (Match match in matches)
                {
                    int qId = int.Parse(match.Groups[1].Value);
                    int ans = int.Parse(match.Groups[2].Value);
                    studentAnswers[qId] = ans;
                }

                for (int i = 1; i <= totalQuestions; i++)
                {
                    int studentAns = studentAnswers.ContainsKey(i) ? studentAnswers[i] : 0;
                    int correctAns = masterKey[i];
                    bool isCorrect = (studentAns == correctAns);

                    if (isCorrect) score++;

                    string resultSymbol = isCorrect ? "CORRECT" : "WRONG";
                    
                    // Highlight result in console
                    if (!isCorrect) Console.ForegroundColor = ConsoleColor.Red;
                    else Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine("Q{0,-8} | Opt {1,-6} | Opt {2,-6} | {3,-8}", 
                        i, studentAns == 0 ? "N/A" : studentAns.ToString(), correctAns, resultSymbol);
                    
                    Console.ResetColor();
                }

                Console.WriteLine("----------------------------------------------");
                Console.WriteLine($"FINAL SCORE: {score} / {totalQuestions}");
                double percentage = ((double)score / totalQuestions) * 100;
                Console.WriteLine($"PERCENTAGE:  {percentage:F2}%");
                Console.WriteLine("==============================================\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}