import sys
import pandas as pd
import joblib
import os

def main():
    if len(sys.argv) < 3:
        print("Usage: python clssification.py <modelFilePath> <poseFilePath> [outputFilePath]")
        return

    modelFilePath = sys.argv[1]
    poseFilePath = sys.argv[2]
    outputFilePath = sys.argv[3] if len(sys.argv) > 3 else generate_output_path(poseFilePath)

    model = joblib.load(modelFilePath)

    df = pd.read_csv(poseFilePath)
    if df.empty:
        print("Pose file is empty. Skipping prediction.")
        return

    X = df.drop(columns=['frame', 'Label'], errors='ignore')
    df['Label'] = model.predict(X)
    df.to_csv(outputFilePath, index=False)
    print(f"save: {outputFilePath}")

def generate_output_path(base_path):
    name, ext = os.path.splitext(base_path)
    return f"{name}UpdateLabeled{ext}"

if __name__ == "__main__":
    main()
