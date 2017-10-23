using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScatterSearch
{
    public class SSMethods
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        private static Dictionary<char, int> CharDictionary = new Dictionary<char, int>();

        public reference Generate(string pSecret_Phrase)
        {


            //genes = new char[pSecret_Phrase.Length];
            //string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            //string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";

            // to generate elete solution
            CharDictionary = CountLetters(pSecret_Phrase);

            int position = 0;

            char[] new_phrase = new char[pSecret_Phrase.Length];
            for (int j = 0; j < CharDictionary.Count; j++)
            {
                char key = CharDictionary.Keys.ElementAt(j);
                int value = CharDictionary.Values.ElementAt(j);


                for (int k = 0; k < value; k++)
                {
                    lock (syncLock)
                        position = random.Next(pSecret_Phrase.Length);

                    if (new_phrase[position] == '\0')
                    {
                        new_phrase[position] = key;
                    }
                    else
                    {
                        int count = 0;
                        bool flag = true;
                        while (flag)
                        {
                            if (new_phrase[count] == '\0')
                            {
                                new_phrase[count] = key;
                                flag = false;
                            }
                            count++;
                        }
                    }

                }


            }
            reference tempRef = new reference(pSecret_Phrase);
            tempRef.phrase = new_phrase;
            tempRef.fitness = Fitness(new_phrase, pSecret_Phrase);

            return tempRef;
        }

        //Calculate fitness
        public double Fitness(char[] pPhrase, string pSecretPhrase)
        {
            int score = 0;
            //string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";
            for (int i = 0; i < pPhrase.Length; i++)
            {
                if (pPhrase[i] == pSecretPhrase[i])
                    score++;
            }
            // Its important to convert INT to Double, otherwise you will get Fitness score 0 all the time           
            return Math.Round(Convert.ToDouble(score) / Convert.ToDouble(pSecretPhrase.Length), 2);
            //return FitnesScore;
        }

        public void FitnessOfSet(List<reference> pSet, string pSecretPhrase)
        {
            foreach (var item in pSet)
            {
                int score = 0;
                //string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";
                for (int i = 0; i < item.phrase.Length; i++)
                {
                    if (item.phrase[i] == pSecretPhrase[i])
                        score++;
                }
                // Its important to convert INT to Double, otherwise you will get Fitness score 0 all the time           
                item.fitness  = Math.Round(Convert.ToDouble(score) / Convert.ToDouble(pSecretPhrase.Length), 2);                
            }
        }


        public void ImprovementMethod(List<reference> pRefSet, string pSecretPhrase )
        {
            foreach (var item in pRefSet)
            {
                for (int i = 0; i < pSecretPhrase.Length; i++)
                {
                    if (System.Char.IsUpper(pSecretPhrase[i]) == true)
                        item.phrase[i] = System.Char.ToUpper(item.phrase[i]);
                    if (System.Char.IsLower(pSecretPhrase[i]) == true)
                        item.phrase[i] = System.Char.ToLower(item.phrase[i]);
                }
            }
        }

        public double PointDistance(List<reference> pRefSet)
        {
            double tempPoint = 0;
            for (int i = 0; i < pRefSet.Count; i++)
                tempPoint = pRefSet[i].fitness * pRefSet[i].fitness;

            return Math.Sqrt(tempPoint);
        }

        public Dictionary<char, int> CountLetters(string secret_phrase)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";

            Dictionary<char, int> tempDictionary = new Dictionary<char, int>();

            for (int i = 0; i < chars.Length; i++)
            {
                int countLetters = 0;
                for (int j = 0; j < secret_phrase.Length; j++)
                {
                    if (chars[i] == secret_phrase[j])
                        countLetters++;
                }
                if (countLetters != 0)
                    tempDictionary.Add(chars[i], countLetters);
            }

            return tempDictionary;
        }

        public reference Crossover( reference pParentA, reference pParentB, string pSecret_Phrase)
        {
            int crossover_point;
            lock (syncLock)
                crossover_point = random.Next(pParentA.phrase.Length);

            reference tempChild = new reference(pSecret_Phrase);

            for (int i = 0; i < crossover_point; i++)
                tempChild.phrase[i] = pParentA.phrase[i];
            for (int i = crossover_point; i < pParentA.phrase.Length; i++)
                tempChild.phrase[i] = pParentB.phrase[i];

            var fitness = Fitness(tempChild.phrase, pSecret_Phrase);
            tempChild.fitness = fitness;


            return tempChild; 
        }

        public List<string> getPhrase(List<reference> RefSet)
        {
            List<string> tempList = new List<string>();

            foreach(var item in RefSet)
            {
                tempList.Add(new string(item.phrase));
            }

            return tempList;
        }
    }
}