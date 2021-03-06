﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Statistics.Models.Markov;

namespace ModelLearning.Learners {
    class KentManfredLearner : Learner {

        private Random ran;
        private HiddenMarkovModel bestHmm;
        private double bestLikelihood;

        //settings
        private int maxStates;
        private double threshold;


        public KentManfredLearner(int max_states, double threshold) {
            this.maxStates = max_states;
            ran = new Random();     
            this.threshold = threshold;
        }

        private HMMGraph Random2NodeGraph(int num_symbols) {
            HMMGraph g = new HMMGraph(num_symbols);
            Node n1 = new Node();
            //Node n2 = new Node();
            n1.SetTransition(n1, ran.NextDouble());
            //n2.SetTransition(n1, ran.NextDouble());
            for (int i = 0; i < num_symbols; i++) {
                n1.SetEmission(i, ran.NextDouble());
                //n2.SetEmission(i, ran.NextDouble());
            }
            n1.InitialProbability = ran.NextDouble();
            //n2.InitialProbability = ran.NextDouble();
            g.AddNode(n1);
            //g.AddNode(n2);
            g.Normalize();
            return g;
        }

        /// <summary>
        /// Returns the loglikelihood that the graph have generated the given sequences
        /// </summary>
        /// <param name="g"></param>
        /// <param name="evaluationData"></param>
        /// <returns></returns>
        public double LogLikelihood(HiddenMarkovModel hmm, SequenceData evaluationData) {
            double loglikelihood = 0;
            for (int i = 0; i < evaluationData.Count; i++)
                loglikelihood += hmm.Evaluate(evaluationData[i], true);
            return loglikelihood;
        }

        public override void Learn(SequenceData trainingData, SequenceData validationData, SequenceData testData) {
            HMMGraph graph = Random2NodeGraph(trainingData.NumSymbols);
            bestHmm = ModelConverter.Graph2HMM(graph);
            bestLikelihood = LogLikelihood(bestHmm, trainingData);
            int start_states = bestHmm.States;
            for (int expands = 0; expands < maxStates - start_states; expands++){
                WriteLine("Number of states: " + bestHmm.States);
                //each iteration will extend the graph with one additional node
                //try adding a new node with random parameters n different times and choose the best solution
                HiddenMarkovModel current_best_hmm = null;
                for (int i = 0; i < 10; i++) {
                    graph = ModelConverter.HMM2Graph(bestHmm);
                    RandomlyExtendGraphSparsely(graph);
                    HiddenMarkovModel hmm = ModelConverter.Graph2HMM(graph);
                    hmm.Learn(testData.GetNonempty(), 10); //Run the BaumWelch algorithm
                    double likelihood = LogLikelihood(hmm, validationData);
                    if (likelihood > bestLikelihood) {
                        bestLikelihood = likelihood;
                        current_best_hmm = hmm;
                        WriteLine("+");
                    }
                    else {
                        WriteLine("-");
                    }
                }
                if (current_best_hmm != null){
                    bestHmm = current_best_hmm;
                    WriteLine("Likelihood increased to " + bestLikelihood);
                }
                else
                    WriteLine("Likelihood stays the same");
            }
            WriteLine("Runs Baum Welch last time with the right threshold");
            bestHmm.Learn(trainingData.GetNonempty(), threshold);
        }


        /// <summary>
        /// Extends the graph by adding a new node with random transitions, emissions and initial prabability.
        /// The new node will be connected to Log n random nodes (back and forth) with random transition, emission and initial probabilities.
        /// </summary>
        /// <param name="g"></param>
        private void RandomlyExtendGraphSparsely(HMMGraph g) {
            //add a new node
            Node new_node = new Node();
            g.AddNode(new_node);

            //Set random initial probability
            new_node.InitialProbability = ran.NextDouble();

            //Set random emission probabilities for the new node
            for (int i = 0; i < g.NumSymbols; i++ )
                new_node.SetEmission(i, ran.NextDouble());

            //Set random transition probabilities from log n random nodes to the new node (in both directions)
            int out_degree = (int)Math.Ceiling(Math.Log(g.NumNodes));
            List<Node> node_list_copy = g.Nodes.Select(n => n).ToList();
            Utilities.Shuffle(node_list_copy);
            for (int i = 0; i < out_degree; i++) {
                node_list_copy[i].SetTransition(new_node, ran.NextDouble());
                new_node.SetTransition(node_list_copy[i], ran.NextDouble());
            }

            //Normalize graph
            g.Normalize();
        }

        /// <summary>
        /// Shuffle any (I)List with an extension method based on the Fisher-Yates shuffle :
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void Shuffle<T>(IList<T> list) {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public override string Name() {
            return "KentManfredLearner";
        }

        public override double CalculateProbability(int[] sequence) {
            if (sequence.Length == 0)
                return 1.0;
            else
                return bestHmm.Evaluate(sequence);
        }

        public override void Initialise(LearnerParameters parameters, int iteration)
        {
            throw new NotImplementedException();
        }

        public override void Save(System.IO.StreamWriter outputWriter, System.IO.StreamWriter csvWriter)
        {
            throw new NotImplementedException();
        }

    }
}
