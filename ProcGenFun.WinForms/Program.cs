namespace ProcGenFun.WinForms;

using RandN;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        ApplicationConfiguration.Initialize();
        
        var rng = StandardRng.Create();

        Application.Run(new MainForm(rng));
    }
}