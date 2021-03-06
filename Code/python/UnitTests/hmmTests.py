import unittest
from Models.hmm import HMM

class TestHMM(unittest.TestCase):
    def test_hmm_3States_2Sym_1Char(self):
        hmm = HMM(3, 2, False)
        hmm.emission_matrix = [[0.5, 0.3, 0.2], [0, 1, 0], [0.1, 0.1, 0.8]]
        hmm.initial_matrix = [0.1, 0.2, 0.7]
        hmm.transition_matrix = [[0, 1, 0], [0.85, 0.1, 0.05], [0.3, 0.2, 0.5]]
        self.assertAlmostEqual(hmm.calc_probability([0]), 0.0322, 7)

    def test_hmm_2States_3Sym_1Char(self):
        hmm = HMM(2, 3, False)
        hmm.emission_matrix = [[0.1, 0.5, 0.2, 0.2], [0, 0, 0.5, 0.5]]
        hmm.initial_matrix = [0.2, 0.8]
        hmm.transition_matrix = [[0.8, 0.2], [0.8, 0.2]]
        self.assertAlmostEqual(hmm.calc_probability([0]), 0.0052, 7)
        self.assertAlmostEqual(hmm.calc_probability([1]), 0.026, 7)
        self.assertAlmostEqual(hmm.calc_probability([2]), 0.1144, 7)
        self.assertAlmostEqual(hmm.calc_probability([]), 0.44, 7)
