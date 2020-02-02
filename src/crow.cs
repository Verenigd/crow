using System;
using System.IO;
using System.Collections.Generic;

/*
TODO
Find
    Initial         DONE
    Move back path  DONE
Update
    Initial         DONE
Add
    Entry
    Container
Delete
    Entry
    Container
Add lists?
*/

public class crow : IDisposable {
    private bool disposed = false;
    private System.ComponentModel.Component comp = new System.ComponentModel.Component();
    
    const string DelimContent = ":";
    const string DelimStruct = "'";
    const string DelimPath = ".";
    const string PathIndent = "    ";

    const string ErrNotFound = DelimContent +"ERR_NOT_FOUND" + DelimContent;
    //const string ErrContainer = DelimContent + "ERR_CONTAINER" + DelimContent;
    //const string ErrListNull = DelimContent + "ERR_LIST_NULL" + DelimContent;

    static string Filepath = "";
    static List<string> Contents = new List<string>();
    //static List<string> ContentList = new List<string>();

    public crow(string f) {
        Filepath = string.Format(@"{0}",f);
        Contents = new List<string>(File.ReadAllLines(f));
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

    private string fGetPriorPath(string s) {
        return fLeft(s, s.LastIndexOf(DelimPath)).Trim();
    }

    private string fGetLastAttr(string s) {
        return fRight(s, s.LastIndexOf(DelimPath)+2).Trim();
    }

    private static string fGetEntryVal(string s) {
        return fRight(s, s.Length-s.IndexOf(DelimContent)-1).Trim();
    }

    private int fGetPath(string EntryAttr) {
        int rIndex = -1; //-1 As not found
        string CurrPath = "";
        bool PathReset = false;

        for(int n = 0; n < Contents.Count; n++) {
            if(PathReset == true) {
                CurrPath = fLeft(CurrPath, CurrPath.LastIndexOf(DelimPath));
            }

            if(false) {
                //reserved for debuging
            } else if(Contents[n].Trim().ToString() == DelimContent) {
                PathReset = true;
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length == Contents[n].Trim().IndexOf(DelimContent) + 1) {
                PathReset = false;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length > Contents[n].Trim().IndexOf(DelimContent) - 1) {
                PathReset = true;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            }

            if(CurrPath == DelimPath + EntryAttr) {
                rIndex = n;
                break;
            }
        }
        return rIndex;
    }

    private void sWriteFile() {
        File.WriteAllLines(Filepath, Contents, System.Text.Encoding.UTF8);
    }

    public string Find(string EntryAttr) {
        string rVal = ErrNotFound;
        int rIndex = fGetPath(EntryAttr);

        rVal = rIndex == -1 ? ErrNotFound : fGetEntryVal(Contents[rIndex].ToString());
        return rVal;
    }

    public void Update(string EntryAttr, string EntryVal) {
        int rIndex = fGetPath(EntryAttr);

        if(rIndex != -1) {
            Contents[rIndex] = fLeft(Contents[rIndex].ToString(), 1+Contents[rIndex].ToString().IndexOf(DelimContent)) + " " + EntryVal;
        }
        sWriteFile();
    }

    public void AddEntry(string EntryAttr, string EntryVal) {
        string AppendPath = fGetPriorPath(EntryAttr);
        string NewKey = fGetLastAttr(EntryAttr);
        int Position = -1;
        int Indentation = (EntryAttr.Length - EntryAttr.Replace(".", "").Length);
        
        if((Position = fGetPath(AppendPath)) != -1) {
            Contents.Insert(Position+1, String.Concat(System.Linq.Enumerable.Repeat(PathIndent, Indentation)) + NewKey + DelimContent + " " + EntryVal);
            sWriteFile();
        }
    }
}