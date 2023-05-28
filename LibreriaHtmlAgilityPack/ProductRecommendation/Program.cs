﻿using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using ProductRecommendation;
using System;
using System.IO;

using Dominio;

internal class Program
{

    public static string GetAbsolutePath(string relativeDatasetPath)
    {
        FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
        string assemblyFolderPath = _dataRoot.Directory.FullName;

        string fullPath = Path.Combine(assemblyFolderPath, relativeDatasetPath);

        return fullPath;
    }


    private static string BaseDataSetRelativePath = @"../../../DATA";
    private static string TrainingDataRelativePath = $"{BaseDataSetRelativePath}/Amazon0302.txt";
    private static string TrainingDataLocationRelative = GetAbsolutePath(TrainingDataRelativePath);
    private static void Main(string[] args)
    {
        /*********************SQL*********/
        var productosSql = new ProductosSQL();
        // Obtiene la lista de productos
        var productos = productosSql.GetProductos();

        foreach (var producto in productos)
        {
            Console.WriteLine($"ID: {producto.Id}, Nombre: {producto.Nombre}");
        }

        // Obtiene la lista de productos
        /*******************************/

        //STEP 1: Create MLContext to be shared across the model creation workflow objects
        MLContext mlContext = new MLContext();


        //STEP 2: Read the trained data using TextLoader by defining the schema for reading the product co-purchase dataset
        //        Do remember to replace amazon0302.txt with dataset from https://snap.stanford.edu/data/amazon0302.html
        // Especifica la ubicación real de tus datos de entrenamiento 
        //string TrainingDataLocation = "C:\\web3\\Pruebas\\LibreriaHtmlAgilityPack\\ProductRecommendation\\Data\\Amazon0302.txt";
        string TrainingDataLocation = TrainingDataLocationRelative;
         var traindata = mlContext.Data.LoadFromTextFile(path: TrainingDataLocation,
                                                          columns: new[]
                                                          {
                                                                    new TextLoader.Column("Label", DataKind.Single, 0),
                                                      new TextLoader.Column(name:nameof(ProductEntry.ProductID), dataKind:DataKind.UInt32, source: new [] { new TextLoader.Range(0) }, keyCount: new KeyCount(262111)),
                                                      new TextLoader.Column(name:nameof(ProductEntry.CoPurchaseProductID), dataKind:DataKind.UInt32, source: new [] { new TextLoader.Range(1) }, keyCount: new KeyCount(262111))
                                                          },
        hasHeader: true,
                                                          separatorChar: '\t');

        //STEP 3: Your data is already encoded so all you need to do is specify options for MatrxiFactorizationTrainer with a few extra hyperparameters
        //        LossFunction, Alpa, Lambda and a few others like K and C as shown below and call the trainer.
        MatrixFactorizationTrainer.Options options = new MatrixFactorizationTrainer.Options();
        options.MatrixColumnIndexColumnName = nameof(ProductEntry.ProductID);
        options.MatrixRowIndexColumnName = nameof(ProductEntry.CoPurchaseProductID);
        options.LabelColumnName = "Label";
        options.LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass;
        options.Alpha = 0.01;
        options.Lambda = 0.025;
        // For better results use the following parameters
        //options.K = 100;
        //options.C = 0.00001;

        //Step 4: Call the MatrixFactorization trainer by passing options.
        var est = mlContext.Recommendation().Trainers.MatrixFactorization(options);


        //STEP 5: Train the model fitting to the DataSet
        //Please add Amazon0302.txt dataset from https://snap.stanford.edu/data/amazon0302.html to Data folder if FileNotFoundException is thrown.
        ITransformer model = est.Fit(traindata);

        //STEP 6: Create prediction engine and predict the score for Product 63 being co-purchased with Product 3.
        //The higher the score the higher the probability for this particular productID being co-purchased
        var predictionengine = mlContext.Model.CreatePredictionEngine<ProductEntry, Copurchase_prediction>(model);
        ProductEntry product = new ProductEntry()
        {
            ProductID = 3,
            CoPurchaseProductID = 67
        };
        var prediction = predictionengine.Predict(product);

        Console.WriteLine("\n For ProductID = " + product.ProductID + " and  CoPurchaseProductID = " + product.CoPurchaseProductID + " the predicted score is " + Math.Round(prediction.Score, 1) * 100 + "%");


        // find the top 5 combined products for product 6
        Console.WriteLine("Calculating the top 5 products for product 3...");
        var top5 = (from m in Enumerable.Range(1, 100)
                    let p = predictionengine.Predict(
                       new ProductEntry()
                       {
                           ProductID = 3,
                           CoPurchaseProductID = (uint)m
                       })
                    orderby p.Score descending
                    select (ProductID: m, p.Score)).Take(5);
        foreach (var t in top5)
            Console.WriteLine($"  Score:{t.Score}\tProduct: {t.ProductID}");


        Console.ReadLine();
    }
}

public class Copurchase_prediction
{
    public float Score { get; set; }
}

public class ProductEntry
{
    [KeyType(count: 262111)]
    public uint ProductID { get; set; }

    [KeyType(count: 262111)]
    public uint CoPurchaseProductID { get; set; }
}


