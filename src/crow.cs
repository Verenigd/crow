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
    Entry           DONE
        Insert      DONE
        Add
    Container       DONE
Delete
    Entry           DONE
    Container       DONE
Add lists
Indentations        Meh...
*/

/*
Bugs
    Delete entry also deletes container start   DONE
*/

public class crow : IDisposable {
    private bool disposed = false;
    public bool Indentation = true;
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
        return fRight(s, s.Length - s.LastIndexOf(DelimPath)-1).Trim();
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

    public int fContainerEnd(string EntryPath) {
        int rIndex = -1; //-1 As not found
        string CurrPath = "";
        bool PathReset = false;
        int rInside = 0;
        bool rStart = false;

        for(int n = 0; n < Contents.Count; n++) {
            if(PathReset == true) {
                CurrPath = fLeft(CurrPath, CurrPath.LastIndexOf(DelimPath));
            }

            if(rStart == false) {
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
            } else {
                if(Contents[n].ToString().Trim() == DelimContent) {
                    rInside -= 1;
                } else if(Contents[n].IndexOf(DelimContent) > 0 && Contents[n].Trim().Length == Contents[n].Trim().IndexOf(DelimContent) + 1) {
                    rInside += 1;
                }
            }

            if(CurrPath == DelimPath + EntryPath && rStart == false) {
                rInside += 1;
                rStart = true;
            }

            if(rStart == true && rInside == 0) {
                rIndex = n;
                break;
            }
        }
        return rIndex;
    }

    private void sWriteFile() {
        File.WriteAllLines(Filepath, Contents, System.Text.Encoding.UTF8);
    }

    public string Find(string EntryPath) {
        string rVal = ErrNotFound;
        int rIndex = fGetPath(EntryPath);

        rVal = rIndex == -1 ? ErrNotFound : fGetEntryVal(Contents[rIndex].ToString());
        return rVal;
    }

    public void Update(string EntryPath, string EntryVal) {
        int rIndex = fGetPath(EntryPath);

        if(rIndex != -1) {
            Contents[rIndex] = fLeft(Contents[rIndex].ToString(), 1+Contents[rIndex].ToString().IndexOf(DelimContent)) + " " + EntryVal;
        }
        sWriteFile();
    }

    public void AddEntry(string EntryPath, string EntryVal) {
        string AppendPath = fGetPriorPath(EntryPath);
        string NewKey = fGetLastAttr(EntryPath);
        int Position = -1;
        
        if((Position = fGetPath(AppendPath)) != -1) {
            int IndentationLevel = (EntryPath.Length - EntryPath.Replace(".", "").Length);
            string IntendationString = Indentation ? String.Concat(System.Linq.Enumerable.Repeat(PathIndent, IndentationLevel)) : "";
            Contents.Insert(Position+1, IntendationString + NewKey + DelimContent + " " + EntryVal);
            sWriteFile();
        }
    }

    public void EntryStack() {

    }

    public void EntryInsert() {

    }

    public void EntryDel(string EntryPath) {
        int Position = -1;
        
        if((Position = fGetPath(EntryPath)) != -1) {
            if(fGetEntryVal(Contents[Position].ToString().Trim()) != "") {
                Contents.RemoveAt(Position);
                sWriteFile();
            }
        }
    }

    public void AddContainer(string EntryPath) {
        string AppendPath = fGetPriorPath(EntryPath);
        string NewKey = fGetLastAttr(EntryPath);
        int Position = -1;
        
        if((Position = fGetPath(AppendPath)) != -1) {
            int IndentationLevel = Indentation ? (EntryPath.Length - EntryPath.Replace(".", "").Length) : 0;
            string IntendationString = Indentation ? String.Concat(System.Linq.Enumerable.Repeat(PathIndent, IndentationLevel)) : "";
            Contents.Insert(Position+1, IntendationString + NewKey + DelimContent);
            Contents.Insert(Position+2, IntendationString + DelimContent);
            sWriteFile();
        }
    }

    public void DelContainer(string EntryPath) {
        int PositionIni = -1;
        int PositionEnd = -1;
        
        if((PositionIni = fGetPath(EntryPath)) != -1 && (PositionEnd = fContainerEnd(EntryPath)) != -1) {
            Contents.RemoveRange(PositionIni, PositionEnd - PositionIni + 1);
            sWriteFile();
        }
    }

    public string TestingNew(string s) {
        return fGetEntryVal(s);
    }
}