﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelLearning {
    static class Benchmarker {
        public static void Run(IEnumerable<Learner> learners, IEnumerable<int> datasets) {
            foreach (int i in datasets) {
                Console.WriteLine("Benchmarking dataset "+i);
                SequenceData trainSequences = DataLoader.LoadSequences(@"Data/" + i + ".pautomac.train");
                SequenceData testSequences = DataLoader.LoadSequences(@"Data/" + i + ".pautomac.test");
                double[] solutions = DataLoader.LoadSolutions(@"Data/" + i + ".pautomac_solution.txt");
                Console.WriteLine("Loading data");
                foreach (Learner learner in learners){
                    Console.WriteLine(learner.Name() + " is learning a model");
                    learner.Learn(trainSequences, testSequences);
                    Console.WriteLine("Evaluating learner");
                    double score = PautomacEvaluator.Evaluate(learner, testSequences, solutions);
                    Console.WriteLine(learner.Name() + " score: "+score);
                }
            }
        }
    }
}