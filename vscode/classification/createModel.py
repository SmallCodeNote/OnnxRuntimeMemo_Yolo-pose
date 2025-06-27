import pandas as pd
import numpy as np
import sys
import os
#from sklearn.experimental import enable_hist_gradient_boosting
from sklearn.ensemble import HistGradientBoostingClassifier
from sklearn.model_selection import train_test_split
import joblib

labeledFilePath = sys.argv[1]
modelFilePath = (
    sys.argv[2] if len(sys.argv) > 2
    else os.path.splitext(labeledFilePath)[0] + ".joblib"
)

df = pd.read_csv(labeledFilePath)
df.replace(-1, np.nan, inplace=True)
df_train = df[df["Label"].notna()]
X = df_train.drop(columns=["frame", "Label"])
y = df_train["Label"]

if X is not None and len(X) > 0:
    model = HistGradientBoostingClassifier()
    model.fit(X, y)
    joblib.dump(model, modelFilePath)
    print(f"Model saved to: {modelFilePath}")
else:
    print("data empty")


