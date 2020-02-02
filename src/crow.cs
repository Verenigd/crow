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

    const string ErrNotFound = DelimContent +"ERR_NOT_FOUND" + DelimContent;
    const string ErrContainer = DelimContent + "ERR_CONTAINER" + DelimContent;

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

    private static string fGetEntryVal(string s) {
        return fRight(s, s.Length-s.IndexOf(DelimContent)-1).Trim();
    }

    private int GetPath(string EntryAttr) {
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

    public string Find(string EntryAttr) {
        string rVal = ErrNotFound;
        int rIndex = GetPath(EntryAttr);

        rVal = rIndex == -1 ? ErrNotFound : fGetEntryVal(Contents[rIndex].ToString());
        return rVal;
    }

    public void Update(string EntryAttr, string EntryVal) {
        int rIndex = GetPath(EntryAttr);

        if(rIndex != -1) {
            Contents[rIndex] = fLeft(Contents[rIndex].ToString(), 1+Contents[rIndex].ToString().IndexOf(DelimContent)) + " " + EntryVal;
        }

        File.WriteAllLines(Filepath, Contents, System.Text.Encoding.UTF8);
    }

    /*public void AddEntry(string EntryAttr, string EntryVal) {
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
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length == Contents[n].Trim().IndexOf(DelimContent) + 1) {
                PathReset = false;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length > Contents[n].Trim().IndexOf(DelimContent) - 1) {
                PathReset = true;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            }

            if(CurrPath == DelimPath + EntryAttr) {
                //Contents[]
                File.WriteAllLines(Filepath, Contents, System.Text.Encoding.UTF8);
                break;
            }
        }
    }*/
}