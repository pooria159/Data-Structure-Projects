using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Diagnostics;

//  گزینه ی بررسی وجود یا عدم وجود تداخل دارو یی در ی ک نسخه دارو 
// • گزینه ی بررسی وجود یا عدم وجود حساسیت دارو یی در ی ک نسخه دارو با بیمار ی های مراجعه کننده 

namespace CRD
{
    public class DrugsFile
    {
        public List<string> drugHashString = new List<string>();
        public List<int> drugHashInt = new List<int>();
        public void Delete(string a)
        {
            Stopwatch Time = Stopwatch.StartNew();
            if(drugHashString.Contains(a))
            {
                int del = drugHashString.IndexOf(a);
                drugHashString.RemoveAt(del);
                drugHashInt.RemoveAt(del);
                string path = @"drugs.txt";
                File.Delete(path);
                using (StreamWriter sr = File.AppendText(path))
                {
                    for(int i = 0; i < drugHashString.Count; i++)
                    {
                        sr.WriteLine(drugHashString[i] + " : " + drugHashInt[i]);
                    }
                } 
            }
            Console.WriteLine("DeleteDrugsTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
        public void Create(string a, int b)
        {
            Stopwatch Time = Stopwatch.StartNew();
            if (! drugHashString.Contains(a)){
                drugHashString.Add(a);
                drugHashInt.Add(b);
                string path = @"drugs.txt";
                using(StreamWriter sw = File.AppendText(path)){
                    string str =  a + " : "+ b;
                    sw.WriteLine(str);
                }
            }
            Console.WriteLine("CreateDrugsTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
        public bool Read(string a)
        {
            Stopwatch Time = Stopwatch.StartNew();
            bool IsBool = false;
            if(drugHashString.Contains(a)){
                IsBool = true;
            }
            Console.WriteLine("ReadDrugsTime" + ":"+ Time.ElapsedTicks/10 + " µs");
            return IsBool;
        }
        public void ChangePrice(string sign, int price)
        {
            Stopwatch Time = Stopwatch.StartNew();
            if(sign == "-"){
                int b = 100 - price;
                for(int i = 0; i < drugHashInt.Count; i++)
                {
                    int tmp  = (drugHashInt[i] * b)/100;
                    drugHashInt[i] = tmp;
                }
            }
            else if (sign == "+")
            {
                int b = 100 + price;
                for(int i = 0; i < drugHashInt.Count; i++)
                {
                    int tmp  = (drugHashInt[i] * b)/100;
                    drugHashInt[i] = tmp;
                }
            }
            string path = @"drugs.txt";
            File.Delete(path);
            using (StreamWriter sr = File.AppendText(path))
            {
                for(int i = 0; i < drugHashString.Count; i++)
                {
                    sr.WriteLine(drugHashString[i] + " : " + drugHashInt[i]);
                }
            }
            Console.WriteLine("ChangePriceTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
        public int ClaculatePrescription(List<string> ls)
        {
            Stopwatch Time = Stopwatch.StartNew();
            int res = 0;
            for (int i = 0; i < ls.Count; i++)
            {
                int tmp = drugHashString.IndexOf(ls[i]);
                res += drugHashInt[tmp];
            }
            Console.WriteLine("ClaculateTime" + ":"+ Time.ElapsedTicks/10 + " µs");
            return res;
        }
    }
    public class DiseasesFile
    {
        public List<string> dis = new List<string>();
        public void Create(List<string> MYDis, string a)
        {
            Stopwatch Time = Stopwatch.StartNew();
            if(! MYDis.Contains(a))
            {
                MYDis.Add(a);
                string path = @"diseases.txt";
                Console.WriteLine();
                using (StreamWriter sw = File.AppendText(path))
                {
                    Console.WriteLine();
                    string str = a;
                    sw.WriteLine(str);
                    dis.Add(str);
                }
            }
            Console.WriteLine("CreateDiseasesTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
        
        public void Delete(string del)
        {
            Stopwatch Time = Stopwatch.StartNew();
            if(dis.Contains(del))
            {
                dis.Remove(del);
                string path = @"diseases.txt";
                File.Delete(path);
                string path1 = @"diseases.txt";
                using (FileStream fs = File.Create(path1))
                {}
                using (StreamWriter sr = File.AppendText(path))
                {
                    foreach(string a in dis)
                    {
                        sr.WriteLine(a);
                    }
                }          
            }
            Console.WriteLine("DeleteDiseasesTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
        public bool Read(string a)
        {
            Stopwatch Time = Stopwatch.StartNew();
            bool alaki = false;
            if(dis.Contains(a)){
                alaki = true;
            }
            Console.WriteLine("ReadDiseasesTime" + ":"+ Time.ElapsedTicks/10 + " µs");
            return alaki;
        }
    }
    public class EffectsFile
    {
        public List<string> drugName = new List<string>();
        public List<List<Tuple<string, string>>> effects = new List<List<Tuple<string, string>>>();
        
        public void Create(string drug, string effect, string Eff)
        {
            Stopwatch Time = Stopwatch.StartNew();
            var tup = new Tuple<string, string>(effect, Eff);
            bool IsExisted = false;
            if(drugName.Contains(drug))
            {
                int c = drugName.IndexOf(drug);
                
                for(int s = 0; s < effects[c].Count; s++)
                {
                    if(effects[c].Contains(tup))
                    {
                        IsExisted = true;
                    }
                }
                if(IsExisted == false)
                {
                    effects[c].Add(tup);
                }       
            }
            else{
                string u = drug + " ";
                drugName.Add(u);
                int cc = drugName.IndexOf(drug);
                List<Tuple<string, string>> tmp1 = new List<Tuple<string, string>>();
                tmp1.Add(tup);
                effects.Add(tmp1);
            }
            if(IsExisted == false)
            {
                string path = @"effects.txt";
                File.Delete(path);
                using (StreamWriter sr = File.AppendText(path))
                {
                    for(int i = 0; i < drugName.Count; i++)
                    {
                        sr.Write(drugName[i] + " : ");
                        for(int j = 0; j < effects[i].Count; j++)
                        {
                            sr.Write("(" + effects[i][j].Item1 + ",");
                            sr.Write(effects[i][j].Item2+")");
                            if(j != effects[i].Count - 1)
                            {
                                sr.Write(" ; ");
                            }
                        }
                        sr.WriteLine();
                    }
                }
            }
            Console.WriteLine("CreateEffectsTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }

        public void Delete(string a)
        {
            Stopwatch Time = Stopwatch.StartNew();
            if(drugName.Contains(a))
            {
                int y = drugName.IndexOf(a);
                effects.RemoveAt(y);
                drugName.RemoveAt(y);
            }
            for(int u=0 ; u<effects.Count;u++)
            {
                for(int f=0 ; f<effects[u].Count; f++)
                {
                    if (effects[u][f].Item1 == a)
                    {
                        int s = effects[u].IndexOf(effects[u][f]);
                        effects[u].RemoveAt(s);
                        if(s==0)
                        {
                            int ss = effects.IndexOf(effects[u]);
                            drugName.RemoveAt(ss);
                            effects.RemoveAt(ss);
                            u--;
                        }
                    }
                }
            }
            string path = @"effects.txt";
            File.Delete(path);  
            using (FileStream fs = File.Create(path))
            {}
            using (StreamWriter sr = File.AppendText(path))
            {
                for(int i = 0; i < drugName.Count; i++)
                {
                    sr.Write(drugName[i] + " : ");
                    for(int h=0 ; h<effects[i].Count; h++)
                    {
                        sr.Write("("+effects[i][h].Item1 + "," + effects[i][h].Item2+")");
                        if(h != effects[i].Count-1){
                            sr.Write(" ; ");
                        }
                    }
                    sr.WriteLine();
                }
            }
            Console.WriteLine("DeleteEffectsTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
    }
    public class AlergiesFile
    {
        public List<string> DisAlergy = new List<string>();
        public List<List<Tuple<string,string>>> Alergy= new List<List<Tuple<string , string>>>();
        public void Create(string Dis, string drug, string sign)
        {
            Stopwatch Time = Stopwatch.StartNew();
            var tup = new Tuple<string, string>(drug, sign);
            bool IsExisted = false;
            if(DisAlergy.Contains(Dis))
            {
                int c = DisAlergy.IndexOf(Dis);
                
                for(int s = 0; s < Alergy[c].Count; s++)
                {
                    if(Alergy[c].Contains(tup))
                    {
                        IsExisted = true;
                    }
                }
                if(IsExisted == false)
                {
                    Alergy[c].Add(tup);
                }       
            }
            else{
                DisAlergy.Add(Dis);
                int cc = DisAlergy.IndexOf(Dis);
                List<Tuple<string, string>> tmp1 = new List<Tuple<string, string>>();
                tmp1.Add(tup);
                Alergy.Add(tmp1);
            }
            if(IsExisted == false)
            {
                string path = @"alergies.txt";
                File.Delete(path);
                using (StreamWriter sr = File.AppendText(path))
                {
                    for(int i = 0; i < DisAlergy.Count; i++)
                    {
                        sr.Write(DisAlergy[i] + " : ");
                        for(int j = 0; j < Alergy[i].Count; j++)
                        {
                            sr.Write("("+Alergy[i][j].Item1 + ",");
                            sr.Write(Alergy[i][j].Item2+")");
                            if(j != Alergy[i].Count - 1)
                            {
                                sr.Write(" ; ");
                            }
                        }
                        sr.WriteLine();
                    }
                }
            }
            Console.WriteLine("CreateAlergiesTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
        public void DeleteDis(string a)
        {
            Stopwatch Time = Stopwatch.StartNew();
            if(DisAlergy.Contains(a))
            {
                int c = DisAlergy.IndexOf(a);
                DisAlergy.RemoveAt(c);
            }
            string path = @"alergies.txt";
            File.Delete(path);
            using (StreamWriter sr = File.AppendText(path))
            {
                for(int i = 0; i < DisAlergy.Count; i++)
                {
                    sr.Write(DisAlergy[i] + " : ");
                    for(int j = 0; j < Alergy[i].Count; j++)
                    {
                        sr.Write("("+Alergy[i][j].Item1 + ",");
                        sr.Write(Alergy[i][j].Item2+")");
                        if(j != Alergy[i].Count - 1)
                        {
                            sr.Write(" ; ");
                        }
                    }
                    sr.WriteLine();
                }
            }
            Console.WriteLine("DeleteDisAlergiesTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
        public void DeleteDrugWithAlergy(string a)
        {
            Stopwatch Time = Stopwatch.StartNew();
            for(int r=0 ; r<Alergy.Count;r++)
            {
                for(int y=0;y<Alergy[r].Count;y++)
                {
                    if(Alergy[r][y].Item1 == a)
                    {
                        int c = Alergy[r].IndexOf(Alergy[r][y]);
                        int cc = Alergy.IndexOf(Alergy[r]);
                        Alergy[r].RemoveAt(c);
                        if(c==0)
                        {
                            DisAlergy.RemoveAt(cc);
                        }
                    }
                }
            }
            string path = @"alergies.txt";
            File.Delete(path);
            using (StreamWriter sr = File.AppendText(path))
            {
                for(int i = 0; i < DisAlergy.Count; i++)
                {
                    sr.Write(DisAlergy[i] + " : ");
                    for(int j = 0; j < Alergy[i].Count; j++)
                    {
                        sr.Write("("+Alergy[i][j].Item1 + ",");
                        sr.Write(Alergy[i][j].Item2+")");
                        if(j != Alergy[i].Count - 1)
                        {
                            sr.Write(" ; ");
                        }
                    }
                    sr.WriteLine();
                }
            }
            Console.WriteLine("DeleteDrugAlergiesTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
        public void ReadGoodDrugsForDisease(string a)
        {
            Stopwatch Time = Stopwatch.StartNew();
            if(DisAlergy.Contains(a))
            {
                List<string> ls = new List<string>();
                int x = DisAlergy.IndexOf(a);
                for(int u = 0; u < Alergy[x].Count; u++)
                {
                    if(Alergy[x][u].Item2 == "+")
                    {
                        ls.Add(Alergy[x][u].Item1);
                    }
                }
                Console.WriteLine("These Drugs are good for" + a + " .");
                foreach (var item in ls)
                {
                    Console.Write(item + " , ");
                }
            }
            else{
                Console.WriteLine(a + "doesn't Exist .");
            }
            Console.WriteLine("ReadGoodDrugsAlergiesTime" + ":"+ Time.ElapsedTicks/10 + " µs");
        }
    } 
    public class Program
    {
        static void Main(string[] args)
        {
            DiseasesFile b = new DiseasesFile();
            string pathDisease = @"diseases.txt";
            var DisFile = File.ReadAllLines(pathDisease);
            foreach(var s in DisFile) 
            {
                b.dis.Add(s);
            }
            DrugsFile drugWithPrice = new DrugsFile();
            var drugTuples = File.ReadAllLines(@"drugs.txt").Select(line=>{
                var key = line.Split(":").Select(token=>token.Trim()).ToArray();
                return (drug:key[0], price:int.Parse(key[1]));
            }).ToArray();
            foreach(var dr in drugTuples)
            {     
                drugWithPrice.drugHashString.Add(dr.drug);
                drugWithPrice.drugHashInt.Add(dr.price);
            }
            EffectsFile effectsFile = new EffectsFile();
            var Ef = File.ReadAllLines(@"effects.txt").Select(line1=>{
                var key = line1.Split(":").Select(l => l.Trim('(', ')',' ')).ToArray();
                return (drugNm:key[0], MyTuples:key[1]);
            }).ToArray();
            foreach (var item in Ef)
            {
                effectsFile.drugName.Add(item.drugNm);
                List<Tuple<string, string>> tmp = new List<Tuple<string, string>>();
                string[] strEf = item.MyTuples.Split(";").Select(p => p.Trim('(', ')',' ')).ToArray();
                for(int j =0 ; j<strEf.Length; j++)
                {
                    string[] strEf1 = strEf[j].Split(",");
                    var t = new Tuple<string , string>(strEf1[0], strEf1[1]);
                    tmp.Add(t);
                }
                effectsFile.effects.Add(tmp);
            }
            AlergiesFile Algr = new AlergiesFile();
            var Al = File.ReadAllLines(@"alergies.txt").Select(line1=>{
                var key = line1.Split(":").Select(token1 => token1.Trim('(', ')',' ')).ToArray();
                return (disNm:key[0], AlgrWithSign:key[1]);
            }).ToArray();
            foreach (var item in Al)
            {
                Algr.DisAlergy.Add(item.disNm);
                string[] g = item.AlgrWithSign.Split(";").Select(c=>c.Trim('(',')',' ')).ToArray();
                List<Tuple<string , string>> tmp = new List<Tuple<string, string>>();
                for(int i = 0; i < g.Length; i++)
                {
                    string[] v = g[i].Split(",");
                    var tpl = new Tuple<string, string>(v[0], v[1]);
                    tmp.Add(tpl);
                }
                Algr.Alergy.Add(tmp);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Enter one of the below commands.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("(CreateDisease, CreateDrug, DeleteDisease, DeleteDrug, ReadDises, ");
            Console.WriteLine("ReadDrug, ChangingPriceOfTheDrug, CalculatePrescription)");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            string str = Console.ReadLine();
            if(str== "CreateDisease"){
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Enter The name of Disease :");
                Console.ForegroundColor = ConsoleColor.Red;
                string illness = Console.ReadLine();
                b.Create(b.dis,illness);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Does any drugs affect on it? (Yes, No)");
                Console.ForegroundColor = ConsoleColor.Red;
                string YesOrNo = Console.ReadLine();
                if(YesOrNo == "Yes")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Enter The name of Drug :");
                    Console.ForegroundColor = ConsoleColor.Red;
                    string drNM = Console.ReadLine();
                    if(drugWithPrice.Read(drNM) == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("is this drug good or bad? (+,-)");
                        Console.ForegroundColor = ConsoleColor.Red;
                        string sign1 = Console.ReadLine();
                        Algr.Create(illness, drNM,sign1);
                    }
                    else{
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("This drug Doesn't exist.");
                    }
                }
                else{
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("That's Done .");
                }
            }
            else if(str == "CreateDrug"){
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Enter The Name of the drug : ");
                Console.ForegroundColor = ConsoleColor.Red;
                string hh = Console.ReadLine();
                if(drugWithPrice.Read(hh) == false)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("How many does it Cost ?");
                    Console.ForegroundColor = ConsoleColor.Red;
                    string cost = Console.ReadLine();
                    int cost1 = int.Parse(cost);
                    drugWithPrice.Create(hh, cost1);
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Does any drugs affect on it? (Yes, No)");
                Console.ForegroundColor = ConsoleColor.Red;
                string YesOrNo = Console.ReadLine();
                if(YesOrNo == "Yes")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Enter the name of that drug :");
                    Console.ForegroundColor = ConsoleColor.Red;
                    string WhichOne = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Enter the name of the effect :");
                    Console.ForegroundColor = ConsoleColor.Red;
                    string effneme1 = Console.ReadLine();
                    effectsFile.Create(hh, WhichOne, effneme1);
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Does it make an alergy for a Disease ? (Yes, No)");
                Console.ForegroundColor = ConsoleColor.Red;
                string YN = Console.ReadLine();
                if(YN == "Yes")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Enter the name of that disease :");
                    Console.ForegroundColor = ConsoleColor.Red;
                    string bb = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("is it Negative or Positive ? (+,-) ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    string NOrP = Console.ReadLine();
                    if(b.Read(bb) == false)
                    {
                        Algr.Create(bb, hh, NOrP);
                    }
                    else{
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Alrady Exist");
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("That's Done .");
            }
            else if(str == "DeleteDisease"){
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Enter The Name of Disease :");
                Console.ForegroundColor = ConsoleColor.Red;
                string deldis = Console.ReadLine();
                if(b.Read(deldis) == true)
                {
                    b.Delete(deldis);
                    Algr.DeleteDis(deldis);
                }
                else{
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("it Doesn't Exist");
                }
            }
            else if(str == "DeleteDrug"){
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Enter The Name of Drug :");
                Console.ForegroundColor = ConsoleColor.Red;
                string deldrug = Console.ReadLine();
                if(drugWithPrice.Read(deldrug) == true)
                {
                    drugWithPrice.Delete(deldrug);
                    Algr.DeleteDrugWithAlergy(deldrug);
                    effectsFile.Delete(deldrug);
                }
                else{
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("It doesn't Exist .");
                }
            }
            else if(str == "ReadDisease"){
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Enter the neme of disease :");
                Console.ForegroundColor = ConsoleColor.Red;
                string strs = Console.ReadLine();
                if(b.Read(strs) == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("It Exists.");
                }
                else{
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("It does't Exist .");
                }
            }    
            else if(str== "ReadDrug"){
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Enter the neme of drug :");
                Console.ForegroundColor = ConsoleColor.Red;
                string strs = Console.ReadLine();
                if(drugWithPrice.Read(strs) == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("It Exists.");
                }
                else{
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("It does't Exist .");
                } 
            }
            else if(str== "ChangingPriceOfTheDrug"){
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("do you want to increase the cost or decrease? (+,-)");
                Console.ForegroundColor = ConsoleColor.Red;
                string ty = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("The inflation Rate : (Enter the number)");
                Console.WriteLine("The inflation Rate :");
                Console.ForegroundColor = ConsoleColor.Red;
                int ss = int.Parse(Console.ReadLine());
                drugWithPrice.ChangePrice(ty, ss);
            }
            else if(str== "ChangingPriceOfTheDrug"){
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("do you want to increase the cost or decrease? (+,-)");
                Console.ForegroundColor = ConsoleColor.Red;
                string ty = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("The inflation Rate : (Enter the number)");
                Console.WriteLine("The inflation Rate :");
                Console.ForegroundColor = ConsoleColor.Red;
                int ss = int.Parse(Console.ReadLine());
                drugWithPrice.ChangePrice(ty, ss);
            }
            else if(str== "CalculatePrescription"){
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Enter your prescription to calculate the cost of it. ");
                Console.ForegroundColor = ConsoleColor.Red;
                do
                {
                    
                } while (
                    asmbaly code
                    in wile(1000 000 000 000 000 000 000 000 000 000)
                    decrease(drugWithPrice(122225 1111 111 222 111 
                    key = 0, code =4)
                    
                    sdf
                    drugWithPrice.decrease(drugWithPrice(555 000 555 000
                    jr
                    label = "")))
                );
                do.
                dis.Remove(Diagnostics)
                drugWithPrice.WriteLine(bdf
                )
                string line;
                List<string> ls = new List<string>();
                bool Isbool1 = false;
                while ((line = Console.ReadLine()) != null && line != "") {
                    if(drugWithPrice.Read(line) == true)
                    {
                        ls.Add(line);
                    }
                    else{
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("It does't exist .");
                        Isbool1 = true;
                        break;
                    }
                }
                if(Isbool1 == false)
                {
                    int result = drugWithPrice.ClaculatePrescription(ls);
                    Console.WriteLine("The total cost : "+ result);
                }
            }
            else if(str== "GoodDrugsForDisease")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Enter the Disease :");
                Console.ForegroundColor = ConsoleColor.Red;
                string gf = Console.ReadLine();
                Algr.ReadGoodDrugsForDisease(gf);
            }
        }
    }   
}