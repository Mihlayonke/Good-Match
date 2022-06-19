using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace Good_Match_Assessment
{
    class Program
    {
        //Count and reduce to two dijit number...
        static string Reduce(string name){
            bool isEven = true;
            double first;
            double last;
            double tempAdd;
            string sum = "";

            if (name.Length % 2 != 0)
            {
                isEven = false;
            }

            for (int i=0; i < name.Length/2; i++)
            {
                first = Char.GetNumericValue(name, i);
                last = Char.GetNumericValue(name, name.Length-1-i);
                tempAdd = first + last;
                sum += tempAdd.ToString();
            }
            if (isEven == false)
            {
                sum += name[name.Length/2];
            }
            
            return sum;
        }

        //Match the name1 and name2 then store the results...
        static string Match(string name_1, string name_2)
        {

            string joined = name_1.ToLower() + "matches" + name_2.ToLower();
            string CheckedChars = "";
            string StrCount = "";
            int TempCount;

            //Count the charactor that appearing for the first time...
            foreach (char c in joined){
                if (CheckedChars.Contains(c) == false) 
                {
                    CheckedChars += c; 
                    TempCount = joined.Split(c).Length - 1;
                    StrCount += TempCount.ToString();
                }
            }
            
            //Here we reduce the number...
            string reduced;
            if (StrCount.Length <= 2)
            {
                reduced = StrCount; 
            }
            else
            {
                reduced = Reduce(StrCount);
                while (reduced.Length > 2){
                    reduced = Reduce(reduced);
                }
            }
            
            int percentage = Int32.Parse(reduced);
            string result =  percentage > 80 ? (name_1 + " matches "+ name_2 + " "+ reduced +"%, Good Match!!!") : name_1 + " matches "+ name_2 + " "+ reduced +"%";
            
            return(result);
        }

        //Match the name1 and name2 then store the results...
        static Tuple<string, int> Match2(string name_one, string name_two)
        {
            string joined = name_one.ToLower() + "matches" + name_two.ToLower();
            string CheckedChars = "";
            string StrCount = "";
            int TempCount;

            //Count the charactor that appearing for the first time...
            foreach (char c in joined){
                if (CheckedChars.Contains(c) == false) 
                {
                    CheckedChars += c;
                    TempCount = joined.Split(c).Length - 1;
                    StrCount += TempCount.ToString();
                }
            }

            //Here we reduce the number...
            string reduced;
            if (StrCount.Length <= 2)
            {
                reduced = StrCount; 
            }
            else
            {
                reduced = Reduce(StrCount);
                while (reduced.Length > 2){
                    reduced = Reduce(reduced);
                }
            }
            
            int percentage = Int32.Parse(reduced);
            string result =  percentage > 80 ? (name_one + " matches "+ name_two + " "+ reduced +"%, Good Match!!!") : name_one + " matches "+ name_two + " "+ reduced +"%";
            
            var myTuple = Tuple.Create(result,percentage);
            return(myTuple);

        }

        //Match for every entry in the first set against every entry in the second set then store the results...
        static Tuple<string,int>[,] MatchCSVnames(List<string> males, List<string> females)
        {

            //Store all the results...
            Tuple<string,int>[,] ResultsArray = new Tuple<string,int>[males.Count, females.Count]; 
            int i = 0;
            int j;

            foreach (string male in males){
                j=0;

                foreach(string female in females){
                    Tuple<string,int> result = Match2(male, female);
                    ResultsArray[i,j] = result; 
                    j += 1;

                }
                i += 1;
            }
            
            return(ResultsArray);
        }
        
        static void Main(string[] args)
        {
            
            Console.WriteLine("___Tennis Good Match.___\n");
            Console.WriteLine("---FIRST PART---");
            Console.WriteLine("Enter the names to match in alphabetic characters only.\n");
            Console.WriteLine("Enter first name: ");
            string userName1 = Console.ReadLine();
            
            if (userName1.All(char.IsLetter))
            {
                Console.WriteLine("First name entered is: " + userName1 + "\n");
            }
            else
            {
                while( userName1.All(char.IsLetter) != true )
                {
                    Console.WriteLine("Please only use alphabetic characters");
                    Console.WriteLine("Enter first name: ");
                    userName1 = Console.ReadLine();
                }
                
            }
            
            Console.WriteLine("Enter second name: ");
            string userName2 = Console.ReadLine();
            
            if (userName2.All(char.IsLetter))
            {
                Console.WriteLine("Second name entered is: " + userName2 + "\n");
            }
            else
            {
                while( userName2.All(char.IsLetter) != true )
                {
                    Console.WriteLine("Please only use alphabetic characters");
                    Console.WriteLine("Enter second name: ");
                    userName2 = Console.ReadLine();
                }
                
            }

            //timer...
            var watch = new System.Diagnostics.Stopwatch();

            //Begining of timer for matching inputed names...
            watch.Start();

            // Algorithm matches two names then assign output to a string variable...
            string OutputString = Match(userName1, userName2); 

            //End of timer for matching inputed names...
            watch.Stop();

            Console.WriteLine("___1st Part Output:___ \n");
            Console.WriteLine(OutputString );
            Console.WriteLine();
            Console.WriteLine($"Time taken to match {userName1} and {userName2}: {watch.ElapsedMilliseconds} ms\n");
            File.WriteAllText("1st_Part_log_time.txt", $"Time to match {userName1} and {userName2}: {watch.ElapsedMilliseconds} ms\n" );

            List<string> listMales = new List<string>();
            List<string> listFemales = new List<string>();

            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string strWorkPath = Path.GetDirectoryName(strExeFilePath);
            string filePath = Path.Combine(strWorkPath, "names.csv" );
            Console.WriteLine("---SECOND PART---\n");

            try
            {
                using(var reader = new StreamReader(filePath))
                {
                    /*
                    While the are charactors in the CVS_file to read, read the line and separate name and gender...
                    Resgister males in malesList and female in femaleList...
                     */

                    Console.WriteLine("___Process of adding names to match:___ \n");
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        values[1] = values[1].Replace(" ", String.Empty);
                        
                        if (values[1] == "m"){
                            listMales.Add(values[0]);
                            Console.WriteLine(values[0] +", as male was added" );
                        }
                        else
                        {
                            listFemales.Add(values[0]);
                            Console.WriteLine(values[0] +", as female was added");
                        } 
                        
                    }
                }

            }

            //If the time is not found, throw an exception....
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found! File should be found in directory --> Good_Match_Assessment/bin/Debug/net5.0.");
                Console.WriteLine(e.ToString());
            }

            //Here duplicates are removed and esults returned alphabetical in order...... 
            listMales = listMales.Distinct().ToList();
            listFemales = listFemales.Distinct().ToList();
            
            //List is sorted and then reversed so it is in decesnding order
            listMales.Sort();
            listMales.Reverse();
            listFemales.Sort();
            listFemales.Reverse();

            //Begining of timer for matching names...
            watch.Start();
            
            Tuple<string,int>[,] MyResultsArr = new Tuple<string,int>[listMales.Count, listFemales.Count]; 
            MyResultsArr = MatchCSVnames(listMales,listFemales);

            //End of timer for matching names...
            watch.Stop();

            Console.WriteLine();
            Console.WriteLine($"Time taken to match names from the CSV file is {watch.ElapsedMilliseconds} ms\n");
            File.WriteAllText("2nd_Part_log_time_match_taken.txt", $"Time taken to match possible names from CSV file is {watch.ElapsedMilliseconds} ms\n" );

            //Begining of timer for second part...
            watch.Start();
            
            List<string> ResultStr = new List<string>();
            List<int> ResultInt = new List<int>();


            foreach (Tuple<string, int> result in MyResultsArr)
            {
                ResultStr.Add(result.Item1);
                ResultInt.Add(result.Item2);
                
            } 

            //Ordering in percentage order...
            var newOrdering = ResultInt.Select((Int32, index) => new { Int32, index }).OrderBy(item => item.Int32).ToArray();

            //Sort the list... 
            ResultStr = newOrdering.Select(item => ResultStr[item.index]).ToList();

            //Reverse the list to be in descending order...
            ResultStr.Reverse();
            
            //End of timer for matching names...
            watch.Stop();

            File.WriteAllLines("Output.txt", ResultStr.Select(x => string.Join(",", x)));
            Console.WriteLine("___2nd Part Output (found in Good_Match_Assessment/bin/Debug/net5.0/Output.txt):___ \n");
            Console.WriteLine(String.Join(",\n",ResultStr));
            Console.WriteLine();
            Console.WriteLine($"Time taken to sort the matched names is {watch.ElapsedMilliseconds} ms\n");
            File.WriteAllText("2nd_Part_log_time_sort_taken.txt", $"Time to sort the matched names is {watch.ElapsedMilliseconds} ms\n");

        }
    }
}
