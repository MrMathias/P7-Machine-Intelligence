﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Models.Markov;

namespace ModelLearning {
    class Program {
        static void Main(string[] args) {
            List<Learner> learners = new List<Learner>() { 
                //new Learners.ManfredLearner(10, 10)
                new Learners.BaumWelchLearner(3, 0.001)
            };
            List<int> datasets = new List<int>(){1, 2, 3};
            Benchmarker.Run(learners, datasets);
            Console.ReadLine();
        }
    }
}