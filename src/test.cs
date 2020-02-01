using System;

static class test {
    static void Main(string[] args) {
        using(crow f = new crow("AT.crow")) {
            if(args[0] == "f") {
                Console.WriteLine(f.Find(args[1]));
            } else if(args[0] == "ue") {
                f.EntryUpd(args[1],args[2]);
            }
        }
    }
}