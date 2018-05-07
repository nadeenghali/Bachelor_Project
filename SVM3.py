import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
import scipy
import pyodbc

from sklearn.metrics import classification_report

from sklearn.model_selection import LeaveOneOut
from sklearn.model_selection import RandomizedSearchCV
from sklearn.model_selection import GridSearchCV
from sklearn.model_selection import cross_val_score
from sklearn.svm import SVC
from sklearn.svm import SVR
from sklearn.model_selection import train_test_split

from sklearn.datasets import make_multilabel_classification
from sklearn.multiclass import OneVsRestClassifier
from sklearn.preprocessing import LabelBinarizer
from sklearn.decomposition import PCA
from sklearn.cross_decomposition import CCA
from sklearn.cross_validation import train_test_split


# params = {'C': scipy.stats.expon(scale=100),'gamma': scipy.stats.expon(scale=0.1),
# 'kernel': ['rbf'], 'class_weight':['balanced', None]}

param_grid = [{'C':[0.1,0.5,1,5,10,100,1000], 'kernel': ['rbf'], 'gamma': [0.00001,0.018,0.0001,0.001,0.01,0.1,1]}]

# Importing the dataset
dataset = pd.read_csv('KinectDataset.csv')
X_train = dataset.iloc[:, 0:55].values
y_train = dataset.iloc[:, 55].values

datasettst = pd.read_csv('KinectDatasettst.csv')
X_test = datasettst.iloc[:, 0:55].values
y_test = datasettst.iloc[:, 55].values

datasetpred = pd.read_csv('KinectDatasetSignInAttempts.csv')
X_testpred = datasetpred.iloc[:, 0:55].values

# Splitting the dataset into the Training set and Test set
# from sklearn.model_selection import train_test_split
# X_train, X_test, y_train, y_test = train_test_split(X, y, test_size = 0.2, random_state = 0)


# manual scaling
# min_train = X_train.min(axis=0)
# range_train = (X_train - min_train).max(axis=0)

# X_train = (X_train - min_train)/range_train
# X_test = (X_test - min_train)/range_train

# Feature Scaling.
from sklearn.preprocessing import StandardScaler
sc = StandardScaler()
X_train = sc.fit_transform(X_train)
X_test = sc.fit_transform(X_test)
X_testpred = sc.fit_transform(X_testpred)


svm= SVC(probability=True, C=0.5, gamma=0.01)
svm.fit(X_train,y_train)

svmm= GridSearchCV(SVC(),param_grid,cv=4)
# svmm= RandomizedSearchCV(SVC(),params,cv=4,scoring='f1_macro')
svmm.fit(X_train,y_train)
print(svmm.best_params_)

#create the Cross validation object
# loo = LeaveOneOut()

# calculate cross validated accuracy score
scoresKfoldTrain = cross_val_score(svm, X_train, y_train, cv = 3)
scoresKfoldTest = cross_val_score(svm, X_test, y_test, cv = 3)

print("{:.3f}".format(svm.score(X_train,y_train)))
print("{:.3f}".format(svm.score(X_test,y_test)))

print(svm.predict([X_test[34]]))
print([X_test[34]])
print(svm.predict(X_testpred))
print(X_testpred)

print(svm.classes_)
print(svm.predict_proba(X_test))

print( scoresKfoldTrain.mean() )
print( scoresKfoldTest.mean() )

print(classification_report(y_test,svm.predict(X_test)))

predictions = svm.predict(X_testpred);
print(svm.predict_proba(X_testpred))

conn = pyodbc.connect(r'Driver={SQL Server};Server={nadeens-pc\sqlexpress};Database=KinectDatabaseForTen;Trusted_Connection=yes;')
cursor = conn.cursor()
cursor.execute("Select * from Users where Id=" + str(predictions[len(predictions)-1]))

row = cursor.fetchone()

print(float(svm.predict_proba(X_testpred)[len(X_testpred)-1][predictions[len(predictions)-1]-2]))

if float(svm.predict_proba(X_testpred)[len(X_testpred)-1][predictions[len(predictions)-1]-2]) > 0.30:
	if row:
		print(row.User_Name)
else: print("No match found!")

conn.close()