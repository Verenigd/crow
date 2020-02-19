using System;

static class test {
    static void Main(string[] args) {
        using(crow f = new crow("AT.crow")) {
            Chronos t = new Chronos();
            if(args[0] == "f") {
                string s = "";
                //t.Start();
                //try {
                s = f.Find(args[1]);
                //} catch {
                //}
                //t.Stop();
                Console.WriteLine("'" + s + "'");
                //Console.WriteLine(t.GetMilliseconds());
            } else if(args[0] == "ue") {
                //t.Start();
                f.Update(args[1],args[2]);
                //t.Stop();
                //Console.WriteLine(t.GetMilliseconds());
            } else if(args[0] == "ae") {
                //t.Start();
                f.AddEntry(args[1], args[2]);
                //t.Stop();
                //Console.WriteLine(t.GetMilliseconds());
            } else if(args[0] == "ac") {
                //t.Start();
                f.AddContainer(args[1]);
                //t.Stop();
                //Console.WriteLine(t.GetMilliseconds());
            } else if(args[0] == "de") {
                //t.Start();
                f.EntryDel(args[1]);
                //t.Stop();
                //Console.WriteLine(t.GetMilliseconds());
            } else if(args[0] == "dc") {
                //t.Start();
                f.DelContainer(args[1]);
                //t.Stop();
                //Console.WriteLine(t.GetMilliseconds());
            } else if(args[0] == "t") {
                //t.Start();
                Console.WriteLine(f.TestingNew(args[1]));
                //t.Stop();
                //Console.WriteLine(t.GetMilliseconds());
            }
        }
    }
}