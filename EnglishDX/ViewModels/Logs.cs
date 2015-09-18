using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EnglishDX {
  public static  class Logs {
    
      public static void Write(string st) {
          Log lg = ViewModel.generalEntity.Logs.Create();
          lg.DTime = DateTime.Now;
          lg.Message = st;
          ViewModel.generalEntity.Logs.Add(lg);
          WriteToFile(lg);
      }
      public static void Write(MyWord wrd,bool IsRight) {
          Log lg = ViewModel.generalEntity.Logs.Create();
          lg.DTime = DateTime.Now;
          lg.Datum = wrd.parentWordEntity;
          lg.Result = IsRight.ToString();
         // lg.Message = st;
          ViewModel.generalEntity.Logs.Add(lg);
          WriteToFile(lg);
        
      }

      static void WriteToFile(Log lg) {
          StreamWriter sw = new StreamWriter("log.txt",true);
          string st;
          if (lg.Datum != null) {
               st = string.Format("{0} | {1} | {2} | {3}", lg.DTime, lg.WordId,lg.Datum.Word, lg.Result);
          }
          else {
               st = string.Format("{0} | {1}", lg.DTime, lg.Message);
          }
          sw.WriteLine(st);
          sw.Close();
      }
    }
}
