import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

from sklearn.svm import SVC
from sklearn.model_selection import train_test_split
from sklearn.datasets import load_breast_cancer

from sklearn.datasets import make_multilabel_classification
from sklearn.multiclass import OneVsRestClassifier
from sklearn.preprocessing import LabelBinarizer
from sklearn.decomposition import PCA
from sklearn.cross_decomposition import CCA

from sklearn.datasets import load_files
from sklearn.cross_validation import train_test_split

# bunch = load_files('../KinectDatasetTest')

# X = np.loadtxt('KinectDatasetTest.csv', delimiter=',')

# X, y = np.arange(10).reshape((5, 2)), range(5)

# X_trn, X_tst, y_trn, y_tst = train_test_split(X, y, test_size=0.5, random_state=42)

# X_train, X_test, Y_train, Y_test = train_test_split(data.data, data.target, random_state=0)

# X_trn, X_tst, y_trn, y_tst = train_test_split(bunch.data, bunch.target, test_size=0.2, random_state=0)


dataset = pd.read_csv('KinectDatasetTest.csv')
X = dataset.drop('column_9', 1).values
y = dataset['column_9'].values
X_train, X_test, Y_train, Y_test = train_test_split(dataset.data, dataset.target, random_state=0)

svm= SVC()
svm.fit(X_trn,y_trn)

print("{:.3f}".format(svm.score(X_trn,y_trn)))
print("{:.3f}".format(svm.score(X_tst,y_tst)))