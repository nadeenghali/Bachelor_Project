print(__doc__)

import numpy as np
import matplotlib.pyplot as plt

from sklearn.svm import SVC
from sklearn.model_selection import train_test_split
from sklearn.datasets import load_breast_cancer

from sklearn.datasets import make_multilabel_classification
from sklearn.multiclass import OneVsRestClassifier
from sklearn.preprocessing import LabelBinarizer
from sklearn.decomposition import PCA
from sklearn.cross_decomposition import CCA

cancer = load_breast_cancer()
X_train, X_test, Y_train, Y_test = train_test_split(cancer.data, cancer.target, random_state=0)

svm= SVC()
svm.fit(X_train,Y_train)

print("{:.3f}".format(svm.score(X_train,Y_train)))
print("{:.3f}".format(svm.score(X_test,Y_test)))

min_train = X_train.min(axis=0)
range_train = (X_train - min_train).max(axis=0)

X_train_scaled = (X_train - min_train)/range_train

X_test_scaled = (X_test - min_train)/range_train

svm= SVC()
svm.fit(X_train_scaled,Y_train)


print("{:.3f}".format(svm.score(X_train_scaled,Y_train)))
print("{:.3f}".format(svm.score(X_test_scaled,Y_test)))