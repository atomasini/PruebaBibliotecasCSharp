using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HelperJsonToCsv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese la ruta del archivo JSON de entrada:");
            string jsonFilePath = Console.ReadLine();

            //Console.WriteLine("Ingrese la ruta del archivo CSV de salida:");
            //string csvFilePath = Console.ReadLine();

            // Validar si el archivo JSON existe
            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine("El archivo JSON de entrada no existe.");
                return;
            }

            // Obtener el nombre del archivo JSON sin la extensión
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jsonFilePath);

            // Construir la ruta del archivo CSV de salida con el mismo nombre
            string csvFilePath = Path.Combine(Path.GetDirectoryName(jsonFilePath), $"{fileNameWithoutExtension}.csv");

            // Validar si el archivo CSV de salida ya existe
            if (File.Exists(csvFilePath))
            {
                Console.WriteLine("El archivo CSV de salida ya existe. No se puede sobrescribir.");
                return;
            }

            // Leer el archivo JSON
            string jsonContent = File.ReadAllText(jsonFilePath);

            // Convertir el JSON a una lista de diccionarios
            List<Dictionary<string, object>> jsonData;
            try
            {
                jsonData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonContent);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al deserializar el JSON: {ex.Message}");
                return;
            }


            // Obtener las claves de las propiedades del primer elemento como encabezados de columna
            if (jsonData.Count == 0)
            {
                Console.WriteLine("El archivo JSON no contiene datos.");
                return;
            }

            // Obtener las claves de las propiedades del primer elemento como encabezados de columna

            string[] headers = jsonData[0].Keys.ToArray();

            // Crear el contenido CSV
            string csvContent = string.Join(",", headers) + Environment.NewLine;

            foreach (var item in jsonData)
            {
                var values = item.Values;
                string csvLine = string.Join(",", values);
                csvContent += csvLine + Environment.NewLine;
            }

            // Escribir el contenido CSV en el archivo
            try
            {
                File.WriteAllText(csvFilePath, csvContent);
                Console.WriteLine("La conversión de JSON a CSV se ha completado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir en el archivo CSV: {ex.Message}");
            }

        }

    }
}