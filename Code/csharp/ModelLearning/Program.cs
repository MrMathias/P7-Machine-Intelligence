﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Models.Markov;

namespace ModelLearning {
    class Program {
        static void Main(string[] args) {
//            double threshold = 0.001;
//            int max_states = 20;
//            List<Learner> learners = new List<Learner>() { 
//                new Learners.KentManfredLearner(max_states, threshold),
//                new Learners.BaumWelchLearner(max_states, threshold),
//                new Learners.SparseBaumWelchLearner(max_states, threshold),
//                new Learners.StrictJaegerLearner(max_states, threshold),
//                new Learners.JaegerLearner(max_states, threshold, 0.95)
//            };

            //Console.WriteLine("Select Dataset:");
			//BWBenchmarker benchmarker = new BWBenchmarker(Int32.Parse(Console.ReadLine()));

			//hardcoded datasets to be run
			int[] datasetarray = new int[] {1, 6, 23, 41, 36, 8, 43, 37, 32, 10, 35};

			Console.WriteLine("Number of Runs:");
			int numberOfRuns = Int32.Parse(Console.ReadLine());

			Console.WriteLine("Threshold: 0.01 to whatevz");
			double thresh = Double.Parse(Console.ReadLine());

			Console.WriteLine("Minimum Number of States:");
			int nMin = Int32.Parse(Console.ReadLine());

			Console.WriteLine("Maximum Number of States:");
			int nMax = Int32.Parse(Console.ReadLine());

			Console.WriteLine("Step Size:");
			int stepSize = Int32.Parse(Console.ReadLine());

			Console.WriteLine("File Name:");
			string name = Console.ReadLine();

			foreach(int dataset in datasetarray){
				BWBenchmarker benchmarker = new BWBenchmarker(dataset);
				benchmarker.Run(name, numberOfRuns, thresh, nMin, nMax, stepSize, dataset);
			};

            //while (true) {
            //    //Select number of runs
            //    int num_runs = ShowInterfaceSelectNumRuns();
            //    if (num_runs == -1)
            //        continue;

            //    //select output file
            //    string output_file = ShowInterfaceSelectOutputFile();

            //    //select learners
            //    List<Learner> selected_learners = ShowInterfaceSelectLearners(learners);
            //    if (selected_learners == null)
            //        continue;

            //    //select datasets
            //    List<int> selected_datasets = ShowInterfaceSelectDatasets(10);
            //    if (selected_datasets == null)
            //        continue;

            //    //Select verbosity
            //    bool verbose = ShowInterfaceSelectYesNo("Show intermediate output from learners?");
            //    foreach (Learner learner in learners)
            //        learner.SetVerbosity(verbose);

            //    //Run benchmarker
            //    Console.WriteLine("\nStarting benchmarker");
            //    Benchmarker.Run(selected_learners, selected_datasets, output_file, num_runs);
                
            //    Console.WriteLine("\nBenchmarking has finished with success!");
            //    if (ShowInterfaceSelectYesNo("Do another bencmark?"))
            //        continue;
            //    else
            //        break;
            //}
        }

        static string ShowInterfaceSelectOutputFile() {
            Console.WriteLine("\nEnter name for output file (default: 'benchmark_result.txt')");
            string response = Console.ReadLine();
            return response == "" ? "benchmark_result.txt" : response;
        }

        static int ShowInterfaceSelectNumRuns() {
            Console.WriteLine("\nEnter number of runs (default: 1, min: 1, max: 1000)");
            string response = Console.ReadLine();
            int runs;
            if (!Int32.TryParse(response, out runs))
                return -1;
            if (runs <= 0 || runs > 1000)
                return -1;
            else
                return runs;
        }


        static List<int> ShowInterfaceSelectDatasets(int num_datasets) {
            List<int> selected_datasets = new List<int>();
            Console.WriteLine("\nChoose datasets [1-" + (num_datasets) + "] (e.g. '1,5' for 1 and 5):");
            string response = Console.ReadLine();
            foreach (string s in response.Split(',')) {
                int dataset_index;
                if (!Int32.TryParse(s, out dataset_index)) {
                    Console.WriteLine("Could not parse dataset index: '" + s +"'");
                    return null;
                }
                selected_datasets.Add(dataset_index);
                if (dataset_index <= 0 || dataset_index > num_datasets) {
                    Console.WriteLine("Dataset must be within range [1-"+num_datasets+"], received: " + response);
                    return null;
                }
            }
            return selected_datasets;
        }

        /// <summary>
        /// Returns true if user selectes yes or false if no
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static bool ShowInterfaceSelectYesNo(string str) {
            Console.WriteLine();
            Console.WriteLine(str + " y/n");
            string response = Console.ReadLine();
            if (response.Length > 0 && response.ToLower()[0] == 'y')
                return true;
            else
                return false;
        }

        static List<Learner> ShowInterfaceSelectLearners(List<Learner> learners) {
            List<Learner> selected_learners = new List<Learner>();
            for (int i = 0; i < learners.Count; i++) {
                Console.WriteLine(i + ": " + learners[i].Name());
            }
            Console.WriteLine("\nChoose Learners: (e.g. '0,2') for 0 and 2");
            string response = Console.ReadLine();
            foreach (string s in response.Split(',')) {
                int learner_index;
                if (!Int32.TryParse(s, out learner_index)) {
                    Console.WriteLine("Could not parse learner index: '" + s +"'");
                    return null;
                }
                if (learner_index < 0 || learner_index >= learners.Count) {
                    Console.WriteLine("Learner index must be within range [0-" + learners.Count + "], received " + learner_index);
                    return null;
                }
                selected_learners.Add(learners[learner_index]);
            }
            return selected_learners;
        }
    }
}
