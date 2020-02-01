using System;
using System.IO;

/*
TODO
Add lists?
Find                DONE
    Move back path  DONE
Update
Enter
Delete
    Entry       
    Container   
*/

public class crow : IDisposable {
    private bool disposed = false;
    private System.ComponentModel.Component comp = new System.ComponentModel.Component();
    
    const string DelimContent = ":";
    const string DelimStruct = "'";
    const string DelimPath = ".";

    const string ErrNotFound = DelimContent +"ERR_NOT_FOUND" + DelimContent;
    const string ErrContainer = DelimContent + "ERR_CONTAINER" + DelimContent;

    static string Filepath = "";
    static string[] Contents = new string[0];

    public crow(string f) {
        Filepath = string.Format(@"{0}",f);
        Contents = File.ReadAllLines(f);
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if(!this.disposed) {
            if(disposing) {
                comp.Dispose();
            }
            disposed = true;
        }
    }

    ~crow() {
        Dispose(false);
    }

    private static string fLeft(string s, int l) {
        return s.Substring(0, l);
    }
    
    private static string fRight(string s, int l) {
        return s.Substring(s.Length-l);
    }

    private static string fGetEntryAttr(string s) {
        return fLeft(s, s.IndexOf(DelimContent)).Trim();
    }

    private static string fGetEntryVal(string s) {
        return fRight(s, s.Length-s.IndexOf(DelimContent)-1).Trim();
    }

    private static string fUpdateVal(string Content, string NewVal) {
        return fLeft(Content, 1+Content.IndexOf(DelimContent)) + " " + NewVal;
    }

    public string Find(string EntryAttr) {
        string rVal = ErrNotFound;
        string CurrPath = "";
        bool PathReset = false;

        for(int n = 0; n < Contents.Length; n++) {
            if(PathReset == true) {
                CurrPath = fLeft(CurrPath, CurrPath.LastIndexOf(DelimPath));
            }

            if(false) {
                //reserved for debuging
            } else if(Contents[n].Trim().ToString() == DelimContent) {
                PathReset = true;
                Console.WriteLine(CurrPath);
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length == Contents[n].Trim().IndexOf(DelimContent) + 1) {
                PathReset = false;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
                Console.WriteLine(CurrPath);
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length > Contents[n].Trim().IndexOf(DelimContent) - 1) {
                PathReset = true;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            }

            if(CurrPath == DelimPath + EntryAttr) {
                rVal = fGetEntryVal(Contents[n]);
                break;
            }
        }
        return "'" + rVal + "'";
    }

    
    public void EntryUpd(string EntryAttr, string EntryVal) {
        File.WriteAllLines(Filepath, Contents, System.Text.Encoding.UTF8);
    }
}