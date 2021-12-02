using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lanuguage;
using System.Data;
using System.IO;
using Excel;
using UnityEditor;

namespace EditorTool
{
    public class ExcelConfig
    {
        public static readonly string excelsFolderPath = Application.dataPath + "/Excels/";
        public static readonly string assetPath = "Assets/Resources/DataAssets/";
    }

    public class ExcelTool
    {
        public static List<LanguageData> CreateLanguageArrayWithExcel(string filePath)
        {

            List<LanguageData> dataList = new List<LanguageData>();
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);

            //根据excel的定义，第二行开始才是数据
            //LanguageData[] array = new LanguageData[rowNum - 1];
            for (int i = 1; i < rowNum; i++)
            {
                LanguageData item = new LanguageData();
                //解析每列的数据
                item.Key = collect[i][0].ToString();
                item.Chinese = collect[i][1].ToString();
                item.English = collect[i][2].ToString();
                dataList.Add(item);
            }
            return dataList;
        }

        static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet();
            //Tables[0] 下标0表示excel文件中第一张表的数据
            columnNum = result.Tables[0].Columns.Count;
            rowNum = result.Tables[0].Rows.Count;
            return result.Tables[0].Rows;
        }

    }

    public class ExcelBuild : Editor
    {

        [MenuItem("CustomEditor/CreateItemAsset")]
        public static void CreateItemAsset()
        {
            //LanguageManager manager;
            //manager = Resources.Load<LanguageManager>("DataAssets/Language");
            //if (manager == null)
            //{
            //    manager = ScriptableObject.CreateInstance<LanguageManager>();
            //    if (!Directory.Exists(ExcelConfig.assetPath))
            //    {
            //        Directory.CreateDirectory(ExcelConfig.assetPath);
            //    }

            //    //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            //    string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Language");
            //    //生成一个Asset文件
            //    AssetDatabase.CreateAsset(manager, assetPath);
            //    AssetDatabase.SaveAssets();
            //    AssetDatabase.Refresh();
            //}
            bool hasAsset = false;
            LanguageManager manager;
            manager = Resources.Load<LanguageManager>("DataAssets/Language");
            if (manager == null)
                manager = ScriptableObject.CreateInstance<LanguageManager>();
            else
                hasAsset = true;
            if (!Directory.Exists(ExcelConfig.assetPath))
            {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
            //赋值要在CreateAsset之前，否则重启会导致数据丢失
            manager.dataArray = ExcelTool.CreateLanguageArrayWithExcel(ExcelConfig.excelsFolderPath + "LanguageExcel.xlsx");

            if (!hasAsset)
            {
                //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
                string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Language");
                //生成一个Asset文件
                AssetDatabase.CreateAsset(manager, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            //赋值
            //manager.dataArray = ExcelTool.CreateLanguageArrayWithExcel(ExcelConfig.excelsFolderPath + "LanguageExcel.xlsx");


        }
    }
}


