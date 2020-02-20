using System;

static class test {
    static void Main(string[] args) {
        using(crow f = new crow("AT.crow")) {
            Chronos t = new Chronos();
            if(args[0] == "f") { //Find
                string s = "";
                s = f.Find(args[1]);
                Console.WriteLine("'" + s + "'");
            } else if(args[0] == "ue") { //Update entry
                f.Update(args[1],args[2]);
            } else if(args[0] == "ea") { //Entry add
                f.EntryAdd(args[1], args[2], Int32.Parse(args[3]));
            } else if(args[0] == "ac") { //Add container
                f.AddContainer(args[1]);
            } else if(args[0] == "de") { //Delete entry
                f.EntryDel(args[1]);
            } else if(args[0] == "dc") { //Delete container
                f.DelContainer(args[1]);
            } else if(args[0] == "t") { //Testing
                Console.WriteLine(f.TestingNew(args[1]));
            }
        }
    }
}