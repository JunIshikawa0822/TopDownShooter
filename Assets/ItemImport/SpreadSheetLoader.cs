using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;

public static class SpreadsheetLoader
{
    // フェーズ2: 非同期通信で文字列を取得
    public static async Task<string> DownloadCsvAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            // サーバーからのレスポンスを待つ（await）
            return await client.GetStringAsync(url);
        }
    }

    // フェーズ1: URLをブラウザ用からCSV用へ変換
    public static string ConvertToCsvUrl(string url)
    {
        //Google SpreadsheetsのURLの "/edit..." 以降を "/export?format=csv" に差し替える
        if (url.Contains("/edit"))
        {
            // #記号以降（特定のシート指定など）を削除してから置換
            int editIndex = url.IndexOf("/edit");
            //0文字目から/editが登場するまでのurl文字列
            string baseUrl = url.Substring(0, editIndex);
            
            // gidの抽出
            string gidParam = "";
            //gid=でページ数がわかる
            if (url.Contains("gid="))
            {
                int gidStart = url.IndexOf("gid=");
                string sub = url.Substring(gidStart); // "gid=0#gid=0" のように取得

                // '#' 以降を切り捨てる処理
                if (sub.Contains("#"))
                {
                    sub = sub.Split('#')[0]; // '#' で分割して最初（[0]）だけ取る
                }
                gidParam = "&" + sub;
            }

            return $"{baseUrl}/export?format=csv{gidParam}";
        }

        Debug.LogError($"{url}はスプレッドシートに対応していません");
        return url;
    }

    // パース
    public static List<string[]> ParseCsv(string csvText)
    {
        List<string[]> rows = new List<string[]>();
        string[] lines = csvText.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        //,(?=)　次の事柄が満たされる,を探す
        //(?:) 非キャプチャグループ　ただ単にひとかたまりとして扱いたいだけで、中身を記憶しておく必要はない

        //[^\"]* 「"」でない文字の繰り返し
        //\"[^\"]*\" 「"」と「"」でない文字と「"」　つまり「"」に囲まれた場所
        //[^\"]* 「"」でない文字がの繰り返し
        //右側（行末まで）以上の3ブロックで構成される,だけを探す
        Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

        foreach (string line in lines)
        {
            //一行をfieldの集まりに分離
            string[] fields = csvParser.Split(line);
            for (int i = 0; i < fields.Length; i++) fields[i] = fields[i].Trim(' ', '"');
            rows.Add(fields);
        }
        return rows;
    }
}