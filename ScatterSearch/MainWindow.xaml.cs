using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScatterSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // only letters cap & non cap , and spaces are ok; no numbers or special symbols ie # 
        public string secret_phrase = "To be or not to be";
        public int referenceSetSize = 20;
        public int initialSolutionSize = 200;
        public int maxIteration = 100;

        public List<reference> InitialSolutions = new List<reference>();
        public List<reference> RefSet = new List<reference>();
        

        double initialFitness = 0;

        public MainWindow()
        {
            InitializeComponent();

            //SS test = new SS(secret_phrase, referenceSetSize);

            //int a = test.CharDictionary.Sum(x => x.Value);

            //test.ImprovementMethod(); 

            //char[] c = new char[3];
            //test.CountLetters(secret_phrase);


        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            SSMethods myssMethods = new SSMethods();

            //1. generate inital solution & evaluate fitness of each solution
            for (int i = 0; i < initialSolutionSize; i++)
            {
                InitialSolutions.Add(myssMethods.Generate(secret_phrase));
            }
            //2. apply improvment mehtod to solutions
            myssMethods.ImprovementMethod(InitialSolutions, secret_phrase);
            // evaluate fitness again
            myssMethods.FitnessOfSet(InitialSolutions, secret_phrase);

            // select best solutions set and make a reference set;             
            var InitialSolutionsOrdered = (from r in InitialSolutions
                           orderby r.fitness descending
                           select r).ToList(); ;

            //3. create a reference set by taking top 20 from Initial Solution
            for (int i =0; i< referenceSetSize; i++)
            {
                // have to create a temp object othervise it is copied by ref not by value;
                reference temp = new reference();
                temp.fitness = InitialSolutionsOrdered[i].fitness;
                temp.phrase = InitialSolutionsOrdered[i].phrase;
                RefSet.Add(temp);
            }

            initialFitness = myssMethods.PointDistance(RefSet);
            
            bool solution_not_found = true;
            int iteration_count = 0;

            int test_refupdate_count = 0;

            while (iteration_count < maxIteration && solution_not_found)
            {
                //4. Crossover reference set 
                List<reference> Children = new List<reference>();

                for (int i = 0; i < RefSet.Count; i++)
                {
                    var ParentA = RefSet[i];
                    for (int j = i + 1; j < referenceSetSize; j++)
                    {
                        var tempChild = myssMethods.Crossover(ParentA, RefSet[j], secret_phrase);
                        Children.Add(tempChild);
                    }
                }

                myssMethods.ImprovementMethod(Children, secret_phrase);
                myssMethods.FitnessOfSet(Children, secret_phrase);

                var childrenInOrder = (from c in Children
                                       orderby c.fitness descending
                                       select c).ToList();


                //5. update reference set with better fitness
                for (int j = 0; j < referenceSetSize; j++)
                {
                    bool flag = true;
                    int count = 0;
                    while (flag)
                    {
                        if (RefSet[j].fitness < childrenInOrder[count].fitness)
                        {
                            reference tempRef = new reference(secret_phrase);
                            tempRef.fitness = childrenInOrder[count].fitness;
                            tempRef.phrase = childrenInOrder[count].phrase;

                            RefSet[j].fitness = tempRef.fitness;
                            RefSet[j].phrase = tempRef.phrase;

                            childrenInOrder.RemoveAt(count);

                            test_refupdate_count++;

                            if (RefSet[j].fitness == 1)
                                solution_not_found = false;

                            flag = false;
                        }

                        count++;
                        if (count == childrenInOrder.Count)
                            flag = false;
                    }

                }
                iteration_count++;
            }

            //things to improve 
            //1. when the referense set stops updating, the we need to generate new Inital Solutions
            //2. add initial solutions to reference set for deversification
            //3. make sure that when crossing all letters are present in the phrase and not being excluded
            // right now when crossing i loose some of letters
            //4. improvement function is ok but can be so much better good , it just 


            double finalFitness = RefSet[0].fitness; //myssMethods.PointDistance(RefSet);
            var refset_strings = myssMethods.getPhrase(RefSet);
            //RefSet[0].fitness = 1;

            double improvment = finalFitness - initialFitness;

            int b = 0;
        }
    }
}
