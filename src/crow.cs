using System;
using System.IO;

/*
Errors are displayed as
    :ERR_NOT_FOUND:
    :ERR_LIST_NULL:
    :ERR_CONTAINER:
*/

public class crow : IDisposable {
    private bool disposed = false;
    private System.ComponentModel.Component comp = new System.ComponentModel.Component();
    
    const string DelimContent = ":";
    const string DelimStruct = "'";
    const string DelimPath = ".";

    const string ErrNotFound = DelimContent + "ERR_NOT_FOUND" + DelimContent;
    const string ErrListNull = DelimContent + "ERR_LIST_NULL" + DelimContent; //Set as warning?
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
        byte CurrItem = 0; //0-Null | 1-Start | 2-Item | 3-End | 4-Item | 5-Container

        for(int n = 0; n < Contents.Length; n++) {
            if(PathReset == true) {
                CurrPath = fLeft(CurrPath, CurrPath.LastIndexOf(DelimPath));
            }

            if(Contents[n].Trim() == DelimContent) { //List or container end
                PathReset = true;
                CurrItem = 3;
            } else if(Contents[n].ToString().Trim().IndexOf(DelimContent+DelimStruct) == Contents[n].Trim().Length -2) { //List start
                PathReset = false;
                CurrItem = 1;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            } else if(Contents[n].Trim().IndexOf(DelimContent) == -1) { //List item
                PathReset = false;
                CurrItem = 2;
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length == Contents[n].Trim().IndexOf(DelimContent) + 1) { //Container start
                PathReset = false;
                CurrItem = 5;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length > Contents[n].Trim().IndexOf(DelimContent) - 1) { //Entry
                PathReset = true;
                CurrItem = 4;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            }

            if(CurrPath == DelimPath + EntryAttr) {
                if(CurrItem == 1) {
                    rVal = "";
                } else if(CurrItem == 2) {
                    rVal = rVal + Contents[n].Trim() + DelimContent;
                } else if(CurrItem == 3) {
                    rVal = rVal == "" ? rVal = ErrListNull : fLeft(rVal, rVal.Length-1);
                    break;
                } else if(CurrItem == 4) {
                    rVal = fGetEntryVal(Contents[n]);
                    break;
                } else if(CurrItem == 5) {
                    rVal = DelimStruct;
                    break;
                }
            }
        }
        return rVal;
    }

    public void EntryUpd(string EntryAttr, string EntryVal) {
        string CurrPath = "";
        bool PathReset = false;
        bool CurrItem = false; //false not entry, true entry

        for(int n = 0; n < Contents.Length; n++) {
            if(PathReset == true) {
                CurrPath = fLeft(CurrPath, CurrPath.LastIndexOf(DelimPath));
            }

            if(Contents[n].Trim() == DelimContent) { //List or container end
                PathReset = true;
                CurrItem = false;
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length > Contents[n].Trim().IndexOf(DelimContent) - 1) { //Entry
                PathReset = true;
                CurrItem = true;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            } else {
                PathReset = false;
            }

            if((CurrPath == DelimPath + EntryAttr) && (CurrItem == true)) {
                Contents[n] = fLeft(Contents[n], 1+Contents[n].IndexOf(DelimContent)) + " " + EntryVal; //fUpdateVal(Contents[n], EntryVal);
                break;
            }
        }
        File.WriteAllLines(Filepath, Contents, System.Text.Encoding.UTF8);
    }

    public void EntryAdd(string EntryAttr, string EntryVal) {
        string CurrPath = "";
        bool PathReset = false;
        bool CurrItem = false; //false not entry, true entry

        for(int n = 0; n < Contents.Length; n++) {
            if(PathReset == true) {
                CurrPath = fLeft(CurrPath, CurrPath.LastIndexOf(DelimPath));
            }

            if(Contents[n].Trim() == DelimContent) { //List or container end
                PathReset = true;
                CurrItem = false;
            } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length > Contents[n].Trim().IndexOf(DelimContent) - 1) { //Entry
                PathReset = true;
                CurrItem = true;
                CurrPath = CurrPath + DelimPath + fGetEntryAttr(Contents[n]);
            } else {
                PathReset = false;
            }

            if((CurrPath == DelimPath + EntryAttr) && (CurrItem == true)) {
                Contents[n+1] = "" ;
                break;
            }
        }
        File.WriteAllLines(Filepath, Contents, System.Text.Encoding.UTF8);
    }
}